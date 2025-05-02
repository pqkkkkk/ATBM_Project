--Cài VPD cho từng vai trò với thao tác SELECT:
create or REPLACE function SV_SELECT(
   p_schemma VARCHAR2,
   p_object_name VARCHAR2)
return VARCHAR2
AUTHID CURRENT_USER
as
   username VARCHAR2(10);
   facultyOfTeacher VARCHAR2(10);
   isSV INTEGER;
   isGV INTEGER;
begin
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_SV');
   isGV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_GV');
   
   if isSV >= 1 then
      username := SUBSTR(SYS_CONTEXT('USERENV','SESSION_USER'), INSTR(SYS_CONTEXT('USERENV','SESSION_USER'), '_') + 1);
      RETURN 'MASV = ''' || username || '''';
   ELSIF isGV >= 1 then
      username := SUBSTR(SYS_CONTEXT('USERENV','SESSION_USER'), INSTR(SYS_CONTEXT('USERENV','SESSION_USER'), '_') + 1);
      SELECT MADV INTO facultyOfTeacher FROM X_ADMIN.NHANVIEN WHERE MANV = username;
      RETURN 'KHOA = ''' || facultyOfTeacher || '''';
   ELSE
      return '1=1';
   END IF;
end SV_SELECT;
/

--Gắn hàm thực hiện chính sách SV_SELECT vào bảng SINHVIEN:
BEGIN
 DBMS_RLS.ADD_POLICY(
   object_schema   => 'X_ADMIN',
   object_name     => 'SINHVIEN',
   policy_name     => 'SV_SELECT',
   function_schema => 'SYS',
   policy_function => 'SV_SELECT',
   statement_types => 'SELECT',
   update_check    => TRUE
    );
END;
/

BEGIN
   DBMS_RLS.DROP_POLICY(
      object_schema   => 'X_ADMIN',
      object_name     => 'SINHVIEN',
      policy_name     => 'SV_SELECT'
   );
END;
/
COMMIT;

--Cài VPD cho từng vai trò với thao tác UPDATE:
create function SV_UPDATE
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin
   if 'SV' in (select * from SESSION_ROLES) then
      return 'MASV = '||SYS_CONTEXT('USERENV','SESSION_USER');
   if 'NVPDT' or 'NVCTSV' in (select * from SESSION_ROLES) then
      return '1=1';
   end if;
   return '1=0';


end SV_UPDATE;


--Gắn hàm thực hiện chính sách SV_UPDATE vào bảng SINHVIEN:
		EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'SINHVIEN',
        policy_name     => 'SV_UPDATE',
        function_schema => 'DAIHOCX',
        policy_function => 'SV_UPDATE',
        statement_types => 'UPDATE',
        update_check    => TRUE
    );

--Cấp quyền UPDATE(địa chỉ, sdt) cho vai trò ‘SV’
--Cấp quyền INSERT, DELETE, UPDATE trên tất cả thuộc tính trừ TINHTRANG cho vai trò ‘NV PCTSV’
--Cấp quyền UPDATE(TINHTRANG) cho vai trò ‘NV PĐT’
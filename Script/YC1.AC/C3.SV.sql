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
   username := SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_SV');
   isGV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_GV');
   
   if isSV >= 1 then
      RETURN 'MASV = ''' || username || '''';
   ELSIF isGV >= 1 then
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
create or REPLACE function SV_UPDATE
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2
as
   username VARCHAR2(10);
   isSV INTEGER;
   isNVPDT INTEGER;
   isNVCTSV INTEGER;
begin
   username := SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_SV');
   isNVPDT := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_NVPDT');
   isNVCTSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_NVCTSV');

   if isSV >= 1 then
      RETURN 'MASV = ''' || username || '''';
   ELSIF isNVPDT >= 1 or isNVCTSV >= 1  then
      return '1=1';
   ELSE
      return '1=0';
   end if;
end SV_UPDATE;
/
COMMIT;
--Gắn hàm thực hiện chính sách SV_UPDATE vào bảng SINHVIEN:
BEGIN
		DBMS_RLS.ADD_POLICY(
        object_schema   => 'X_ADMIN',
        object_name     => 'SINHVIEN',
        policy_name     => 'SV_UPDATE',
        function_schema => 'SYS',
        policy_function => 'SV_UPDATE',
        statement_types => 'UPDATE',
        update_check    => TRUE
    );
END;
/


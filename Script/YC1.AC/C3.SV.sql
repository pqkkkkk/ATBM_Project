--Cài VPD cho từng vai trò với thao tác SELECT:
create function SV_SELECT
   (p_schemma VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin
   if 'SV' in (select * from SESSION_ROLES) then
      return 'MASV = '||SYS_CONTEXT('USERENV','SESSION_USER');
   if 'GV' in (select * from SESSION_ROLES) then
      return 'KHOA = (select MAĐV from NHANVIEN where MANLĐ = ''' || SYS_CONTEXT('USERENV','SESSION_USER') || ''')';
   return '1=0';
end SV_SELECT;


--Gắn hàm thực hiện chính sách SV_SELECT vào bảng SINHVIEN:
EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'SINHVIEN',
        policy_name     => 'SV_SELECT',
        function_schema => 'DAIHOCX',
        policy_function => 'SV_SELECT',
        statement_types => 'SELECT',
        update_check    => TRUE
    );


--Cài VPD cho từng vai trò với thao tác UPDATE:
create function SV_UPDATE
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin
   if 'SV' in (select * from SESSION_ROLES) then
      return 'MASV = '||SYS_CONTEXT('USERENV','SESSION_USER');
   if 'NV PĐT' or 'NV PCTSV' in (select * from SESSION_ROLES) then
      return '1=1’;
   return '1=0';


end SV_UPDATE;


--Gắn hàm thực hiện chính sách SV_UPDATE vào bảng SINHVIEN:
		EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'SINHVIEN',
        policy_name     => 'SV_UPDATE',
        function_schema => 'DAIHOCX',
        policy_function => 'SV_UPDATE',
        statement_types => ‘UPDATE',
        update_check    => TRUE
    );

--Cấp quyền UPDATE(địa chỉ, sdt) cho vai trò ‘SV’
--Cấp quyền INSERT, DELETE, UPDATE trên tất cả thuộc tính trừ TINHTRANG cho vai trò ‘NV PCTSV’
--Cấp quyền UPDATE(TINHTRANG) cho vai trò ‘NV PĐT’

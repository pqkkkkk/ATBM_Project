--Cài VPD cho từng vai trò với thao tác SELECT:
create function DANGKY_SELECT
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin
   if 'SV' in (select * from SESSION_ROLES) then
      return 'MASV = '||SYS_CONTEXT('USERENV','SESSION_USER');
   if 'NV PKT' 
in (select * from SESSION_ROLES) then
      return '1=1';
   if 'GV' in (select * from SESSION_ROLES) then
      return 'MAMM = (select MAMM from MOMON where MAGV = ''' || SYS_CONTEXT('USERENV','SESSION_USER') || ''')';
   return '1=0';
end DANGKY_SELECT;


--Gắn hàm thực hiện chính sách DANGKY_SELECT vào bảng DANGKY:
EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_SELECT',
        function_schema => 'DAIHOCX',
        policy_function => 'DANGKY_SELECT',
        statement_types => 'SELECT',
        update_check    => TRUE
    );


--Cài VPD cho từng vai trò với thao tác INSERT, UPDATE, DELETE với các trường MASV, MAMM:
create function DANGKY_INS_DEL_UPD
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin


   if 'SV' or 'NV PĐT' in (select * from SESSION_ROLES) then
      return q'[MAMM = (select MAMM from MOMON m1 where (SYSDATE - (
         SELECT
            CASE
                WHEN HK = 1 THEN TO_DATE('01-09-' || NAM, 'DD-MM-YYYY')
                WHEN HK = 2 THEN TO_DATE('01-01-' || (NAM + 1), 'DD-MM-YYYY')
                WHEN HK = 3 THEN TO_DATE('01-05-' || (NAM + 1), 'DD-MM-YYYY')
                ELSE NULL
         FROM MOMON m2
         WHERE m2.MAMM = m1.MAMM
      )) <= 14)]';
   return '1=0';
end DANGKY_INS_DEL_UPD;

--Gắn hàm thực hiện chính sách DANGKY_INS_DEL_UPD vào bảng DANGKY:
EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_INS_DEL_UPD',
        function_schema => 'DAIHOCX',
        policy_function => 'DANGKY_INS_DEL_UPD',
        statement_types => 'INSERT, UPDATE, DELETE',
        sec_relevant_cols  => 'MASV, MAMM',
        update_check    => TRUE
    );


--Cài VPD cho vai trò SV với thao tác INSERT, UPDATE, DELETE với các trường MASV, MAMM:
create function DANGKY_INS_DEL_UPD_SV
   (p_schema VARCHAR2, p_obj VARCHAR2)
return VARCHAR2 as
begin
   if 'SV' in (select * from SESSION_ROLES) then
      return 'MASV = '||SYS_CONTEXT('USERENV','SESSION_USER');
   return ' ';
end DANGKY_INS_DEL_UPD_SV;


--Gắn hàm thực hiện chính sách DANGKY_INS_DEL_UPD_SV vào bảng DANGKY:
EXECUTE DBMS_RLS.ADD_POLICY(
        object_schema   => 'DAIHOCX',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_INS_DEL_UPD_SV',
        function_schema => 'DAIHOCX',
        policy_function => 'DANGKY_INS_DEL_UPD_SV',
        statement_types => 'INSERT, UPDATE, DELETE',
        sec_relevant_cols  => 'MASV, MAMM',
        update_check    => TRUE
    );

--Cấp quyền UPDATE(DIEMTH, DIEMQT, DIEMCK, DIEMTK) cho vai trò “NV PKT”

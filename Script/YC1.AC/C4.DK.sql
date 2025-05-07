--Cài VPD cho từng vai trò với thao tác SELECT:

CREATE OR REPLACE FUNCTION DANGKY_SELECT(
   p_schema VARCHAR2,
   p_object_name VARCHAR2
) RETURN VARCHAR2 
AS
   username VARCHAR2(10);
   isSV INTEGER;
   isGV INTEGER;
   isNVPKT INTEGER;
   predicate VARCHAR2(4000);
BEGIN
   username := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'USER_NAME');
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_SV');
   isGV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_GV');
   isNVPKT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPKT');

   IF isSV >= 1 THEN
      RETURN 'MASV = ''' || username || '''';
   ELSIF isNVPKT >= 1 THEN
      RETURN '1=1';
   ELSIF isGV >= 1 THEN
      RETURN  'MAMM IN (SELECT MAMM FROM X_ADMIN.MOMON WHERE MAGV = ''' || username || ''')';
      
   ELSE
      RETURN '1=0';
   END IF;

   RETURN predicate;
END DANGKY_SELECT;
/

--Gắn hàm thực hiện chính sách DANGKY_SELECT vào bảng DANGKY:
BEGIN
 DBMS_RLS.ADD_POLICY(
        object_schema   => 'X_ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_SELECT',
        function_schema => 'SYS',
        policy_function => 'DANGKY_SELECT',
        statement_types => 'SELECT',
        update_check    => TRUE
    );
END;
/

BEGIN
    DBMS_RLS.DROP_POLICY(
        object_schema   => 'X_ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_SELECT'
    );
END;
COMMIT;
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
        object_schema   => 'X_ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_INS_DEL_UPD',
        function_schema => 'SYS',
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

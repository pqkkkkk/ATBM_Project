--Cài VPD cho từng vai trò với thao tác SELECT:
<<<<<<< HEAD

CREATE OR REPLACE FUNCTION DANGKY_SELECT(
   p_schema VARCHAR2,
   p_object_name VARCHAR2
) RETURN VARCHAR2 
AS
   username VARCHAR2(10);
   v_mamm VARCHAR2(10);
   isSV INTEGER;
   isGV INTEGER;
   isNVPKT INTEGER;
   isNVPDT INTEGER;
BEGIN
   username := SUBSTR(SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME'),1);
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_SV');
   isGV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_GV');
   isNVPKT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPKT');
   isNVPDT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPDT');

   IF isSV >= 1 THEN
      RETURN 'MASV = ''' || username || '''';
   ELSIF isNVPKT >= 1 or isNVPDT >= 1 THEN
      RETURN '1=1';
   ELSIF isGV >= 1 THEN
      RETURN  'MAMM IN (SELECT MAMM FROM X_ADMIN.MOMON WHERE MAGV = ''' || username || ''')';
   ELSE
      RETURN '1=0';
   END IF;
END DANGKY_SELECT;
/

--Gắn hàm thực hiện chính sách DANGKY_SELECT vào bảng DANGKY:
BEGIN
 DBMS_RLS.ADD_POLICY(
        object_schema   => 'X_ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_SELECT',
        function_schema => 'X_ADMIN',
        policy_function => 'DANGKY_SELECT',
        statement_types => 'SELECT',
        update_check    => TRUE
    );
END;
/

--BEGIN
--    DBMS_RLS.DROP_POLICY(
--        object_schema   => 'X_ADMIN',
--        object_name     => 'DANGKY',
--        policy_name     => 'DANGKY_SELECT'
--    );
--END;
COMMIT;



--Cài VPD cho từng vai trò với thao tác INSERT, UPDATE, DELETE với các trường MASV, MAMM:
CREATE OR REPLACE FUNCTION DANGKY_INS_DEL_UPD
   (p_schema VARCHAR2, p_obj VARCHAR2)
RETURN VARCHAR2 
AS
   username VARCHAR2(10);
   isSV INTEGER;
   isNVPKT INTEGER;
   isNVPDT INTEGER;
   
   v_today      DATE := SYSDATE;
   v_startHK    DATE;
   v_month      NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'MM'));
   v_year       NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'));
BEGIN
   username := SUBSTR(SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME'),1);
   isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_SV');
   isNVPKT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPKT');
   isNVPDT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPDT');
   
   IF v_month BETWEEN 9 AND 12 THEN
         v_startHK := TO_DATE('01-09-' || v_year, 'DD-MM-YYYY');
      ELSIF v_month = 1 THEN
         v_startHK := TO_DATE('01-09-' || (v_year - 1), 'DD-MM-YYYY');
      ELSIF v_month BETWEEN 2 AND 6 THEN
         v_startHK := TO_DATE('01-02-' || v_year, 'DD-MM-YYYY');
      ELSE
         v_startHK := TO_DATE('01-07-' || v_year, 'DD-MM-YYYY');
   END IF;
   
   IF isSV >= 1 THEN
   BEGIN
      IF v_today - v_startHK <= 14 THEN
         RETURN 'MASV = ''' || username || '''';
      ELSE
         RETURN '1=0';
      END IF;
   END;
   
   ELSIF isNVPDT >= 1 THEN
   BEGIN
      IF v_today - v_startHK <= 14 THEN
         RETURN '1=1';
      ELSE
         RETURN '1=0';
      END IF;
   END;
   
   ELSIF isNVPKT >= 1 THEN
      RETURN '1=1';
   ELSE 
      RETURN '1=0';
   END IF;
END DANGKY_INS_DEL_UPD;
/
BEGIN
 DBMS_RLS.ADD_POLICY(
        object_schema   => 'X_ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_INS_DEL_UPD',
        function_schema => 'X_ADMIN',
        policy_function => 'DANGKY_INS_DEL_UPD',
        statement_types => 'INSERT, UPDATE, DELETE',
        update_check    => TRUE
    );
END;
/

GRANT SELECT ON X_ADMIN.DANGKY TO XR_GV;
GRANT SELECT, UPDATE(DIEMTH, DIEMCT, DIEMCK, DIEMTK) ON X_ADMIN.DANGKY TO XR_NVPKT;
GRANT SELECT, UPDATE(MASV, MAMM), DELETE, INSERT ON X_ADMIN.DANGKY TO XR_SV;
GRANT SELECT, UPDATE(MASV, MAMM), DELETE, INSERT ON X_ADMIN.DANGKY TO XR_NVPDT;
=======
   CREATE OR REPLACE FUNCTION DANGKY_SELECT(
      p_schema VARCHAR2,
      p_object_name VARCHAR2
   ) RETURN VARCHAR2 
   AS
      username VARCHAR2(10);
      v_mamm VARCHAR2(10);
      isSV INTEGER;
      isGV INTEGER;
      isNVPKT INTEGER;
      isNVPDT INTEGER;
   BEGIN
      username := SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
      isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_SV');
      isGV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_GV');
      isNVPKT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPKT');
      isNVPDT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPDT');

      IF isSV >= 1 THEN
         RETURN 'MASV = ''' || username || '''';
      ELSIF isNVPKT >= 1 or isNVPDT >= 1 THEN
         RETURN '1=1';
      ELSIF isGV >= 1 THEN
         RETURN  'MAMM IN (SELECT MAMM FROM X_ADMIN.MOMON WHERE MAGV = ''' || username || ''')';
      ELSE
         RETURN '1=0';
      END IF;
   END DANGKY_SELECT;
   /
   --Gắn hàm thực hiện chính sách DANGKY_SELECT vào bảng DANGKY:
   BEGIN
   DBMS_RLS.ADD_POLICY(
         object_schema   => 'X_ADMIN',
         object_name     => 'DANGKY',
         policy_name     => 'DANGKY_SELECT',
         function_schema => 'X_ADMIN',
         policy_function => 'DANGKY_SELECT',
         statement_types => 'SELECT',
         update_check    => TRUE
      );
   END;
   /
   COMMIT;


--Cài VPD cho từng vai trò với thao tác INSERT, UPDATE, DELETE với các trường MASV, MAMM:
   CREATE OR REPLACE FUNCTION DANGKY_INS_DEL_UPD
      (p_schema VARCHAR2, p_obj VARCHAR2)
   RETURN VARCHAR2 
   AS
      username VARCHAR2(10);
      isSV INTEGER;
      isNVPKT INTEGER;
      isNVPDT INTEGER;
      v_today      DATE := SYSDATE;
      v_startHK    DATE;
      v_month      NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'MM'));
      v_year       NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'));
   BEGIN
      username := SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
      isSV := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_SV');
      isNVPKT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPKT');
      isNVPDT := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'IS_NVPDT');
      
      IF v_month BETWEEN 9 AND 12 THEN
            v_startHK := TO_DATE('01-09-' || v_year, 'DD-MM-YYYY');
         ELSIF v_month = 1 THEN
            v_startHK := TO_DATE('01-09-' || (v_year - 1), 'DD-MM-YYYY');
         ELSIF v_month BETWEEN 2 AND 6 THEN
            v_startHK := TO_DATE('01-02-' || v_year, 'DD-MM-YYYY');
         ELSE
            v_startHK := TO_DATE('01-07-' || v_year, 'DD-MM-YYYY');
      END IF;
      
      IF isSV >= 1 THEN
      BEGIN
         IF v_today - v_startHK <= 14 THEN
            RETURN 'MASV = ''' || username || '''';
         ELSE
            RETURN '1=0';
         END IF;
      END;
      
      ELSIF isNVPDT >= 1 THEN
      BEGIN
         IF v_today - v_startHK <= 14 THEN
            RETURN '1=1';
         ELSE
            RETURN '1=0';
         END IF;
      END;
      
      ELSIF isNVPKT >= 1 THEN
         RETURN '1=1';
      ELSE 
         RETURN '1=0';
      END IF;
   END DANGKY_INS_DEL_UPD;
   /
   BEGIN
   DBMS_RLS.ADD_POLICY(
         object_schema   => 'X_ADMIN',
         object_name     => 'DANGKY',
         policy_name     => 'DANGKY_INS_DEL_UPD',
         function_schema => 'X_ADMIN',
         policy_function => 'DANGKY_INS_DEL_UPD',
         statement_types => 'INSERT, UPDATE, DELETE',
         update_check    => TRUE
      );
   END;
   /
   GRANT SELECT ON X_ADMIN.DANGKY TO XR_GV;
   GRANT SELECT, UPDATE(DIEMTH, DIEMCT, DIEMCK, DIEMTK) ON X_ADMIN.DANGKY TO XR_NVPKT;
   GRANT SELECT, UPDATE(MASV, MAMM), DELETE, INSERT ON X_ADMIN.DANGKY TO XR_SV;
   GRANT SELECT, UPDATE(MASV, MAMM), DELETE, INSERT ON X_ADMIN.DANGKY TO XR_NVPDT;
   COMMIT;
>>>>>>> 4faf2d14a50582d7d7e1fc5157e1e224208108d8

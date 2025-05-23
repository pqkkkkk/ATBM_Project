select value
from v$option
where parameter = 'Unified Auditing';

SHOW PARAMETER audit_trail;

SELECT * FROM DBA_AUDIT_TRAIL;

SELECT CURRENT_USER,
       ACTION_NAME,
       OBJECT_NAME,
       EVENT_TIMESTAMP,
       SQL_TEXT, 
       RETURN_CODE,
       FGA_POLICY_NAME
FROM UNIFIED_AUDIT_TRAIL
--WHERE FGA_POLICY_NAME = 'AUDIT_INSERT_UPDATE_DELETE_DANGKY' OR FGA_POLICY_NAME ='AUDIT_NOT_IN_MODIFY_TIME_DANGKY'
ORDER BY EVENT_TIMESTAMP DESC;


----------------------------Bài làm-----------------------------
-----------------------------2.1--------------------------------
AUDIT SELECT ON X_ADMIN.DANGKY;

SELECT * FROM X_ADMIN.DANGKY;
COMMIT;

SELECT * FROM SYS.AUD$; -- Bảng gốc

-- Hoặc bảng view tiện lợi hơn:
SELECT * FROM DBA_AUDIT_TRAIL;
SELECT * FROM DBA_FGA_AUDIT_TRAIL; -- Nếu dùng Fine-Grained Auditing
SELECT * FROM UNIFIED_AUDIT_TRAIL ORDER BY EVENT_TIMESTAMP DESC;

ALTER SYSTEM SET audit_sys_operations=TRUE SCOPE=SPFILE;


SELECT * FROM DBA_AUDIT_POLICIES;

-----------------------------3.1--------------------------------
--Ta dùng Fine-grained Audit để thực hiện audit
BEGIN
  DBMS_FGA.add_policy(
    object_schema   => 'X_ADMIN',
    object_name     => 'DANGKY',
    policy_name     => 'AUDIT_UPDATE_DIEM',
    audit_condition => 'SYS_CONTEXT(''X_UNIVERITY_CONTEXT'', ''IS_NVPKT'') < 1',
    audit_column    => 'DIEMTH, DIEMCT, DIEMCK, DIEMTK',
    statement_types => 'UPDATE',
    enable          => TRUE
  );
END;
/

--update  X_ADMIN.DANGKY
--set DIEMTH = 8
--where MASV = 'SV0010';
--
--BEGIN
--  DBMS_FGA.drop_policy(
--    object_schema => 'X_ADMIN',
--    object_name   => 'DANGKY',
--    policy_name   => 'AUDIT_UPDATE_DIEM'
--  );
--END;
--/
--
--select * from X_ADMIN.DANGKY;

--------------------------------------3.2----------------------------------
BEGIN
  DBMS_FGA.add_policy(
    object_schema   => 'X_ADMIN',
    object_name     => 'NHANVIEN',
    policy_name     => 'AUDIT_SELECT_NHANVIEN',
    audit_condition => 'SYS_CONTEXT(''X_UNIVERITY_CONTEXT'', ''IS_NVTCHC'') < 1',
    audit_column    => 'LUONG, PHUCAP',
    statement_types => 'SELECT',
    enable          => TRUE
  );
END;
/

--BEGIN
--  DBMS_FGA.drop_policy(
--    object_schema => 'X_ADMIN',
--    object_name   => 'NHANVIEN',
--    policy_name   => 'AUDIT_SELECT_NHANVIEN'
--  );
--END;
--/
--
--select LUONG, PHUCAP from X_ADMIN.NHANVIEN;

----------------------------3.3-------------------------------
--Tạo function để kiểm tra có nằm trong thời gian hiệu chỉnh học phần hay không
CREATE OR REPLACE FUNCTION isInModifyTime
RETURN NUMBER DETERMINISTIC
AS   
   v_today      DATE := SYSDATE;
   v_startHK    DATE;
   v_month      NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'MM'));
   v_year       NUMBER := TO_NUMBER(TO_CHAR(SYSDATE, 'YYYY'));
BEGIN
   IF v_month BETWEEN 9 AND 12 THEN
         v_startHK := TO_DATE('01-09-' || v_year, 'DD-MM-YYYY');
      ELSIF v_month = 1 THEN
         v_startHK := TO_DATE('01-09-' || (v_year - 1), 'DD-MM-YYYY');
      ELSIF v_month BETWEEN 2 AND 6 THEN
         v_startHK := TO_DATE('01-02-' || v_year, 'DD-MM-YYYY');
      ELSE
         v_startHK := TO_DATE('01-07-' || v_year, 'DD-MM-YYYY');
   END IF;
   
   IF v_today - v_startHK > 14 THEN
      RETURN 1;
   ELSE RETURN 0;
   END IF;
END isInModifyTime;
/

--Ta dùng Fine-grained Audit để thực hiện audit với trường hợp Hành vi thêm, xóa, sửa trên quan hệ DANGKY của sinh viên nhưng trên dòng
-- dữ liệu của sinh viên khác
BEGIN
  DBMS_FGA.add_policy(
    object_schema   => 'X_ADMIN',
    object_name     => 'DANGKY',
    policy_name     => 'AUDIT_NOT_IN_MODIFY_TIME_DANGKY',
    audit_condition => 'X_ADMIN.isInModifyTime = 1',
    statement_types => 'INSERT, UPDATE, DELETE',
    enable          => TRUE
  );
END;
/

--BEGIN
--  DBMS_FGA.drop_policy(
--    object_schema => 'X_ADMIN',
--    object_name   => 'DANGKY',
--    policy_name   => 'AUDIT_NOT_IN_MODIFY_TIME_DANGKY'
--  );
--END;
--/
--
--update X_ADMIN.DANGKY
--SET MAMM = 'MM011'
--WHERE MASV = 'SV0010';

--thêm xóa sửa trên dòng dữ liệu của sinh viên khác
CREATE OR REPLACE FUNCTION get_audit_condition(
  v_is_student IN NUMBER,
  v_username IN VARCHAR2,
  v_masv IN VARCHAR2) RETURN NUMBER
AUTHID CURRENT_USER
AS
BEGIN
--RETURN 'MASV !=  SYS_CONTEXT(''X_UNIVERITY_CONTEXT'', ''USER_NAME'')';
  IF v_is_student >=1 AND v_masv != v_username THEN
    RETURN 1;
  ELSE 
   RETURN 0;
  END IF;
END;
/

BEGIN
  DBMS_FGA.add_policy(
    object_schema   => 'X_ADMIN',
    object_name     => 'DANGKY',
    policy_name     => 'AUDIT_INSERT_UPDATE_DELETE_DANGKY',
    audit_condition => 'X_ADMIN.get_audit_condition(SYS_CONTEXT(''X_UNIVERITY_CONTEXT'', ''IS_SV''), SYS_CONTEXT(''X_UNIVERITY_CONTEXT'', ''USER_NAME''), MASV) = 1',
    statement_types => 'INSERT, UPDATE, DELETE'
  );
END;
/

--BEGIN
--  DBMS_FGA.drop_policy(
--    object_schema => 'X_ADMIN',
--    object_name   => 'DANGKY',
--    policy_name   => 'AUDIT_INSERT_UPDATE_DELETE_DANGKY'
--  );
--END;
--/
--
--update X_ADMIN.DANGKY
--SET MAMM = 'MM009'
--WHERE MASV = 'SV0010';
--
--
--SELECT *
--FROM dba_audit_policies
--WHERE object_schema = 'X_ADMIN'
--  AND object_name = 'DANGKY';




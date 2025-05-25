-- 1. Bật tắt audit
  ALTER SYSTEM SET audit_sys_operations=TRUE SCOPE=SPFILE;

-- 2. Dùng Standard Audit
  AUDIT SELECT ON X_ADMIN.DANGKY BY ACCESS;
  COMMIT;

-- 3. Dùng Fine-Grained Audit
  -- a. Hành vi cập nhật quan hệ ĐANGKY tại các trường liên quan đến điểm số nhưng
  --người đó không thuộc vai trò “NV PKT”.
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
  -- b. Hành vi của người dùng (không thuộc vai trò “NV TCHC”) có thể đọc trên
  --trường LUONG, PHUCAP của người khác hoặc cập nhật ở quan hệ NHANVIEN.
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
  -- c. Hành vi thêm, xóa, sửa trên quan hệ DANGKY của sinh viên nhưng trên dòng
  --dữ liệu của sinh viên khác hoặc thực hiện hiệu chỉnh đăng ký học phần ngoài
  --thời gian cho phép hiệu chỉnh đăng ký học phần.

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




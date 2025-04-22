DECLARE
    v_roles SYS_REFCURSOR; -- Con trỏ tham chiếu
    v_role_record DBA_OBJECTS%ROWTYPE; -- Biến để lưu từng dòng dữ liệu
BEGIN
    -- Gọi thủ tục để lấy danh sách roles
    GetAllInstanceOfSpecificObject('VIEW', v_roles);

    -- Duyệt qua từng dòng dữ liệu trong con trỏ
    LOOP
        FETCH v_roles INTO v_role_record;
        EXIT WHEN v_roles%NOTFOUND;

        -- In ra từng dòng dữ liệu
        DBMS_OUTPUT.PUT_LINE('object name: ' || v_role_record.object_name);
        DBMS_OUTPUT.PUT_LINE('object type: ' || v_role_record.object_type);
    END LOOP;

    -- Đóng con trỏ
    CLOSE v_roles;
END;
/

SELECT *
FROM DBA_TAB_PRIVS 
WHERE  GRANTEE = 'C##ADMIN1';

SELECT *
FROM DBA_OBJECTS
WHERE OBJECT_TYPE = 'TABLE'
FETCH FIRST 10 ROWS ONLY;

SELECT * FROM DBA_SYS_PRIVS;

SELECT COLUMN_NAME
FROM USER_TAB_COLUMNS
WHERE TABLE_NAME = UPPER('DBA_SYS_PRIVS');

SELECT *
FROM DBA_TAB_COLUMNS
WHERE GRANTEE = 'C##ADMIN1';

GRANT EXECUTE ON getColumns TO C##ADMIN1;
GRANT INSERT ON C##ADMIN1.QLDT_DETAI TO C##ADMIN1;

-- câu lệnh truy vấn lấy danh sách tất cả procedure có ngày tạo trong năm 2025
SELECT * FROM DBA_OBJECTS 
WHERE OBJECT_TYPE = 'PROCEDURE' AND CREATED BETWEEN TO_DATE('2025-01-01', 'YYYY-MM-DD') AND TO_DATE('2025-12-31', 'YYYY-MM-DD')
AND OWNER = 'C##ADMIN1';

GRANT SELECT ANY DICTIONARY TO C##ADMIN1;

GRANT SELECT ON ACCOUNTS_22120174 TO C##ADMIN1;
GRANT UPDATE(ACCNO) ON ACCOUNTS_22120174 TO C##ADMIN1;
REVOKE UPDATE ON ACCOUNTS_22120174 FROM C##ADMIN1;

GRANT UPDATE(ACCNO) ON ADMIN_ACCOUNTS_22120174_VIEW TO C##ADMIN1;

SELECT * FROM SYSTEM_PRIVILEGE_MAP;
COMMIT;
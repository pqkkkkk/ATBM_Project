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
WHERE TYPE = 'PROCEDURE';

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
WHERE TABLE_NAME = UPPER('EMP_22120174');
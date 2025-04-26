
----------------------------------------------------------------------
-- Check role exist 
CREATE OR REPLACE PROCEDURE checkRoleExist(
    roleName IN VARCHAR2,
    exist out NUMBER
) 
AS
BEGIN
    SELECT COUNT(*) INTO exist
    FROM user_role_privs
    WHERE GRANTED_ROLE = UPPER(roleName);
END;
/

-- Create Role
CREATE OR REPLACE PROCEDURE createRole(user_role in VARCHAR2)
AS
BEGIN
    EXECUTE IMMEDIATE 'CREATE ROLE ' || user_role;
    DBMS_OUTPUT.PUT_LINE('Role created successfully.');
    EXCEPTION 
        WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
        RAISE;
END;
/

-- DROP ROLE
CREATE OR REPLACE PROCEDURE dropRole(user_role in VARCHAR2)
AS
BEGIN
    EXECUTE IMMEDIATE 'DROP ROLE ' || user_role;
    DBMS_OUTPUT.PUT_LINE('Role dropped successfully.');
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Role not exist.');
        RAISE;
END;
/

-- Lấy tất cả các role của user hiện tại
CREATE OR REPLACE PROCEDURE GetAllRoles(
    role_list OUT SYS_REFCURSOR
) 
AS
BEGIN
    OPEN role_list FOR
        SELECT GRANTED_ROLE, ADMIN_OPTION, DEFAULT_ROLE, OS_GRANTED, COMMON, INHERITED
        FROM user_role_privs;
END;
/

CREATE OR REPLACE PROCEDURE X_ADMIN_GetAllInstanceOfSpecificObject(
    type IN VARCHAR2, 
    result_ OUT SYS_REFCURSOR
)
AS
BEGIN
    DBMS_OUTPUT.PUT_LINE('>> Đã nhận tham số object_type: ' || UPPER(type));

    OPEN result_ FOR
    SELECT *
    FROM ALL_OBJECTS
    WHERE OBJECT_TYPE = UPPER(type);

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
END;
/

CREATE OR REPLACE PROCEDURE X_ADMIN_getColumns(
    object_name IN VARCHAR2, 
    result OUT SYS_REFCURSOR
) 
AS
BEGIN
    OPEN result FOR
        SELECT COLUMN_NAME
        FROM USER_TAB_COLUMNS
        WHERE TABLE_NAME = UPPER(object_name)
        ORDER BY COLUMN_ID;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        OPEN result FOR SELECT 'Table not found' AS COLUMN_NAME FROM DUAL;
    WHEN OTHERS THEN
        OPEN result FOR SELECT 'Error: ' AS COLUMN_NAME FROM DUAL;
END;
/
CREATE OR REPLACE PROCEDURE X_ADMIN_RevokePrivilegesOfUserOnSpecificObjectType(
    privilege_ IN VARCHAR2,    -- Quyền cần thu hồi
    name_ IN VARCHAR2,         -- Tên user/role muốn thu hồi quyền
    object_ IN VARCHAR2        -- Tên object (table, view, ...)
)
AS
BEGIN
    EXECUTE IMMEDIATE 'REVOKE ' || privilege_ || ' ON ' || object_ || ' FROM ' || name_;
    DBMS_OUTPUT.PUT_LINE('Revoke Privilege successfully from' || name_ || ' on ' || object_ );
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE X_ADMIN_GrantPrivilegesOnSpecificColumnsOfTableOrView (
    p_withGrantOption IN VARCHAR2,
    p_privilege IN VARCHAR2,
    p_user    IN VARCHAR2,
    p_columns IN VARCHAR2,
    p_object  IN VARCHAR2 
)
AS
    sqlQuery VARCHAR2(4000);
BEGIN
    sqlQuery := 'GRANT ' || p_privilege || ' (' || p_columns || ') ON ' || p_object || ' TO ' || p_user;
    IF p_withGrantOption = 'YES' THEN
        sqlQuery := sqlQuery || ' WITH GRANT OPTION';
    END IF;

    DBMS_OUTPUT.PUT_LINE('Executing SQL: ' || sqlQuery);

    EXECUTE IMMEDIATE sqlQuery;
    DBMS_OUTPUT.PUT_LINE('Grant privileges on specific columns successfully.');
EXCEPTION
    WHEN OTHERS THEN
       RAISE;
END;
/
CREATE OR REPLACE PROCEDURE X_ADMIN_GetUserRoles(
    p_username IN VARCHAR2, 
    p_roles OUT SYS_REFCURSOR
) 
AS
BEGIN
    OPEN p_roles FOR
        SELECT * 
        FROM dba_role_privs 
        WHERE GRANTEE = UPPER(p_username);
END;
/

-- grant role cho 1 user
CREATE OR REPLACE PROCEDURE grantRole(
    role_name in VARCHAR2,
    user_name in VARCHAR2,
    withGrantOption in VARCHAR2 -- Nếu có truyền 'YES', không truyền 'NO'
    )
AS
    sqlQuery VARCHAR2(4000);  -- câu lệnh SQL 
BEGIN
    sqlQuery := 'GRANT ' || role_name || ' TO ' || user_name;
    IF withGrantOption = 'YES' THEN
        sqlQuery:= sqlQuery || ' WITH ADMIN OPTION';
    END IF;
    EXECUTE IMMEDIATE sqlQuery;
    DBMS_OUTPUT.PUT_LINE('Grant roles succesfully');
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
        RAISE;
END;
/
-- revoke role khỏi 1 user
CREATE OR REPLACE PROCEDURE revokeRoleFromUser(
    role_ IN VARCHAR2,        
    user_name IN VARCHAR2         
)
AS
BEGIN
    EXECUTE IMMEDIATE 'REVOKE ' || role_ || ' FROM ' || user_name;
    DBMS_OUTPUT.PUT_LINE('Revoke role successfully from ' || user_name);
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;
/
COMMIT;

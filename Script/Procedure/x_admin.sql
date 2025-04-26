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

-- Lấy các role của 1 user cụ thể  
CREATE OR REPLACE PROCEDURE GetUserRoles(
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
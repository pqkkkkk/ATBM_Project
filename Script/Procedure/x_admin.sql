-- kiểm tra user tồn tại
    CREATE OR REPLACE PROCEDURE X_ADMIN_checkExistUser(
        name_ in VARCHAR2,
        exist out NUMBER
    )
    AS 
    BEGIN 

        SELECT COUNT(*) INTO exist
        FROM ALL_USERS 
        WHERE USERNAME = name_;
    END;
    /

-- TẠO USER
    CREATE OR REPLACE PROCEDURE X_ADMIN_createUser(
        user_name IN VARCHAR2, 
        pwd IN VARCHAR2
    )
    AS
    BEGIN
        EXECUTE IMMEDIATE 'CREATE USER ' || user_name || ' IDENTIFIED BY "' || pwd || '"';

        EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || user_name;
        EXECUTE IMMEDIATE 'GRANT CONNECT TO ' || user_name;
        EXECUTE IMMEDIATE 'GRANT RESOURCE TO ' || user_name;
        DBMS_OUTPUT.PUT_LINE('User ' || user_name || ' created successfully.');
        
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
            RAISE;
    END;
    /

-- Xoá user
    CREATE OR REPLACE PROCEDURE X_ADMIN_deleteUser(user_name in VARCHAR2)
    AS
    BEGIN
        EXECUTE IMMEDIATE 'DROP USER ' || user_name;
        DBMS_OUTPUT.PUT_LINE('User dropped successfully.');
        EXCEPTION
        WHEN OTHERS THEN    
        DBMS_OUTPUT.PUT_LINE('USER NOT EXIST');
        RAISE;
    END;
    /

    CREATE OR REPLACE PROCEDURE X_ADMIN_updatePassword(
        username in VARCHAR2,
        new_pwd in VARCHAR2
        )
    AS
    BEGIN
        EXECUTE IMMEDIATE 'ALTER USER ' || username || ' IDENTIFIED BY ' || new_pwd;
        EXCEPTION 
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
            RAISE;
    END;
    /
-- Get all users
    CREATE OR REPLACE PROCEDURE X_ADMIN_getAllUsers(user_list OUT SYS_REFCURSOR)
    AS
    BEGIN
        OPEN user_list FOR
        SELECT * FROM all_users
        WHERE username LIKE 'X\_%' ESCAPE '\';
    END;
    /

----------------------------------------------------------------------
-- Check role exist 
    CREATE OR REPLACE PROCEDURE X_ADMIN_checkRoleExist(
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
    CREATE OR REPLACE PROCEDURE X_ADMIN_createRole(user_role in VARCHAR2)
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
    CREATE OR REPLACE PROCEDURE X_ADMIN_dropRole(user_role in VARCHAR2)
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
    CREATE OR REPLACE PROCEDURE X_ADMIN_GetAllRoles(
        role_list OUT SYS_REFCURSOR
    ) 
    AS
    BEGIN
        OPEN role_list FOR
            SELECT GRANTED_ROLE, ADMIN_OPTION, DEFAULT_ROLE, OS_GRANTED, COMMON, INHERITED
            FROM user_role_privs;
    END;
    /
-- Lấy danh sách của kiểu object tương ứng (table, view, procedure, function...)
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
        WHERE OBJECT_TYPE = UPPER(type) AND OWNER = 'X_ADMIN';

    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
    END;
    /

-- Lấy danh sách các cột của table hoặc view
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
-- Thu hồi quyền cho user/role trên table, view, procedure, function
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
-- Cấp quyền cho user/role trên table, view trên 1 số cột nhất định
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
-- Lấy danh sách role của user
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
    CREATE OR REPLACE PROCEDURE X_ADMIN_grantRole(
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
    CREATE OR REPLACE PROCEDURE X_ADMIN_revokeRoleFromUser(
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
-- Lấy danh sách quyền của user/role trên table, view, procedure, function
    CREATE OR REPLACE PROCEDURE X_ADMIN_GetPrivilegesOfUserOnSpecificObjectType(
        name_ IN VARCHAR2,  
        object_type IN VARCHAR2 ,
        result_ OUT SYS_REFCURSOR
    )
    AS
    BEGIN
        OPEN result_ FOR
        SELECT *
        FROM DBA_TAB_PRIVS 
        WHERE GRANTEE = UPPER(name_)
        AND TYPE = UPPER(object_type);
        EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('ERROR:' || SQLERRM);
    END;
    /
-- Cấp quyền cho user/role trên table, view, procedure, function
    CREATE OR REPLACE PROCEDURE X_ADMIN_grantPrivileges(
        -- Truyền vào quyền cần cấp, cứ viết theo format của câu SQL nếu muốn cấp nhiều quyền (string là được)
        privilege_ in VARCHAR2, 
        name_ in VARCHAR2, -- Tên user hoặc role muốn cấp
        object_ in VARCHAR2, -- tên object được cấp (table, views,...)
        withGrantOption in VARCHAR2 -- Nếu có truyền 'YES', không truyền 'NO'
        )
    AS
        sqlQuery VARCHAR2(4000);  -- câu lệnh SQL 
    BEGIN
        sqlQuery := 'GRANT ' || privilege_ || ' ON ' || object_ || ' TO ' || name_;
        IF withGrantOption = 'YES' THEN
            sqlQuery:= sqlQuery || ' WITH GRANT OPTION';
        END IF;
        EXECUTE IMMEDIATE sqlQuery;
        DBMS_OUTPUT.PUT_LINE('Grant priviledges succesfully');
        EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
            RAISE;
    END;
    /
-- Lấy danh sách quyền trên cột
    CREATE OR REPLACE PROCEDURE X_ADMIN_GetColumnPrivilegesOfUser(
        name_ IN VARCHAR2,  
        result_ OUT SYS_REFCURSOR
    )
    AS
    BEGIN
        OPEN result_ FOR
        SELECT * 
        FROM DBA_COL_PRIVS 
        WHERE GRANTEE = UPPER(name_);
        EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('ERROR:' || SQLERRM);
    END;
    /
COMMIT;

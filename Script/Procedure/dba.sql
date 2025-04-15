-- kiểm tra user tồn tại hoặc role tồn tại
CREATE OR REPLACE PROCEDURE checkExist(
    type_ in VARCHAR2,
    name_ in VARCHAR2,
    exist out NUMBER
)
AS 
BEGIN 
    IF (type_ = 'ROLE') THEN
        SELECT COUNT(*) INTO exist
        FROM DBA_ROLES 
        WHERE ROLE = name_;
    ELSIF (type_  = 'USER') THEN
        SELECT COUNT(*) INTO exist
        FROM ALL_USERS 
        WHERE USERNAME = name_;
    END IF;
END;
/
-- DECLARE 
--     EXIST NUMBER;
-- BEGIN
--     CHECKEXIST('USER', 'NV0003', EXIST);
--     DBMS_OUTPUT.PUT_LINE('EXIST: ' || EXIST);
-- END;

-- Tạo user
CREATE OR REPLACE PROCEDURE createUser(
    user_name in VARCHAR2, 
    pwd in VARCHAR2
    )
AS
BEGIN
    EXECUTE IMMEDIATE 'CREATE USER ' || user_name || ' IDENTIFIED BY ' || pwd;
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || user_name; 
    EXECUTE IMMEDIATE 'GRANT CONNECT, RESOURCE TO ' || user_name; 
    DBMS_OUTPUT.PUT_LINE('User created successfully.');
    EXCEPTION
    WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
    RAISE;
END;
/

SELECT * FROM ALL_USERS WHERE USERNAME LIKE 'NV%' ORDER BY USERNAME;

-- Xoá user
CREATE OR REPLACE PROCEDURE deleteUser(user_name in VARCHAR2)
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

-- Sửa (Hiệu chỉnh người dùng) - sửa password
create or replace PROCEDURE updatePassword(
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

-- Tạo role
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
-- EXECUTE createRole('NVCB');
-- EXECUTE createRole('GV');
-- EXECUTE createRole('NVTCHC');
-- EXECUTE createRole('NVPDT');
-- EXECUTE createRole('NVPKT');
-- EXECUTE createRole('TRGDV');
-- EXECUTE createRole('SV');
-- Xoá role
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
-- EXECUTE dropRole('TRGDV');
-- EXECUTE dropRole('NVTCHC');
-- EXECUTE dropRole('NVCB');
-- EXECUTE dropRole('GV');
-- EXECUTE dropRole('NVPKT');
-- EXECUTE dropRole('NVPDT');
-- EXECUTE dropRole('SV');
-- Lấy role của user
CREATE OR REPLACE PROCEDURE GetUserRoles(
    p_username IN VARCHAR2, 
    p_roles OUT SYS_REFCURSOR
) 
AS
BEGIN
    OPEN p_roles FOR
        SELECT * 
        FROM dba_role_privs 
        WHERE grantee = UPPER(p_username);
END;
/

-- Lấy danh sách các role đang có trong hệ thống
CREATE OR REPLACE PROCEDURE getAllRoles(role_list out SYS_REFCURSOR)
AS 
BEGIN
    OPEN role_list FOR
    SELECT * FROM DBA_ROLES;
END;
/

-- VAR v_roles REFCURSOR;
-- EXECUTE getAllRoles(:v_roles);
-- PRINT v_roles;

-- danh sách các quyền mà người dùng hoặc role hiện tại đang có
CREATE OR REPLACE PROCEDURE getUserPrivileges(
    name_ IN VARCHAR2,  
    result_ OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN result_ FOR
    SELECT *
    FROM DBA_TAB_PRIVS 
    WHERE GRANTEE = name_;
END;
/
-- VAR v_roles REFCURSOR;
-- EXECUTE getUserPrivileges( 'NVCB',:v_roles);
-- PRINT v_roles;

-- Cấp quyền cho user/role trên 1 object (with grant option)
CREATE OR REPLACE PROCEDURE grantPrivileges(
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

-- EXECUTE GRANTPRIVILEGES('SELECT', 'NV0001', 'NHANVIEN', 'NO');


CREATE OR REPLACE TYPE string_list AS TABLE OF VARCHAR2(4000);

CREATE OR REPLACE PROCEDURE GrantPrivilegesOnSpecificColumnsOfTableOrView (
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
EXECUTE GrantPrivilegesOnSpecificColumnsOfTableOrView('YES', 'UPDATE', 'sys', 'MADT,MAGV,VAITRO', 'QLDT_THAMGIA');

-- Cấp role cho user (with grant option)
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
    DBMS_OUTPUT.PUT_LINE('Grant priviledges succesfully');
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
        RAISE;
END;
/
-- EXECUTE grantRoles('NVPDT', 'NV0002', 'NO');
-- thu hồi quyền của User/Role
CREATE OR REPLACE PROCEDURE RevokePrivilegesOfUserOnSpecificObjectType(
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
CREATE OR REPLACE PROCEDURE RevokeSystemPrivilegesFromUser(
    privilege_ IN VARCHAR2,    -- Quyền cần thu hồi
    name_ IN VARCHAR2          -- Tên user/role muốn thu hồi quyền
)
AS
BEGIN
    EXECUTE IMMEDIATE 'REVOKE ' || privilege_ || ' FROM ' || name_;
    DBMS_OUTPUT.PUT_LINE('Revoke system privilege successfully from' || name_ );
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE RevokePrivilegesFromUserOnSpecificColumnsOfTableOrView(
    p_privilege IN VARCHAR2,
    p_user    IN VARCHAR2,
    p_columns IN VARCHAR2,
    p_object  IN VARCHAR2 
)
AS
    sqlQuery VARCHAR2(4000);
BEGIN
    sqlQuery := 'REVOKE ' || p_privilege || ' (' || p_columns || ') ON ' || p_object || ' FROM ' || p_user;
    DBMS_OUTPUT.PUT_LINE('Executing SQL: ' || sqlQuery);

    EXECUTE IMMEDIATE sqlQuery;
    DBMS_OUTPUT.PUT_LINE('Revoke privileges on specific columns successfully.');
EXCEPTION
    WHEN OTHERS THEN
       RAISE;
END;
/

-- EXECUTE REVOKEPRIVILEGESFROMUSER('SELECT', 'NV0001', 'NHANVIEN')
-- thu hồi role khỏi user
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
-- EXECUTE REVOKEROLEFROMUSER('NVPDT', 'NV0002');
-- Cấp quyền ở mức cột đối với SELECT,UPDATE tức là tạo view cho user hoặc role đó 
-- và cấp quyền trên view đó cho role hoặc user

-- Hàm tạo view
CREATE OR REPLACE PROCEDURE createView(
    view_name IN VARCHAR2,    
    select_columns IN VARCHAR2,
    object_ IN VARCHAR2,
    condition_ IN VARCHAR2
)
AS
BEGIN
    IF condition_ = 'NO' THEN
    EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || view_name || ' AS '
                || 'SELECT ' || select_columns || ' FROM ' || object_;
    ELSE 
        EXECUTE IMMEDIATE 'CREATE OR REPLACE VIEW ' || view_name || ' AS '
                || 'SELECT ' || select_columns || ' FROM ' || object_ || ' WHERE ' || condition_; 
    DBMS_OUTPUT.PUT_LINE('View ' || view_name || ' created successfully');
    END IF;
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR: ' || SQLERRM);
END;
/
-- EXECUTE createView('view_NVCB_NV', '*', 'NHANVIEN', 'MANV = SYS_CONTEXT(''USERENV'', ''CURRENT_USER'')')

-- Lấy các quyền mà user hoặc role đang có trên các đối tượng dữ liệu
CREATE OR REPLACE PROCEDURE getPrivilegesOnObjectType(
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

CREATE OR REPLACE PROCEDURE GetSystemPrivilegesOfUser(
    name_ IN VARCHAR2,  
    result_ OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN result_ FOR
    SELECT *
    FROM DBA_SYS_PRIVS 
    WHERE GRANTEE = UPPER(name_);
    EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('ERROR:' || SQLERRM);
END;
/
CREATE OR REPLACE PROCEDURE GetColumnPrivilegesOfUser(
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
-- VAR v_roles REFCURSOR;
-- EXECUTE getPrivilegesOnObjectType( 'NVCB', 'VIEW' ,:v_roles);
-- PRINT v_roles;

-- lấy các cột của 1 table/view 
CREATE OR REPLACE PROCEDURE getColumns(
    object_name IN VARCHAR2, 
    result OUT SYS_REFCURSOR
) 
AS
BEGIN
    OPEN result FOR
        SELECT COLUMN_NAME
        FROM DBA_TAB_COLUMNS
        WHERE TABLE_NAME = UPPER(object_name)
        ORDER BY COLUMN_ID;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        OPEN result FOR SELECT 'Table not found' AS COLUMN_NAME FROM DUAL;
    WHEN OTHERS THEN
        OPEN result FOR SELECT 'Error: ' AS COLUMN_NAME FROM DUAL;
END;
/
-- VAR v_roles REFCURSOR;
-- EXECUTE getColumns( 'NHANVIEN' ,:v_roles);
-- PRINT v_roles;

CREATE OR REPLACE PROCEDURE GetAllInstanceOfSpecificObject(
    type IN VARCHAR2, 
    result_ OUT SYS_REFCURSOR
)
AS
BEGIN
    DBMS_OUTPUT.PUT_LINE('>> Đã nhận tham số object_type: ' || UPPER(type));

    OPEN result_ FOR
    SELECT *
    FROM DBA_OBJECTS
    WHERE OBJECT_TYPE = UPPER(type);

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
END;
/

COMMIT;
CREATE OR REPLACE PROCEDURE getAllUsers(user_list OUT SYS_REFCURSOR)
AS
BEGIN
    OPEN user_list FOR
    SELECT * FROM DBA_USERS;
END;
/

COMMIT;

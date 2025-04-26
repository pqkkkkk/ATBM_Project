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
    full_user_name VARCHAR2(100);
BEGIN
    full_user_name := 'X_' || user_name;

    EXECUTE IMMEDIATE 'CREATE USER ' || full_user_name || ' IDENTIFIED BY "' || pwd || '"';

    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || full_user_name;
    EXECUTE IMMEDIATE 'GRANT CONNECT TO ' || full_user_name;
    EXECUTE IMMEDIATE 'GRANT RESOURCE TO ' || full_user_name;
    DBMS_OUTPUT.PUT_LINE('User ' || full_user_name || ' created successfully.');
    
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

CREATE OR REPLACE PROCEDURE X_ADMIN_getAllUsers(user_list OUT SYS_REFCURSOR)
AS
BEGIN
    OPEN user_list FOR
    SELECT * FROM all_users
    WHERE username LIKE 'X\_%' ESCAPE '\';
END;
/

COMMIT;
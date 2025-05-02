-- select all tables which this user created
SELECT * FROM ALL_ALL_TABLES WHERE OWNER = 'SYS' ORDER BY TABLE_NAME;

-- select all users
SELECT * FROM ALL_USERS;

-- select all views
SELECT VIEW_NAME FROM ALL_VIEWS WHERE OWNER = 'c##admin';
-- select all stored procedures
SELECT OBJECT_NAME FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'PROCEDURE' AND OWNER = 'c##admin';

-- select all users (only for dba)
SELECT username FROM dba_users ORDER BY username;

-- select all tables which this user created (only for dba)
SELECT * FROM dba_tables WHERE  OWNER = 'C##ADMIN1';
SELECT * FROM user_tables WHERE table_name = 'NHANVIEN';
-- select all role which this user created (only for dba)
SELECT * FROM dba_roles WHERE ORACLE_MAINTAINED = 'N' ORDER BY role;

-- select all role which this user has (only for dba)
SELECT * FROM user_role_privs;

-- select all privileges which this user has
SELECT *
FROM user_sys_privs WHERE privilege LIKE '%ANY%';
--  select all privileges which this user has on tables
SELECT * FROM user_tab_privs;

SELECT * FROM user_col_privs;
-- select all privileges which specific role has
SELECT *
FROM role_tab_privs WHERE OWNER = 'C##ADMIN1';


-- DBA Views
SELECT * FROM DBA_USERS;
SELECT * FROM DBA_SYS_PRIVS;
SELECT * FROM DBA_ROLES;
SELECT * FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'X_SV001';
SELECT * FROM DBA_TAB_PRIVS WHERE TABLE_NAME = 'DBA_TAB_PRIVS';
SELECT * FROM ROLE_TAB_PRIVS;
SELECT * FROM DBA_COL_PRIVS;
-- get all procedures of a user
SELECT * FROM DBA_OBJECTS WHERE OBJECT_TYPE = 'PROCEDURE' AND OBJECT_NAME LIKE '%Privilege%';
SELECT * FROM v$database;
SELECT * FROM v$pdbs;

-- Query metadata of X_ADMIN schema
    -- Tables 
    SELECT *
    FROM   all_tables
    WHERE  owner = 'X_ADMIN';
    -- Views
    SELECT view_name
    FROM   all_views
    WHERE  owner = 'X_ADMIN';

    -- Procedures and Functions
    SELECT *
    FROM   all_objects
    WHERE  owner = 'X_ADMIN'
    AND  object_type IN ('PROCEDURE','FUNCTION','PACKAGE');

    -- Roles
    SELECT *
    FROM   user_role_privs;
    SELECT * 
    FROM ROLE_TAB_PRIVS;
    SELECT * FROM all_users;

SELECT * from X_ADMIN.SINHVIEN;
SELECT * from X_ADMIN.DANGKY;
SELECT * from X_ADMIN.MOMON;
SELECT * from X_ADMIN.NHANVIEN;
SELECT * FROM X_ADMIN.DONVI;
SELECT * FROM SESSION_ROLES;


DECLARE
    isSV INTEGER;
    isGV INTEGER;
    username VARCHAR2(10);
BEGIN
    username := SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
    DBMS_OUTPUT.PUT_LINE('username: ' || username);
    isSV:= SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_SV');
    DBMS_OUTPUT.PUT_LINE('isSV: ' || isSV);
    isGV:= SYS_CONTEXT('X_UNIVERITY_CONTEXT','IS_GV');
    DBMS_OUTPUT.PUT_LINE('isGV: ' || isGV);
END;
/

SELECT * FROM SESSION_ROLEs;
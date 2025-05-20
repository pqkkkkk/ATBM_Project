CREATE OR REPLACE PROCEDURE X_ADMIN_GetPrivilegesOfUserOnSpecificObjectTypeThrouthRole(
    p_object_type IN VARCHAR2,
    p_user_role   IN VARCHAR2,
    v_result      OUT SYS_REFCURSOR
)
AUTHID CURRENT_USER
AS
BEGIN
  OPEN v_result FOR
    'SELECT * 
    FROM ROLE_TAB_PRIVS
    WHERE OWNER = :1'
    USING 'X_ADMIN';
EXCEPTION
  WHEN OTHERS THEN
    DBMS_OUTPUT.PUT_LINE('Lỗi: ' || SQLERRM);
END;
/


GRANT EXECUTE ON X_ADMIN_GetPrivilegesOfUserOnSpecificObjectTypeThrouthRole TO PUBLIC;

CREATE OR REPLACE PROCEDURE X_ADMIN_GetTextOfView(
    p_view_name IN VARCHAR2,
    v_result OUT CLOB
)
AS
    temp LONG;
BEGIN
    SELECT TEXT
    INTO temp
    FROM ALL_VIEWS
    WHERE VIEW_NAME = p_view_name AND OWNER = 'X_ADMIN';
    v_result := temp;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        v_result := NULL;
    WHEN OTHERS THEN
        v_result := NULL;
END;
/
GRANT EXECUTE ON X_ADMIN_GetTextOfView TO PUBLIC;
COMMit;

DECLARE
    v_result CLOB;
BEGIN
    X_ADMIN.X_ADMIN_GETTEXTOFVIEW('VIEW_NVCB_NV', v_result);
    DBMS_OUTPUT.PUT_LINE('Text of view: ' || v_result);
END;
/

COMMIT;
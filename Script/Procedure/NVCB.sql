--Tạo procedure để cập nhật trường dt trên View_NVCB
    CREATE OR REPLACE PROCEDURE X_ADMIN_Update_NHANVIEN_ForNVCB(
        NEWDT IN VARCHAR2,
        username IN VARCHAR2,
        ROW_AFFECTED OUT INTEGER
    )
    AS
        CURRENT_USERNAME VARCHAR2(10);
    BEGIN
        CURRENT_USERNAME := SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'USER_NAME');
        IF CURRENT_USERNAME != username THEN
            RAISE_APPLICATION_ERROR(-20001, 'You do not have permission to update this record.');
            RETURN;
        END IF;
        UPDATE X_ADMIN.view_NVCB_NV
        SET DT = NEWDT
        WHERE MANV = SYS_CONTEXT('X_UNIVERITY_CONTEXT', 'USER_NAME');
        ROW_AFFECTED := SQL%ROWCOUNT;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
            RAISE;
        -- Gán số dòng bị ảnh hưởng
    END X_ADMIN_Update_NHANVIEN_ForNVCB;
    /
    --GÁN QUYỀN CHO NVCB
    GRANT EXECUTE ON X_ADMIN_Update_NHANVIEN_ForNVCB TO XR_NVCB;
-- SELECT tren bang NHANVIEN
    CREATE OR REPLACE PROCEDURE X_ADMIN_Select_NHANVIEN_ForNVCB(
        p_result OUT SYS_REFCURSOR
    )
    AS
    BEGIN
        OPEN p_result FOR
        SELECT * FROM X_ADMIN.view_NVCB_NV;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
            RAISE;
    END;
    /
    GRANT EXECUTE ON X_ADMIN_Select_NHANVIEN_ForNVCB TO XR_NVCB;
COMMIT;
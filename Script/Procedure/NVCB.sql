--Tạo procedure để cập nhật trường dt trên View_NVCB
    CREATE OR REPLACE PROCEDURE X_ADMIN_update_DT(
        NEWDT IN VARCHAR2,
        ROW_AFFECTED OUT INTEGER
    )
    AS
    BEGIN
        UPDATE X_ADMIN.view_NVCB_NV
        SET DT = NEWDT
        WHERE MANV = SYS_CONTEXT('X_UNIVERSITY_CONTEXT', 'USER_NAME');

        -- Gán số dòng bị ảnh hưởng
        ROW_AFFECTED := SQL%ROWCOUNT;
    END X_ADMIN_update_DT;
    /

    --GÁN QUYỀN CHO NVCB
    GRANT EXECUTE ON X_ADMIN_update_DT TO XR_NVCB;

COMMIT;
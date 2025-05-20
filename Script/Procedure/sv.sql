-- Select trên bảng SINHVIEN
    CREATE OR REPLACE PROCEDURE X_ADMIN_Select_SINHVIEN_Table_ForSV(
        p_result OUT SYS_REFCURSOR
    )
        AS
        BEGIN
            OPEN p_result FOR
            SELECT * FROM X_ADMIN.SINHVIEN;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
                RAISE;
        END;
        /
    GRANT EXECUTE ON X_ADMIN_Select_SINHVIEN_Table_ForSV TO PUBLIC;
-- Update trên bảng SINHVIEN
    CREATE OR REPLACE PROCEDURE X_ADMIN_Update_SINHVIEN_Table_ForSV(
        p_dChi IN VARCHAR2,
        p_dt IN VARCHAR2
    )
    AS
    BEGIN
        UPDATE X_ADMIN.SINHVIEN
        SET DCHI = p_dChi, DT = p_dt;
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
            RAISE;
    END;
    /
    GRANT EXECUTE ON X_ADMIN_Update_SINHVIEN_Table_ForSV TO PUBLIC;
-- SELECT trên bảng DANGKY
    CREATE OR REPLACE PROCEDURE X_ADMIN_Select_DANGKY_Table_ForSV(
        p_result OUT SYS_REFCURSOR
    )
        AS
        BEGIN
            OPEN p_result FOR
            SELECT * FROM X_ADMIN.DANGKY;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
                RAISE;
        END;
        /
    GRANT EXECUTE ON X_ADMIN_Select_DANGKY_Table_ForSV TO PUBLIC;

-- INSERT trên bảng DANGKY
    CREATE OR REPLACE PROCEDURE X_ADMIN_Insert_DANGKY_Table_ForSV(
        p_maSV IN VARCHAR2,
        p_mamm IN VARCHAR2
    )
        AS
        BEGIN
            INSERT INTO X_ADMIN.DANGKY (MASV, MAMM)
            VALUES (p_maSV, p_mamm);
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
                RAISE;
        END;
        /
    GRANT EXECUTE ON X_ADMIN_Insert_DANGKY_Table_ForSV TO PUBLIC;

-- SELECT trên bảng MOMON
    CREATE OR REPLACE PROCEDURE X_ADMIN_Select_MOMON_Table_ForSV(
        p_result OUT SYS_REFCURSOR
    )
        AS
        BEGIN
            OPEN p_result FOR
            SELECT * FROM X_ADMIN.view_SV_MOMON;
        EXCEPTION
            WHEN OTHERS THEN
                DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
                RAISE;
        END;
        /
    GRANT EXECUTE ON X_ADMIN_Select_MOMON_Table_ForSV TO PUBLIC;

COMMIT;
-- Tạo view trên tất cả các thuộc tính của bảng MOMON các dòng liên quan đến chính bản thân mình cho vai trò GV 
    CREATE OR REPLACE VIEW view_GV_MOMON AS 
    SELECT * FROM X_ADMIN.MOMON where MaGV = SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME');
    -- Cấp quyền SELECT trên view cho GV
    GRANT SELECT ON X_ADMIN.view_GV_MOMON TO XR_GV;


-- Hàm tạo view trên bảng MOMON của tất cá các môn thuộc học kì hiện tại của năm học đang diễn ra
    CREATE OR REPLACE VIEW view_PDT_MOMON AS
    SELECT * FROM X_ADMIN.MOMON
    WHERE EXTRACT(YEAR FROM SYSDATE) = NAM
    AND HK = CASE
    WHEN EXTRACT(MONTH FROM SYSDATE) BETWEEN 9 AND 1 THEN 1
    WHEN EXTRACT(MONTH FROM SYSDATE) BETWEEN 2 AND 6 THEN 2
    ELSE 3
    END;
    -- Cấp quyền SELECT, INSERT, UPDATE, DELETE trên view này dành cho NV PDT
    GRANT SELECT, UPDATE, INSERT, DELETE ON X_ADMIN.view_PDT_MOMON TO XR_NVPDT;


-- Tạo view xem phân công giảng dạy của các GV thuộc đơn vị của mình cho TRGDV
    CREATE OR REPLACE VIEW view_TRGDV_MOMON AS 
    SELECT m.* FROM X_ADMIN.MOMON m JOIN NHANVIEN nv ON m.MAGV = nv.MANV
    WHERE nv.MADV IN (
        SELECT nv2.MADV             
        FROM X_ADMIN.NHANVIEN nv2 
        WHERE nv2.MANV = SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME')
    );
    -- Cấp quyền SELECT trên view
    GRANT SELECT ON view_TRGDV_MOMON TO XR_TRGDV;


-- Tạo view gồm tất cả các mở môn thuộc khoa của sinh viên đang theo học
    CREATE OR REPLACE VIEW  view_SV_MOMON AS
    SELECT m.*, hp.TENHP, hp.MADV, hp.SOTC, hp.STLT, hp.STTH 
    FROM X_ADMIN.MOMON m JOIN HOCPHAN hp ON m.MAHP = hp.MAHP
    WHERE hp.MADV IN (
        SELECT KHOA 
        FROM X_ADMIN.SINHVIEN 
        WHERE MASV = SYS_CONTEXT('X_UNIVERITY_CONTEXT','USER_NAME')
    ) ;
    -- Cấp quyền SELECT trên view cho SV
    GRANT SELECT ON view_SV_MOMON TO XR_SV;

COMMIT;

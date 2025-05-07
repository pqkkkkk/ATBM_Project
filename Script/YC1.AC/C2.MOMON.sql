-- Tạo view trên tất cả các thuộc tính của bảng MOMON các dòng liên quan đến chính bản thân mình cho vai trò GV 
CREATE OR REPLACE VIEW view_GV_MOMON AS
SELECT * FROM MOMON 
WHERE MAGV = SYS_CONTEXT('USERENV', 'SESSION_USER');
-- Cấp quyền SELECT trên view cho GV
GRANT SELECT ON view_GV_MOMON TO XR_GV;

-- Hàm tạo view trên bảng MOMON của tất cá các môn thuộc học kì hiện tại của năm học đang diễn ra
CREATE OR REPLACE VIEW view_PDT_MOMON AS
SELECT * FROM MOMON
WHERE EXTRACT(YEAR FROM SYSDATE) = NAM 
AND HK = CASE 
    WHEN EXTRACT(MONTH FROM SYSDATE) BETWEEN 9 AND 12 THEN 1
    WHEN EXTRACT(MONTH FROM SYSDATE) BETWEEN 1 AND 4 THEN 2
    ELSE 3
    END;

-- Cấp quyền SELECT, INSERT, UPDATE, DELETE trên view này dành cho NV PDT
GRANT SELECT, UPDATE, INSERT, DELETE ON view_PDT_MOMON TO XR_NVPDT;

-- Tạo view xem phân công giảng dạy của các GV thuộc đơn vị của mình cho TRGDV
CREATE OR REPLACE VIEW view_TRGDV_MOMON AS 
SELECT m.* FROM MOMON m JOIN NHANVIEN nv ON m.MAGV = nv.MANV
WHERE nv.MADV IN (
    SELECT nv2.MADV             
    FROM NHANVIEN nv2 
    WHERE nv2.MANV = SUBSTR(SYS_CONTEXT('USERENV','SESSION_USER'), INSTR(SYS_CONTEXT('USERENV','SESSION_USER'), '_') + 1));
-- Cấp quyền SELECT trên view
GRANT SELECT ON view_TRGDV_MOMON TO XR_TRGDV;

-- Tạo view gồm tất cả các mở môn thuộc khoa của sinh viên đang theo học
CREATE OR REPLACE VIEW  view_SV_MOMON AS
SELECT m.*, hp.TENHP, hp.MADV, hp.SOTC, hp.STLT, hp.STTH 
FROM MOMON m JOIN HOCPHAN hp ON m.MAHP = hp.MAHP
WHERE hp.MADV IN (
    SELECT KHOA 
    FROM SINHVIEN 
    WHERE MASV = SUBSTR(SYS_CONTEXT('USERENV','SESSION_USER'), INSTR(SYS_CONTEXT('USERENV','SESSION_USER'), '_') + 1)
) ;
-- Cấp quyền SELECT trên view cho SV
GRANT SELECT ON view_SV_MOMON TO XR_SV;

COMMIT;
-- SELECT SYS_CONTEXT('USERENV', 'SESSION_USER') FROM dual; -- Xem người dùng hiện tại là ai
-- SELECT SYS_CONTEXT('USERENV', 'CURRENT_SCHEMA') FROM dual; -- Xem schema hiện tại

-- TEST
-- NV0001 - NVPDT
-- NV0005 - NVTCHC
-- NV0009 - GV
-- NV0018 - TRGDV
-- SV0001 - CNTT

-- Xem ROLE
-- SELECT GRANTEE, GRANTED_ROLE FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'NV0001';
-- SELECT GRANTEE, GRANTED_ROLE FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'NV0005';
-- SELECT GRANTEE, GRANTED_ROLE FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'NV0009';
-- SELECT GRANTEE, GRANTED_ROLE FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'NV0018';
-- SELECT GRANTEE, GRANTED_ROLE FROM DBA_ROLE_PRIVS WHERE GRANTEE = 'SV0001';
-------------------------------------------
-- -- Giáo viên xem đươc các MOMON của chính mình 
-- SELECT * FROM sys.view_GV_MOMON;

-- -- PDT xem được các mở môn của hiện tại
-- SELECT * FROM sys.view_PDT_MOMON;

-- -- PDT có quyền INSERT, DELETE, UPDATE MOMON
-- INSERT INTO sys.view_PDT_MOMON (MAMM, MAHP, MAGV, HK, NAM) VALUES
-- ('MM012', 'HP011', 'NV0009', 2, 2025);
-- UPDATE SYS.view_PDT_MOMON SET MAHP = 'HP011' WHERE MAMM = 'MM012';
-- DELETE FROM sys.view_PDT_MOMON WHERE MAMM = 'MM012';

-- -- TRGDV xem được mở môn của các NV thuộc đơn vị của mình
-- SELECT * FROM sys.view_TRGDV_MOMON;

-- -- SV xem được các mở môn của khoa mình
-- SELECT * FROM sys.view_SV_MOMON;

-- Cấp quyền execute cho X_ADMIN
GRANT EXECUTE ON LBACSYS.SA_COMPONENTS TO X_ADMIN WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_user_admin TO X_ADMIN WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_label_admin TO X_ADMIN WITH GRANT OPTION;
GRANT EXECUTE ON sa_policy_admin TO X_ADMIN WITH GRANT OPTION;
GRANT EXECUTE ON char_to_label TO X_ADMIN WITH GRANT OPTION;
GRANT INHERIT PRIVILEGES ON USER LBACSYS TO X_ADMIN WITH GRANT OPTION;
GRANT LBAC_DBA TO X_ADMIN;
GRANT EXECUTE ON sa_sysdba TO X_ADMIN;
GRANT EXECUTE ON TO_LBAC_DATA_LABEL TO X_ADMIN;


BEGIN
 SA_SYSDBA.CREATE_POLICY (
  policy_name      => 'NOTIFICATION_POLICY',
  column_name      => 'LABEL'
);
END;
/
EXEC SA_SYSDBA.ENABLE_POLICY ('NOTIFICATION_POLICY'); -- Khởi động lại SQL dev

-- Tạo label cho policy notification_policy
-- Tạo level
BEGIN
  SA_COMPONENTS.CREATE_LEVEL('NOTIFICATION_POLICY', 100, 'TRGDV' , 'TRUONGDONVI');
  SA_COMPONENTS.CREATE_LEVEL('NOTIFICATION_POLICY', 80, 'NV','NHANVIEN');
  SA_COMPONENTS.CREATE_LEVEL('NOTIFICATION_POLICY', 60, 'SV','SINHVIEN');
END;
/
-- Tạo compartment
BEGIN
  SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 1,'T','TOAN');
  SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 2, 'L','LY');
  SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 3, 'H','HOA');
  SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 4, 'HC','HANHCHINH');
END;
/
-- Tạo group
BEGIN
  SA_COMPONENTS.CREATE_GROUP('NOTIFICATION_POLICY', 10, 'CS1', 'COSO1');
  SA_COMPONENTS.CREATE_GROUP('NOTIFICATION_POLICY', 20, 'CS2', 'COSO2');
END;
/

SELECT * FROM DBA_SA_LEVELS;
SELECT * FROM DBA_SA_COMPARTMENTS;
SELECT * FROM DBA_SA_GROUPS;

-- Tạo nhãn cho policy notification_policy

BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '1',
  label_value     => 'TRGDV',
  data_label      => TRUE);
END;
/
-- nhãn t2: cần phát tán đến tất cả nhân viên
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '2',
  label_value     => 'NV',
  data_label      => TRUE);
END;
/
-- nhãn t3: cần phát tán đến tất cả sinh viên
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '3',
  label_value     => 'SV',
  data_label      => TRUE);
END;
/
-- nhãn t4: Sinh viên thuộc khoa Hóa cs1
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '4',
  label_value     => 'SV:H:CS1',
  data_label      => TRUE);
END;
/
-- nhãn t5: Sinh viên thuộc khoa Hóa cs2
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '5',
  label_value     => 'SV:H:CS2',
  data_label      => TRUE);
END;
/
-- nhãn t6: Sinh viên thuộc khoa hóa cả 2 cơ sở
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '6',
  label_value     => 'SV:H:CS1,CS2',
  data_label      => TRUE);
END;
/
-- nhãn t7: tất cả sinh viên cả 2 cơ sở -> đã bao gồm trong t3
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '7',
  label_value     => 'SV::CS1,CS2',
  data_label      => TRUE);
END;
/
-- nhãn t8: trưởng khoa hóa cơ sở 1
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '8',
  label_value     => 'TRGDV:H:CS1',
  data_label      => TRUE);
END;
/
-- nhãn t9: trưởng khoa hóa cơ sở 1 và cơ sở 2
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '9',
  label_value     => 'TRGDV:H:CS1,CS2',
  data_label      => TRUE);
END;
/
SELECT * from DBA_SA_LABELS;

-- CẬP NHẬT NHÃN BẢO MẬT CHO DỮ LIỆU TRONG BẢNG THONGBAO
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV') WHERE MATB = 1;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'NV') WHERE MATB = 2;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV') WHERE MATB = 3;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:H:CS1') WHERE MATB = 4;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:H:CS2') WHERE MATB = 5;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:H:CS1,CS2') WHERE MATB = 6;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV::CS1,CS2') WHERE MATB = 7;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:H:CS1') WHERE MATB = 8;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:H:CS1,CS2') WHERE MATB = 9;
update THONGBAO set LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:HC:CS1,CS2') WHERE MATB = 10;
UPDATE THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'NV:HC:CS1') WHERE MATB = 11;
-- Áp dụng chính sách bảo vệ cho bảng THONGBAO
BEGIN
    SA_POLICY_ADMIN.APPLY_TABLE_POLICY
    (policy_name => 'NOTIFICATION_POLICY',
    schema_name => 'X_ADMIN',
    table_name => 'THONGBAO',
    table_optionS => 'READ_CONTROL');
END;
/
-- Nhãn người dùng
-- u1: TRGDV:T,L,H,HC:CS1,CS2: trưởng đơn vị có thể đọc được tất cả thông báo
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '10',
  label_value     => 'TRGDV:T,L,H,HC:CS1,CS2');
END;
/
-- u2: TRGDV:H:CS2: trưởng đơn vị phụ trách khoa hóa tại cơ sở 2
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '11',
  label_value     => 'TRGDV:H:CS2');
END;
/
-- u3: TRGDV:L:CS2: truong đơn vị phụ trách khoa lý tại cơ sở 2
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '12',
  label_value     => 'TRGDV:L:CS2');
END;
/
-- u4: NV:H:CS2: nhân viên thuộc khoa hóa tại cơ sở 2
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '13',
  label_value     => 'NV:H:CS2');
END;
/
-- u5: SV:H:CS2: sinh viên thuộc khoa hóa tại cơ sở 2 (đã có nhãn)
-- u6: Trưởng đơn vị đọc thông báo hành chính
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '14',
  label_value     => 'TRGDV:HC:CS1,CS2');
END;
/
-- u7: NV:T,L,H,HC:CS1,CS2: NV đọc được thông báo dành cho nhân viên
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '15',
  label_value     => 'NV:T,L,H,HC:CS1,CS2');
END;
/
-- u8: NV:HC:CS1: nhân viên đọc được thông báo hành chính tại cơ sở 1
BEGIN
 SA_LABEL_ADMIN.CREATE_LABEL  (
  policy_name     => 'NOTIFICATION_POLICY',
  label_tag       => '16',
  label_value     => 'NV:HC:CS1');
END;
/

---------------------------- TEST -----------------------------
-- Nhớ tạo người dùng và cấp quyền SELECT cho bảng THONGBAO
-- Test quyền truy cập của người dùng
-- Gán quyền cho NV0019 (đang là TRGDV) quyền đọc được tất cả thông báo (u1)
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0019',
    max_read_label  => 'TRGDV:T,L,H,HC:CS1,CS2',
    def_label       => 'TRGDV:T,L,H,HC:CS1,CS2'
  );
END;
/
CONNECT X_NV0019/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng trưởng khoa Hóa cơ sở 2 (u2)
-- CẤP CHO NV0018 nhãn tương ứng
-- u2: TRGDV:H:CS2: trưởng đơn vị phụ trách khoa hóa tại cơ sở 2
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0018',
    max_read_label  => 'TRGDV:H:CS2',
    def_label       => 'TRGDV:H:CS2'
  );
END;
/

-- Kết nối với X_NV0018 và kiểm tra quyền truy cập
CONNECT X_NV0018/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng trưởng khoa Lý cơ sở 2 (u3)
-- CẤP CHO NV0020 nhãn tương ứng
-- u3: TRGDV:L:CS2: truong đơn vị phụ trách khoa lý tại cơ sở 2
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0020',
    max_read_label  => 'TRGDV:L:CS2',
    def_label       => 'TRGDV:L:CS2'
  );
END;
GRANT SELECT ON X_ADMIN.THONGBAO TO X_NV0020;
-- Đăng nhập vào X_NV0020 và kiểm tra quyền truy cập
CONNECT X_NV0020/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng nhân viên khoa Hóa cơ sở 2 (u4)
-- CẤP CHO NV0012 nhãn tương ứng
-- u4: NV:H:CS2: nhân viên thuộc khoa hóa tại cơ sở 2

BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0012',
    max_read_label  => 'NV:H:CS2',
    def_label       => 'NV:H:CS2'
  );
END;
-- Đăng nhập vào X_NV0012 và kiểm tra quyền truy cập
CONNECT X_NV0012/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng sinh viên khoa Hóa cơ sở 2 (u5)
-- CẤP CHO SV0011 nhãn tương ứng
-- u5: SV:H:CS2: sinh viên thuộc khoa hóa tại cơ sở 2 (đã có nhãn)

BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_SV0011',
    max_read_label  => 'SV:H:CS2',
    def_label       => 'SV:H:CS2'
  );
END;
-- Đăng nhập vào X_SV0011 và kiểm tra quyền truy cập
CONNECT X_SV0011/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng trưởng đơn vị đọc thông báo hành chính (u6)
-- CẤP CHO NV0021 nhãn tương ứng
-- u6: TRGDV:HC:CS1,CS2: trưởng đơn vị đọc thông báo hành chính tại cơ sở 1 và 2
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0021',
    max_read_label  => 'TRGDV:HC:CS1,CS2',
    def_label       => 'TRGDV:HC:CS1,CS2'
  );
END;
-- Đăng nhập vào X_NV0021 và kiểm tra quyền truy cập
CONNECT X_NV0021/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng nhân viên đọc được thông báo dành cho nhân viên (u7)
-- CẤP CHO NV0014 nhãn tương ứng
-- u7: NV:T,L,H,HC:CS1,CS2: NV đọc được thông báo dành cho nhân viên
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0014',
    max_read_label  => 'NV:T,L,H,HC:CS1,CS2',
    def_label       => 'NV:T,L,H,HC:CS1,CS2'
  );
END;
-- Đăng nhập vào X_NV0014 và kiểm tra quyền truy cập
CONNECT X_NV0014/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;

-- Test quyền của người dùng nhân viên đọc được thông báo hành chính tại cơ sở 1 (u8)
-- CẤP CHO NV0013 nhãn tương ứng
-- u8: NV:HC:CS1: nhân viên đọc được thông báo hành chính tại cơ sở 1
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0013',
    max_read_label  => 'NV:HC:CS1',
    def_label       => 'NV:HC:CS1'
  );
END;
-- Đăng nhập vào X_NV0013 và kiểm tra quyền truy cập
CONNECT X_NV0013/123@localhost:11521/ORCLPDB1;
SELECT MATB, NOIDUNG FROM X_ADMIN.THONGBAO;


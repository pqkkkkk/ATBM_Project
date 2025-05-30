-- Tạo policy notification_policy áp dụng cho bảng THONGBAO
  CONNECT x_admin/123@localhost:1521/XEPDB1;
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
      SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 1,'TOAN','KHOA TOAN');
      SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 2, 'VLY','KHOA VAT LY');
      SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 3, 'HOA','KHOA HOA');
      SA_COMPONENTS.CREATE_COMPARTMENT('NOTIFICATION_POLICY', 4, 'HC','HANHCHINH');
    END;
    /
  -- Tạo group
    BEGIN
      SA_COMPONENTS.CREATE_GROUP('NOTIFICATION_POLICY', 10, 'CS1', 'COSO1');
      SA_COMPONENTS.CREATE_GROUP('NOTIFICATION_POLICY', 20, 'CS2', 'COSO2');
    END;
    /

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
    label_value     => 'SV:HOA:CS1',
    data_label      => TRUE);
  END;
  /
  -- nhãn t5: Sinh viên thuộc khoa Hóa cs2
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '5',
    label_value     => 'SV:HOA:CS2',
    data_label      => TRUE);
  END;
  /
  -- nhãn t6: Sinh viên thuộc khoa hóa cả 2 cơ sở
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '6',
    label_value     => 'SV:HOA:CS1,CS2',
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
    label_value     => 'TRGDV:HOA:CS1',
    data_label      => TRUE);
  END;
  /
  -- nhãn t9: trưởng khoa hóa cơ sở 1 và cơ sở 2
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '9',
    label_value     => 'TRGDV:HOA:CS1,CS2',
    data_label      => TRUE);
  END;
  /

  -- Nhãn người dùng
  -- u1: TRGDV:T,L,H,HC:CS1,CS2: trưởng đơn vị có thể đọc được tất cả thông báo
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '10',
    label_value     => 'TRGDV:TOAN,VLY,HOA,HC:CS1,CS2');
  END;
  /
  -- u2: TRGDV:H:CS2: trưởng đơn vị phụ trách khoa hóa tại cơ sở 2
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '11',
    label_value     => 'TRGDV:HOA:CS2');
  END;
  /
  -- u3: TRGDV:L:CS2: truong đơn vị phụ trách khoa lý tại cơ sở 2
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '12',
    label_value     => 'TRGDV:VLY:CS2');
  END;
  /
  -- u4: NV:H:CS2: nhân viên thuộc khoa hóa tại cơ sở 2
  BEGIN
  SA_LABEL_ADMIN.CREATE_LABEL  (
    policy_name     => 'NOTIFICATION_POLICY',
    label_tag       => '13',
    label_value     => 'NV:HOA:CS2');
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
    label_value     => 'NV:TOAN,VLY,HOA,HC:CS1,CS2');
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

-- CẬP NHẬT NHÃN BẢO MẬT CHO DỮ LIỆU TRONG BẢNG THONGBAO
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV') WHERE MATB = 1;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'NV') WHERE MATB = 2;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV') WHERE MATB = 3;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:HOA:CS1') WHERE MATB = 4;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:HOA:CS2') WHERE MATB = 5;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV:HOA:CS1,CS2') WHERE MATB = 6;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'SV::CS1,CS2') WHERE MATB = 7;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:HOA:CS1') WHERE MATB = 8;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:HOA:CS1,CS2') WHERE MATB = 9;
    update X_ADMIN.THONGBAO set LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'TRGDV:HC:CS1,CS2') WHERE MATB = 10;
    UPDATE X_ADMIN.THONGBAO SET LABEL = CHAR_TO_LABEL('NOTIFICATION_POLICY', 'NV:HC:CS1') WHERE MATB = 11;

-- Set admin privileges 
    BEGIN
    SA_USER_ADMIN.SET_USER_PRIVS(
        policy_name     => 'NOTIFICATION_POLICY',
        user_name       => 'X_ADMIN',
        privileges      => 'FULL'
    );
    END;
    /
  -- Áp dụng chính sách bảo vệ cho bảng THONGBAO
  BEGIN
      SA_POLICY_ADMIN.APPLY_TABLE_POLICY
      (policy_name => 'NOTIFICATION_POLICY',
      schema_name => 'X_ADMIN',
      table_name => 'THONGBAO',
      table_optionS => 'READ_CONTROL');
  END;
  /

  COMMIT;
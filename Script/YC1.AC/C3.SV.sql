--Cài VPD cho từng vai trò với thao tác SELECT:
   create or REPLACE function SV_SELECT(
      p_schemma VARCHAR2,
      p_object_name VARCHAR2)
   return VARCHAR2
   AUTHID CURRENT_USER
   as
      username VARCHAR2(10);
      facultyOfTeacher VARCHAR2(10);
      isSV INTEGER;
      isGV INTEGER;
      isNVPDT INTEGER;
      isNVCTSV INTEGER;
      isAdmin INTEGER;
   begin
      username := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','USER_NAME');
      isSV := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_SV');
      isGV := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_GV');
      isNVPDT := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_NVPDT');
      isNVCTSV := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_NVCTSV');
      isAdmin := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_ADMIN');

      IF isAdmin >= 1 then
         RETURN '1=1';
      ELSIF isNVPDT >= 1 or isNVCTSV >= 1 then
         RETURN '1=1';
      ELSIF isGV >= 1 then
         SELECT MADV INTO facultyOfTeacher FROM X_ADMIN.NHANVIEN WHERE MANV = username;
         RETURN 'KHOA = ''' || facultyOfTeacher || '''';
      ELSIF isSV >= 1 then
         RETURN 'MASV = ''' || username || '''';
      ELSE 
         RETURN '1=0';
      END IF;
   end SV_SELECT;
   /
   --Gắn hàm thực hiện chính sách SV_SELECT vào bảng SINHVIEN:
   BEGIN
   DBMS_RLS.ADD_POLICY(
      object_schema   => 'X_ADMIN',
      object_name     => 'SINHVIEN',
      policy_name     => 'SV_SELECT',
      function_schema => 'X_ADMIN',
      policy_function => 'SV_SELECT',
      statement_types => 'SELECT',
      update_check    => TRUE
      );
   END;
   /
   --Cấp quyền SELECT, UPDATE(DT, DCHI) cho SV
   GRANT SELECT, UPDATE(DT, DCHI) ON X_ADMIN.SINHVIEN TO XR_SV;
   COMMIT;


--Cài VPD cho từng vai trò với thao tác UPDATE:
   create or REPLACE function SV_UPDATE
      (p_schema VARCHAR2, p_obj VARCHAR2)
   return VARCHAR2
   as
      username VARCHAR2(10);
      isSV INTEGER;
      isNVPDT INTEGER;
      isNVCTSV INTEGER;
      isAdmin INTEGER;
   begin
      username := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','USER_NAME');
      isSV := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_SV');
      isNVPDT := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_NVPDT');
      isNVCTSV := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_NVCTSV');
      isAdmin := SYS_CONTEXT('X_UNIVERSITY_CONTEXT','IS_ADMIN');
      if isAdmin >= 1 then
         RETURN '1=1';
      ELSIF isSV >= 1 then
         RETURN 'MASV = ''' || username || '''';
      ELSIF isNVPDT >= 1 or isNVCTSV >= 1  then
         return '1=1';
      ELSE
         return '1=0';
      end if;
   end SV_UPDATE;
   /
   --Gắn hàm thực hiện chính sách SV_UPDATE vào bảng SINHVIEN:
   BEGIN
         DBMS_RLS.ADD_POLICY(
         object_schema   => 'X_ADMIN',
         object_name     => 'SINHVIEN',
         policy_name     => 'SV_UPDATE',
         function_schema => 'X_ADMIN',
         policy_function => 'SV_UPDATE',
         statement_types => 'UPDATE',
         update_check    => TRUE
      );
   END;
   /
   
   GRANT SELECT, INSERT, DELETE, UPDATE(MASV, HOTEN, PHAI, NGSINH, DCHI, DT, KHOA, COSO) ON X_ADMIN.SINHVIEN TO XR_NVCTSV;
   GRANT SELECT, UPDATE(TINHTRANG) ON X_ADMIN.SINHVIEN TO XR_NVPDT;
   GRANT SELECT ON X_ADMIN.SINHVIEN TO XR_GV;

   COMMIT;


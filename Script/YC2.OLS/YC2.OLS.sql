-- SELECT VALUE FROM v$option WHERE parameter = 'Oracle Label Security'; 
-- EXEC LBACSYS.CONFIGURE_OLS;
-- EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;

-- Set user labels
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0019',
    max_read_label  => 'TRGDV:TOAN,VLY,HOA,HC:CS1,CS2',
    def_label       => 'TRGDV:TOAN,VLY,HOA,HC:CS1,CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0018',
    max_read_label  => 'TRGDV:HOA:CS2',
    def_label       => 'TRGDV:HOA:CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0020',
    max_read_label  => 'TRGDV:VLY:CS2',
    def_label       => 'TRGDV:VLY:CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0012',
    max_read_label  => 'NV:HOA:CS2',
    def_label       => 'NV:HOA:CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_SV0011',
    max_read_label  => 'SV:HOA:CS2',
    def_label       => 'SV:HOA:CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0021',
    max_read_label  => 'TRGDV:HC:CS1,CS2',
    def_label       => 'TRGDV:HC:CS1,CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0015',
    max_read_label  => 'NV:TOAN,VLY,HOA,HC:CS1,CS2',
    def_label       => 'NV:TOAN,VLY,HOA,HC:CS1,CS2'
  );
END;
/
BEGIN
  SA_USER_ADMIN.SET_USER_LABELS(
    policy_name     => 'NOTIFICATION_POLICY',
    user_name       => 'X_NV0013',
    max_read_label  => 'NV:HC:CS1',
    def_label       => 'NV:HC:CS1'
  );
END;
/

COMMIT;

# X_University_IS - project of the "Data Security in Information Systems" course

## Introduction
X_University_IS is the basic information systems of a university. Manage basic data of a university such as student, employee, course, grade.
- Using MVVM model approach
- UI: window application built with WinUI 3
- Database: Oracle
## Project structure
```
.
├── Script/         # scripts to create database, enforce security policies
├── Source/         # UI source code
├── .gitignore/     
└── README.md    
```
## Main Features
- Sign in to Oracle database
- Admin
    - Manage user and role of system
    - Grant, revoke privileges on tables, views, procedure,...
- User
    - Manage data related to themselves (follow security policies)
- Enforce security policies for users by using RBAC, VPD, OLS.
- Enforce auditing policy.
- Backup and recovery data by using Oracle tools such as datapump, RMAN.

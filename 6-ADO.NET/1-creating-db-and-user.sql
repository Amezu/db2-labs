IF OBJECT_ID('MSSQLSERVER\Lab6user') IS NOT NULL
 EXEC sp_revokelogin 'MSSQLSERVER\Lab6user';
GO
EXEC sp_grantlogin 'MSSQLSERVER\Lab6user';

IF DB_ID('Lab6db') IS NOT NULL
 DROP DATABASE Lab6db;
GO
CREATE DATABASE Lab6db;
GO

USE [Lab6db]
GO
CREATE USER [MSSQLSERVER\Lab6user] FOR LOGIN [MSSQLSERVER\Lab6user]
GO
EXEC sp_addrolemember N'db_owner', N'MSSQLSERVER\Lab6user'
GO
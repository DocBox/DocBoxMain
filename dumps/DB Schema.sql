CREATE DATABASE DX_DOCBOX
ON
PRIMARY (NAME=DocBox_Data, FILENAME='C:\Projects\Databases\DocBox_Data.MDF'),
FILEGROUP DBFS CONTAINS FILESTREAM (NAME=DOCBOX_FS, FILENAME='C:\Projects\DOCBOX_FS')
LOG ON (NAME=DB_LOG,FILENAME='C:\Projects\Databases\Docbox_Log.ldf')
GO

USE DX_DOCBOX
IF OBJECT_ID('DX_USER','U') IS NOT NULL 
	DROP TABLE [DX_USER];
CREATE TABLE [dbo].[DX_USER](
[userid] varchar(100) NOT NULL PRIMARY KEY,
[fname] varchar(100) NOT NULL,
[lname] varchar(100) NOT NULL,
[role] varchar(40) NOT NULL,
[pwdhash] varchar(200) NOT NULL,
[questionid] int NOT NULL,
[phone] varchar(30) NOT NULL,
[anshash] varchar(200) NOT NULL,
[actcodehash] varchar(200) DEFAULT NULL,
[accesslevel] varchar(100) NOT NULL,
[salt] varchar(200) NOT NULL
)

IF OBJECT_ID('DX_FILES','U') IS NOT NULL
	DROP TABLE [DX_FILES];
CREATE TABLE [dbo].[DX_FILES](
 [fileid] bigint NOT NULL PRIMARY KEY IDENTITY(100000,1),
 
 [filename] varchar(100) NOT NULL,
 [parentpath] varchar(300) NOT NULL,
 [ownerid] varchar(100) NOT NULL,
 [size] bigint NOT NULL,
 [isencrypted] varchar(5) NOT NULL,
 [isarchived] varchar(5) NOT NULL,
 [islocked] varchar(5) NOT NULL,
 [lockedby] varchar(100),
 [type] varchar(20) NOT NULL,
 [creationdate] datetime NOT NULL,
 FOREIGN KEY (lockedby) REFERENCES DX_USER(userid),
 FOREIGN KEY (ownerid) REFERENCES DX_USER(userid)
 )
 
 IF OBJECT_ID('DX_PRIVILEGE','U') IS NOT NULL
	DROP TABLE [DX_PRIVILEGE];
 CREATE TABLE [dbo].[DX_PRIVILEGE](
 [privilegeid] bigint NOT NULL PRIMARY KEY IDENTITY(500000,1),
 [fileid] bigint NOT NULL,
 [userid] varchar(100) NOT NULL,
 [read] varchar(5) NOT NULL DEFAULT 'false',
 [write] varchar(5) NOT NULL DEFAULT 'false',
 [update] varchar(5) NOT NULL DEFAULT 'false',
 [check] varchar(5) NOT NULL DEFAULT 'false'
 FOREIGN KEY (fileid) REFERENCES DX_FILES(fileid) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (userid) REFERENCES DX_USER(userid) ON DELETE CASCADE ON UPDATE CASCADE
 )
 
 IF OBJECT_ID('DX_FILEVERSION','U') IS NOT NULL
	DROP TABLE [DX_FILEVERSION];
CREATE TABLE [dbo].[DX_FILEVERSION](
 [version] bigint NOT NULL PRIMARY KEY IDENTITY(500000,1), 
 [fileid] bigint NOT NULL,
 [versionid] UNIQUEIDENTIFIER ROWGUIDCOL NOT NULL UNIQUE,
 [versionnumber] int NOT NULL,
 [updatedate] datetime NOT NULL,
 [description] varchar(300) NOT NULL,
 [size] int NOT NULL,
 [updatedby] varchar(100) NOT NULL,
 [filedata] varbinary(max) FILESTREAM NULL,
 FOREIGN KEY (fileid) REFERENCES DX_FILES(fileid) ON DELETE CASCADE ON UPDATE CASCADE,
 FOREIGN KEY (updatedby) REFERENCES DX_USER(userid) ON DELETE CASCADE ON UPDATE CASCADE
)
 
 IF OBJECT_ID('DX_DEPARTMENT','U') IS NOT NULL
	DROP TABLE [DX_DEPARTMENT];
CREATE TABLE [dbo].[DX_DEPARTMENT](
[deptid] int NOT NULL PRIMARY KEY,
[name] varchar(100) NOT NULL,
)
 
 IF OBJECT_ID('DX_USERDEPT','U') IS NOT NULL
	DROP TABLE [DX_USERDEPT];
CREATE TABLE [dbo].[DX_USERDEPT](
[userdeptid] bigint NOT NULL PRIMARY KEY IDENTITY(500000,1), 
[userid] varchar(100) NOT NULL,
[deptid] int NOT NULL,
FOREIGN KEY (userid) REFERENCES DX_USER(userid) ON DELETE CASCADE ON UPDATE CASCADE,
FOREIGN KEY (deptid) REFERENCES DX_DEPARTMENT(deptid) ON DELETE CASCADE ON UPDATE CASCADE
)
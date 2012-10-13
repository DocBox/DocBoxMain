CREATE DATABASE  IF NOT EXISTS `dxss_docbox` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `dxss_docbox`;
-- MySQL dump 10.13  Distrib 5.5.16, for Win32 (x86)
--
-- Host: 127.0.0.1    Database: dxss_docbox
-- ------------------------------------------------------
-- Server version	5.5.27-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `dx_files`
--

DROP TABLE IF EXISTS `dx_files`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_files` (
  `fileid` bigint(20) NOT NULL AUTO_INCREMENT,
  `filename` varchar(100) NOT NULL,
  `parentpath` varchar(300) NOT NULL,
  `ownerid` varchar(100) NOT NULL,
  `size` int(11) NOT NULL,
  `isencrypted` binary(1) NOT NULL DEFAULT '0',
  `isarchived` binary(1) NOT NULL DEFAULT '0',
  `islocked` binary(1) NOT NULL DEFAULT '0',
  `type` varchar(20) NOT NULL,
  `creationdate` datetime NOT NULL,
  `lockedby` varchar(100) NOT NULL,
  PRIMARY KEY (`fileid`),
  KEY `ownerid_idx` (`ownerid`),
  KEY `lockedby_key_idx` (`lockedby`),
  CONSTRAINT `lockedby_key` FOREIGN KEY (`lockedby`) REFERENCES `dx_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `owner_id_key` FOREIGN KEY (`ownerid`) REFERENCES `dx_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dx_user`
--

DROP TABLE IF EXISTS `dx_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_user` (
  `userid` varchar(100) NOT NULL,
  `fname` varchar(100) NOT NULL,
  `lname` varchar(100) NOT NULL,
  `dob` date NOT NULL,
  `role` varchar(20) NOT NULL,
  `pwdhash` varchar(100) NOT NULL,
  `questionid` int(11) NOT NULL,
  `phone` varchar(30) NOT NULL,
  `anshash` varchar(100) NOT NULL,
  `actcodehash` varchar(100) DEFAULT NULL,
  `accesslevel` int(11) NOT NULL,
  PRIMARY KEY (`userid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dx_privilege`
--

DROP TABLE IF EXISTS `dx_privilege`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_privilege` (
  `privilege_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `fileid` bigint(20) NOT NULL,
  `userid` varchar(100) NOT NULL,
  `read` binary(1) NOT NULL DEFAULT '0',
  `write` binary(1) NOT NULL DEFAULT '0',
  `update` binary(1) NOT NULL DEFAULT '0',
  `check` binary(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`privilege_id`),
  KEY `fileid_priv_key_idx` (`fileid`),
  KEY `user_priv_key_idx` (`userid`),
  CONSTRAINT `fileid_priv_key` FOREIGN KEY (`fileid`) REFERENCES `dx_files` (`fileid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `user_priv_key` FOREIGN KEY (`userid`) REFERENCES `dx_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dx_userdept`
--

DROP TABLE IF EXISTS `dx_userdept`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_userdept` (
  `userid` varchar(100) NOT NULL,
  `deptid` int(11) NOT NULL,
  KEY `user_dept_key_idx` (`userid`),
  KEY `dept_user_key_idx` (`deptid`),
  CONSTRAINT `user_dept_key` FOREIGN KEY (`userid`) REFERENCES `dx_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `dept_user_key` FOREIGN KEY (`deptid`) REFERENCES `dx_department` (`deptid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dx_department`
--

DROP TABLE IF EXISTS `dx_department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_department` (
  `deptid` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`deptid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dx_fileversion`
--

DROP TABLE IF EXISTS `dx_fileversion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dx_fileversion` (
  `fileid` bigint(20) NOT NULL,
  `versionid` bigint(20) NOT NULL AUTO_INCREMENT,
  `versionnumber` int(11) NOT NULL,
  `updatedate` datetime NOT NULL,
  `description` varchar(300) NOT NULL,
  `size` int(11) NOT NULL,
  `updatedby` varchar(100) NOT NULL,
  PRIMARY KEY (`versionid`),
  KEY `fileid_key_idx` (`fileid`),
  KEY `updatedby_key_idx` (`updatedby`),
  CONSTRAINT `fileid_key` FOREIGN KEY (`fileid`) REFERENCES `dx_files` (`fileid`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `updatedby_key` FOREIGN KEY (`updatedby`) REFERENCES `dx_user` (`userid`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2012-10-11 22:56:48

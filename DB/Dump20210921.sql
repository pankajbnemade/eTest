-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: erpdb
-- ------------------------------------------------------
-- Server version	8.0.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `advanceadjustment`
--

DROP TABLE IF EXISTS `advanceadjustment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advanceadjustment` (
  `AdvanceAdjustmentId` int NOT NULL AUTO_INCREMENT,
  `AdvanceAdjustmentNo` varchar(250) DEFAULT NULL,
  `AdvanceAdjustmentDate` datetime DEFAULT NULL,
  `ParticularLedgerId` int DEFAULT NULL,
  `PaymentVoucherId` int DEFAULT NULL,
  `ReceiptVoucherId` int DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AdvanceAdjustmentId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`AdvanceAdjustmentNo`),
  KEY `IX_AdvanceAdjustment_ParticularLedgerId` (`ParticularLedgerId`),
  KEY `IX_AdvanceAdjustment_CompanyId` (`CompanyId`),
  KEY `IX_AdvanceAdjustment_FinancialYearId` (`FinancialYearId`),
  KEY `IX_AdvanceAdjustment_StatusId` (`StatusId`),
  KEY `IX_AdvanceAdjustment_ReceiptVoucherId` (`ReceiptVoucherId`),
  KEY `IX_AdvanceAdjustment_PaymentVoucherId` (`PaymentVoucherId`),
  KEY `tf_idx` (`VoucherStyleId`),
  KEY `FK_AdvanceAdjustment_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_AdvanceAdjustment_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  KEY `FK_AdvanceAdjustment_Currency_CurrencyId_idx` (`CurrencyId`),
  CONSTRAINT `FK_AdvanceAdjustment_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`),
  CONSTRAINT `FK_AdvanceAdjustment_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_PaymentVoucher_CompanyId` FOREIGN KEY (`PaymentVoucherId`) REFERENCES `paymentvoucher` (`PaymentVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_ReceiptVoucher_CompanyId` FOREIGN KEY (`ReceiptVoucherId`) REFERENCES `receiptvoucher` (`ReceiptVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advanceadjustment`
--

LOCK TABLES `advanceadjustment` WRITE;
/*!40000 ALTER TABLE `advanceadjustment` DISABLE KEYS */;
/*!40000 ALTER TABLE `advanceadjustment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advanceadjustmentdetail`
--

DROP TABLE IF EXISTS `advanceadjustmentdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advanceadjustmentdetail` (
  `AdvanceAdjustmentDetId` int NOT NULL AUTO_INCREMENT,
  `AdvanceAdjustmentId` int DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AdvanceAdjustmentDetId`),
  KEY `IX_AdvanceAdjustmentDetails_AdvanceAdjustmentId` (`AdvanceAdjustmentId`) /*!80000 INVISIBLE */,
  KEY `IX_AdvanceAdjustmentDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_AdvanceAdjustmentDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_AdvanceAdjustmentDetails_CreditNoteId` (`CreditNoteId`),
  KEY `IX_AdvanceAdjustmentDetails_DebitNoteId` (`DebitNoteId`),
  KEY `IX_AdvanceAdjustmentDetails_PurchaseInvoiceId` (`PurchaseInvoiceId`),
  KEY `IX_AdvanceAdjustmentDetails_SalesInvoiceId` (`SalesInvoiceId`),
  CONSTRAINT `FK_AdvanceAdjustmentDetails_AdvanceAdj_AdvanceAdjustmentId` FOREIGN KEY (`AdvanceAdjustmentId`) REFERENCES `advanceadjustment` (`AdvanceAdjustmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustmentDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustmentDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustmentDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`),
  CONSTRAINT `FK_AdvanceAdjustmentDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`),
  CONSTRAINT `FK_AdvanceAdjustmentDetails_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`),
  CONSTRAINT `FK_AdvanceAdjustmentDetails_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advanceadjustmentdetail`
--

LOCK TABLES `advanceadjustmentdetail` WRITE;
/*!40000 ALTER TABLE `advanceadjustmentdetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `advanceadjustmentdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` int NOT NULL,
  `RoleId` int NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES (1,'pankajbnemade@gmail.com','PANKAJBNEMADE@GMAIL.COM','pankajbnemade@gmail.com','PANKAJBNEMADE@GMAIL.COM',0,'AQAAAAEAACcQAAAAENmYRZUUmr8yY3aWOHQmN/zSoLiiSuiyNYe6wQuGYxkcqCq+1S1vLE35vrpXa/un/w==','ZP6HXHJ6DNSG5LDNTN665AXIDJVWSSAB','97ea0247-6cba-497d-8e5d-4c1e81c4eada',NULL,0,0,NULL,1,0),(2,'p@g.com','P@G.COM','p@g.com','P@G.COM',0,'AQAAAAEAACcQAAAAEFzZA6pzEfaW1Irk9ovX/53qKVRlxd3u4lbcg85vTjjlqFPFTs5LIdzfzD2YR9RVrg==','C52OPV7UZJ43LO6YKGXYYLCRWVUK3QWI','434b9b5e-265f-493e-bb27-c29ac0094d41',NULL,0,0,NULL,1,0),(3,'admin@gmail.com','ADMIN@GMAIL.COM','admin@gmail.com','ADMIN@GMAIL.COM',0,'AQAAAAEAACcQAAAAEItBQw5Y7S5BMR/qnKKavjY8XJq5wP9tF4uoRY+20NdZgVJSQqo5F7bDbwtRU+HIng==','HM7AKWATLNJLELJEP7ZBTDHCYQBZ2LZW','a4eb8c6a-fd22-4491-9ef1-fd22025419c9',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` int NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chargetype`
--

DROP TABLE IF EXISTS `chargetype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chargetype` (
  `ChargeTypeId` int NOT NULL AUTO_INCREMENT,
  `ChargeTypeName` varchar(250) NOT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ChargeTypeId`),
  KEY `FK_ChargeType_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_ChargeType_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_ChargeType_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ChargeType_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chargetype`
--

LOCK TABLES `chargetype` WRITE;
/*!40000 ALTER TABLE `chargetype` DISABLE KEYS */;
INSERT INTO `chargetype` VALUES (1,'Transportation',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Documentation',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Finance Charges',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Expediting',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'Machining',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `chargetype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `city`
--

DROP TABLE IF EXISTS `city`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `city` (
  `CityId` int NOT NULL AUTO_INCREMENT,
  `CityName` varchar(500) NOT NULL,
  `StateId` int NOT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CityId`),
  KEY `IX_City_StateId` (`StateId`),
  KEY `FK_City_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_City_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_City_State_StateId` FOREIGN KEY (`StateId`) REFERENCES `state` (`StateId`) ON DELETE CASCADE,
  CONSTRAINT `FK_City_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_City_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `city`
--

LOCK TABLES `city` WRITE;
/*!40000 ALTER TABLE `city` DISABLE KEYS */;
INSERT INTO `city` VALUES (2,'Surat1',2,1,'2021-01-01 00:00:00',1,'2021-04-24 19:22:55'),(9,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-26 22:01:54'),(11,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(12,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(13,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(14,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(15,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(16,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(17,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(21,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(28,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(29,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(30,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(31,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(32,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(33,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(34,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(35,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(36,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(37,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(38,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(39,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(40,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-23 21:51:43'),(41,'Surat11',2,1,'2021-04-24 19:23:11',1,'2021-04-24 19:23:11');
/*!40000 ALTER TABLE `city` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `company`
--

DROP TABLE IF EXISTS `company`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `company` (
  `CompanyId` int NOT NULL AUTO_INCREMENT,
  `CompanyName` varchar(250) NOT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `EmailAddress` varchar(100) DEFAULT NULL,
  `Website` varchar(100) DEFAULT NULL,
  `PhoneNo` varchar(20) DEFAULT NULL,
  `AlternatePhoneNo` varchar(20) DEFAULT NULL,
  `FaxNo` varchar(20) DEFAULT NULL,
  `PostalCode` varchar(20) DEFAULT NULL,
  `CurrencyId` int DEFAULT NULL,
  `NoOfDecimals` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CompanyId`),
  KEY `IX_Company_CurrencyId` (`CurrencyId`),
  KEY `FK_Company_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Company_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Company_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Company_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Company_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `company`
--

LOCK TABLES `company` WRITE;
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` VALUES (1,'Company 1','Thane, Mumbai 1','company1@gmail.com','www.compnay1.com','123456789','987654321','23450987','123456',1,2,1,'2021-01-01 00:00:00',1,'2021-08-15 14:52:55'),(3,'Company 2','dubai','dubai@gmail.com','www.dubai.com','344334343',NULL,NULL,NULL,2,2,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `company` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contravoucher`
--

DROP TABLE IF EXISTS `contravoucher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contravoucher` (
  `ContraVoucherId` int NOT NULL AUTO_INCREMENT,
  `VoucherNo` varchar(250) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `ChequeNo` varchar(50) DEFAULT NULL,
  `ChequeDate` datetime DEFAULT NULL,
  `ChequeAmount_FC` decimal(18,4) DEFAULT NULL,
  `ChequeAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) DEFAULT NULL,
  `DebitAmount` decimal(18,4) DEFAULT NULL,
  `CreditAmount_FC` decimal(18,4) DEFAULT NULL,
  `CreditAmount` decimal(18,4) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ContraVoucherId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`VoucherNo`),
  KEY `IX_ContraVoucher_CompanyId` (`CompanyId`),
  KEY `IX_ContraVoucher_CurrencyId` (`CurrencyId`),
  KEY `IX_ContraVoucher_FinancialYearId` (`FinancialYearId`),
  KEY `IX_ContraVoucher_StatusId` (`StatusId`),
  KEY `IX_ContraVoucher_VoucherStyleId` (`VoucherStyleId`),
  KEY `IX_ContraVoucher_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_ContraVoucher_UpdatedByUserId` (`UpdatedByUserId`),
  CONSTRAINT `FK_ContraVoucher_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucher_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contravoucher`
--

LOCK TABLES `contravoucher` WRITE;
/*!40000 ALTER TABLE `contravoucher` DISABLE KEYS */;
/*!40000 ALTER TABLE `contravoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contravoucherdetail`
--

DROP TABLE IF EXISTS `contravoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contravoucherdetail` (
  `ContraVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `ContraVoucherId` int DEFAULT NULL,
  `ParticularLedgerId` int DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) DEFAULT NULL,
  `DebitAmount` decimal(18,4) DEFAULT NULL,
  `CreditAmount_FC` decimal(18,4) DEFAULT NULL,
  `CreditAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ContraVoucherDetId`),
  KEY `IX_ContraVoucherDetails_ContraVoucherId` (`ContraVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_ContraVoucherDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_ContraVoucherDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_ContraVoucherDetails_ParticularLedgerId` (`ParticularLedgerId`),
  CONSTRAINT `FK_ContraVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucherDetails_ContraVoucher_ContraVoucherId` FOREIGN KEY (`ContraVoucherId`) REFERENCES `contravoucher` (`ContraVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ContraVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contravoucherdetail`
--

LOCK TABLES `contravoucherdetail` WRITE;
/*!40000 ALTER TABLE `contravoucherdetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `contravoucherdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `country`
--

DROP TABLE IF EXISTS `country`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `country` (
  `CountryId` int NOT NULL AUTO_INCREMENT,
  `CountryName` varchar(500) NOT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CountryId`),
  UNIQUE KEY `Name_UNIQUE` (`CountryName`),
  KEY `FK_Country_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Country_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Country_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Country_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `country`
--

LOCK TABLES `country` WRITE;
/*!40000 ALTER TABLE `country` DISABLE KEYS */;
INSERT INTO `country` VALUES (1,'INDIA',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'United Arab Emirates',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Oman1',NULL,NULL,NULL,NULL),(4,'KSA',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `country` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnote`
--

DROP TABLE IF EXISTS `creditnote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnote` (
  `CreditNoteId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteNo` varchar(250) DEFAULT NULL,
  `CreditNoteDate` datetime DEFAULT NULL,
  `PartyLedgerId` int DEFAULT NULL,
  `BillToAddressId` int DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `PartyReferenceNo` varchar(250) DEFAULT NULL,
  `PartyReferenceDate` datetime DEFAULT NULL,
  `OurReferenceNo` varchar(250) DEFAULT NULL,
  `OurReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int DEFAULT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) DEFAULT NULL,
  `TotalLineItemAmount` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CreditNoteId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`CreditNoteNo`),
  KEY `IX_CreditNote_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_CreditNote_CompanyId` (`CompanyId`),
  KEY `IX_CreditNote_CurrencyId` (`CurrencyId`),
  KEY `IX_CreditNote_FinancialYearId` (`FinancialYearId`),
  KEY `IX_CreditNote_StatusId` (`StatusId`),
  KEY `IX_CreditNote_PartyLedgerId` (`PartyLedgerId`),
  KEY `IX_CreditNote_TaxRegisterId` (`TaxRegisterId`),
  KEY `tf_idx` (`VoucherStyleId`),
  KEY `FK_CreditNote_Ledger_BillToAddressId_idx` (`BillToAddressId`),
  KEY `FK_CreditNote_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CreditNote_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CreditNote_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_Ledger_BillToAddressId` FOREIGN KEY (`BillToAddressId`) REFERENCES `ledgeraddress` (`AddressId`),
  CONSTRAINT `FK_CreditNote_Ledger_PartyLedgerId` FOREIGN KEY (`PartyLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_TaxRegister_TaxRegisterId` FOREIGN KEY (`TaxRegisterId`) REFERENCES `taxregister` (`TaxRegisterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNote_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnote`
--

LOCK TABLES `creditnote` WRITE;
/*!40000 ALTER TABLE `creditnote` DISABLE KEYS */;
INSERT INTO `creditnote` VALUES (14,'21-01-02-00001','2021-08-31 00:00:00',109,1,3,'534','2021-08-04 00:00:00','344343','2021-08-19 00:00:00',3434,'34343','34343','SubTotal',1,2,0.050000,40.0000,2.0000,41.6000,2.0800,'Percentage',4.0000,1.6000,0.0800,0.0000,0.0000,43.2000,2.1600,'DHM Fourty Three and Two Paisa Only',1,1,1,1,1,NULL,NULL,NULL,NULL),(15,'21-01-03-00002','2021-09-07 00:00:00',111,5,3,'534','2021-09-07 00:00:00','344343','2021-08-19 00:00:00',NULL,NULL,NULL,'SubTotal',NULL,2,0.050428,0.0000,0.0000,0.0000,0.0000,NULL,NULL,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'',1,1,1,2,1,NULL,NULL,NULL,NULL),(16,'21-01-03-00003','2021-09-07 00:00:00',111,5,3,'534','2021-09-07 00:00:00','344343','2021-08-19 00:00:00',NULL,NULL,NULL,'SubTotal',NULL,2,0.050428,0.0000,0.0000,0.0000,0.0000,'Percentage',5.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',1,1,1,3,1,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `creditnote` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnotedetail`
--

DROP TABLE IF EXISTS `creditnotedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnotedetail` (
  `CreditNoteDetId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int DEFAULT NULL,
  `Quantity` decimal(18,2) DEFAULT NULL,
  `PerUnit` int DEFAULT NULL,
  `UnitPrice` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CreditNoteDetId`),
  KEY `IX_CreditNoteDetails_CreditNoteId` (`CreditNoteId`) /*!80000 INVISIBLE */,
  KEY `IX_CreditNoteDetails_UnitOfMeasurementId` (`UnitOfMeasurementId`),
  KEY `FK_CreditNoteDetails_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CreditNoteDetails_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CreditNoteDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetails_UnitOfMeasurement_UnitOfMeasurementId` FOREIGN KEY (`UnitOfMeasurementId`) REFERENCES `unitofmeasurement` (`UnitOfMeasurementId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotedetail`
--

LOCK TABLES `creditnotedetail` WRITE;
/*!40000 ALTER TABLE `creditnotedetail` DISABLE KEYS */;
INSERT INTO `creditnotedetail` VALUES (6,14,1,'rtyrtrty',2,4.00,2,5.0000,40.0000,2.0000,10.8000,0.5400,50.8000,2.5400,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `creditnotedetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnotedetailtax`
--

DROP TABLE IF EXISTS `creditnotedetailtax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnotedetailtax` (
  `CreditNoteDetTaxId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteDetId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CreditNoteDetTaxId`),
  KEY `IX_CreditNoteDetailTax_CreditNoteDetId` (`CreditNoteDetId`),
  KEY `IX_CreditNoteDetailTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_CreditNoteDetailTax_Aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CreditNoteDetailTax_Aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CreditNoteDetailTax_Aspnetusers_aspnetusersUpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetailTax_Aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetailTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PIDetailTax_InvoiceDetail_CreditNoteDetId` FOREIGN KEY (`CreditNoteDetId`) REFERENCES `creditnotedetail` (`CreditNoteDetId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotedetailtax`
--

LOCK TABLES `creditnotedetailtax` WRITE;
/*!40000 ALTER TABLE `creditnotedetailtax` DISABLE KEYS */;
INSERT INTO `creditnotedetailtax` VALUES (22,6,1,105,'Percentage',23.0000,'Add',9.2000,NULL,'33',NULL,NULL,NULL,NULL),(23,6,2,105,'Percentage',4.0000,'Add',1.6000,NULL,'4 re rewer',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `creditnotedetailtax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnotetax`
--

DROP TABLE IF EXISTS `creditnotetax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnotetax` (
  `CreditNoteTaxId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CreditNoteTaxId`),
  KEY `IX_CreditNoteTax_CreditNoteId` (`CreditNoteId`),
  KEY `IX_CreditNoteTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_CreditNoteTax_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CreditNoteTax_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CreditNoteTax_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteTax_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteTax_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotetax`
--

LOCK TABLES `creditnotetax` WRITE;
/*!40000 ALTER TABLE `creditnotetax` DISABLE KEYS */;
INSERT INTO `creditnotetax` VALUES (6,14,1,105,'Percentage',65.0000,'Add',27.0400,NULL,'6565',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `creditnotetax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `currency`
--

DROP TABLE IF EXISTS `currency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `currency` (
  `CurrencyId` int NOT NULL AUTO_INCREMENT,
  `CurrencyCode` varchar(50) DEFAULT NULL,
  `CurrencyName` varchar(250) NOT NULL,
  `Denomination` varchar(50) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CurrencyId`),
  UNIQUE KEY `Name_UNIQUE` (`CurrencyName`),
  KEY `FK_Currency_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Currency_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Currency_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Currency_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currency`
--

LOCK TABLES `currency` WRITE;
/*!40000 ALTER TABLE `currency` DISABLE KEYS */;
INSERT INTO `currency` VALUES (1,'INR','Indian Rupee','Paisa',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'DHM','UAE Dhiram','Paisa',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'ewe','weewew','ewew',1,'2021-08-30 09:09:40',1,'2021-08-30 09:09:40');
/*!40000 ALTER TABLE `currency` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `currencyconversion`
--

DROP TABLE IF EXISTS `currencyconversion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `currencyconversion` (
  `ConversionId` int NOT NULL AUTO_INCREMENT,
  `CompanyId` int DEFAULT NULL,
  `CurrencyId` int DEFAULT NULL,
  `EffectiveDateTime` datetime DEFAULT NULL,
  `ExchangeRate` decimal(18,6) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ConversionId`),
  KEY `IX_CurrencyConversion_CompanyId` (`CompanyId`),
  KEY `IX_CurrencyConversion_CurrencyId` (`CurrencyId`),
  KEY `FK_CurrencyConversion_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CurrencyConversion_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CurrencyConversion_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CurrencyConversion_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CurrencyConversion_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CurrencyConversion_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currencyconversion`
--

LOCK TABLES `currencyconversion` WRITE;
/*!40000 ALTER TABLE `currencyconversion` DISABLE KEYS */;
INSERT INTO `currencyconversion` VALUES (1,1,1,'2021-01-01 00:00:00',1.000000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,1,2,'2021-01-01 00:00:00',0.050428,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `currencyconversion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnote`
--

DROP TABLE IF EXISTS `debitnote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnote` (
  `DebitNoteId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteNo` varchar(250) DEFAULT NULL,
  `DebitNoteDate` datetime DEFAULT NULL,
  `PartyLedgerId` int DEFAULT NULL,
  `BillToAddressId` int DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `PartyReferenceNo` varchar(250) DEFAULT NULL,
  `PartyReferenceDate` datetime DEFAULT NULL,
  `OurReferenceNo` varchar(250) DEFAULT NULL,
  `OurReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int DEFAULT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) DEFAULT NULL,
  `TotalLineItemAmount` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DebitNoteId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`DebitNoteNo`),
  KEY `IX_DebitNote_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_DebitNote_CompanyId` (`CompanyId`),
  KEY `IX_DebitNote_CurrencyId` (`CurrencyId`),
  KEY `IX_DebitNote_FinancialYearId` (`FinancialYearId`),
  KEY `IX_DebitNote_StatusId` (`StatusId`),
  KEY `IX_DebitNote_PartyLedgerId` (`PartyLedgerId`),
  KEY `IX_DebitNote_TaxRegisterId` (`TaxRegisterId`),
  KEY `tf_idx` (`VoucherStyleId`),
  KEY `FK_DebitNote_Ledger_BillToAddressId_idx` (`BillToAddressId`),
  KEY `FK_DebitNote_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_DebitNote_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_DebitNote_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_Ledger_BillToAddressId` FOREIGN KEY (`BillToAddressId`) REFERENCES `ledgeraddress` (`AddressId`),
  CONSTRAINT `FK_DebitNote_Ledger_PartyLedgerId` FOREIGN KEY (`PartyLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_TaxRegister_TaxRegisterId` FOREIGN KEY (`TaxRegisterId`) REFERENCES `taxregister` (`TaxRegisterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNote_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnote`
--

LOCK TABLES `debitnote` WRITE;
/*!40000 ALTER TABLE `debitnote` DISABLE KEYS */;
INSERT INTO `debitnote` VALUES (14,'21-01-04-00001','2021-09-07 00:00:00',109,1,15,'534','2021-09-07 00:00:00','344343','2021-09-20 00:00:00',1,'2 fvfdfddff',NULL,'SubTotal',1,2,0.050000,0.0000,0.0000,0.0000,0.0000,'Percentage',45.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',1,1,1,1,1,NULL,NULL,NULL,NULL),(15,'21-01-04-00002','2021-09-07 00:00:00',111,5,2,'534','2021-08-04 00:00:00','344343','2021-08-19 00:00:00',NULL,NULL,NULL,'SubTotal',1,1,1.000000,0.0000,0.0000,0.0000,0.0000,'Percentage',0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',1,1,1,2,1,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `debitnote` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnotedetail`
--

DROP TABLE IF EXISTS `debitnotedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnotedetail` (
  `DebitNoteDetId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int DEFAULT NULL,
  `Quantity` decimal(18,2) DEFAULT NULL,
  `PerUnit` int DEFAULT NULL,
  `UnitPrice` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DebitNoteDetId`),
  KEY `IX_DebitNoteDetails_DebitNoteId` (`DebitNoteId`) /*!80000 INVISIBLE */,
  KEY `IX_DebitNoteDetails_UnitOfMeasurementId` (`UnitOfMeasurementId`),
  KEY `FK_DebitNoteDetails_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_DebitNoteDetails_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_DebitNoteDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteDetails_UnitOfMeasurement_UnitOfMeasurementId` FOREIGN KEY (`UnitOfMeasurementId`) REFERENCES `unitofmeasurement` (`UnitOfMeasurementId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnotedetail`
--

LOCK TABLES `debitnotedetail` WRITE;
/*!40000 ALTER TABLE `debitnotedetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `debitnotedetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnotedetailtax`
--

DROP TABLE IF EXISTS `debitnotedetailtax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnotedetailtax` (
  `DebitNoteDetTaxId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteDetId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DebitNoteDetTaxId`),
  KEY `IX_DebitNoteDetailTax_DebitNoteDetId` (`DebitNoteDetId`),
  KEY `IX_DebitNoteDetailTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_DebitNoteDetailTax_Aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_DebitNoteDetailTax_Aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_DebitNoteDetailTax_Aspnetusers_aspnetusersUpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteDetailTax_Aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteDetailTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PIDetailTax_InvoiceDetail_DebitNoteDetId` FOREIGN KEY (`DebitNoteDetId`) REFERENCES `debitnotedetail` (`DebitNoteDetId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnotedetailtax`
--

LOCK TABLES `debitnotedetailtax` WRITE;
/*!40000 ALTER TABLE `debitnotedetailtax` DISABLE KEYS */;
/*!40000 ALTER TABLE `debitnotedetailtax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnotetax`
--

DROP TABLE IF EXISTS `debitnotetax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnotetax` (
  `DebitNoteTaxId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DebitNoteTaxId`),
  KEY `IX_DebitNoteTax_DebitNoteId` (`DebitNoteId`),
  KEY `IX_DebitNoteTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_DebitNoteTax_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_DebitNoteTax_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_DebitNoteTax_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteTax_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteTax_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DebitNoteTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnotetax`
--

LOCK TABLES `debitnotetax` WRITE;
/*!40000 ALTER TABLE `debitnotetax` DISABLE KEYS */;
/*!40000 ALTER TABLE `debitnotetax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `department`
--

DROP TABLE IF EXISTS `department`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `department` (
  `DepartmentId` int NOT NULL AUTO_INCREMENT,
  `DepartmentName` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DepartmentId`),
  UNIQUE KEY `Name_UNIQUE` (`DepartmentName`),
  KEY `FK_Department_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Department_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Department_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Department_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `department`
--

LOCK TABLES `department` WRITE;
/*!40000 ALTER TABLE `department` DISABLE KEYS */;
INSERT INTO `department` VALUES (1,'Sales',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Purchase',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Operation',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `department` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `designation`
--

DROP TABLE IF EXISTS `designation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `designation` (
  `DesignationId` int NOT NULL AUTO_INCREMENT,
  `DesignationName` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`DesignationId`),
  UNIQUE KEY `Name_UNIQUE` (`DesignationName`),
  KEY `FK_Designation_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Designation_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Designation_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Designation_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `designation`
--

LOCK TABLES `designation` WRITE;
/*!40000 ALTER TABLE `designation` DISABLE KEYS */;
INSERT INTO `designation` VALUES (1,'Sales Person',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Buyer',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Expeditor',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Manager',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `designation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `EmployeeId` int NOT NULL AUTO_INCREMENT,
  `EmployeeCode` varchar(50) DEFAULT NULL,
  `FirstName` varchar(250) DEFAULT NULL,
  `LastName` varchar(250) DEFAULT NULL,
  `DesignationId` int DEFAULT NULL,
  `DepartmentId` int DEFAULT NULL,
  `EmailAddress` text,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`EmployeeId`),
  UNIQUE KEY `Code_UNIQUE` (`EmployeeCode`),
  KEY `IX_Employee_DepartmentId` (`DepartmentId`),
  KEY `IX_Employee_DesignationId` (`DesignationId`),
  KEY `FK_Employee_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Employee_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Employee_Department_DepartmentId` FOREIGN KEY (`DepartmentId`) REFERENCES `department` (`DepartmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Employee_Designation_DesignationId` FOREIGN KEY (`DesignationId`) REFERENCES `designation` (`DesignationId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Employee_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Employee_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES (1,'EMP00001','Sam','Ten',2,1,'employee1@gmail.com',1,'2021-01-01 00:00:00',1,'2021-08-15 14:53:26'),(2,'EMP00002','Harry','Potter',1,1,'employee2@gmail.com',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'E0003','Bret','Lee',2,2,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `financialyear`
--

DROP TABLE IF EXISTS `financialyear`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `financialyear` (
  `FinancialYearId` int NOT NULL AUTO_INCREMENT,
  `FinancialYearName` varchar(250) DEFAULT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`FinancialYearId`),
  UNIQUE KEY `Name_UNIQUE` (`FinancialYearName`),
  KEY `FK_FinancialYear_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_FinancialYear_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_FinancialYear_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_FinancialYear_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `financialyear`
--

LOCK TABLES `financialyear` WRITE;
/*!40000 ALTER TABLE `financialyear` DISABLE KEYS */;
INSERT INTO `financialyear` VALUES (1,'01-Jan-2021 To 31-Dec-2021 ','2021-01-01 00:00:00','2021-12-31 00:00:00',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'01-Jan-2022 To 31-Dec-2022','2022-01-01 00:00:00','2022-12-31 00:00:00',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `financialyear` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `financialyearcompanyrelation`
--

DROP TABLE IF EXISTS `financialyearcompanyrelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `financialyearcompanyrelation` (
  `RelationId` int NOT NULL AUTO_INCREMENT,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`RelationId`),
  KEY `IX_FinancialYearCompanyRelation_CompanyId` (`CompanyId`),
  KEY `IX_FinancialYearCompanyRelation_FinancialYearId` (`FinancialYearId`),
  KEY `FK_FinancialYearCompanyRelation_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_FinancialYearCompanyRelation_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_FinancialYearCompanyRelation_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_FinancialYearCompanyRelation_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_FinancialYearCompanyRelation_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_FinancialYearCompanyRelation_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `financialyearcompanyrelation`
--

LOCK TABLES `financialyearcompanyrelation` WRITE;
/*!40000 ALTER TABLE `financialyearcompanyrelation` DISABLE KEYS */;
INSERT INTO `financialyearcompanyrelation` VALUES (1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,1,2,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `financialyearcompanyrelation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `form`
--

DROP TABLE IF EXISTS `form`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `form` (
  `FormId` int NOT NULL AUTO_INCREMENT,
  `FormName` varchar(500) NOT NULL,
  `ModuleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`FormId`),
  UNIQUE KEY `Name_UNIQUE` (`FormName`),
  KEY `IX_Form_Module_ModuleId_idx` (`ModuleId`),
  KEY `FK_Form_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Form_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Form_Module_ModuleId` FOREIGN KEY (`ModuleId`) REFERENCES `module` (`ModuleId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Form_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Form_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `form`
--

LOCK TABLES `form` WRITE;
/*!40000 ALTER TABLE `form` DISABLE KEYS */;
INSERT INTO `form` VALUES (1,'Ledger',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(2,'Sales Invoice List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(3,'Sales Invoice Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(4,'Purchase Invoice List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(5,'Purchase Invoice Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(6,'Currency List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(7,'Currency Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(8,'Currency Conversion List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(9,'Currency Conversion',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(10,'Financial Year List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(11,'Financial Year Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(12,'Financial Year Company Relation',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(13,'Ledger Company Relation',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(14,'Opening Balance Transfer',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(15,'Tax Register List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(16,'Tax Register Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(17,'Receipt Voucher List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(18,'Receipt Voucher Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(19,'Payment Voucher List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(20,'Payment Voucher Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(21,'Journal Voucher List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(22,'Journal Voucher Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(23,'Credit Note List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(24,'Credit Note Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(25,'Debit Note List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(26,'Debit Note Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(27,'Advance Adjustment List',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(28,'Advance Adjustment Add',6,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `form` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `journalvoucher`
--

DROP TABLE IF EXISTS `journalvoucher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `journalvoucher` (
  `JournalVoucherId` int NOT NULL AUTO_INCREMENT,
  `VoucherNo` varchar(250) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) DEFAULT NULL,
  `DebitAmount` decimal(18,4) DEFAULT NULL,
  `CreditAmount_FC` decimal(18,4) DEFAULT NULL,
  `CreditAmount` decimal(18,4) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`JournalVoucherId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`VoucherNo`),
  KEY `IX_JournalVoucher_CompanyId` (`CompanyId`),
  KEY `IX_JournalVoucher_CurrencyId` (`CurrencyId`),
  KEY `IX_JournalVoucher_FinancialYearId` (`FinancialYearId`),
  KEY `IX_JournalVoucher_StatusId` (`StatusId`),
  KEY `IX_JournalVoucher_VoucherStyleId` (`VoucherStyleId`),
  KEY `IX_JournalVoucher_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_JournalVoucher_UpdatedByUserId` (`UpdatedByUserId`),
  CONSTRAINT `FK_JournalVoucher_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucher_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journalvoucher`
--

LOCK TABLES `journalvoucher` WRITE;
/*!40000 ALTER TABLE `journalvoucher` DISABLE KEYS */;
/*!40000 ALTER TABLE `journalvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `journalvoucherdetail`
--

DROP TABLE IF EXISTS `journalvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `journalvoucherdetail` (
  `JournalVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `JournalVoucherId` int DEFAULT NULL,
  `ParticularLedgerId` int DEFAULT NULL,
  `TransactionTypeId` int DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) DEFAULT NULL,
  `DebitAmount` decimal(18,4) DEFAULT NULL,
  `CreditAmount_FC` decimal(18,4) DEFAULT NULL,
  `CreditAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`JournalVoucherDetId`),
  KEY `IX_JournalVoucherDetails_JournalVoucherId` (`JournalVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_JournalVoucherDetails_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `IX_JournalVoucherDetails_UpdatedByUserId_idx` (`UpdatedByUserId`),
  KEY `IX_JournalVoucherDetails_CreditNoteId_idx` (`CreditNoteId`),
  KEY `IX_JournalVoucherDetails_DebitNoteId_idx` (`DebitNoteId`),
  KEY `IX_JournalVoucherDetails_SalesInvoiceId_idx` (`SalesInvoiceId`),
  KEY `IX_JournalVoucherDetails_PurchaseInvoiceId_idx` (`PurchaseInvoiceId`),
  KEY `IX_JournalVoucherDetails_ParticularLedgerId_idx` (`ParticularLedgerId`),
  CONSTRAINT `FK_JournalVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucherDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`),
  CONSTRAINT `FK_JournalVoucherDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`),
  CONSTRAINT `FK_JournalVoucherDetails_JournalVoucher_JournalVoucherId` FOREIGN KEY (`JournalVoucherId`) REFERENCES `journalvoucher` (`JournalVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JournalVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_JournalVoucherDetails_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`),
  CONSTRAINT `FK_JournalVoucherDetails_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journalvoucherdetail`
--

LOCK TABLES `journalvoucherdetail` WRITE;
/*!40000 ALTER TABLE `journalvoucherdetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `journalvoucherdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledger`
--

DROP TABLE IF EXISTS `ledger`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledger` (
  `LedgerId` int NOT NULL AUTO_INCREMENT,
  `LedgerCode` varchar(100) NOT NULL,
  `LedgerName` varchar(500) NOT NULL,
  `IsGroup` tinyint DEFAULT NULL,
  `IsMasterGroup` tinyint DEFAULT NULL,
  `ParentGroupId` int DEFAULT NULL,
  `IsDeActived` tinyint DEFAULT NULL,
  `TaxRegisteredNo` varchar(100) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`LedgerId`),
  UNIQUE KEY `Code_UNIQUE` (`LedgerCode`),
  KEY `IX_Ledger_ParentGroupId` (`ParentGroupId`),
  KEY `FK_Ledger_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Ledger_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Ledger_Ledger_ParentGroupId` FOREIGN KEY (`ParentGroupId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_Ledger_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Ledger_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=136 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledger`
--

LOCK TABLES `ledger` WRITE;
/*!40000 ALTER TABLE `ledger` DISABLE KEYS */;
INSERT INTO `ledger` VALUES (1,'M000001','LIABILITIES',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'M000002','ASSETS',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'M000003','EXPENDITURES (Trading A/C)',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'M000004','INCOME (Trading A/C)',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'M000005','EXPENDITURES (P & L A/C)',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,'M000006','INCOME (P & L A/C)',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'M000007','PROFIT & LOSS A/C',1,1,NULL,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,'M000008','Capital Accounts  ',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,'M000009','Reserves & Surplus',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(10,'M000010','Loans (LIABILITIES)',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(11,'M000011','Current Liabilities',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,'M000012','Branch/Division',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,'M000013','Suspense A/C',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,'M000014','Bank OD A/C',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,'M000015','Secured Loans',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,'M000016','Sundry Creditor',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,'M000017','Duties & Taxes',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,'M000018','Privision',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(19,'M000019','Unsecured Loans',1,1,1,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(20,'M000020','Fixed Assets',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(21,'M000021','Investments',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(22,'M000022','Current Assets',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(23,'M000023','Loans and Advances (ASSET)',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(24,'M000024','Misc. Expenditures (ASSET)',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(25,'M000025','Opening Stock',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(26,'M000026','Sundry Debtor',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(27,'M000027','Bank A/C',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(28,'M000028','Cash-in-hand',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(29,'M000029','Deposits (Assets)',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(30,'M000030','Stock-in-hand (Closing Stock)',1,1,2,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(31,'M000031','Direct Expenses',1,1,3,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(32,'M000032','Direct Income',1,1,4,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(33,'M000033','Closing Stock',1,1,4,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(34,'M000034','Indirect Expenses',1,1,5,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(35,'M000035','Indirect Income',1,1,6,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(36,'M000036','Current Period',1,1,7,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(37,'M000037','Opening Balance',1,1,7,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(38,'M000038','Freight & Handling Charges',1,1,36,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(101,'UG000101','Sales Account',0,0,32,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(102,'UG000102','Purchase Account',0,0,31,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(103,'UG000103','Other Sales',0,0,35,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(104,'UG000104','Other Purchase',0,0,34,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(105,'UG000105','VAT Input',0,0,17,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(106,'UG000106','VAT Output',0,0,17,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(107,'UG000107','Bank 1',0,0,27,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(108,'UG000108','Bank 2',0,0,27,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(109,'UG000109','Sundry Creditor 1',0,0,16,0,'123456',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(110,'UG000110','Sundry Creditor 2',0,0,16,0,'8765432',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(111,'UG000111','Sundry Debtor 1',0,0,26,0,'DADFS34',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(112,'UG000112','Sundry Debtor 2',0,0,26,0,'545454DR',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(113,'UG000113','Opening Stock 1',0,0,25,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(114,'UG000114','Closing Stock 1',0,0,33,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(115,'UG000115','Cash In hand 1',0,0,28,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(116,'UG000116','Retained Profit 2020',0,0,9,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(117,'UG000117','Profit Account',0,0,7,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(118,'UG000118','Loss Account',0,0,7,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(119,'UG000119','Due from Directors',0,0,22,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(120,'UG000120','Goods in Transit',0,0,22,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(121,'UG000121','Stock in Hand',0,0,33,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(122,'UG000122','Electricity Deposit',0,0,29,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(123,'UG000123','Telecommunication Deposit',0,0,29,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(124,'UG000124','MIDC Deposit',0,0,29,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(125,'UG000125','Advances to Suppliers',0,0,23,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(126,'UG000126','Accrued Expenses Payable',0,0,11,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(127,'UG000127','Advances from Customers',0,0,11,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(128,'UG000128','Other Payables',0,0,11,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(129,'UG000129','VAT PAYABLE',0,0,11,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(130,'UG000130','Commission Provision',0,0,18,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(131,'UG000131','LD Charges Provision',0,0,18,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(132,'UG000132','Staff Benefits Provision',0,0,18,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(133,'UG000133','Bonus Expenses',0,0,34,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(134,'UG000134','Gratuity Expenses',0,0,34,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(135,'UG000135','Salary Expenses',0,0,34,0,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `ledger` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledgeraddress`
--

DROP TABLE IF EXISTS `ledgeraddress`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgeraddress` (
  `AddressId` int NOT NULL AUTO_INCREMENT,
  `LedgerId` int DEFAULT NULL,
  `AddressDescription` varchar(1000) DEFAULT NULL,
  `CountryId` int DEFAULT NULL,
  `StateId` int DEFAULT NULL,
  `CityId` int DEFAULT NULL,
  `EmailAddress` varchar(100) DEFAULT NULL,
  `PhoneNo` varchar(20) DEFAULT NULL,
  `PostalCode` varchar(20) DEFAULT NULL,
  `FaxNo` varchar(20) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AddressId`),
  KEY `IX_LedgerAddress_CityId` (`CityId`),
  KEY `IX_LedgerAddress_CountryId` (`CountryId`),
  KEY `IX_LedgerAddress_LedgerId` (`LedgerId`),
  KEY `IX_LedgerAddress_StateId` (`StateId`),
  KEY `FK_LedgerAddress_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerAddress_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerAddress_City_CityId` FOREIGN KEY (`CityId`) REFERENCES `city` (`CityId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAddress_Country_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `country` (`CountryId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAddress_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAddress_State_StateId` FOREIGN KEY (`StateId`) REFERENCES `state` (`StateId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAddress_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAddress_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledgeraddress`
--

LOCK TABLES `ledgeraddress` WRITE;
/*!40000 ALTER TABLE `ledgeraddress` DISABLE KEYS */;
INSERT INTO `ledgeraddress` VALUES (1,109,'Dubai 1, 123456, 76jjljl',1,1,2,'sandry1@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,110,'Dubai 2, 123456, 76jjljl',1,1,2,'sandry2@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,110,'Dubai 3, 123456, 76jjljl',1,1,2,'sandry3@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,110,'Dubai 4, 123456, 76jjljl',1,1,2,'sandry4@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,111,'Dubai 1, 123456, 76jjljl',1,1,2,'sandry1@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,111,'Dubai 2, 123456, 76jjljl',1,1,2,'sandry2@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,112,'Dubai 3, 123456, 76jjljl',1,1,2,'sandry3@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,113,'Dubai 4, 123456, 76jjljl',1,1,2,'sandry4@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `ledgeraddress` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledgercompanyrelation`
--

DROP TABLE IF EXISTS `ledgercompanyrelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgercompanyrelation` (
  `RelationId` int NOT NULL AUTO_INCREMENT,
  `CompanyId` int DEFAULT NULL,
  `LedgerId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`RelationId`),
  KEY `IX_LedgerCompanyRelation_CompanyId` (`CompanyId`),
  KEY `IX_LedgerCompanyRelation_LedgerId` (`LedgerId`),
  KEY `FK_LedgerCompanyRelation_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerCompanyRelation_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerCompanyRelation_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerCompanyRelation_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerCompanyRelation_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerCompanyRelation_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledgercompanyrelation`
--

LOCK TABLES `ledgercompanyrelation` WRITE;
/*!40000 ALTER TABLE `ledgercompanyrelation` DISABLE KEYS */;
INSERT INTO `ledgercompanyrelation` VALUES (1,1,101,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,1,102,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,1,103,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,1,104,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,1,105,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,1,106,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,1,107,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,1,108,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,1,109,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(10,1,110,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(11,1,111,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,1,112,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,1,113,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,1,114,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,1,115,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,1,116,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,1,117,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,1,118,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(19,1,119,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(20,1,120,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(21,1,121,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(22,1,122,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(23,1,123,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(24,1,124,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(25,1,125,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(26,1,126,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(27,1,127,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(28,1,128,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(29,1,129,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(30,1,130,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(31,1,131,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(32,1,132,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(33,1,133,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(34,1,134,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(35,1,135,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `ledgercompanyrelation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledgerfinancialyearbalance`
--

DROP TABLE IF EXISTS `ledgerfinancialyearbalance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgerfinancialyearbalance` (
  `LedgerBalanceId` int NOT NULL AUTO_INCREMENT,
  `LedgerId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `CurrencyId` int DEFAULT NULL,
  `ExchangeRate` decimal(18,6) DEFAULT NULL,
  `OpeningBalanceAmount_FC` decimal(18,4) DEFAULT NULL,
  `OpeningBalanceAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`LedgerBalanceId`),
  KEY `IX_LedgerFinancialYearBalance_CompanyId` (`CompanyId`),
  KEY `IX_LedgerFinancialYearBalance_CurrencyId` (`CurrencyId`),
  KEY `IX_LedgerFinancialYearBalance_FinancialYearId` (`FinancialYearId`),
  KEY `IX_LedgerFinancialYearBalance_LedgerId` (`LedgerId`),
  KEY `FK_LedgerFinancialYearBalance_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerFinancialYearBalance_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerFinancialYearBalance_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerFinancialYearBalance_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE CASCADE,
  CONSTRAINT `FK_LedgerFinancialYearBalance_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerFinancialYearBalance_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerFinancialYearBalance_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerFinancialYearBalance_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledgerfinancialyearbalance`
--

LOCK TABLES `ledgerfinancialyearbalance` WRITE;
/*!40000 ALTER TABLE `ledgerfinancialyearbalance` DISABLE KEYS */;
INSERT INTO `ledgerfinancialyearbalance` VALUES (1,101,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,102,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,103,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,104,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,105,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,106,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,107,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,108,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,109,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(10,110,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(11,111,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,112,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,113,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,114,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,115,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,116,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,117,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,118,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(19,119,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(20,120,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(21,121,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(22,122,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(23,123,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(24,124,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(25,125,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(26,126,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(27,127,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(28,128,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(29,129,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(30,130,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(31,131,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(32,132,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(33,133,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(34,134,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(35,135,1,1,1,1.000000,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `ledgerfinancialyearbalance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `module`
--

DROP TABLE IF EXISTS `module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `module` (
  `ModuleId` int NOT NULL AUTO_INCREMENT,
  `ModuleName` varchar(500) NOT NULL,
  `IsActive` tinyint DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ModuleId`),
  UNIQUE KEY `Name_UNIQUE` (`ModuleName`),
  KEY `FK_Module_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Module_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Module_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Module_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `module`
--

LOCK TABLES `module` WRITE;
/*!40000 ALTER TABLE `module` DISABLE KEYS */;
INSERT INTO `module` VALUES (1,'Admin',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Master',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Sales',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Purchase',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'Stores',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,'Accounts',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'HR',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,'Asset Management',1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paymentvoucher`
--

DROP TABLE IF EXISTS `paymentvoucher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymentvoucher` (
  `PaymentVoucherId` int NOT NULL AUTO_INCREMENT,
  `VoucherNo` varchar(250) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `TypeCorB` varchar(1) DEFAULT NULL,
  `PaymentTypeId` int DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `ChequeNo` varchar(50) DEFAULT NULL,
  `ChequeDate` datetime DEFAULT NULL,
  `ChequeAmount_FC` decimal(18,4) DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PaymentVoucherId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`VoucherNo`),
  KEY `IX_PaymentVoucher_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_PaymentVoucher_CompanyId` (`CompanyId`),
  KEY `IX_PaymentVoucher_CurrencyId` (`CurrencyId`),
  KEY `IX_PaymentVoucher_FinancialYearId` (`FinancialYearId`),
  KEY `IX_PaymentVoucher_StatusId` (`StatusId`),
  KEY `IX_PaymentVoucher_VoucherStyleId` (`VoucherStyleId`),
  KEY `IX_PaymentVoucher_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_PaymentVoucher_UpdatedByUserId` (`UpdatedByUserId`),
  CONSTRAINT `FK_PaymentVoucher_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucher_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentvoucher`
--

LOCK TABLES `paymentvoucher` WRITE;
/*!40000 ALTER TABLE `paymentvoucher` DISABLE KEYS */;
INSERT INTO `paymentvoucher` VALUES (17,'21-01-06-00001','2021-09-09 00:00:00',115,'C',1,1,1.000000,'76868tut','2021-09-08 00:00:00',545.0000,'ddffdsfsd',148.0000,148.0000,'INR One Hundred Fourty Eight  Only',1,1,1,1,1,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `paymentvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paymentvoucherdetail`
--

DROP TABLE IF EXISTS `paymentvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymentvoucherdetail` (
  `PaymentVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `PaymentVoucherId` int DEFAULT NULL,
  `ParticularLedgerId` int DEFAULT NULL,
  `TransactionTypeId` int DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PaymentVoucherDetId`),
  KEY `IX_PaymentVoucherDetails_PaymentVoucherId` (`PaymentVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_PaymentVoucherDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_PaymentVoucherDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_PaymentVoucherDetails_CreditNoteId` (`CreditNoteId`),
  KEY `IX_PaymentVoucherDetails_DebitNoteId` (`DebitNoteId`),
  KEY `IX_PaymentVoucherDetails_PurchaseInvoiceId` (`PurchaseInvoiceId`),
  KEY `IX_PaymentVoucherDetails_ParticularLedgerId` (`ParticularLedgerId`),
  CONSTRAINT `FK_PaymentVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`),
  CONSTRAINT `FK_PaymentVoucherDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`),
  CONSTRAINT `FK_PaymentVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_PaymentVoucherDetails_PaymentVoucher_PaymentVoucherId` FOREIGN KEY (`PaymentVoucherId`) REFERENCES `paymentvoucher` (`PaymentVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentvoucherdetail`
--

LOCK TABLES `paymentvoucherdetail` WRITE;
/*!40000 ALTER TABLE `paymentvoucherdetail` DISABLE KEYS */;
INSERT INTO `paymentvoucherdetail` VALUES (28,17,114,1,56.0000,56.0000,'666',NULL,NULL,NULL,NULL,NULL,NULL,NULL),(29,17,130,1,45.0000,45.0000,'fg',NULL,NULL,NULL,NULL,NULL,NULL,NULL),(31,17,130,1,45.0000,45.0000,'hhi',NULL,NULL,NULL,NULL,NULL,NULL,NULL),(32,17,114,2,2.0000,0.0000,'ddss ',NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `paymentvoucherdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoice`
--

DROP TABLE IF EXISTS `purchaseinvoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoice` (
  `PurchaseInvoiceId` int NOT NULL AUTO_INCREMENT,
  `InvoiceNo` varchar(250) DEFAULT NULL,
  `InvoiceDate` datetime DEFAULT NULL,
  `SupplierLedgerId` int DEFAULT NULL,
  `BillToAddressId` int DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `SupplierReferenceNo` varchar(250) DEFAULT NULL,
  `SupplierReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int DEFAULT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) DEFAULT NULL,
  `TotalLineItemAmount` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PurchaseInvoiceId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`InvoiceNo`),
  KEY `IX_PurchaseInvoice_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_PurchaseInvoice_CompanyId` (`CompanyId`),
  KEY `IX_PurchaseInvoice_CurrencyId` (`CurrencyId`),
  KEY `IX_PurchaseInvoice_FinancialYearId` (`FinancialYearId`),
  KEY `IX_PurchaseInvoice_StatusId` (`StatusId`),
  KEY `IX_PurchaseInvoice_SupplierLedgerId` (`SupplierLedgerId`),
  KEY `IX_PurchaseInvoice_TaxRegisterId` (`TaxRegisterId`),
  KEY `tf_idx` (`VoucherStyleId`),
  KEY `FK_PurchaseInvoice_Ledger_BillToAddressId_idx` (`BillToAddressId`),
  KEY `FK_PurchaseInvoice_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PurchaseInvoice_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PurchaseInvoice_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_Ledger_BillToAddressId` FOREIGN KEY (`BillToAddressId`) REFERENCES `ledgeraddress` (`AddressId`),
  CONSTRAINT `FK_PurchaseInvoice_Ledger_SupplierLedgerId` FOREIGN KEY (`SupplierLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_TaxRegister_TaxRegisterId` FOREIGN KEY (`TaxRegisterId`) REFERENCES `taxregister` (`TaxRegisterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoice_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoice`
--

LOCK TABLES `purchaseinvoice` WRITE;
/*!40000 ALTER TABLE `purchaseinvoice` DISABLE KEYS */;
INSERT INTO `purchaseinvoice` VALUES (16,'21-01-02-00001','2021-09-20 00:00:00',109,1,107,'5454','2021-08-10 00:00:00',45,'2 fvfdfddff','5  555 ','SubTotal',1,1,1.000000,264.0000,264.0000,277.2000,277.2000,'Percentage',5.0000,13.2000,13.2000,0.0000,0.0000,290.4000,290.4000,'INR Two Hundred Ninety and Four Paisa Only',1,1,1,1,1,3,'2021-09-20 15:26:58',3,'2021-09-20 15:27:47');
/*!40000 ALTER TABLE `purchaseinvoice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoicedetail`
--

DROP TABLE IF EXISTS `purchaseinvoicedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoicedetail` (
  `PurchaseInvoiceDetId` int NOT NULL AUTO_INCREMENT,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int DEFAULT NULL,
  `Quantity` decimal(18,2) DEFAULT NULL,
  `PerUnit` int DEFAULT NULL,
  `UnitPrice` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PurchaseInvoiceDetId`),
  KEY `IX_PurchaseInvoiceDetails_PurchaseInvoiceId` (`PurchaseInvoiceId`) /*!80000 INVISIBLE */,
  KEY `IX_PurchaseInvoiceDetails_UnitOfMeasurementId` (`UnitOfMeasurementId`),
  KEY `FK_PurchaseInvoiceDetails_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PurchaseInvoiceDetails_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PurchaseInvoiceDetails_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetails_UnitOfMeasurement_UnitOfMeasurementId` FOREIGN KEY (`UnitOfMeasurementId`) REFERENCES `unitofmeasurement` (`UnitOfMeasurementId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetails_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetails_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicedetail`
--

LOCK TABLES `purchaseinvoicedetail` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicedetail` DISABLE KEYS */;
INSERT INTO `purchaseinvoicedetail` VALUES (6,16,1,'ABCD EFH ',1,12.00,2,11.0000,264.0000,264.0000,0.0000,0.0000,264.0000,264.0000,3,'2021-09-20 15:27:21',3,'2021-09-20 15:27:47');
/*!40000 ALTER TABLE `purchaseinvoicedetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoicedetailtax`
--

DROP TABLE IF EXISTS `purchaseinvoicedetailtax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoicedetailtax` (
  `PurchaseInvoiceDetTaxId` int NOT NULL AUTO_INCREMENT,
  `PurchaseInvoiceDetId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PurchaseInvoiceDetTaxId`),
  KEY `IX_PurchaseInvoiceDetailTax_PurchaseInvoiceDetId` (`PurchaseInvoiceDetId`),
  KEY `IX_PurchaseInvoiceDetailTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_PurchaseInvoiceDetailTax_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PurchaseInvoiceDetailTax_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PIDetailTax_InvoiceDetail_PurchaseInvoiceDetId` FOREIGN KEY (`PurchaseInvoiceDetId`) REFERENCES `purchaseinvoicedetail` (`PurchaseInvoiceDetId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetailTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetailTax_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceDetailTax_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicedetailtax`
--

LOCK TABLES `purchaseinvoicedetailtax` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicedetailtax` DISABLE KEYS */;
/*!40000 ALTER TABLE `purchaseinvoicedetailtax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoicetax`
--

DROP TABLE IF EXISTS `purchaseinvoicetax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoicetax` (
  `PurchaseInvoiceTaxId` int NOT NULL AUTO_INCREMENT,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PurchaseInvoiceTaxId`),
  KEY `IX_PurchaseInvoiceTax_PurchaseInvoiceId` (`PurchaseInvoiceId`),
  KEY `IX_PurchaseInvoiceTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_PurchaseInvoiceTax_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PurchaseInvoiceTax_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PurchaseInvoiceTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceTax_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceTax_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PurchaseInvoiceTax_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicetax`
--

LOCK TABLES `purchaseinvoicetax` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicetax` DISABLE KEYS */;
/*!40000 ALTER TABLE `purchaseinvoicetax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receiptvoucher`
--

DROP TABLE IF EXISTS `receiptvoucher`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receiptvoucher` (
  `ReceiptVoucherId` int NOT NULL AUTO_INCREMENT,
  `VoucherNo` varchar(250) DEFAULT NULL,
  `VoucherDate` datetime DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `TypeCorB` varchar(1) DEFAULT NULL,
  `PaymentTypeId` int DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `ChequeNo` varchar(50) DEFAULT NULL,
  `ChequeDate` datetime DEFAULT NULL,
  `ChequeAmount_FC` decimal(18,4) DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ReceiptVoucherId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`VoucherNo`),
  KEY `IX_ReceiptVoucher_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_ReceiptVoucher_CompanyId` (`CompanyId`),
  KEY `IX_ReceiptVoucher_CurrencyId` (`CurrencyId`),
  KEY `IX_ReceiptVoucher_FinancialYearId` (`FinancialYearId`),
  KEY `IX_ReceiptVoucher_StatusId` (`StatusId`),
  KEY `tf` (`VoucherStyleId`),
  KEY `IX_ReceiptVoucher_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_ReceiptVoucher_UpdatedByUserId` (`UpdatedByUserId`),
  CONSTRAINT `FK_ReceiptVoucher_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucher_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptvoucher`
--

LOCK TABLES `receiptvoucher` WRITE;
/*!40000 ALTER TABLE `receiptvoucher` DISABLE KEYS */;
/*!40000 ALTER TABLE `receiptvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receiptvoucherdetail`
--

DROP TABLE IF EXISTS `receiptvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receiptvoucherdetail` (
  `ReceiptVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `ReceiptVoucherId` int DEFAULT NULL,
  `ParticularLedgerId` int DEFAULT NULL,
  `TransactionTypeId` int DEFAULT NULL,
  `Amount_FC` decimal(18,4) DEFAULT NULL,
  `Amount` decimal(18,4) DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ReceiptVoucherDetId`),
  KEY `IX_ReceiptVoucherDetails_ReceiptVoucherId` (`ReceiptVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_ReceiptVoucherDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_ReceiptVoucherDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_ReceiptVoucherDetails_CreditNoteId` (`CreditNoteId`),
  KEY `IX_ReceiptVoucherDetails_DebitNoteId` (`DebitNoteId`),
  KEY `IX_ReceiptVoucherDetails_SalesInvoiceId` (`SalesInvoiceId`),
  KEY `IX_ReceiptVoucherDetails_ParticularLedgerId` (`ParticularLedgerId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_ReceiptVoucher_ReceiptVoucherId` FOREIGN KEY (`ReceiptVoucherId`) REFERENCES `receiptvoucher` (`ReceiptVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='FK_ReceiptVoucherDetails_Ledger_ParticularLedgerId';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptvoucherdetail`
--

LOCK TABLES `receiptvoucherdetail` WRITE;
/*!40000 ALTER TABLE `receiptvoucherdetail` DISABLE KEYS */;
/*!40000 ALTER TABLE `receiptvoucherdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoice`
--

DROP TABLE IF EXISTS `salesinvoice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoice` (
  `SalesInvoiceId` int NOT NULL AUTO_INCREMENT,
  `InvoiceNo` varchar(250) DEFAULT NULL,
  `InvoiceDate` datetime DEFAULT NULL,
  `CustomerLedgerId` int DEFAULT NULL,
  `BillToAddressId` int DEFAULT NULL,
  `AccountLedgerId` int DEFAULT NULL,
  `BankLedgerId` int DEFAULT NULL,
  `CustomerReferenceNo` varchar(250) DEFAULT NULL,
  `CustomerReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int DEFAULT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int DEFAULT NULL,
  `CurrencyId` int DEFAULT NULL,
  `ExchangeRate` decimal(18,6) DEFAULT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) DEFAULT NULL,
  `TotalLineItemAmount` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPercentage` decimal(18,4) DEFAULT NULL,
  `DiscountAmount_FC` decimal(18,4) DEFAULT NULL,
  `DiscountAmount` decimal(18,4) DEFAULT NULL,
  `StatusId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `MaxNo` int DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SalesInvoiceId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`InvoiceNo`),
  KEY `IX_SalesInvoice_AccountLedgerId` (`AccountLedgerId`),
  KEY `IX_SalesInvoice_BankLedgerId` (`BankLedgerId`),
  KEY `IX_SalesInvoice_CompanyId` (`CompanyId`),
  KEY `IX_SalesInvoice_CurrencyId` (`CurrencyId`),
  KEY `IX_SalesInvoice_CustomerLedgerId` (`CustomerLedgerId`),
  KEY `IX_SalesInvoice_FinancialYearId` (`FinancialYearId`),
  KEY `IX_SalesInvoice_StatusId` (`StatusId`),
  KEY `IX_SalesInvoice_TaxRegisterId` (`TaxRegisterId`),
  KEY `IX_SalesInvoice_VoucherStyle_VoucherStyleId_idx` (`VoucherStyleId`),
  KEY `FK_SalesInvoice_Ledger_BillToAddressId_idx` (`BillToAddressId`),
  KEY `FK_SalesInvoice_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_SalesInvoice_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_SalesInvoice_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_Ledger_AccountLedgerId` FOREIGN KEY (`AccountLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_Ledger_BankLedgerId` FOREIGN KEY (`BankLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_Ledger_BillToAddressId` FOREIGN KEY (`BillToAddressId`) REFERENCES `ledgeraddress` (`AddressId`),
  CONSTRAINT `FK_SalesInvoice_Ledger_CustomerLedgerId` FOREIGN KEY (`CustomerLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_TaxRegister_TaxRegisterId` FOREIGN KEY (`TaxRegisterId`) REFERENCES `taxregister` (`TaxRegisterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoice_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoice`
--

LOCK TABLES `salesinvoice` WRITE;
/*!40000 ALTER TABLE `salesinvoice` DISABLE KEYS */;
INSERT INTO `salesinvoice` VALUES (1,'21-01-01-00001','2021-04-22 00:00:00',111,5,16,NULL,'dsfsdfw324423','2021-04-07 00:00:00',NULL,'errere','rrvv  434343','SubTotal',1,2,0.050000,8993.5200,449.6760,9623.0664,481.1533,10252.6128,512.6306,'DHM Ten Thousand Two Hundred Fifty Two and Six One Two Eight Paisa Only',2509.2313,125.4616,'Percentage',7.0000,629.5464,31.4773,NULL,1,1,1,1,NULL,1,NULL,'2021-08-29 16:48:06'),(2,'21-01-01-00002','2021-04-16 00:00:00',111,5,14,NULL,'dsfsdfw324423','2021-03-30 00:00:00',NULL,'kjhkh',NULL,'SubTotal',1,1,5.000000,448.0000,2240.0000,455.0000,2275.0000,462.0000,2310.0000,'INR Four Hundred Sixty Two  Only',0.0000,0.0000,'Amount',7.0000,7.0000,35.0000,NULL,1,1,2,1,NULL,3,NULL,'2021-09-08 15:53:28'),(11,'21-01-01-00003','2021-04-16 00:00:00',111,5,15,NULL,'reet','2021-04-14 00:00:00',NULL,'ererre',NULL,'SubTotal',1,1,3.000000,86860.0000,260580.0000,86915.0000,260745.0000,86970.0000,260910.0000,'INR Eighty Six Thousand Nine Hundred Seventy  Only',0.0000,0.0000,'Amount',55.0000,55.0000,165.0000,NULL,1,1,3,1,NULL,3,NULL,'2021-09-08 15:53:39'),(12,'21-01-01-00004','2021-04-16 00:00:00',112,7,16,108,'dsfsdfw324423','2021-03-30 00:00:00',60,'kjhkh',NULL,'SubTotal',1,1,44.000000,86.0000,3784.0000,130.0000,5720.0000,174.0000,7656.0000,'INR One Hundred Seventy Four  Only',0.0000,0.0000,'Amount',44.0000,44.0000,1936.0000,NULL,1,1,4,1,NULL,3,NULL,'2021-09-08 15:54:06'),(14,'21-01-01-00005','2021-08-29 00:00:00',111,5,17,107,'45543354','2021-08-25 00:00:00',555,'555','fgfgfg','SubTotal',1,1,1.000000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'',0.0000,0.0000,'Percentage',55.0000,0.0000,0.0000,NULL,1,1,5,1,1,1,'2021-08-29 16:48:06','2021-08-29 16:48:06'),(15,'21-01-01-00006','2021-08-30 00:00:00',111,6,16,107,'66','2021-08-12 00:00:00',6,'66','rrtrnrt','SubTotal',1,2,0.050000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',0.0000,0.0000,'Percentage',6.0000,0.0000,0.0000,NULL,1,1,6,1,1,1,'2021-08-30 09:02:24','2021-09-06 20:27:10'),(16,'21-01-01-00007','2021-08-30 00:00:00',111,6,16,107,'66','2021-08-12 00:00:00',6,'66','rrtrnrt','SubTotal',1,2,0.050428,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',0.0000,0.0000,'Percentage',6.0000,0.0000,0.0000,NULL,1,1,7,1,1,1,'2021-08-30 09:02:34','2021-08-30 09:09:15'),(17,'21-01-01-00008','2021-08-30 00:00:00',111,5,19,107,'545454','2021-08-03 00:00:00',44,'455','fdfdfdfd','LineWise',1,1,1.000000,0.0000,0.0000,5.0000,5.0000,10.0000,10.0000,'INR Ten  Only',0.0000,0.0000,'Amount',5.0000,5.0000,5.0000,NULL,1,1,8,1,1,1,'2021-08-30 09:10:56','2021-08-30 09:11:10'),(18,'21-01-01-00009','2021-08-30 00:00:00',112,7,13,108,'5654','2021-08-02 00:00:00',45,'45','4554','LineWise',1,2,0.050428,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',0.0000,0.0000,'Percentage',5.0000,0.0000,0.0000,NULL,1,1,9,1,1,1,'2021-08-30 10:11:46','2021-08-30 10:11:47');
/*!40000 ALTER TABLE `salesinvoice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoicedetail`
--

DROP TABLE IF EXISTS `salesinvoicedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoicedetail` (
  `SalesInvoiceDetId` int NOT NULL AUTO_INCREMENT,
  `SalesInvoiceId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int DEFAULT NULL,
  `Quantity` decimal(18,2) DEFAULT NULL,
  `PerUnit` int DEFAULT NULL,
  `UnitPrice` decimal(18,4) DEFAULT NULL,
  `GrossAmount_FC` decimal(18,4) DEFAULT NULL,
  `GrossAmount` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT '0.0000',
  `TaxAmount_FC` decimal(18,4) DEFAULT '0.0000',
  `NetAmount_FC` decimal(18,4) DEFAULT NULL,
  `NetAmount` decimal(18,4) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SalesInvoiceDetId`),
  KEY `IX_SalesInvoiceDetails_SalesInvoiceId` (`SalesInvoiceId`),
  KEY `IX_SalesInvoiceDetails_UnitOfMeasurementId` (`UnitOfMeasurementId`),
  KEY `FK_SalesInvoiceDetails_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_SalesInvoiceDetails_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_SalesInvoiceDetails_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetails_UnitOfMeasurement_UnitOfMeasurementId` FOREIGN KEY (`UnitOfMeasurementId`) REFERENCES `unitofmeasurement` (`UnitOfMeasurementId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetails_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetails_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicedetail`
--

LOCK TABLES `salesinvoicedetail` WRITE;
/*!40000 ALTER TABLE `salesinvoicedetail` DISABLE KEYS */;
INSERT INTO `salesinvoicedetail` VALUES (1,1,1,'rwrrre',2,20.10,1,45.2000,908.5200,45.4260,36.7951,735.9012,1644.4212,82.2211,NULL,NULL,1,'2021-08-29 16:33:36'),(2,1,2,'rwrrre',1,3.00,35,77.0000,8085.0000,24255.0000,1212.7500,404.2500,8489.2500,25467.7500,1,'2021-08-15 15:07:41',1,'2021-08-24 22:54:32'),(6,2,1,'fffd',1,4.00,4,4.0000,64.0000,627008.0000,31350.4000,3.2000,67.2000,658358.4000,1,'2021-08-24 21:13:39',3,'2021-09-08 15:53:11'),(7,2,2,'32fv  ff ',1,32.00,4,3.0000,384.0000,3762048.0000,0.0000,0.0000,384.0000,3762048.0000,1,'2021-08-24 21:14:48',3,'2021-09-08 15:50:52'),(8,11,1,'sdf fds dsffsdd',2,4343.00,4,5.0000,86860.0000,29966700.0000,17250.0000,50.0000,86910.0000,29983950.0000,1,'2021-08-25 16:36:31',1,'2021-08-25 16:37:54'),(21,12,1,'Item 1',1,43.00,1,2.0000,86.0000,3784.0000,0.0000,0.0000,86.0000,3784.0000,3,'2021-09-08 15:54:06',3,'2021-09-08 15:54:06');
/*!40000 ALTER TABLE `salesinvoicedetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoicedetailtax`
--

DROP TABLE IF EXISTS `salesinvoicedetailtax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoicedetailtax` (
  `SalesInvoiceDetTaxId` int NOT NULL AUTO_INCREMENT,
  `SalesInvoiceDetId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SalesInvoiceDetTaxId`),
  KEY `IX_SalesInvoiceDetailsTax_SalesInvoiceDetId` (`SalesInvoiceDetId`),
  KEY `IX_SalesInvoiceDetailsTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_SalesInvoiceDetailsTax_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_SalesInvoiceDetailsTax_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_SalesInvoiceDetailsTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetailsTax_SalesInvoiceDetails_SalesInvoiceDetId` FOREIGN KEY (`SalesInvoiceDetId`) REFERENCES `salesinvoicedetail` (`SalesInvoiceDetId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetailsTax_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceDetailsTax_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicedetailtax`
--

LOCK TABLES `salesinvoicedetailtax` WRITE;
/*!40000 ALTER TABLE `salesinvoicedetailtax` DISABLE KEYS */;
INSERT INTO `salesinvoicedetailtax` VALUES (1,1,1,105,'Percentage',70.0000,'Add',635.9640,NULL,'fgfgf',1,'2021-08-24 20:13:31',1,'2021-08-29 16:33:36'),(2,6,1,105,'Percentage',5.0000,'Add',3.2000,NULL,NULL,1,'2021-08-24 21:15:05',1,'2021-08-24 21:15:05'),(3,2,1,105,'Percentage',5.0000,'Add',404.2500,NULL,NULL,1,'2021-08-24 21:18:41',1,'2021-08-24 22:54:31'),(4,1,2,105,'Percentage',5.0000,'Add',45.4260,NULL,NULL,1,'2021-08-25 16:27:48',1,'2021-08-25 16:27:48'),(5,8,1,105,'Amount',50.0000,'Add',50.0000,NULL,NULL,1,'2021-08-25 16:36:51',1,'2021-08-25 16:37:50'),(6,1,3,105,'Percentage',4.0000,'Add',36.3408,NULL,NULL,1,'2021-08-27 10:02:53',1,'2021-08-27 10:02:53'),(7,1,4,106,'Percentage',2.0000,'Add',18.1704,NULL,NULL,1,'2021-08-27 10:18:28',1,'2021-08-27 10:18:28');
/*!40000 ALTER TABLE `salesinvoicedetailtax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoicetax`
--

DROP TABLE IF EXISTS `salesinvoicetax`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoicetax` (
  `SalesInvoiceTaxId` int NOT NULL AUTO_INCREMENT,
  `SalesInvoiceId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) DEFAULT NULL,
  `TaxAmount` decimal(18,4) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SalesInvoiceTaxId`),
  KEY `IX_SalesInvoiceTax_SalesInvoiceId` (`SalesInvoiceId`),
  KEY `IX_SalesInvoiceTax_TaxLedgerId` (`TaxLedgerId`),
  KEY `FK_SalesInvoiceTax_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_SalesInvoiceTax_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_SalesInvoiceTax_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceTax_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceTax_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SalesInvoiceTax_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicetax`
--

LOCK TABLES `salesinvoicetax` WRITE;
/*!40000 ALTER TABLE `salesinvoicetax` DISABLE KEYS */;
INSERT INTO `salesinvoicetax` VALUES (1,1,1,105,'Percentage',5.0000,'Add',2509.2313,NULL,NULL,1,'2021-08-24 21:12:41',1,'2021-08-27 21:51:48');
/*!40000 ALTER TABLE `salesinvoicetax` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `state`
--

DROP TABLE IF EXISTS `state`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `state` (
  `StateId` int NOT NULL AUTO_INCREMENT,
  `StateName` varchar(500) DEFAULT NULL,
  `CountryId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`StateId`),
  KEY `IX_State_CountryId` (`CountryId`),
  KEY `FK_State_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_State_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_State_Country_CountryId` FOREIGN KEY (`CountryId`) REFERENCES `country` (`CountryId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_State_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_State_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `state`
--

LOCK TABLES `state` WRITE;
/*!40000 ALTER TABLE `state` DISABLE KEYS */;
INSERT INTO `state` VALUES (1,'Maharashtra1',1,1,'2021-01-01 00:00:00',1,'2021-08-15 14:53:35'),(2,'Gujarat',1,1,'2021-01-01 00:00:00',1,'2021-08-15 14:53:47'),(3,'Dubai',2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Abu Dhabi',2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `state` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `status`
--

DROP TABLE IF EXISTS `status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `status` (
  `StatusId` int NOT NULL AUTO_INCREMENT,
  `StatusName` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`StatusId`),
  UNIQUE KEY `Description_UNIQUE` (`StatusName`),
  KEY `FK_Status_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Status_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Status_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Status_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'Pending',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Request Sent For Approval',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Rejected',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Approved',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'Closed',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,'Posted',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taxregister`
--

DROP TABLE IF EXISTS `taxregister`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taxregister` (
  `TaxRegisterId` int NOT NULL AUTO_INCREMENT,
  `TaxRegisterName` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`TaxRegisterId`),
  UNIQUE KEY `Name_UNIQUE` (`TaxRegisterName`),
  KEY `FK_TaxRegister_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_TaxRegister_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_TaxRegister_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_TaxRegister_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taxregister`
--

LOCK TABLES `taxregister` WRITE;
/*!40000 ALTER TABLE `taxregister` DISABLE KEYS */;
INSERT INTO `taxregister` VALUES (1,'Tax Register 1',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `taxregister` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taxregisterdetail`
--

DROP TABLE IF EXISTS `taxregisterdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taxregisterdetail` (
  `TaxRegisterDetId` int NOT NULL AUTO_INCREMENT,
  `TaxRegisterId` int DEFAULT NULL,
  `SrNo` int DEFAULT NULL,
  `TaxLedgerId` int DEFAULT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `Rate` decimal(18,4) DEFAULT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`TaxRegisterDetId`),
  KEY `IX_TaxRegisterDetails_TaxLedgerId` (`TaxLedgerId`),
  KEY `IX_TaxRegisterDetails_TaxRegisterId` (`TaxRegisterId`),
  KEY `FK_TaxRegisterDetails_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_TaxRegisterDetails_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_TaxRegisterDetails_Ledger_TaxLedgerId` FOREIGN KEY (`TaxLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_TaxRegisterDetails_TaxRegister_TaxRegisterId` FOREIGN KEY (`TaxRegisterId`) REFERENCES `taxregister` (`TaxRegisterId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_TaxRegisterDetails_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_TaxRegisterDetails_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taxregisterdetail`
--

LOCK TABLES `taxregisterdetail` WRITE;
/*!40000 ALTER TABLE `taxregisterdetail` DISABLE KEYS */;
INSERT INTO `taxregisterdetail` VALUES (1,1,1,105,'Percentage',5.0000,'Add',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `taxregisterdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `unitofmeasurement`
--

DROP TABLE IF EXISTS `unitofmeasurement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `unitofmeasurement` (
  `UnitOfMeasurementId` int NOT NULL AUTO_INCREMENT,
  `UnitOfMeasurementName` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`UnitOfMeasurementId`),
  UNIQUE KEY `Name_UNIQUE` (`UnitOfMeasurementName`),
  KEY `FK_UnitOfMeasurement_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_UnitOfMeasurement_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_UnitOfMeasurement_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_UnitOfMeasurement_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `unitofmeasurement`
--

LOCK TABLES `unitofmeasurement` WRITE;
/*!40000 ALTER TABLE `unitofmeasurement` DISABLE KEYS */;
INSERT INTO `unitofmeasurement` VALUES (1,'KG',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'MTR',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'PCS',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'No',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `unitofmeasurement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vouchersetup`
--

DROP TABLE IF EXISTS `vouchersetup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vouchersetup` (
  `VoucherSetupId` int NOT NULL AUTO_INCREMENT,
  `VoucherSetupName` varchar(500) NOT NULL,
  `ModuleId` int DEFAULT NULL,
  `IsActive` tinyint DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VoucherSetupId`),
  UNIQUE KEY `Name_UNIQUE` (`VoucherSetupName`),
  KEY `IX_VoucherSetup_Module_ModuleId_idx` (`ModuleId`),
  KEY `FK_VoucherSetup_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_VoucherSetup_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_VoucherSetup_Module_ModuleId` FOREIGN KEY (`ModuleId`) REFERENCES `module` (`ModuleId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetup_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetup_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vouchersetup`
--

LOCK TABLES `vouchersetup` WRITE;
/*!40000 ALTER TABLE `vouchersetup` DISABLE KEYS */;
INSERT INTO `vouchersetup` VALUES (1,'Ledger',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(2,'Sales Invoice',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(3,'Purchase Invoice',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(4,'Credit Note',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(5,'Debit Note',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(6,'Receipt Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(7,'Payment Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(8,'Journal Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(9,'Advance Adjustment',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(10,'Currency Conversion',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(11,'Tax Register',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `vouchersetup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vouchersetupdetail`
--

DROP TABLE IF EXISTS `vouchersetupdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vouchersetupdetail` (
  `VoucherSetupDetId` int NOT NULL AUTO_INCREMENT,
  `VoucherSetupId` int DEFAULT NULL,
  `NoPad` char(1) DEFAULT NULL,
  `NoPreString` varchar(100) DEFAULT NULL,
  `NoPostString` varchar(100) DEFAULT NULL,
  `NoSeparator` varchar(100) DEFAULT NULL,
  `FormatText` varchar(100) DEFAULT NULL,
  `VoucherStyleId` int DEFAULT NULL,
  `CompanyId` int DEFAULT NULL,
  `FinancialYearId` int DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VoucherSetupDetId`),
  KEY `IX_VoucherSetupDetails_VoucherSetup_VoucherSetupId` (`VoucherSetupId`),
  KEY `IX_VoucherSetupDetails_VoucherStyle_VoucherStyleId` (`VoucherStyleId`),
  KEY `IX_VoucherSetupDetails_Company_CompanyId` (`CompanyId`),
  KEY `IX_VoucherSetupDetails_FinancialYear_FinancialYearId` (`FinancialYearId`),
  KEY `FK_VoucherSetupDetails_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_VoucherSetupDetails_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_VoucherSetupDetails_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetupDetails_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetupDetails_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetupDetails_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetupDetails_VoucherSetup_VoucherSetupId` FOREIGN KEY (`VoucherSetupId`) REFERENCES `vouchersetup` (`VoucherSetupId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetupDetails_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vouchersetupdetail`
--

LOCK TABLES `vouchersetupdetail` WRITE;
/*!40000 ALTER TABLE `vouchersetupdetail` DISABLE KEYS */;
INSERT INTO `vouchersetupdetail` VALUES (1,1,'0','UG','','','UG00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,2,'0','21-01-01','','-','21-01-01-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,3,'0','21-01-02','','-','21-01-02-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,4,'0','21-01-03','','-','21-01-03-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,5,'0','21-01-04','','-','21-01-04-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,6,'0','21-01-05','','-','21-01-05-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,7,'0','21-01-06','','-','21-01-06-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,8,'0','21-01-07','','-','21-01-07-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `vouchersetupdetail` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `voucherstyle`
--

DROP TABLE IF EXISTS `voucherstyle`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `voucherstyle` (
  `VoucherStyleId` int NOT NULL AUTO_INCREMENT,
  `VoucherStyleName` varchar(500) DEFAULT NULL,
  `PreparedByUserId` int DEFAULT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int DEFAULT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VoucherStyleId`),
  UNIQUE KEY `Name_UNIQUE` (`VoucherStyleName`),
  KEY `FK_VoucherStyle_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_VoucherStyle_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_VoucherStyle_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherStyle_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `voucherstyle`
--

LOCK TABLES `voucherstyle` WRITE;
/*!40000 ALTER TABLE `voucherstyle` DISABLE KEYS */;
INSERT INTO `voucherstyle` VALUES (1,'Aplha Numeric',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Date',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'User Defined',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `voucherstyle` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-09-21 12:29:31

-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: erpdb
-- ------------------------------------------------------
-- Server version	8.0.27

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
  `ParticularLedgerId` int NOT NULL,
  `PaymentVoucherDetId` int DEFAULT NULL,
  `ReceiptVoucherDetId` int DEFAULT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AdvanceAdjustmentId`),
  UNIQUE KEY `DocumentNo_UNIQUE` (`AdvanceAdjustmentNo`),
  KEY `IX_AdvanceAdjustment_ParticularLedgerId` (`ParticularLedgerId`),
  KEY `IX_AdvanceAdjustment_CompanyId` (`CompanyId`),
  KEY `IX_AdvanceAdjustment_FinancialYearId` (`FinancialYearId`),
  KEY `IX_AdvanceAdjustment_StatusId` (`StatusId`),
  KEY `IX_AdvanceAdjustment_VoucherStyleId` (`VoucherStyleId`),
  KEY `FK_AdvanceAdjustment_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_AdvanceAdjustment_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  KEY `FK_AdvanceAdjustment_Currency_CurrencyId_idx` (`CurrencyId`),
  KEY `fk_advanceadjustment_VoucherDet_DetId` (`PaymentVoucherDetId`),
  KEY `fk_advanceadjustment_ReceiptVoucherDet_DetId` (`ReceiptVoucherDetId`),
  CONSTRAINT `FK_AdvanceAdjustment_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`),
  CONSTRAINT `FK_AdvanceAdjustment_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `fk_advanceadjustment_ReceiptVoucherDet_DetId` FOREIGN KEY (`ReceiptVoucherDetId`) REFERENCES `receiptvoucherdetail` (`ReceiptVoucherDetId`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_Status_StatusId` FOREIGN KEY (`StatusId`) REFERENCES `status` (`StatusId`) ON DELETE RESTRICT,
  CONSTRAINT `fk_advanceadjustment_VoucherDet_DetId` FOREIGN KEY (`PaymentVoucherDetId`) REFERENCES `paymentvoucherdetail` (`PaymentVoucherDetId`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `FK_AdvanceAdjustment_VoucherStyle_VoucherStyleId` FOREIGN KEY (`VoucherStyleId`) REFERENCES `voucherstyle` (`VoucherStyleId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advanceadjustment`
--

LOCK TABLES `advanceadjustment` WRITE;
/*!40000 ALTER TABLE `advanceadjustment` DISABLE KEYS */;
INSERT INTO `advanceadjustment` VALUES (15,'21-01-09-00001','2021-10-22 00:00:00',109,320,NULL,'fds ffdf ds fds fd f fdfddfs 76576 574 57 65 750-95 3543',1,1.000000,0.0000,0.0000,'Zero Only',1,1,1,1,1,3,'2021-10-22 20:12:47',3,'2021-10-23 18:04:16'),(16,'21-01-09-00002','2021-10-23 00:00:00',111,NULL,13,'dffssfds',2,0.050428,20.0000,396.6051,'DHM Twenty  Only',4,1,1,2,1,3,'2021-10-23 16:57:37',3,'2021-10-23 18:38:05'),(17,'21-01-09-00003','2021-10-23 00:00:00',111,NULL,13,NULL,2,0.050428,0.0000,0.0000,'Zero Only',1,1,1,3,1,3,'2021-10-23 17:06:01',3,'2021-10-23 18:36:58'),(18,'21-01-09-00004','2021-10-23 00:00:00',109,321,NULL,NULL,2,0.050428,345.0000,6841.4373,'DHM Three Hundred Fourty Five  Only',4,1,1,4,1,3,'2021-10-23 18:07:56',3,'2021-10-23 18:36:11');
/*!40000 ALTER TABLE `advanceadjustment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advanceadjustmentattachment`
--

DROP TABLE IF EXISTS `advanceadjustmentattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advanceadjustmentattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `AdvanceAdjustmentId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_AdvAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_AdvAttachment_AdvanceAdjustment_AdvanceAdjustmentId_idx` (`AdvanceAdjustmentId`),
  KEY `FK_AdvAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_AdvAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_AdvAttachment_AdvanceAdjustment_AdvanceAdjustmentId` FOREIGN KEY (`AdvanceAdjustmentId`) REFERENCES `advanceadjustment` (`AdvanceAdjustmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AdvAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advanceadjustmentattachment`
--

LOCK TABLES `advanceadjustmentattachment` WRITE;
/*!40000 ALTER TABLE `advanceadjustmentattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `advanceadjustmentattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `advanceadjustmentdetail`
--

DROP TABLE IF EXISTS `advanceadjustmentdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advanceadjustmentdetail` (
  `AdvanceAdjustmentDetId` int NOT NULL AUTO_INCREMENT,
  `AdvanceAdjustmentId` int NOT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advanceadjustmentdetail`
--

LOCK TABLES `advanceadjustmentdetail` WRITE;
/*!40000 ALTER TABLE `advanceadjustmentdetail` DISABLE KEYS */;
INSERT INTO `advanceadjustmentdetail` VALUES (24,16,20.0000,396.6051,NULL,20,NULL,NULL,NULL,3,'2021-10-23 17:28:35',3,'2021-10-23 18:37:57'),(27,18,345.0000,6841.4373,'ssdd sdsdsd sdssdd sds ',NULL,23,NULL,NULL,3,'2021-10-23 18:08:13',3,'2021-10-23 18:08:19');
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES (3,'Role 34','ROLE 34','2ba09144-0a47-46a4-a3bf-838c69925106'),(5,'role 1','ROLE 1','9a1fbf49-4b39-4695-9e4d-131fe82d7da8'),(6,'role 2','ROLE 2','e25f5713-cbe8-4eae-b330-82a68ac6a414');
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
INSERT INTO `aspnetuserroles` VALUES (3,3),(3,6);
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
  `EmployeeId` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`),
  KEY `FK_aspnetusers_Employee_EmployeeId_idx` (`EmployeeId`),
  CONSTRAINT `FK_aspnetusers_Employee_EmployeeId` FOREIGN KEY (`EmployeeId`) REFERENCES `employee` (`EmployeeId`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES (1,'aadarsh@gmail.com','AADARSH@GMAIL.COM','aadarsh@gmail.com','AADARSH@GMAIL.COM',0,'AQAAAAEAACcQAAAAENmYRZUUmr8yY3aWOHQmN/zSoLiiSuiyNYe6wQuGYxkcqCq+1S1vLE35vrpXa/un/w==','ZP6HXHJ6DNSG5LDNTN665AXIDJVWSSAB','1086e990-6c98-4438-b23c-b0ff7eac2653',NULL,0,0,'2022-01-09 08:42:54.670118',0,0,1),(3,'admin@gmail.com','ADMIN@GMAIL.COM','admin@gmail.com','ADMIN@GMAIL.COM',1,'AQAAAAEAACcQAAAAEItBQw5Y7S5BMR/qnKKavjY8XJq5wP9tF4uoRY+20NdZgVJSQqo5F7bDbwtRU+HIng==','HM7AKWATLNJLELJEP7ZBTDHCYQBZ2LZW','06823307-be7e-4848-b798-0ebf30a5ce11',NULL,0,0,'2022-01-09 05:08:59.255998',0,0,1),(32,'admin1234@gmail.com','ADMIN1234@GMAIL.COM','admin1234@gmail.com','ADMIN1234@GMAIL.COM',0,'AQAAAAEAACcQAAAAEDwFp+RrqvDQihlQfo/l38Oz6BxpanBVZ7eeF53Nj6JZHve6JO2HH5CR0RP2Yp+YvA==','U4JTWOUL7SIS3EIYTYSF7AJLQK4WLBZU','8db87c3b-b075-49ba-8025-153344a3963a',NULL,0,0,'2022-01-09 05:09:01.072806',1,0,3);
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
-- Table structure for table `attachment`
--

DROP TABLE IF EXISTS `attachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachment` (
  `AttachmentId` int NOT NULL AUTO_INCREMENT,
  `CategoryId` int DEFAULT NULL,
  `Description` varchar(200) DEFAULT NULL,
  `GUIDNo` varchar(100) DEFAULT NULL,
  `ContainerName` varchar(1000) DEFAULT NULL,
  `ServerFileName` varchar(1000) DEFAULT NULL,
  `UserFileName` varchar(1000) DEFAULT NULL,
  `FileExtension` varchar(100) DEFAULT NULL,
  `ContentType` varchar(100) DEFAULT NULL,
  `ContentLength` bigint DEFAULT NULL,
  `URL` varchar(1000) DEFAULT NULL,
  `StorageAccountId` int DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AttachmentId`),
  KEY `FK_Attachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Attachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  KEY `FK_Attachment_AttachmentCategory_CategoryId_idx` (`CategoryId`),
  KEY `FK_Attachment_StorageAccount_StorageAccountId_idx` (`StorageAccountId`),
  CONSTRAINT `FK_Attachment_AttachmentCategory_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `attachmentcategory` (`CategoryId`),
  CONSTRAINT `FK_Attachment_StorageAccount_StorageAccountId` FOREIGN KEY (`StorageAccountId`) REFERENCES `attachmentstorageaccount` (`StorageAccountId`),
  CONSTRAINT `FK_Attachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_Attachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attachment`
--

LOCK TABLES `attachment` WRITE;
/*!40000 ALTER TABLE `attachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `attachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attachmentcategory`
--

DROP TABLE IF EXISTS `attachmentcategory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachmentcategory` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CategoryId`),
  UNIQUE KEY `CategoryName_UNIQUE` (`CategoryName`),
  KEY `FK_AttachmentCategory_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_AttachmentCategory_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_AttachmentCategory_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AttachmentCategory_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attachmentcategory`
--

LOCK TABLES `attachmentcategory` WRITE;
/*!40000 ALTER TABLE `attachmentcategory` DISABLE KEYS */;
INSERT INTO `attachmentcategory` VALUES (1,'Purchase Order',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Sales Order',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Supplier Invoice',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Delivery Order',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'GRN',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `attachmentcategory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attachmentstorageaccount`
--

DROP TABLE IF EXISTS `attachmentstorageaccount`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attachmentstorageaccount` (
  `StorageAccountId` int NOT NULL AUTO_INCREMENT,
  `AccountName` varchar(250) NOT NULL,
  `AccountKey` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`StorageAccountId`),
  KEY `FK_AttchmentAccount_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_AttchmentAccount_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_AttchmentAccount_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_AttchmentAccount_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attachmentstorageaccount`
--

LOCK TABLES `attachmentstorageaccount` WRITE;
/*!40000 ALTER TABLE `attachmentstorageaccount` DISABLE KEYS */;
INSERT INTO `attachmentstorageaccount` VALUES (1,'eteststorage','wY08JqSVcIh70MuMrpgLsJ5D5HU3Bxn4sSvDD3kh4kCN/cwRHaPjkBd6G5uy5e35Tz4fqQttk3JtkkciuPioxg==',1,'2022-01-01 00:00:00',1,'2022-01-01 00:00:00');
/*!40000 ALTER TABLE `attachmentstorageaccount` ENABLE KEYS */;
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
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `city` VALUES (2,'Surat1',2,1,'2021-01-01 00:00:00',1,'2021-04-24 19:22:55'),(9,'Mumbai1 123',1,1,'2021-04-21 21:43:49',3,'2021-09-30 16:18:03'),(11,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(12,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(13,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(14,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(15,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(16,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(17,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(21,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(28,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(29,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(30,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(31,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(32,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(33,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(34,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(35,'Surat',2,1,'2021-01-01 00:00:00',1,'2021-04-21 21:57:31'),(36,'Mumbai1',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:44:24'),(37,'Mumbai',1,1,'2021-04-21 21:43:49',1,'2021-04-21 21:43:49'),(38,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 21:50:22'),(39,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-21 22:00:05'),(40,'Mumbaids',1,1,'2021-04-21 21:50:22',1,'2021-04-23 21:51:43'),(41,'Surat11',2,1,'2021-04-24 19:23:11',1,'2021-04-24 19:23:11');
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
  `CurrencyId` int NOT NULL,
  `NoOfDecimals` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CompanyId`),
  UNIQUE KEY `CompanyName_UNIQUE` (`CompanyName`),
  KEY `IX_Company_CurrencyId` (`CurrencyId`),
  KEY `FK_Company_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Company_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Company_Currency_CurrencyId` FOREIGN KEY (`CurrencyId`) REFERENCES `currency` (`CurrencyId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Company_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Company_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `company`
--

LOCK TABLES `company` WRITE;
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` VALUES (1,'Company 1','Thane, Mumbai 1','company1@gmail.com','www.compnay1.com','123456789','987654321','23450987','123456',1,2,1,'2021-01-01 00:00:00',1,'2021-08-15 14:52:55'),(4,'Company 2','hdskhdsak','ewewew@uytut.com','www.xyx.com','6868768','4445454','5545454','545454',2,3,3,'2022-01-09 12:35:39',3,'2022-01-09 12:35:39');
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
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `DebitAmount_FC` decimal(18,4) NOT NULL,
  `DebitAmount` decimal(18,4) NOT NULL,
  `CreditAmount_FC` decimal(18,4) NOT NULL,
  `CreditAmount` decimal(18,4) NOT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contravoucher`
--

LOCK TABLES `contravoucher` WRITE;
/*!40000 ALTER TABLE `contravoucher` DISABLE KEYS */;
INSERT INTO `contravoucher` VALUES (14,'21-01-08-00001','2021-10-17 00:00:00',2,0.050428,'jghj gjhgj gjgjgj','89778979','2021-10-05 00:00:00',345.0000,'DHM Three Hundred Fourty Five  Only',0.0000,345.0000,17.3977,345.0000,17.3977,4,1,1,1,1,3,'2021-10-17 22:02:33',3,'2021-10-17 22:04:02');
/*!40000 ALTER TABLE `contravoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contravoucherattachment`
--

DROP TABLE IF EXISTS `contravoucherattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contravoucherattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `ContraVoucherId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_CVAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_CVAttachment_ContraVoucher_ContraVoucherId_idx` (`ContraVoucherId`),
  KEY `FK_CVAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CVAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CVAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CVAttachment_ContraVoucher_ContraVoucherId` FOREIGN KEY (`ContraVoucherId`) REFERENCES `contravoucher` (`ContraVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CVAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CVAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contravoucherattachment`
--

LOCK TABLES `contravoucherattachment` WRITE;
/*!40000 ALTER TABLE `contravoucherattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `contravoucherattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contravoucherdetail`
--

DROP TABLE IF EXISTS `contravoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contravoucherdetail` (
  `ContraVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `ContraVoucherId` int NOT NULL,
  `ParticularLedgerId` int NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) NOT NULL,
  `DebitAmount` decimal(18,4) NOT NULL,
  `CreditAmount_FC` decimal(18,4) NOT NULL,
  `CreditAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contravoucherdetail`
--

LOCK TABLES `contravoucherdetail` WRITE;
/*!40000 ALTER TABLE `contravoucherdetail` DISABLE KEYS */;
INSERT INTO `contravoucherdetail` VALUES (6,14,107,'ewew ewwe r wrr ew',0.0000,0.0000,345.0000,17.3977,3,'2021-10-17 22:02:59',3,'2021-10-17 22:03:25'),(7,14,108,NULL,300.0000,15.1284,0.0000,0.0000,3,'2021-10-17 22:03:08',3,'2021-10-17 22:03:25'),(8,14,108,NULL,45.0000,2.2693,0.0000,0.0000,3,'2021-10-17 22:03:25',3,'2021-10-17 22:03:25');
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
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `country` VALUES (1,'INDIA',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'United Arab Emirates',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Oman1',1,NULL,1,NULL),(4,'KSA',1,NULL,1,NULL);
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
  `PartyLedgerId` int NOT NULL,
  `BillToAddressId` int NOT NULL,
  `AccountLedgerId` int NOT NULL,
  `PartyReferenceNo` varchar(250) DEFAULT NULL,
  `PartyReferenceDate` datetime DEFAULT NULL,
  `OurReferenceNo` varchar(250) DEFAULT NULL,
  `OurReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int NOT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) NOT NULL,
  `TotalLineItemAmount` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnote`
--

LOCK TABLES `creditnote` WRITE;
/*!40000 ALTER TABLE `creditnote` DISABLE KEYS */;
INSERT INTO `creditnote` VALUES (15,'21-01-03-00001','2021-10-19 00:00:00',111,5,127,'534','2021-09-29 00:00:00','344343','2021-10-20 00:00:00',33,'2 fvfdfddff','ddfsfd ffds ds fs fdsffsfds','LineWise',1,2,0.050410,45.0000,892.6800,43.2000,856.9728,'Percentage',4.0000,1.8000,35.7072,2.2500,44.6340,45.4500,901.6068,'DHM Fourty Five and Four Five Paisa Only',1,1,1,1,1,3,'2021-10-09 20:37:04',3,'2021-10-09 21:00:24');
/*!40000 ALTER TABLE `creditnote` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnoteattachment`
--

DROP TABLE IF EXISTS `creditnoteattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnoteattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_CNAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_CNAttachment_CreditNote_CreditNoteId_idx` (`CreditNoteId`),
  KEY `FK_CNAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CNAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CNAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CNAttachment_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CNAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CNAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnoteattachment`
--

LOCK TABLES `creditnoteattachment` WRITE;
/*!40000 ALTER TABLE `creditnoteattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `creditnoteattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `creditnotedetail`
--

DROP TABLE IF EXISTS `creditnotedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `creditnotedetail` (
  `CreditNoteDetId` int NOT NULL AUTO_INCREMENT,
  `CreditNoteId` int NOT NULL,
  `SrNo` int NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int NOT NULL,
  `Quantity` decimal(18,2) NOT NULL,
  `PerUnit` int NOT NULL,
  `UnitPrice` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CreditNoteDetId`),
  KEY `IX_CreditNoteDetails_CreditNoteId` (`CreditNoteId`) /*!80000 INVISIBLE */,
  KEY `IX_CreditNoteDetails_UnitOfMeasurementId` (`UnitOfMeasurementId`),
  KEY `FK_CreditNoteDetails_aspnetusers_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_CreditNoteDetails_aspnetusers_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_CreditNoteDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_CreditNoteDetails_UnitOfMeasurement_UnitOfMeasurementId` FOREIGN KEY (`UnitOfMeasurementId`) REFERENCES `unitofmeasurement` (`UnitOfMeasurementId`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotedetail`
--

LOCK TABLES `creditnotedetail` WRITE;
/*!40000 ALTER TABLE `creditnotedetail` DISABLE KEYS */;
INSERT INTO `creditnotedetail` VALUES (8,15,1,'kjlfdsjfldsj',1,1.00,1,45.0000,45.0000,892.6800,2.2500,44.6340,47.2500,937.3140,3,'2021-10-09 20:59:52',3,'2021-10-09 21:00:24');
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
  `CreditNoteDetId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotedetailtax`
--

LOCK TABLES `creditnotedetailtax` WRITE;
/*!40000 ALTER TABLE `creditnotedetailtax` DISABLE KEYS */;
INSERT INTO `creditnotedetailtax` VALUES (24,8,1,105,'Percentage',5.0000,'Add',2.2500,44.6340,NULL,3,'2021-10-09 21:00:12',3,'2021-10-09 21:00:23');
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
  `CreditNoteId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `creditnotetax`
--

LOCK TABLES `creditnotetax` WRITE;
/*!40000 ALTER TABLE `creditnotetax` DISABLE KEYS */;
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
  `CurrencyName` varchar(250) DEFAULT NULL,
  `Denomination` varchar(50) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`CurrencyId`),
  UNIQUE KEY `Name_UNIQUE` (`CurrencyName`),
  UNIQUE KEY `CurrencyCode_UNIQUE` (`CurrencyCode`),
  KEY `FK_Currency_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Currency_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Currency_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Currency_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currency`
--

LOCK TABLES `currency` WRITE;
/*!40000 ALTER TABLE `currency` DISABLE KEYS */;
INSERT INTO `currency` VALUES (1,'INR','Indian Rupee','Paisa',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'DHM','UAE Dhiram','Paisa',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'SAR','Saudi Riyal','Paisa',1,'2021-08-30 09:09:40',3,'2021-10-23 18:50:12'),(5,'USD','United States Dollar','CENTS',3,'2021-10-23 18:51:30',3,'2021-10-23 18:51:30');
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
  `CompanyId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `EffectiveDateTime` datetime DEFAULT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `currencyconversion`
--

LOCK TABLES `currencyconversion` WRITE;
/*!40000 ALTER TABLE `currencyconversion` DISABLE KEYS */;
INSERT INTO `currencyconversion` VALUES (2,1,2,'2021-01-01 00:00:00',0.050429,1,'2021-01-01 00:00:00',3,'2021-12-24 18:07:16'),(6,1,2,'2021-10-23 00:00:00',0.050456,3,'2021-10-23 22:05:13',3,'2021-10-23 22:05:13'),(7,1,1,'2021-12-24 00:00:00',1.000000,3,'2021-12-24 17:59:52',3,'2021-12-24 17:59:52'),(8,1,5,'2021-12-24 00:00:00',0.274709,3,'2021-12-24 18:07:49',3,'2021-12-24 18:07:49');
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
  `PartyLedgerId` int NOT NULL,
  `BillToAddressId` int NOT NULL,
  `AccountLedgerId` int NOT NULL,
  `PartyReferenceNo` varchar(250) DEFAULT NULL,
  `PartyReferenceDate` datetime DEFAULT NULL,
  `OurReferenceNo` varchar(250) DEFAULT NULL,
  `OurReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int NOT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) NOT NULL,
  `TotalLineItemAmount` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnote`
--

LOCK TABLES `debitnote` WRITE;
/*!40000 ALTER TABLE `debitnote` DISABLE KEYS */;
INSERT INTO `debitnote` VALUES (16,'21-01-04-00001','2021-10-09 00:00:00',109,1,127,'534','2021-08-04 00:00:00','344343','2021-08-10 00:00:00',334,'2 fvfdfddff','dfsdf fdfds sfs sfdfsd','LineWise',1,2,0.050428,1014.0000,20107.8766,963.3000,19102.4827,'Percentage',5.0000,50.7000,1005.3938,1.2000,23.7963,964.5000,19126.2791,'DHM Nine Hundred Sixty Four and Five Paisa Only',1,1,1,1,1,3,'2021-10-09 20:26:00',3,'2021-10-09 21:01:02'),(17,'21-01-04-00002','2021-10-09 00:00:00',109,1,118,'534','2021-08-04 00:00:00','344343','2021-08-19 00:00:00',22,'kjhkh','dafd  fdssfdfsd','LineWise',1,2,0.050428,132.0000,2617.5934,128.0400,2539.0656,'Percentage',3.0000,3.9600,78.5278,1.3200,26.1759,129.3600,2565.2415,'DHM One Hundred Twenty Nine and Three Six Paisa Only',1,1,1,2,1,3,'2021-10-09 20:28:57',3,'2021-10-09 20:29:33');
/*!40000 ALTER TABLE `debitnote` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnoteattachment`
--

DROP TABLE IF EXISTS `debitnoteattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnoteattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_DNAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_DNAttachment_DebitNote_DebitNoteId_idx` (`DebitNoteId`),
  KEY `FK_DNAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_DNAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_DNAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DNAttachment_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DNAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_DNAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnoteattachment`
--

LOCK TABLES `debitnoteattachment` WRITE;
/*!40000 ALTER TABLE `debitnoteattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `debitnoteattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `debitnotedetail`
--

DROP TABLE IF EXISTS `debitnotedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `debitnotedetail` (
  `DebitNoteDetId` int NOT NULL AUTO_INCREMENT,
  `DebitNoteId` int NOT NULL,
  `SrNo` int NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int NOT NULL,
  `Quantity` decimal(18,2) NOT NULL,
  `PerUnit` int NOT NULL,
  `UnitPrice` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnotedetail`
--

LOCK TABLES `debitnotedetail` WRITE;
/*!40000 ALTER TABLE `debitnotedetail` DISABLE KEYS */;
INSERT INTO `debitnotedetail` VALUES (6,16,1,' wre wrewrrrwerwe',1,4.00,1,6.0000,24.0000,475.9261,1.2000,23.7963,25.2000,499.7224,3,'2021-10-09 20:26:27',3,'2021-10-09 21:01:02'),(7,16,2,'6',1,5.00,3,66.0000,990.0000,19631.9505,49.5000,981.5975,1039.5000,20613.5480,3,'2021-10-09 20:26:44',3,'2021-10-09 21:01:02'),(8,17,1,' rrr rre',1,3.00,1,44.0000,132.0000,2617.5934,1.3200,26.1759,133.3200,2643.7693,3,'2021-10-09 20:29:13',3,'2021-10-09 20:29:33');
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
  `DebitNoteDetId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `debitnotedetailtax`
--

LOCK TABLES `debitnotedetailtax` WRITE;
/*!40000 ALTER TABLE `debitnotedetailtax` DISABLE KEYS */;
INSERT INTO `debitnotedetailtax` VALUES (22,8,1,105,'Percentage',5.0000,'Add',6.6000,130.8797,'',3,'2021-10-09 20:29:13',3,'2021-10-09 20:29:17'),(23,8,2,105,'Percentage',4.0000,'Deduct',-5.2800,-104.7037,NULL,3,'2021-10-09 20:29:32',3,'2021-10-09 20:29:32'),(24,6,1,105,'Percentage',5.0000,'Add',1.2000,23.7963,'',3,'2021-10-09 21:01:02',3,'2021-10-09 21:01:02'),(25,7,1,105,'Percentage',5.0000,'Add',49.5000,981.5975,'',3,'2021-10-09 21:01:02',3,'2021-10-09 21:01:02');
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
  `DebitNoteId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `DepartmentName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `DesignationName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `EmployeeCode` varchar(50) NOT NULL,
  `FirstName` varchar(250) DEFAULT NULL,
  `LastName` varchar(250) DEFAULT NULL,
  `DesignationId` int NOT NULL,
  `DepartmentId` int NOT NULL,
  `EmailAddress` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `employee` VALUES (1,'EMP00001','Sam','Ten',2,1,'employee1@gmail.com',1,'2021-01-01 00:00:00',1,'2021-08-15 14:53:26'),(2,'EMP00002','Harry','Potter',1,1,'employee2@gmail.com',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'E0003','Bret','Lee',2,2,NULL,1,NULL,1,NULL);
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `errorlog`
--

DROP TABLE IF EXISTS `errorlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `errorlog` (
  `LogId` int NOT NULL AUTO_INCREMENT,
  `Level` varchar(250) DEFAULT NULL,
  `RaiseDateTime` datetime DEFAULT NULL,
  `Message` varchar(5000) DEFAULT NULL,
  `Exception` varchar(5000) DEFAULT NULL,
  `UserName` varchar(250) DEFAULT NULL,
  `RawURL` varchar(1000) DEFAULT NULL,
  `Area` varchar(250) DEFAULT NULL,
  `Controller` varchar(250) DEFAULT NULL,
  `Action` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`LogId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `errorlog`
--

LOCK TABLES `errorlog` WRITE;
/*!40000 ALTER TABLE `errorlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `errorlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `financialyear`
--

DROP TABLE IF EXISTS `financialyear`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `financialyear` (
  `FinancialYearId` int NOT NULL AUTO_INCREMENT,
  `FinancialYearName` varchar(250) NOT NULL,
  `FromDate` datetime DEFAULT NULL,
  `ToDate` datetime DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `financialyear` VALUES (1,'01-Jan-2021 To 31-Dec-2021 ','2021-01-01 00:00:00','2021-12-31 00:00:00',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'01-Jan-2022 To 31-Dec-2022','2022-01-01 00:00:00','2022-12-31 00:00:00',1,NULL,1,NULL);
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
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `financialyearcompanyrelation` VALUES (1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,1,2,1,NULL,1,NULL);
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
  `ModuleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) NOT NULL,
  `DebitAmount` decimal(18,4) NOT NULL,
  `CreditAmount_FC` decimal(18,4) NOT NULL,
  `CreditAmount` decimal(18,4) NOT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journalvoucher`
--

LOCK TABLES `journalvoucher` WRITE;
/*!40000 ALTER TABLE `journalvoucher` DISABLE KEYS */;
INSERT INTO `journalvoucher` VALUES (14,'21-01-06-00001','2021-10-15 00:00:00',2,0.050428,'veevyryyetyytvyvvytrtrvy',50.0000,0.0000,'DHM Fifty  Only',50.0000,2.5214,50.0000,2.5214,7,1,1,1,1,3,'2021-10-15 22:52:04',3,'2021-10-16 23:38:04'),(15,'21-01-06-00002','2021-10-16 00:00:00',2,0.050428,NULL,35.0000,0.0000,'DHM Thirty Five  Only',0.0000,0.0000,0.0000,0.0000,1,1,1,2,1,3,'2021-10-16 20:48:24',3,'2021-10-16 20:48:24');
/*!40000 ALTER TABLE `journalvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `journalvoucherattachment`
--

DROP TABLE IF EXISTS `journalvoucherattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `journalvoucherattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `JournalVoucherId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_JVAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_JVAttachment_JournalVoucher_JournalVoucherId_idx` (`JournalVoucherId`),
  KEY `FK_JVAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_JVAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_JVAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JVAttachment_JournalVoucher_JournalVoucherId` FOREIGN KEY (`JournalVoucherId`) REFERENCES `journalvoucher` (`JournalVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JVAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_JVAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journalvoucherattachment`
--

LOCK TABLES `journalvoucherattachment` WRITE;
/*!40000 ALTER TABLE `journalvoucherattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `journalvoucherattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `journalvoucherdetail`
--

DROP TABLE IF EXISTS `journalvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `journalvoucherdetail` (
  `JournalVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `JournalVoucherId` int NOT NULL,
  `ParticularLedgerId` int NOT NULL,
  `TransactionTypeId` int NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `DebitAmount_FC` decimal(18,4) NOT NULL,
  `DebitAmount` decimal(18,4) NOT NULL,
  `CreditAmount_FC` decimal(18,4) NOT NULL,
  `CreditAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journalvoucherdetail`
--

LOCK TABLES `journalvoucherdetail` WRITE;
/*!40000 ALTER TABLE `journalvoucherdetail` DISABLE KEYS */;
INSERT INTO `journalvoucherdetail` VALUES (55,14,111,2,NULL,NULL,NULL,NULL,NULL,0.0000,0.0000,50.0000,2.5214,3,'2021-10-16 23:29:45',3,'2021-10-16 23:37:47'),(56,14,109,2,NULL,NULL,16,NULL,NULL,45.0000,2.2693,0.0000,0.0000,3,'2021-10-16 23:37:34',3,'2021-10-16 23:37:47'),(57,14,124,1,NULL,NULL,NULL,NULL,NULL,5.0000,0.2521,0.0000,0.0000,3,'2021-10-16 23:37:47',3,'2021-10-16 23:37:47');
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
  `IsGroup` tinyint NOT NULL,
  `IsMasterGroup` tinyint NOT NULL,
  `ParentGroupId` int DEFAULT NULL,
  `IsDeActive` tinyint NOT NULL,
  `TaxRegisteredNo` varchar(100) DEFAULT NULL,
  `MaxNo` int NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`LedgerId`),
  UNIQUE KEY `Code_UNIQUE` (`LedgerCode`),
  KEY `IX_Ledger_ParentGroupId` (`ParentGroupId`),
  KEY `FK_Ledger_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Ledger_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Ledger_Ledger_ParentGroupId` FOREIGN KEY (`ParentGroupId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_Ledger_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Ledger_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=148 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledger`
--

LOCK TABLES `ledger` WRITE;
/*!40000 ALTER TABLE `ledger` DISABLE KEYS */;
INSERT INTO `ledger` VALUES (1,'M000001','LIABILITIES',1,1,NULL,0,NULL,1,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'M000002','ASSETS',1,1,NULL,0,NULL,2,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'M000003','EXPENDITURES (Trading A/C)',1,1,NULL,0,NULL,3,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'M000004','INCOME (Trading A/C)',1,1,NULL,0,NULL,4,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'M000005','EXPENDITURES (P & L A/C)',1,1,NULL,0,NULL,5,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,'M000006','INCOME (P & L A/C)',1,1,NULL,0,NULL,6,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'M000007','PROFIT & LOSS A/C',1,1,NULL,0,NULL,7,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,'M000008','Capital Accounts  ',1,1,1,0,NULL,8,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,'M000009','Reserves & Surplus',1,1,1,0,NULL,9,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(10,'M000010','Loans (LIABILITIES)',1,1,1,0,NULL,10,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(11,'M000011','Current Liabilities',1,1,1,0,NULL,11,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,'M000012','Branch/Division',1,1,1,0,NULL,12,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,'M000013','Suspense A/C',1,1,1,0,NULL,13,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,'M000014','Bank OD A/C',1,1,1,0,NULL,14,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,'M000015','Secured Loans',1,1,1,0,NULL,15,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,'M000016','Sundry Creditor',1,1,1,0,NULL,16,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,'M000017','Duties & Taxes',1,1,1,0,NULL,17,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,'M000018','Privision',1,1,1,0,NULL,18,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(19,'M000019','Unsecured Loans',1,1,1,0,NULL,19,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(20,'M000020','Fixed Assets',1,1,2,0,NULL,20,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(21,'M000021','Investments',1,1,2,0,NULL,21,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(22,'M000022','Current Assets',1,1,2,0,NULL,22,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(23,'M000023','Loans and Advances (ASSET)',1,1,2,0,NULL,23,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(24,'M000024','Misc. Expenditures (ASSET)',1,1,2,0,NULL,24,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(25,'M000025','Opening Stock',1,1,2,0,NULL,25,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(26,'M000026','Sundry Debtor',1,1,2,0,NULL,26,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(27,'M000027','Bank A/C',1,1,2,0,NULL,27,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(28,'M000028','Cash-in-hand',1,1,2,0,NULL,28,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(29,'M000029','Deposits (Assets)',1,1,2,0,NULL,29,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(30,'M000030','Stock-in-hand (Closing Stock)',1,1,2,0,NULL,30,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(31,'M000031','Direct Expenses',1,1,3,0,NULL,31,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(32,'M000032','Direct Income',1,1,4,0,NULL,32,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(33,'M000033','Closing Stock',1,1,4,0,NULL,33,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(34,'M000034','Indirect Expenses',1,1,5,0,NULL,34,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(35,'M000035','Indirect Income',1,1,6,0,NULL,35,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(36,'M000036','Current Period',1,1,7,0,NULL,36,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(37,'M000037','Opening Balance',1,1,7,0,NULL,37,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(38,'M000038','Freight & Handling Charges',1,1,36,0,NULL,38,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(101,'UG000101','Sales Account',0,0,32,0,NULL,101,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(102,'UG000102','Purchase Account',0,0,31,0,NULL,102,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(103,'UG000103','Other Sales',0,0,35,0,NULL,103,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(104,'UG000104','Other Purchase',0,0,34,0,NULL,104,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(105,'UG000105','VAT Input',0,0,17,0,NULL,105,NULL,1,'2021-01-01 00:00:00',3,'2022-01-12 15:22:54'),(106,'UG000106','VAT Output',0,0,17,0,NULL,106,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(107,'UG000107','Bank 1',0,0,27,0,NULL,107,'rrwrwrweewewr r rewre\r\nrew ewrew reerer\r\n54455454 5445 5',1,'2021-01-01 00:00:00',3,'2022-01-13 19:54:52'),(108,'UG000108','Bank 2',0,0,27,0,NULL,108,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(109,'UG000109','Sundry Creditor 1',0,0,16,0,'123456',109,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(110,'UG000110','Sundry Creditor 2',0,0,16,0,'8765432',110,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(111,'UG000111','Sundry Debtor 1',0,0,26,0,'DADFS34',111,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(112,'UG000112','Sundry Debtor 2',0,0,26,0,'545454DR',112,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(113,'UG000113','Opening Stock 1',0,0,25,0,NULL,113,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(114,'UG000114','Closing Stock 1',0,0,33,0,NULL,114,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(115,'UG000115','Cash In hand 1',0,0,28,0,NULL,115,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(116,'UG000116','Retained Profit 2020',0,0,9,0,NULL,116,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(117,'UG000117','Profit Account',0,0,7,0,NULL,117,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(118,'UG000118','Loss Account',0,0,7,0,NULL,118,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(119,'UG000119','Due from Directors',0,0,22,0,NULL,119,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(120,'UG000120','Goods in Transit',0,0,22,0,NULL,120,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(121,'UG000121','Stock in Hand',0,0,33,0,NULL,121,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(122,'UG000122','Electricity Deposit',0,0,29,0,NULL,122,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(123,'UG000123','Telecommunication Deposit',0,0,29,0,NULL,123,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(124,'UG000124','MIDC Deposit',0,0,29,0,NULL,124,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(125,'UG000125','Advances to Suppliers',0,0,23,0,NULL,125,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(126,'UG000126','Accrued Expenses Payable',0,0,11,0,NULL,126,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(127,'UG000127','Advances from Customers',0,0,11,0,NULL,127,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(128,'UG000128','Other Payables',0,0,11,0,NULL,128,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(129,'UG000129','VAT PAYABLE',0,0,11,0,NULL,129,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(130,'UG000130','Commission Provision',0,0,18,0,NULL,130,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(131,'UG000131','LD Charges Provision',0,0,18,0,NULL,131,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(132,'UG000132','Staff Benefits Provision',0,0,18,0,NULL,132,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(133,'UG000133','Bonus Expenses',0,0,34,0,NULL,133,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(134,'UG000134','Gratuity Expenses',0,0,34,0,NULL,134,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(135,'UG000135','Salary Expenses',0,0,34,0,NULL,135,NULL,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(136,'UG000136','dfd sdfsfdf sdfsd qq33',0,0,25,0,'gfdgddf',136,NULL,3,'2021-10-29 00:23:53',3,'2021-10-29 00:27:35'),(142,'UG00137','test 1',1,0,2,0,'45454sfdfsdfsdfsd',137,NULL,3,'2022-01-12 15:14:41',3,'2022-01-12 15:20:07'),(143,'UG00138','test 5454455fdfd ',0,0,27,0,NULL,138,NULL,3,'2022-01-12 15:23:23',3,'2022-01-12 15:23:23'),(144,'UG00139','Bank 323',1,0,27,0,NULL,139,NULL,3,'2022-01-12 20:44:54',3,'2022-01-12 20:45:27'),(145,'UG00140','4553535',0,0,144,0,NULL,140,NULL,3,'2022-01-12 21:49:43',3,'2022-01-12 21:49:43'),(146,'UG00141','43545454',0,0,14,0,NULL,141,NULL,3,'2022-01-12 21:49:57',3,'2022-01-12 21:49:57'),(147,'UG00142','3323223',1,0,16,0,'32323232',142,'wewwewew',3,'2022-01-13 20:32:35',3,'2022-01-13 20:33:16');
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
  `LedgerId` int NOT NULL,
  `AddressDescription` varchar(1000) DEFAULT NULL,
  `CountryId` int NOT NULL,
  `StateId` int NOT NULL,
  `CityId` int NOT NULL,
  `EmailAddress` varchar(100) DEFAULT NULL,
  `PhoneNo` varchar(20) DEFAULT NULL,
  `PostalCode` varchar(20) DEFAULT NULL,
  `FaxNo` varchar(20) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `ledgeraddress` VALUES (1,109,'Dubai 1, 123456, UAE',1,1,16,'sandry1@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:32:13'),(2,110,'Dubai 1, 123456, UAE1',1,1,37,'sandry2@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:32:36'),(3,110,'Dubai 1, 123456, UAE2',1,1,16,'sandry3@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:32:44'),(4,110,'Dubai 1, 123456, UAE3',1,1,11,'sandry4@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:32:53'),(5,111,'Dubai 1, 123456, UAE34',1,2,29,'sandry1@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:33:09'),(6,111,'Dubai 1, 123456, UAE4',1,1,9,'sa45ndry2@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',3,'2022-01-09 11:33:16'),(7,112,'Dubai 3, 123456, 76jjljl',1,1,2,'sandry3@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,113,'Dubai 4, 123456, 76jjljl',1,1,2,'sandry4@gmail.com','68686868','687686','8768768',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
/*!40000 ALTER TABLE `ledgeraddress` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledgerattachment`
--

DROP TABLE IF EXISTS `ledgerattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgerattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `LedgerId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_LedgerAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_LedgerAttachment_Ledger_LedgerId_idx` (`LedgerId`),
  KEY `FK_LedgerAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAttachment_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledgerattachment`
--

LOCK TABLES `ledgerattachment` WRITE;
/*!40000 ALTER TABLE `ledgerattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `ledgerattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ledgercompanyrelation`
--

DROP TABLE IF EXISTS `ledgercompanyrelation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgercompanyrelation` (
  `RelationId` int NOT NULL AUTO_INCREMENT,
  `CompanyId` int NOT NULL,
  `LedgerId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`RelationId`),
  KEY `IX_LedgerCompanyRelation_CompanyId` (`CompanyId`),
  KEY `IX_LedgerCompanyRelation_LedgerId` (`LedgerId`),
  KEY `FK_LedgerCompanyRelation_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerCompanyRelation_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerCompanyRelation_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`),
  CONSTRAINT `FK_LedgerCompanyRelation_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`),
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
  `LedgerId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `CreditAmount_OpBal` decimal(18,4) NOT NULL,
  `DebitAmount_OpBal` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`LedgerBalanceId`),
  KEY `IX_LedgerFinancialYearBalance_CompanyId` (`CompanyId`),
  KEY `IX_LedgerFinancialYearBalance_FinancialYearId` (`FinancialYearId`),
  KEY `IX_LedgerFinancialYearBalance_LedgerId` (`LedgerId`),
  KEY `FK_LedgerFinancialYearBalance_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_LedgerFinancialYearBalance_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_LedgerFinancialYearBalance_Company_CompanyId` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`CompanyId`),
  CONSTRAINT `FK_LedgerFinancialYearBalance_FinancialYear_FinancialYearId` FOREIGN KEY (`FinancialYearId`) REFERENCES `financialyear` (`FinancialYearId`),
  CONSTRAINT `FK_LedgerFinancialYearBalance_Ledger_LedgerId` FOREIGN KEY (`LedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_LedgerFinancialYearBalance_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_LedgerFinancialYearBalance_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=40 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ledgerfinancialyearbalance`
--

LOCK TABLES `ledgerfinancialyearbalance` WRITE;
/*!40000 ALTER TABLE `ledgerfinancialyearbalance` DISABLE KEYS */;
INSERT INTO `ledgerfinancialyearbalance` VALUES (1,101,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,102,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,103,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,104,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,105,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,106,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,107,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,108,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,109,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(10,110,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(11,111,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,112,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,113,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,114,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,115,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,116,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,117,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,118,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(19,119,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(20,120,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(21,121,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(22,122,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(23,123,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(24,124,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(25,125,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(26,126,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(27,127,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(28,128,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(29,129,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(30,130,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(31,131,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(32,132,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(33,133,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(34,134,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(35,135,1,1,0.0000,0.0000,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(39,107,2,1,15.0000,0.0000,3,'2022-01-12 22:43:45',3,'2022-01-12 22:49:30');
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
  `IsActive` tinyint NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `AccountLedgerId` int NOT NULL,
  `TypeCorB` varchar(1) DEFAULT NULL,
  `PaymentTypeId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `ChequeNo` varchar(50) DEFAULT NULL,
  `ChequeDate` datetime DEFAULT NULL,
  `ChequeAmount_FC` decimal(18,4) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentvoucher`
--

LOCK TABLES `paymentvoucher` WRITE;
/*!40000 ALTER TABLE `paymentvoucher` DISABLE KEYS */;
INSERT INTO `paymentvoucher` VALUES (26,'21-01-06-00001','2021-10-13 00:00:00',115,'C',2,2,0.050428,'89778979343','2021-09-28 00:00:00',31.0000,'ddasd dfds ffds fsdfds fdfdsfdsfdsdfsfdsdfs ljdsljl lksjdljdslkjds ljds ldjs ldjsk dlskjdslj dsljdslkdsj lsdjlkdsj dksj  ds\r\nsd jdsldjs dsjds\r\n dsdsj dsjds dsds pdjsljds ldsjl',20.4000,404.5372,'DHM Twenty and Four Paisa Only',1,1,1,1,1,3,'2021-10-13 18:02:21',3,'2022-01-09 12:59:38'),(27,'21-01-06-00002','2021-10-15 00:00:00',108,'B',1,2,0.050428,'pan 133132 dffd','2021-10-05 00:00:00',233.0000,'3ee  e ere re',261.0000,13.1617,'DHM Two Hundred Sixty One  Only',1,1,1,2,1,3,'2021-10-15 11:40:42',3,'2021-10-16 22:52:49'),(28,'21-01-06-00003','2021-10-16 00:00:00',115,'C',1,1,1.000000,'89778979','2021-09-28 00:00:00',3182.6124,'Narration',3182.6124,3182.6124,'INR Three Thousand One Hundred Eighty Two and Six One Two Four Paisa Only',1,1,1,3,1,3,'2021-10-16 22:54:07',3,'2021-10-17 14:39:05'),(29,'21-01-06-00004','2021-10-23 00:00:00',108,'B',2,2,0.050428,'76868tut','2021-10-06 00:00:00',450.0000,'sdsds',450.0000,8923.6139,'DHM Four Hundred Fifty  Only',4,1,1,4,1,3,'2021-10-23 18:06:55',3,'2021-10-23 18:07:41'),(30,'21-01-06-00005','2022-01-09 00:00:00',115,'C',2,1,1.000000,'43232432','2022-01-04 00:00:00',45.0000,'ewrrewrew',45.0000,45.0000,'INR Fourty Five  Only',4,1,1,5,1,3,'2022-01-09 10:28:47',3,'2022-01-09 10:30:12'),(31,'22-01-06-00001','2022-01-12 00:00:00',115,'C',1,2,0.050456,'43434334','2021-12-29 00:00:00',33.0000,'rerereerwtre',0.0000,0.0000,'Zero Only',1,1,2,1,1,3,'2022-01-12 13:05:14',3,'2022-01-12 13:05:14');
/*!40000 ALTER TABLE `paymentvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paymentvoucherattachment`
--

DROP TABLE IF EXISTS `paymentvoucherattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymentvoucherattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `PaymentVoucherId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_PVAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_PVAttachment_PaymentVoucher_PaymentVoucherId_idx` (`PaymentVoucherId`),
  KEY `FK_PVAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PVAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PVAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PVAttachment_PaymentVoucher_PaymentVoucherId` FOREIGN KEY (`PaymentVoucherId`) REFERENCES `paymentvoucher` (`PaymentVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PVAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PVAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentvoucherattachment`
--

LOCK TABLES `paymentvoucherattachment` WRITE;
/*!40000 ALTER TABLE `paymentvoucherattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `paymentvoucherattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paymentvoucherdetail`
--

DROP TABLE IF EXISTS `paymentvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymentvoucherdetail` (
  `PaymentVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `PaymentVoucherId` int NOT NULL,
  `ParticularLedgerId` int NOT NULL,
  `TransactionTypeId` int NOT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `PurchaseInvoiceId` int DEFAULT NULL,
  `DebitNoteId` int DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`PaymentVoucherDetId`),
  KEY `IX_PaymentVoucherDetails_PaymentVoucherId` (`PaymentVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_PaymentVoucherDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_PaymentVoucherDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_PaymentVoucherDetails_DebitNoteId` (`DebitNoteId`),
  KEY `IX_PaymentVoucherDetails_PurchaseInvoiceId` (`PurchaseInvoiceId`),
  KEY `IX_PaymentVoucherDetails_ParticularLedgerId` (`ParticularLedgerId`),
  CONSTRAINT `FK_PaymentVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_DebitNote_DebitNoteId` FOREIGN KEY (`DebitNoteId`) REFERENCES `debitnote` (`DebitNoteId`),
  CONSTRAINT `FK_PaymentVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_PaymentVoucherDetails_PaymentVoucher_PaymentVoucherId` FOREIGN KEY (`PaymentVoucherId`) REFERENCES `paymentvoucher` (`PaymentVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PaymentVoucherDetails_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=324 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentvoucherdetail`
--

LOCK TABLES `paymentvoucherdetail` WRITE;
/*!40000 ALTER TABLE `paymentvoucherdetail` DISABLE KEYS */;
INSERT INTO `paymentvoucherdetail` VALUES (305,27,126,1,233.0000,11.7497,'',NULL,NULL,3,'2021-10-15 11:50:35',3,'2021-10-15 11:50:42'),(306,27,109,2,5.0000,0.2521,NULL,16,NULL,3,'2021-10-15 11:51:02',3,'2021-10-15 11:51:02'),(312,26,127,1,20.0000,1.0086,NULL,NULL,NULL,3,'2021-10-15 20:14:22',3,'2021-10-15 20:14:22'),(313,27,109,2,20.0000,1.0086,NULL,17,NULL,3,'2021-10-16 22:52:21',3,'2021-10-16 22:52:21'),(314,27,109,2,3.0000,0.1513,'dffd ddfdf fd fgf fgghhg',NULL,16,3,'2021-10-16 22:52:49',3,'2021-10-16 22:52:49'),(315,28,109,2,3157.6124,3157.6124,'dfd ffddfd f fddfdf f',16,NULL,3,'2021-10-16 22:55:52',3,'2021-10-16 23:36:31'),(319,26,109,2,0.4000,0.0202,' rrerew sfdfdsfsdfds',17,NULL,3,'2021-10-16 23:21:09',3,'2021-10-16 23:21:09'),(320,28,109,2,25.0000,25.0000,'grr tr trttrttr',NULL,17,3,'2021-10-16 23:34:45',3,'2021-10-16 23:36:31'),(321,29,109,1,450.0000,8923.6139,NULL,NULL,NULL,3,'2021-10-23 18:07:14',3,'2021-10-23 18:07:14'),(322,30,107,1,45.0000,45.0000,NULL,NULL,NULL,3,'2022-01-09 10:29:21',3,'2022-01-09 10:29:21');
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
  `SupplierLedgerId` int NOT NULL,
  `BillToAddressId` int NOT NULL,
  `AccountLedgerId` int NOT NULL,
  `SupplierReferenceNo` varchar(250) DEFAULT NULL,
  `SupplierReferenceDate` datetime DEFAULT NULL,
  `CreditLimitDays` int NOT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) NOT NULL,
  `TaxRegisterId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) NOT NULL,
  `TotalLineItemAmount` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `DiscountPercentageOrAmount` varchar(250) NOT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoice`
--

LOCK TABLES `purchaseinvoice` WRITE;
/*!40000 ALTER TABLE `purchaseinvoice` DISABLE KEYS */;
INSERT INTO `purchaseinvoice` VALUES (16,'21-01-02-00001','2021-09-20 00:00:00',109,1,107,'5454','2021-08-18 00:00:00',45,'2 fvfd fddff dds332','oj ojdoj oj oajoj saojd so saoajo ajdojdo jado dojd ojdodsj oj sao jadsojdo dsaoj dosajdaosdasoj \r\nodsj odsjodsjodsj oadsjodsj od sjdso jdsojdsa ojdoj ojodasj oj odsaj oj dsajoadsjodsa dsaiidhidh dh dasodaojdasjdaojdaso\r\n','SubTotal',1,2,0.052300,690.0000,13193.1166,655.5000,12533.4608,'Percentage',5.0000,34.5000,659.6558,32.7750,626.6730,688.2750,13160.1338,'DHM Six Hundred Eighty Eight and Two Seven Five Paisa Only',7,1,1,1,1,3,'2021-09-20 15:26:58',3,'2021-10-15 11:46:22'),(17,'21-01-02-00002','2021-10-05 00:00:00',109,1,107,'5454','2021-08-10 00:00:00',5,'2 fvfdfddff','hgfhgffhh gfghfg hhfghgf','LineWise',1,2,0.050400,172.0000,3412.6984,161.6800,3207.9365,'Percentage',6.0000,10.3200,204.7619,12.0400,238.8889,173.7200,3446.8254,'DHM One Hundred Seventy Three and Seven Two Paisa Only',1,1,1,2,1,3,'2021-10-05 19:52:57',3,'2021-10-09 20:42:27'),(18,'21-01-02-00003','2021-10-06 00:00:00',109,1,107,'fdffgf fgddf','2021-08-26 00:00:00',5,'2 fvfdfddff','fddffdfd fd fdfdfd ','SubTotal',1,2,0.050430,2239.0000,44398.1757,2117.3260,41985.4456,'Percentage',5.4343,121.6740,2412.7301,106.3525,2108.9133,2239.0000,44398.1757,'DHM Two Thousand Two Hundred Thirty Nine  Only',1,1,1,3,1,3,'2021-10-06 16:43:47',3,'2021-10-06 16:52:42'),(19,'21-01-02-00004','2021-10-20 00:00:00',110,2,118,'supp ref','2021-10-14 00:00:00',3,'e rrwe rew rrew rwe',NULL,'SubTotal',1,2,0.050428,0.0000,0.0000,0.0000,0.0000,'Percentage',0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',2,1,1,4,1,3,'2021-10-06 17:22:16',3,'2021-10-08 10:58:38'),(20,'21-01-02-00005','2021-10-06 00:00:00',109,1,107,'fdv f dddfd','2021-08-25 00:00:00',332,'sdd fdsfds fdsfds',NULL,'SubTotal',1,1,1.000000,125.0000,125.0000,125.0000,125.0000,'Percentage',0.0000,0.0000,0.0000,6.2500,6.2500,125.0000,125.0000,'INR One Hundred Twenty Five  Only',4,1,1,5,1,3,'2021-10-06 17:26:48',3,'2021-10-08 01:06:41'),(21,'21-01-02-00006','2021-10-06 00:00:00',109,1,107,'fdv f dddfd','2021-08-25 00:00:00',332,'sdd fdsfds fdsfds',NULL,'SubTotal',1,1,1.000000,0.0000,0.0000,0.0000,0.0000,'Percentage',0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',1,1,1,6,1,3,'2021-10-06 17:27:42',3,'2021-10-08 11:01:59'),(22,'21-01-02-00007','2021-10-06 00:00:00',109,1,107,'ree  re23342  32423232424342','2021-08-26 00:00:00',5,'ADVFGF GDGDDSGGG RGRE RER RR','D F EW OREWHORRE]REERW\r\nERW\r\nEWRJ ERWJREW\r\n REWREWKREWJ \r\nRERE\r\nK','LineWise',1,2,0.050428,3714.0000,73649.5598,3639.7200,72176.5686,'Percentage',2.0000,74.2800,1472.9912,237.4500,4708.6936,3714.0000,73649.5598,'DHM Three Thousand Seven Hundred Fourteen  Only',1,1,1,7,1,3,'2021-10-06 22:27:35',3,'2021-10-06 23:10:11'),(23,'21-01-02-00008','2021-10-22 00:00:00',109,1,124,'fdv f dddfd','2021-09-28 00:00:00',3,'2 fvfdfddff','ewewew','LineWise',1,2,0.050428,1190.0000,23598.0011,1166.2000,23126.0411,'Percentage',2.0000,23.8000,471.9600,59.5000,1179.9001,1225.7000,24305.9411,'DHM One Thousand Two Hundred Twenty Five and Seven Paisa Only',4,1,1,8,1,3,'2021-10-23 18:05:40',3,'2021-10-23 18:06:05');
/*!40000 ALTER TABLE `purchaseinvoice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoiceattachment`
--

DROP TABLE IF EXISTS `purchaseinvoiceattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoiceattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `PurchaseInvoiceId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_PIAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_PIAttachment_PurchaseInvoice_PurchaseInvoiceId_idx` (`PurchaseInvoiceId`),
  KEY `FK_PIAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_PIAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_PIAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PIAttachment_PurchaseInvoice_PurchaseInvoiceId` FOREIGN KEY (`PurchaseInvoiceId`) REFERENCES `purchaseinvoice` (`PurchaseInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PIAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_PIAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoiceattachment`
--

LOCK TABLES `purchaseinvoiceattachment` WRITE;
/*!40000 ALTER TABLE `purchaseinvoiceattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `purchaseinvoiceattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchaseinvoicedetail`
--

DROP TABLE IF EXISTS `purchaseinvoicedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchaseinvoicedetail` (
  `PurchaseInvoiceDetId` int NOT NULL AUTO_INCREMENT,
  `PurchaseInvoiceId` int NOT NULL,
  `SrNo` int NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int NOT NULL,
  `Quantity` decimal(18,2) NOT NULL,
  `PerUnit` int NOT NULL,
  `UnitPrice` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicedetail`
--

LOCK TABLES `purchaseinvoicedetail` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicedetail` DISABLE KEYS */;
INSERT INTO `purchaseinvoicedetail` VALUES (7,17,1,'sdadsasda',2,43.00,1,4.0000,172.0000,3412.6984,12.0400,238.8889,184.0400,3651.5873,3,'2021-10-05 19:53:29',3,'2021-10-09 20:42:27'),(9,18,1,'gfgfgffg g gf',1,43.00,1,5.0000,215.0000,4263.5044,0.0000,0.0000,215.0000,4263.5044,3,'2021-10-06 16:45:04',3,'2021-10-06 16:51:13'),(10,18,2,'sdadsasda',1,345.00,1,3.0000,1035.0000,20524.3119,0.0000,0.0000,1035.0000,20524.3119,3,'2021-10-06 16:45:55',3,'2021-10-06 16:51:13'),(11,18,3,'rwrrre',1,43.00,1,23.0000,989.0000,19612.1203,0.0000,0.0000,989.0000,19612.1203,3,'2021-10-06 16:46:44',3,'2021-10-06 16:51:14'),(13,22,1,'sdadsasda',1,345.00,1,10.0000,3450.0000,68414.3730,224.2500,4446.9342,3674.2500,72861.3072,3,'2021-10-06 22:37:58',3,'2021-10-06 23:09:42'),(14,22,2,'rwrrre',7,43.00,1,4.0000,172.0000,3410.8035,8.6000,170.5402,180.6000,3581.3437,3,'2021-10-06 22:57:56',3,'2021-10-06 23:10:11'),(15,22,3,'sdadsasda',2,4.00,1,23.0000,92.0000,1824.3833,4.6000,91.2192,96.6000,1915.6024,3,'2021-10-06 22:59:43',3,'2021-10-06 23:02:31'),(17,20,1,'1  eewew',1,1.00,1,125.0000,125.0000,125.0000,0.0000,0.0000,125.0000,125.0000,3,'2021-10-08 01:04:24',3,'2021-10-08 01:06:28'),(18,16,2,'sdadsasda',1,345.00,1,2.0000,690.0000,13193.1166,0.0000,0.0000,690.0000,13193.1166,3,'2021-10-08 11:04:46',3,'2021-10-14 22:29:43'),(19,23,1,'sdadsasda',1,34.00,5,7.0000,1190.0000,23598.0011,59.5000,1179.9001,1249.5000,24777.9012,3,'2021-10-23 18:05:59',3,'2021-10-23 18:06:00');
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
  `PurchaseInvoiceDetId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) NOT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicedetailtax`
--

LOCK TABLES `purchaseinvoicedetailtax` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicedetailtax` DISABLE KEYS */;
INSERT INTO `purchaseinvoicedetailtax` VALUES (32,13,1,105,'Percentage',5.0000,'Add',172.5000,3420.7186,'',3,'2021-10-06 23:02:30',3,'2021-10-06 23:09:41'),(33,14,1,105,'Percentage',5.0000,'Add',8.6000,170.5402,'rtrvtrtvr  te tt tttetete',3,'2021-10-06 23:02:31',3,'2021-10-06 23:10:11'),(34,15,1,105,'Percentage',5.0000,'Add',4.6000,91.2192,'',3,'2021-10-06 23:02:31',3,'2021-10-06 23:02:31'),(35,13,2,106,'Percentage',3.0000,'Add',103.5000,2052.4312,'rrvv  434343',3,'2021-10-06 23:03:02',3,'2021-10-06 23:09:41'),(36,7,1,105,'Percentage',5.0000,'Add',8.6000,170.6349,NULL,3,'2021-10-08 00:07:44',3,'2021-10-09 20:42:16'),(38,7,2,105,'Percentage',2.0000,'Add',3.4400,68.2540,NULL,3,'2021-10-09 20:42:27',3,'2021-10-09 20:42:27'),(40,19,1,105,'Percentage',5.0000,'Add',59.5000,1179.9001,'',3,'2021-10-23 18:05:59',3,'2021-10-23 18:05:59');
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
  `PurchaseInvoiceId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) NOT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchaseinvoicetax`
--

LOCK TABLES `purchaseinvoicetax` WRITE;
/*!40000 ALTER TABLE `purchaseinvoicetax` DISABLE KEYS */;
INSERT INTO `purchaseinvoicetax` VALUES (11,18,1,105,'Percentage',5.0000,'Add',106.3525,2108.9970,'',3,'2021-10-06 16:51:14',3,'2021-10-06 16:51:14'),(15,20,1,105,'Percentage',5.0000,'Add',6.2500,6.2500,'',3,'2021-10-08 01:06:28',3,'2021-10-08 01:06:28'),(16,16,1,105,'Percentage',5.0000,'Add',32.7750,626.6730,'',3,'2021-10-14 22:29:43',3,'2021-10-14 22:29:43');
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
  `AccountLedgerId` int NOT NULL,
  `TypeCorB` varchar(1) DEFAULT NULL,
  `PaymentTypeId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `ChequeNo` varchar(50) DEFAULT NULL,
  `ChequeDate` datetime DEFAULT NULL,
  `ChequeAmount_FC` decimal(18,4) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Amount_FCInWord` varchar(2000) DEFAULT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptvoucher`
--

LOCK TABLES `receiptvoucher` WRITE;
/*!40000 ALTER TABLE `receiptvoucher` DISABLE KEYS */;
INSERT INTO `receiptvoucher` VALUES (14,'21-01-05-00001','2021-10-15 00:00:00',115,'C',1,2,0.050428,'sassa asd asddsa adssdada asdsa','2021-09-27 00:00:00',20.0000,'d f ffs fsf',20.0000,1.0086,'DHM Twenty  Only',7,1,1,1,1,3,'2021-10-15 16:49:51',3,'2021-10-16 23:37:02'),(15,'21-01-05-00002','2021-10-15 00:00:00',108,'B',1,1,1.000000,'76868tut','2021-09-27 00:00:00',23.0000,'cvbvvc',78.0000,78.0000,'INR Seventy Eight  Only',1,1,1,2,1,3,'2021-10-15 16:53:13',3,'2021-10-15 16:57:35'),(16,'21-01-05-00003','2021-10-23 00:00:00',107,'B',1,2,0.050428,'89778979','2021-10-05 00:00:00',500.0000,NULL,500.0000,25.2140,'DHM Five Hundred  Only',4,1,1,3,1,3,'2021-10-23 16:56:51',3,'2021-10-23 16:57:17'),(17,'21-01-05-00004','2021-10-23 00:00:00',115,'C',1,2,0.050428,'76868tut','2021-09-29 00:00:00',17800.0000,'dsfdfdsdsf',17800.0000,897.6184,'DHM Seventeen Thousand Eight Hundred  Only',1,1,1,4,1,3,'2021-10-23 17:04:45',3,'2021-10-23 17:05:09');
/*!40000 ALTER TABLE `receiptvoucher` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receiptvoucherattachment`
--

DROP TABLE IF EXISTS `receiptvoucherattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receiptvoucherattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `ReceiptVoucherId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_RVAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_RVAttachment_ReceiptVoucher_ReceiptVoucherId_idx` (`ReceiptVoucherId`),
  KEY `FK_RVAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_RVAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_RVAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_RVAttachment_ReceiptVoucher_ReceiptVoucherId` FOREIGN KEY (`ReceiptVoucherId`) REFERENCES `receiptvoucher` (`ReceiptVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_RVAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_RVAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptvoucherattachment`
--

LOCK TABLES `receiptvoucherattachment` WRITE;
/*!40000 ALTER TABLE `receiptvoucherattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `receiptvoucherattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `receiptvoucherdetail`
--

DROP TABLE IF EXISTS `receiptvoucherdetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `receiptvoucherdetail` (
  `ReceiptVoucherDetId` int NOT NULL AUTO_INCREMENT,
  `ReceiptVoucherId` int NOT NULL,
  `ParticularLedgerId` int NOT NULL,
  `TransactionTypeId` int NOT NULL,
  `Amount_FC` decimal(18,4) NOT NULL,
  `Amount` decimal(18,4) NOT NULL,
  `Narration` varchar(2000) DEFAULT NULL,
  `SalesInvoiceId` int DEFAULT NULL,
  `CreditNoteId` int DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`ReceiptVoucherDetId`),
  KEY `IX_ReceiptVoucherDetails_ReceiptVoucherId` (`ReceiptVoucherId`) /*!80000 INVISIBLE */,
  KEY `IX_ReceiptVoucherDetails_PreparedByUserId` (`PreparedByUserId`),
  KEY `IX_ReceiptVoucherDetails_UpdatedByUserId` (`UpdatedByUserId`),
  KEY `IX_ReceiptVoucherDetails_CreditNoteId` (`CreditNoteId`),
  KEY `IX_ReceiptVoucherDetails_SalesInvoiceId` (`SalesInvoiceId`),
  KEY `IX_ReceiptVoucherDetails_ParticularLedgerId` (`ParticularLedgerId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_aspnetusers_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_aspnetusers_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_CreditNote_CreditNoteId` FOREIGN KEY (`CreditNoteId`) REFERENCES `creditnote` (`CreditNoteId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_Ledger_ParticularLedgerId` FOREIGN KEY (`ParticularLedgerId`) REFERENCES `ledger` (`LedgerId`),
  CONSTRAINT `FK_ReceiptVoucherDetails_ReceiptVoucher_ReceiptVoucherId` FOREIGN KEY (`ReceiptVoucherId`) REFERENCES `receiptvoucher` (`ReceiptVoucherId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_ReceiptVoucherDetails_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='FK_ReceiptVoucherDetails_Ledger_ParticularLedgerId';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `receiptvoucherdetail`
--

LOCK TABLES `receiptvoucherdetail` WRITE;
/*!40000 ALTER TABLE `receiptvoucherdetail` DISABLE KEYS */;
INSERT INTO `receiptvoucherdetail` VALUES (8,15,126,1,78.0000,78.0000,NULL,NULL,NULL,3,'2021-10-15 16:57:35',3,'2021-10-15 16:57:35'),(12,14,111,2,20.0000,1.0086,'uyuu ',19,NULL,3,'2021-10-16 23:36:12',3,'2021-10-16 23:36:14'),(13,16,111,1,500.0000,25.2140,NULL,NULL,NULL,3,'2021-10-23 16:57:08',3,'2021-10-23 16:57:08'),(14,17,111,2,17800.0000,897.6184,NULL,NULL,15,3,'2021-10-23 17:05:09',3,'2021-10-23 17:05:09');
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
  `CustomerLedgerId` int NOT NULL,
  `BillToAddressId` int NOT NULL,
  `AccountLedgerId` int NOT NULL,
  `BankLedgerId` int NOT NULL,
  `CustomerReferenceNo` varchar(250) DEFAULT NULL,
  `CustomerReferenceDate` datetime DEFAULT NULL,
  `PaymentTerm` varchar(2000) DEFAULT NULL,
  `CreditLimitDays` int NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `TaxModelType` varchar(50) DEFAULT NULL,
  `TaxRegisterId` int NOT NULL,
  `CurrencyId` int NOT NULL,
  `ExchangeRate` decimal(18,6) NOT NULL,
  `TotalLineItemAmount_FC` decimal(18,4) NOT NULL,
  `TotalLineItemAmount` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `NetAmount_FCInWord` varchar(2000) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `DiscountPercentageOrAmount` varchar(250) DEFAULT NULL,
  `DiscountPerOrAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount_FC` decimal(18,4) NOT NULL,
  `DiscountAmount` decimal(18,4) NOT NULL,
  `StatusId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `MaxNo` int NOT NULL,
  `VoucherStyleId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoice`
--

LOCK TABLES `salesinvoice` WRITE;
/*!40000 ALTER TABLE `salesinvoice` DISABLE KEYS */;
INSERT INTO `salesinvoice` VALUES (19,'21-01-01-00001','2021-10-09 00:00:00',111,5,127,107,'545454fsdfdfds','2021-09-29 00:00:00','2 fvfdfddff',23,'dgdddggd','LineWise',1,2,0.050428,157136.0000,3116046.6408,86424.8000,1713825.6524,94281.6000,1869627.9845,'DHM Ninety Four Thousand Two Hundred Eighty One and Six Paisa Only',7856.8000,155802.3320,'Percentage',45.0000,70711.2000,1402220.9883,1,1,1,1,1,3,'2021-10-09 20:48:54',3,'2022-01-09 20:14:46'),(20,'21-01-01-00002','2021-10-22 00:00:00',111,5,127,107,'45543354','2021-09-27 00:00:00','kjhkh',30,NULL,'SubTotal',1,2,0.050428,5500.0000,109066.3917,5455.0000,108174.0303,5727.7500,113582.7318,'DHM Five Thousand Seven Hundred Twenty Seven and Seven Five Paisa Only',272.7500,5408.7015,'Amount',45.0000,45.0000,892.3614,4,1,1,2,1,3,'2021-10-23 16:55:55',3,'2021-10-23 16:56:23'),(21,'21-01-01-00003','2022-01-09 00:00:00',111,5,101,107,'4432432432','2022-01-04 00:00:00','45 dy',15,'ewewewew','SubTotal',1,2,0.050456,12918.0000,256025.0515,12272.1000,243223.7990,12885.7050,255384.9889,'DHM Twelve Thousand Eight Hundred Eighty Five and Seven Zero Five Paisa Only',613.6050,12161.1899,'Percentage',5.0000,645.9000,12801.2526,1,1,1,3,1,3,'2022-01-09 09:18:21',3,'2022-01-09 09:26:35'),(22,'22-01-01-00001','2022-01-09 00:00:00',111,5,118,107,'dfdsfdsfds','2021-12-27 00:00:00','43dsssfd',3,'fdfdsfdsfds','SubTotal',1,2,0.050456,0.0000,0.0000,0.0000,0.0000,0.0000,0.0000,'Zero Only',0.0000,0.0000,'Percentage',34.0000,0.0000,0.0000,1,1,2,1,1,3,'2022-01-09 09:31:29',3,'2022-01-09 09:31:30'),(23,'22-01-01-00002','2022-01-09 00:00:00',112,7,136,107,'sdsfd sdffdd ','2021-12-29 00:00:00','dssfddsffsd',1,NULL,'LineWise',1,2,0.050456,81765.0000,1620520.8498,80947.3500,1604315.6414,85035.6000,1685341.6838,'DHM Eighty Five Thousand Thirty Five and Six Paisa Only',4088.2500,81026.0425,'Percentage',1.0000,817.6500,16205.2085,4,1,2,2,1,3,'2022-01-09 09:32:09',3,'2022-01-12 13:05:58');
/*!40000 ALTER TABLE `salesinvoice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoiceattachment`
--

DROP TABLE IF EXISTS `salesinvoiceattachment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoiceattachment` (
  `AssociationId` int NOT NULL AUTO_INCREMENT,
  `SalesInvoiceId` int NOT NULL,
  `AttachmentId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`AssociationId`),
  KEY `FK_SIAttachment_Attachment_AttachmentId_idx` (`AttachmentId`),
  KEY `FK_SIAttachment_SalesInvoice_SalesInvoiceId_idx` (`SalesInvoiceId`),
  KEY `FK_SIAttachment_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_SIAttachment_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_SIAttachment_Attachment_AttachmentId` FOREIGN KEY (`AttachmentId`) REFERENCES `attachment` (`AttachmentId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SIAttachment_SalesInvoice_SalesInvoiceId` FOREIGN KEY (`SalesInvoiceId`) REFERENCES `salesinvoice` (`SalesInvoiceId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SIAttachment_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_SIAttachment_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoiceattachment`
--

LOCK TABLES `salesinvoiceattachment` WRITE;
/*!40000 ALTER TABLE `salesinvoiceattachment` DISABLE KEYS */;
/*!40000 ALTER TABLE `salesinvoiceattachment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `salesinvoicedetail`
--

DROP TABLE IF EXISTS `salesinvoicedetail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `salesinvoicedetail` (
  `SalesInvoiceDetId` int NOT NULL AUTO_INCREMENT,
  `SalesInvoiceId` int NOT NULL,
  `SrNo` int NOT NULL,
  `Description` varchar(2000) DEFAULT NULL,
  `UnitOfMeasurementId` int NOT NULL,
  `Quantity` decimal(18,2) NOT NULL,
  `PerUnit` int NOT NULL,
  `UnitPrice` decimal(18,4) NOT NULL,
  `GrossAmount_FC` decimal(18,4) NOT NULL,
  `GrossAmount` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL DEFAULT '0.0000',
  `TaxAmount_FC` decimal(18,4) NOT NULL DEFAULT '0.0000',
  `NetAmount_FC` decimal(18,4) NOT NULL,
  `NetAmount` decimal(18,4) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicedetail`
--

LOCK TABLES `salesinvoicedetail` WRITE;
/*!40000 ALTER TABLE `salesinvoicedetail` DISABLE KEYS */;
INSERT INTO `salesinvoicedetail` VALUES (22,19,1,'6',1,8.00,7,78.0000,4368.0000,86618.5453,4330.9273,218.4000,4586.4000,90949.4725,3,'2021-10-09 20:53:30',3,'2022-01-09 20:14:46'),(23,20,1,'dsjkhk',1,55.00,1,100.0000,5500.0000,109066.3917,0.0000,0.0000,5500.0000,109066.3917,3,'2021-10-23 16:56:16',3,'2021-10-23 16:56:16'),(24,19,2,'eewr ewerw',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(25,19,3,'r e',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(26,19,4,'434',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(27,19,5,'ddrwewrerew',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(28,19,6,' 434343 43',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(29,19,7,'4343  43',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(30,19,8,'wrerwe re435 33',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(31,19,9,'435 4353 343',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(32,19,10,' 434343',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(33,19,11,'4 43',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(34,19,12,'ds dssdd f erw',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:31'),(35,19,13,'eewr ewerw',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(36,19,14,'r e  ksdjlkjlds jjd ljds dsljdl djsl',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(37,19,15,'d ssd sdd dsd sd',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(38,19,16,'ddrwewrerew',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(39,19,17,'dsdsds sdsdsds sd',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(40,19,18,' dssdds',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(41,19,19,'ds ds',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(42,19,20,' ssdds',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(43,19,21,'ds  dsdsdss',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(44,19,22,'ds dsds',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(45,19,23,' dsds dsds',7,434.00,4,4.0000,6944.0000,137701.2771,6885.0639,347.2000,7291.2000,144586.3409,3,'2021-11-30 00:31:49',3,'2021-12-06 18:01:32'),(46,21,1,'etteetete',2,12.00,1,15.0000,180.0000,3567.4647,0.0000,0.0000,180.0000,3567.4647,3,'2022-01-09 09:18:45',3,'2022-01-09 09:26:35'),(47,21,2,'dsfsfd f 234223423',2,22.00,1,579.0000,12738.0000,252457.5868,0.0000,0.0000,12738.0000,252457.5868,3,'2022-01-09 09:19:05',3,'2022-01-09 09:26:35'),(48,23,1,'rew ewrre rewwree',1,23.00,1,3444.0000,79212.0000,1569922.3085,78496.1154,3960.6000,83172.6000,1648418.4240,3,'2022-01-09 09:32:27',3,'2022-01-09 19:23:32'),(49,23,2,'3rff eer rewr ew',1,111.00,1,23.0000,2553.0000,50598.5413,2529.9271,127.6500,2680.6500,53128.4684,3,'2022-01-09 09:39:22',3,'2022-01-09 19:23:13');
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
  `SalesInvoiceDetId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicedetailtax`
--

LOCK TABLES `salesinvoicedetailtax` WRITE;
/*!40000 ALTER TABLE `salesinvoicedetailtax` DISABLE KEYS */;
INSERT INTO `salesinvoicedetailtax` VALUES (12,22,1,105,'Percentage',5.0000,'Add',218.4000,4330.9273,NULL,3,'2021-12-06 18:01:31',3,'2022-01-09 20:14:46'),(13,24,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(14,25,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(15,26,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(16,27,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(17,28,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(18,29,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(19,30,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(20,31,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(21,32,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(22,33,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(23,34,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:31',3,'2021-12-06 18:01:31'),(24,35,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(25,36,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(26,37,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(27,38,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(28,39,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(29,40,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(30,41,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(31,42,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(32,43,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(33,44,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(34,45,1,105,'Percentage',5.0000,'Add',347.2000,6885.0639,'',3,'2021-12-06 18:01:32',3,'2021-12-06 18:01:32'),(43,48,1,105,'Percentage',5.0000,'Add',3960.6000,78496.1154,NULL,3,'2022-01-09 19:23:13',3,'2022-01-09 19:23:32'),(44,49,1,105,'Percentage',5.0000,'Add',127.6500,2529.9271,'',3,'2022-01-09 19:23:13',3,'2022-01-09 19:23:13');
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
  `SalesInvoiceId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `TaxPerOrAmount_FC` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `TaxAmount_FC` decimal(18,4) NOT NULL,
  `TaxAmount` decimal(18,4) NOT NULL,
  `Remark` varchar(2000) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `salesinvoicetax`
--

LOCK TABLES `salesinvoicetax` WRITE;
/*!40000 ALTER TABLE `salesinvoicetax` DISABLE KEYS */;
INSERT INTO `salesinvoicetax` VALUES (8,20,1,105,'Percentage',5.0000,'Add',272.7500,5408.7015,'',3,'2021-10-23 16:55:55',3,'2021-10-23 16:56:16'),(13,21,1,105,'Percentage',5.0000,'Add',613.6050,12161.1899,'',3,'2022-01-09 09:26:35',3,'2022-01-09 09:26:35'),(14,22,1,105,'Percentage',5.0000,'Add',0.0000,0.0000,'',3,'2022-01-09 09:31:29',3,'2022-01-09 09:31:29');
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
  `StateName` varchar(500) NOT NULL,
  `CountryId` int DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`StateId`),
  UNIQUE KEY `StateName_UNIQUE` (`StateName`),
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
  `StatusName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`StatusId`),
  UNIQUE KEY `Description_UNIQUE` (`StatusName`),
  KEY `FK_Status_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_Status_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_Status_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_Status_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'Inprocess',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'Approval Requested',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'Approval Rejected',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,'Approved',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,'Closed',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,'Posted',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'Cancelled',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00');
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
  `TaxRegisterName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `TaxRegisterId` int NOT NULL,
  `SrNo` int NOT NULL,
  `TaxLedgerId` int NOT NULL,
  `TaxPercentageOrAmount` varchar(250) DEFAULT NULL,
  `Rate` decimal(18,4) NOT NULL,
  `TaxAddOrDeduct` varchar(250) DEFAULT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
  `UnitOfMeasurementName` varchar(250) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
INSERT INTO `unitofmeasurement` VALUES (1,'KG',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,'MTR',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,'PCS',1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,'No',1,NULL,1,NULL);
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
  `ModuleId` int NOT NULL,
  `IsActive` tinyint NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
  `UpdatedDateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`VoucherSetupId`),
  UNIQUE KEY `Name_UNIQUE` (`VoucherSetupName`),
  KEY `IX_VoucherSetup_Module_ModuleId_idx` (`ModuleId`),
  KEY `FK_VoucherSetup_User_PreparedByUserId_idx` (`PreparedByUserId`),
  KEY `FK_VoucherSetup_User_UpdatedByUserId_idx` (`UpdatedByUserId`),
  CONSTRAINT `FK_VoucherSetup_Module_ModuleId` FOREIGN KEY (`ModuleId`) REFERENCES `module` (`ModuleId`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetup_User_PreparedByUserId` FOREIGN KEY (`PreparedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_VoucherSetup_User_UpdatedByUserId` FOREIGN KEY (`UpdatedByUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vouchersetup`
--

LOCK TABLES `vouchersetup` WRITE;
/*!40000 ALTER TABLE `vouchersetup` DISABLE KEYS */;
INSERT INTO `vouchersetup` VALUES (1,'Ledger',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(2,'Sales Invoice',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(3,'Purchase Invoice',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(4,'Credit Note',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(5,'Debit Note',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(6,'Receipt Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(7,'Payment Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(8,'Journal Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(9,'Contra Voucher',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(10,'Advance Adjustment',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(11,'Tax Register',6,1,1,'2021-01-21 00:00:00',1,'2021-01-01 00:00:00'),(12,'Currency Conversion',6,1,1,'2021-01-21 00:00:00',1,'2021-01-21 00:00:00');
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
  `VoucherSetupId` int NOT NULL,
  `NoPad` char(1) DEFAULT NULL,
  `NoPreString` varchar(100) DEFAULT NULL,
  `NoPostString` varchar(100) DEFAULT NULL,
  `NoSeparator` varchar(100) DEFAULT NULL,
  `FormatText` varchar(100) DEFAULT NULL,
  `VoucherStyleId` int NOT NULL,
  `CompanyId` int NOT NULL,
  `FinancialYearId` int NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vouchersetupdetail`
--

LOCK TABLES `vouchersetupdetail` WRITE;
/*!40000 ALTER TABLE `vouchersetupdetail` DISABLE KEYS */;
INSERT INTO `vouchersetupdetail` VALUES (1,1,'0','UG','','','UG00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(2,2,'0','21-01-01','','-','21-01-01-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(3,3,'0','21-01-02','','-','21-01-02-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(4,4,'0','21-01-03','','-','21-01-03-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(5,5,'0','21-01-04','','-','21-01-04-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(6,6,'0','21-01-05','','-','21-01-05-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(7,7,'0','21-01-06','','-','21-01-06-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(8,8,'0','21-01-07','','-','21-01-07-00001',1,1,1,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(9,9,'0','21-01-08','','-','21-01-08-00001',1,1,1,1,'2021-01-21 00:00:00',1,'2021-01-21 00:00:00'),(10,10,'0','21-01-09','','-','21-01-09-00001',1,1,1,1,'2021-01-21 00:00:00',1,'2021-01-21 00:00:00'),(11,2,'0','22-01-01','','-','22-01-01-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(12,3,'0','22-01-02','','-','22-01-02-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(13,4,'0','22-01-03','','-','22-01-03-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(14,5,'0','22-01-04','','-','22-01-04-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(15,6,'0','22-01-05','','-','22-01-05-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(16,7,'0','22-01-06','','-','22-01-06-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(17,8,'0','22-01-07','','-','22-01-07-00001',1,1,2,1,'2021-01-01 00:00:00',1,'2021-01-01 00:00:00'),(18,9,'0','22-01-08','','-','22-01-08-00001',1,1,2,1,'2021-01-21 00:00:00',1,'2021-01-21 00:00:00'),(19,10,'0','22-01-09','','-','22-01-09-00001',1,1,2,1,'2021-01-21 00:00:00',1,'2021-01-21 00:00:00');
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
  `VoucherStyleName` varchar(500) NOT NULL,
  `PreparedByUserId` int NOT NULL,
  `PreparedDateTime` datetime DEFAULT NULL,
  `UpdatedByUserId` int NOT NULL,
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

-- Dump completed on 2022-01-13 21:24:32

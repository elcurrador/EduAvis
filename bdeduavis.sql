-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: bdeduavis
-- ------------------------------------------------------
-- Server version	9.3.0

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
-- Table structure for table `groups`
--

CREATE DATABASE IF NOT EXISTS bdeduavis;
USE bdeduavis;

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groups` (
  `group_code` varchar(10) NOT NULL,
  `group_name` varchar(50) NOT NULL,
  PRIMARY KEY (`group_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES ('1BACH-A','1st Bachillerato Group A'),('1BACH-B','1st Bachillerato Group B'),('1DAM','1st Development of Multiplatform Applications'),('1DAW','1st Web Application Development'),('1ESO-A','1st ESO Group A'),('1ESO-B','1st ESO Group B'),('2BACH-A','2nd Bachillerato Group A'),('2BACH-B','2nd Bachillerato Group B'),('2DAM','2nd Development of Multiplatform Applications'),('2DAW','2nd Web Application Development'),('2ESO-A','2nd ESO Group A'),('2ESO-B','2nd ESO Group B'),('3ESO-A','3rd ESO Group A'),('3ESO-B','3rd ESO Group B'),('4ESO-A','4th ESO Group A'),('4ESO-B','4th ESO Group B');
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incidents`
--

DROP TABLE IF EXISTS `incidents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incidents` (
  `id` int NOT NULL AUTO_INCREMENT,
  `reason_id` int NOT NULL,
  `event_datetime` datetime NOT NULL,
  `student_nia` varchar(10) NOT NULL,
  `teacher_dni` varchar(10) NOT NULL,
  `registered_datetime` datetime DEFAULT CURRENT_TIMESTAMP,
  `registered_by` varchar(10) NOT NULL,
  `is_sanction` tinyint(1) NOT NULL DEFAULT '0',
  `is_sanctioned` tinyint(1) NOT NULL DEFAULT '0',
  `reason_description` text,
  PRIMARY KEY (`id`),
  KEY `reason_id` (`reason_id`),
  KEY `student_nia` (`student_nia`),
  KEY `teacher_dni` (`teacher_dni`),
  KEY `registered_by` (`registered_by`),
  CONSTRAINT `incidents_ibfk_1` FOREIGN KEY (`reason_id`) REFERENCES `reasons` (`id`),
  CONSTRAINT `incidents_ibfk_2` FOREIGN KEY (`student_nia`) REFERENCES `students` (`nia`),
  CONSTRAINT `incidents_ibfk_3` FOREIGN KEY (`teacher_dni`) REFERENCES `teachers` (`dni`),
  CONSTRAINT `incidents_ibfk_4` FOREIGN KEY (`registered_by`) REFERENCES `teachers` (`dni`)
) ENGINE=InnoDB AUTO_INCREMENT=115 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incidents`
--

LOCK TABLES `incidents` WRITE;
/*!40000 ALTER TABLE `incidents` DISABLE KEYS */;
INSERT INTO `incidents` VALUES (1,1,'2025-04-01 03:09:00','10001','12345678A','2025-05-14 11:12:00','12312332',1,1,'Student repeatedly interrupted the class by making noises and distracting classmates'),(2,2,'2025-04-01 08:30:00','10002','23456789B','2025-04-01 09:00:00','23456789B',1,1,'Student arrived 15 minutes late without justification'),(3,5,'2025-04-02 11:20:00','10021','34567890C','2025-04-02 12:00:00','34567890C',1,1,'Student disrespectfully answered back to the teacher when asked to pay attention'),(4,6,'2025-04-02 13:10:00','10031','45678901D','2025-04-02 14:00:00','45678901D',1,0,'Student used mobile phone during explanation despite warnings'),(5,3,'2025-04-03 10:30:00','10041','56789012E','2025-04-03 11:00:00','56789012E',1,1,'Student pushed a classmate during physical education class'),(6,4,'2025-04-03 09:45:00','10051','67890123F','2025-04-03 10:30:00','67890123F',1,1,'Student repeatedly insulted another classmate during class'),(7,7,'2025-04-04 08:00:00','10061','78901234G','2025-04-04 09:00:00','78901234G',1,0,'Student did not appear in class without justification'),(8,8,'2025-04-04 12:30:00','10062','89012345H','2025-04-04 13:00:00','89012345H',1,1,'Student drew on classroom wall and damaged school materials'),(9,9,'2025-04-05 11:00:00','10067','90123456I','2025-04-05 11:30:00','90123456I',1,1,'Student was caught smoking in the school bathrooms'),(10,10,'2025-04-05 14:15:00','10068','01234567J','2025-04-05 15:00:00','01234567J',1,0,'Student left school premises without permission during break time'),(11,11,'2025-04-06 10:00:00','10069','12345678K','2025-04-06 10:30:00','12345678K',1,1,'Student was caught copying during exam'),(12,12,'2025-04-06 09:00:00','10070','23456789L','2025-04-06 09:30:00','23456789L',1,0,'Student did not submit assignments for the third time this month'),(13,13,'2025-04-07 10:15:00','10003','34567890C','2025-04-07 10:45:00','34567890C',0,0,'Student felt ill during class and was sent to the nurse'),(14,14,'2025-04-07 12:30:00','10004','45678901D','2025-04-07 13:00:00','45678901D',0,0,'Student had a minor accident during break time and received first aid'),(15,15,'2025-04-08 09:10:00','10005','56789012E','2025-04-08 09:40:00','56789012E',0,0,'Student complained of headache, parents were contacted and picked up student'),(16,16,'2025-04-08 08:45:00','10006','67890123F','2025-04-08 09:15:00','67890123F',0,0,'Student arrived too late and waited with the guard teacher until next period'),(17,17,'2025-04-09 13:20:00','10007','78901234G','2025-04-09 13:50:00','78901234G',0,0,'Student reported losing personal belongings from their backpack'),(18,18,'2025-04-09 11:30:00','10008','89012345H','2025-04-09 12:00:00','89012345H',0,0,'Student found a wallet and turned it in to the school office'),(21,1,'2025-04-11 11:55:00','10001','12345678A','2025-04-11 10:00:00','12345678A',1,0,'Student continued disruptive behavior despite previous warning'),(22,6,'2025-04-11 11:45:00','10031','45678901D','2025-04-11 12:15:00','45678901D',1,1,'Student used mobile phone again to record video during class'),(23,7,'2025-04-12 08:30:00','10061','78901234G','2025-04-12 09:00:00','78901234G',1,1,'Student skipped class for the second time this week'),(24,5,'2025-04-12 10:20:00','10021','34567890C','2025-04-12 10:50:00','34567890C',1,1,'Student continued to be disrespectful to teaching staff'),(25,5,'2025-04-13 09:00:00','10001','23456789B','2025-04-13 09:30:00','23456789B',1,1,'Student was disrespectful to substitute teacher'),(26,10,'2025-04-13 13:40:00','10068','01234567J','2025-04-13 14:10:00','01234567J',1,1,'Student left school premises again without permission'),(28,15,'2025-04-14 11:30:00','10005','56789012E','2025-04-14 12:00:00','56789012E',0,0,'Student felt ill again and parents were contacted'),(29,1,'2025-04-15 10:00:00','10011','89012345R','2025-04-15 10:30:00','89012345R',1,0,'Student disrupted class by constantly talking to neighbors'),(30,2,'2025-04-15 08:45:00','10012','90123456S','2025-04-15 09:15:00','90123456S',1,0,'Student arrived late to class without valid excuse'),(32,6,'2025-04-16 09:30:00','10014','89012345R','2025-04-16 10:00:00','89012345R',1,0,'Student was using mobile phone to play games during test'),(33,12,'2025-04-17 11:15:00','10015','90123456S','2025-04-17 11:45:00','90123456S',1,0,'Student has not submitted any assignments this week'),(35,14,'2025-04-18 09:45:00','10017','89012345R','2025-04-18 10:15:00','89012345R',0,0,'Student had minor accident in laboratory and received first aid'),(36,16,'2025-05-14 00:00:00','10018','01234567J','2025-05-14 11:04:00','90123456S',0,1,'Student arrived 20 minutes late due to bus delay'),(38,18,'2025-04-19 10:30:00','10020','89012345R','2025-04-19 11:00:00','89012345R',0,0,'Student found keys in the hallway and brought them to reception');
/*!40000 ALTER TABLE `incidents` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permissions`
--

DROP TABLE IF EXISTS `permissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permissions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(50) NOT NULL,
  `description` text NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `code` (`code`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permissions`
--

LOCK TABLES `permissions` WRITE;
/*!40000 ALTER TABLE `permissions` DISABLE KEYS */;
INSERT INTO `permissions` VALUES (1,'ADD_INCIDENT','Add new incidents'),(2,'MODIFY_OWN_INCIDENTS','Modify/Delete incidents where the teacher is involved'),(3,'MODIFY_REGISTERED_INCIDENTS','Modify/Delete incidents registered by themselves'),(4,'MODIFY_OTHER_INCIDENTS','Modify/Delete incidents registered by other teachers'),(9,'CHANGE_OWN_PASSWORD','Change own password'),(11,'MANAGE_ROLES','Add/modify/delete roles and permissions'),(14,'VIEW_TUTOR_GROUP_INCIDENTS','View incidents related to students of their tutor group');
/*!40000 ALTER TABLE `permissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reasons`
--

DROP TABLE IF EXISTS `reasons`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reasons` (
  `id` int NOT NULL AUTO_INCREMENT,
  `short_description` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reasons`
--

LOCK TABLES `reasons` WRITE;
/*!40000 ALTER TABLE `reasons` DISABLE KEYS */;
INSERT INTO `reasons` VALUES (1,'Disruptive behavior in class'),(2,'Late arrival to class'),(3,'Physical aggression against classmate'),(4,'Verbal aggression against classmate'),(5,'Disrespect to teacher'),(6,'Use of mobile phone during class'),(7,'Skipping class'),(8,'Damaging school property'),(9,'Smoking in school premises'),(10,'Leaving school without permission'),(11,'Copying during exam'),(12,'Not completing assignments'),(13,'Illness during school hours'),(14,'Minor accident during break time'),(15,'Student fell ill and parents were notified'),(16,'Student arrived too late and stayed with guard teacher'),(17,'Student lost personal belongings'),(18,'Student found unknown belongings'),(19,'Verbal warning'),(20,'Inappropriate language'),(21,'Other');
/*!40000 ALTER TABLE `reasons` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role_permission`
--

DROP TABLE IF EXISTS `role_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role_permission` (
  `role_id` int NOT NULL,
  `permission_id` int NOT NULL,
  PRIMARY KEY (`role_id`,`permission_id`),
  KEY `permission_id` (`permission_id`),
  CONSTRAINT `role_permission_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`),
  CONSTRAINT `role_permission_ibfk_2` FOREIGN KEY (`permission_id`) REFERENCES `permissions` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role_permission`
--

LOCK TABLES `role_permission` WRITE;
/*!40000 ALTER TABLE `role_permission` DISABLE KEYS */;
INSERT INTO `role_permission` VALUES (1,1),(2,1),(3,1),(4,1),(1,2),(2,2),(3,2),(4,2),(1,3),(2,3),(3,3),(4,3),(3,4),(4,4),(1,9),(2,9),(3,9),(4,9),(4,11),(2,14);
/*!40000 ALTER TABLE `role_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (4,'Administrator'),(3,'Director'),(1,'Teacher'),(2,'Tutor');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `nia` varchar(10) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `phone` varchar(15) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  `group_code` varchar(10) NOT NULL,
  PRIMARY KEY (`nia`),
  KEY `group_code` (`group_code`),
  CONSTRAINT `students_ibfk_1` FOREIGN KEY (`group_code`) REFERENCES `groups` (`group_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES ('10001','Jack','Wilson','6969696969','jack.wilson@student.edu','1ESO-A'),('10002','Sophie','Taylor','600111223','sophie.taylor@student.edu','1ESO-A'),('10003','Lucas','Brown','600111224','lucas.brown@student.edu','1ESO-A'),('10004','Olivia','Johnson','600111225','olivia.johnson@student.edu','1ESO-A'),('10005','Noah','Davis','600111226','noah.davis@student.edu','1ESO-A'),('10006','Emma','Martin','600111227','emma.martin@student.edu','1ESO-B'),('10007','Liam','Rodriguez','600111228','liam.rodriguez@student.edu','1ESO-B'),('10008','Ava','Garcia','600111229','ava.garcia@student.edu','1ESO-B'),('10009','William','Martinez','600111230','william.martinez@student.edu','1ESO-B'),('10010','Isabella','Anderson','600111231','isabella.anderson@student.edu','1ESO-B'),('10011','James','Lopez','600111232','james.lopez@student.edu','2ESO-A'),('10012','Mia','Perez','600111233','mia.perez@student.edu','2ESO-A'),('10013','Benjamin','Sanchez','600111234','benjamin.sanchez@student.edu','2ESO-A'),('10014','Charlotte','Gonzalez','600111235','charlotte.gonzalez@student.edu','2ESO-A'),('10015','Mason','Ramirez','600111236','mason.ramirez@student.edu','2ESO-A'),('10016','Amelia','Torres','600111237','amelia.torres@student.edu','2ESO-B'),('10017','Elijah','Flores','600111238','elijah.flores@student.edu','2ESO-B'),('10018','Harper','Rivera','600111239','harper.rivera@student.edu','2ESO-B'),('10019','Ethan','Gomez','600111240','ethan.gomez@student.edu','2ESO-B'),('10020','Evelyn','Diaz','600111241','evelyn.diaz@student.edu','2ESO-B'),('10021','Alexander','Cruz','600111242','alexander.cruz@student.edu','3ESO-A'),('10022','Abigail','Reyes','600111243','abigail.reyes@student.edu','3ESO-A'),('10023','Michael','Morales','600111244','michael.morales@student.edu','3ESO-A'),('10024','Emily','Ortiz','600111245','emily.ortiz@student.edu','3ESO-A'),('10025','Daniel','Gutierrez','600111246','daniel.gutierrez@student.edu','3ESO-A'),('10026','Elizabeth','Castillo','600111247','elizabeth.castillo@student.edu','3ESO-B'),('10027','Henry','Ramos','600111248','henry.ramos@student.edu','3ESO-B'),('10028','Sofia','Chavez','600111249','sofia.chavez@student.edu','3ESO-B'),('10029','Sebastian','Herrera','600111250','sebastian.herrera@student.edu','3ESO-B'),('10030','Avery','Mendoza','600111251','avery.mendoza@student.edu','3ESO-B'),('10031','Jackson','Vargas','600111252','jackson.vargas@student.edu','4ESO-A'),('10032','Scarlett','Jimenez','600111253','scarlett.jimenez@student.edu','4ESO-A'),('10033','Aiden','Vazquez','600111254','aiden.vazquez@student.edu','4ESO-A'),('10034','Chloe','Romero','600111255','chloe.romero@student.edu','4ESO-A'),('10035','Matthew','Soto','600111256','matthew.soto@student.edu','4ESO-A'),('10036','Riley','Castro','600111257','riley.castro@student.edu','4ESO-B'),('10038','Layla','Ruiz','600111259','layla.ruiz@student.edu','4ESO-B'),('10039','David','Navarro','600111260','david.navarro@student.edu','4ESO-B'),('10041','Christopher','Contreras','600111262','christopher.contreras@student.edu','1BACH-A'),('10042','Lily','Aguilar','600111263','lily.aguilar@student.edu','1BACH-A'),('10043','Andrew','Delgado','600111264','andrew.delgado@student.edu','1BACH-A'),('10044','Hannah','Sandoval','600111265','hannah.sandoval@student.edu','1BACH-A'),('10045','Joseph','Marquez','600111266','joseph.marquez@student.edu','1BACH-A'),('10046','Victoria','Espinoza','600111267','victoria.espinoza@student.edu','1BACH-B'),('10047','Gabriel','Vega','600111268','gabriel.vega@student.edu','1BACH-B'),('10048','Natalie','Acosta','600111269','natalie.acosta@student.edu','1BACH-B'),('10049','Carter','Miranda','600111270','carter.miranda@student.edu','1BACH-B'),('10050','Grace','Fuentes','600111271','grace.fuentes@student.edu','1BACH-B'),('10051','Anthony','Carrillo','600111272','anthony.carrillo@student.edu','2BACH-A'),('10052','Zoey','Rojas','600111273','zoey.rojas@student.edu','2BACH-A'),('10053','Julian','Cabrera','600111274','julian.cabrera@student.edu','2BACH-A'),('10054','Penelope','Valencia','600111275','penelope.valencia@student.edu','2BACH-A'),('10055','Leo','Santiago','600111276','leo.santiago@student.edu','2BACH-A'),('10056','Stella','Montoya','600111277','stella.montoya@student.edu','2BACH-B'),('10057','Dominic','Cano','600111278','dominic.cano@student.edu','2BACH-B'),('10058','Aurora','Serrano','600111279','aurora.serrano@student.edu','2BACH-B'),('10059','Austin','Medina','600111280','austin.medina@student.edu','2BACH-B'),('10060','Hazel','Pacheco','600111281','hazel.pacheco@student.edu','2BACH-B'),('10061','Thomas','Cardenas','600111282','thomas.cardenas@student.edu','1DAM'),('10062','Lucy','Guzman','600111283','lucy.guzman@student.edu','1DAM'),('10063','Aaron','Munoz','600111284','aaron.munoz@student.edu','1DAM'),('10064','Ellie','Herrera','600111285','ellie.herrera@student.edu','1DAM'),('10065','Charles','Sosa','600111286','charles.sosa@student.edu','1DAM'),('10066','Paisley','Pena','600111287','paisley.pena@student.edu','2DAM'),('10067','Eli','Escobar','600111288','eli.escobar@student.edu','2DAM'),('10068','Madelyn','Velasquez','600111289','madelyn.velasquez@student.edu','2DAM'),('10069','Miles','Castaneda','600111290','miles.castaneda@student.edu','2DAM'),('10070','Audrey','Meza','600111291','audrey.meza@student.edu','2DAM'),('10071','Nolan','Barrera','600111292','nolan.barrera@student.edu','1DAW'),('10072','Brooklyn','Guerra','600111293','brooklyn.guerra@student.edu','1DAW'),('10073','Caleb','Cisneros','600111294','caleb.cisneros@student.edu','1DAW'),('10074','Savannah','Acevedo','600111295','savannah.acevedo@student.edu','1DAW'),('10075','Isaac','Campos','600111296','isaac.campos@student.edu','1DAW'),('10076','Claire','Tapia','600111297','claire.tapia@student.edu','2DAW'),('10078','Anna','Bautista','600111299','anna.bautista@student.edu','2DAW'),('10079','Nathan','Hurtado','600111300','nathan.hurtado@student.edu','2DAW'),('10080','Violet','Ochoa','600111301','violet.ochoa@student.edu','2DAW');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teachers`
--

DROP TABLE IF EXISTS `teachers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `teachers` (
  `dni` varchar(10) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `tutor_group` varchar(10) NOT NULL,
  `role_id` int NOT NULL,
  `is_tutor` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`dni`),
  UNIQUE KEY `email` (`email`),
  KEY `tutor_group` (`tutor_group`),
  KEY `role_id` (`role_id`),
  CONSTRAINT `teachers_ibfk_2` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teachers`
--

LOCK TABLES `teachers` WRITE;
/*!40000 ALTER TABLE `teachers` DISABLE KEYS */;
INSERT INTO `teachers` VALUES ('01234567J','Patricia','Wilson','patricia.wilson@school.edu','admin','1BACH-B',2,1),('01234567T','Sharon','Hall','sharon.hall@school.edu','admin','1DAM',3,0),('12312332','admin','admin','admin','admin','1ESO-A',4,0),('12345678A','John','Smith','john.smith@school.edu','admin','1ESO-A',2,1),('12345678K','Richard','Martinez','richard.martinez@school.edu','admin','2BACH-A',2,1),('12345678U','Mark','Allen','mark.allen@school.edu','admin','1BACH-B',3,0),('23456789B','Emma','Johnson','emma.johnson@school.edu','admin','1ESO-B',2,1),('23456789L','Elizabeth','Anderson','elizabeth.anderson@school.edu','admin','2BACH-B',2,1),('23456789V','Laura','Young','laura.young@school.edu','admin','1DAM',4,0),('34567890C','Michael','Williams','michael.williams@school.edu','admin','2ESO-A',2,1),('34567890M','Thomas','Taylor','thomas.taylor@school.edu','admin','1DAM',2,1),('45678901D','Sarah','Brown','sarah.brown@school.edu','admin','2ESO-B',2,1),('45678901N','Nancy','Thomas','nancy.thomas@school.edu','admin','2DAM',2,0),('56789012E','Robert','Jones','robert.jones@school.edu','admin','3ESO-A',2,1),('56789012O','Charles','Hernandez','charles.hernandez@school.edu','admin','1DAW',2,1),('67890123F','Lisa','Miller','lisa.miller@school.edu','admin','3ESO-B',2,1),('67890123P','Margaret','Moore','margaret.moore@school.edu','admin','2DAW',2,0),('78901234G','David','Davis','david.davis@school.edu','admin','4ESO-A',2,1),('78901234Q','Joseph','Martin','joseph.martin@school.edu','admin','2DAM',1,0),('89012345H','Jennifer','Garcia','jennifer.garcia@school.edu','admin','4ESO-B',2,1),('89012345R','Susan','Lee','susan.lee@school.edu','admin','2DAM',1,0),('90123456I','James','Rodriguez','james.rodriguez@school.edu','admin','1BACH-A',2,1),('90123456S','Paulinho','Walker','paul.walker@school.edu','admin123','2DAM',1,0),('93128312A','Manolo','Lama','pacogonzales@gmail.com','123312312','2ESO-B',1,0);
/*!40000 ALTER TABLE `teachers` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-08 17:06:24

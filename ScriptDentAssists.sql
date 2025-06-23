-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         11.8.2-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.10.0.7000
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para dentassistdb
CREATE DATABASE IF NOT EXISTS `dentassistdb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `dentassistdb`;

-- Volcando estructura para tabla dentassistdb.aspnetroleclaims
CREATE TABLE IF NOT EXISTS `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetroleclaims: ~0 rows (aproximadamente)

-- Volcando estructura para tabla dentassistdb.aspnetroles
CREATE TABLE IF NOT EXISTS `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetroles: ~5 rows (aproximadamente)
INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
	('26f89c57-ae60-419b-8a03-b46c3fe36673', 'Paciente', 'PACIENTE', NULL),
	('2ea52951-947c-4b7c-9099-f424d30e0d5c', 'Administrador', 'ADMINISTRADOR', NULL),
	('40ec38a7-40fb-40d8-8dee-9e15b816dc66', 'Recepcionista', 'RECEPCIONISTA', NULL),
	('a5578208-29ec-4198-a7f6-6ef79efed58b', 'Admin', 'ADMIN', NULL),
	('cf8c8cd6-445d-49b6-95ed-37c397b9479e', 'Odontologo', 'ODONTOLOGO', NULL);

-- Volcando estructura para tabla dentassistdb.aspnetuserclaims
CREATE TABLE IF NOT EXISTS `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetuserclaims: ~0 rows (aproximadamente)

-- Volcando estructura para tabla dentassistdb.aspnetuserlogins
CREATE TABLE IF NOT EXISTS `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetuserlogins: ~0 rows (aproximadamente)

-- Volcando estructura para tabla dentassistdb.aspnetuserroles
CREATE TABLE IF NOT EXISTS `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetuserroles: ~7 rows (aproximadamente)
INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
	('e51c34e0-1f11-4947-8212-ea9226703cf6', '2ea52951-947c-4b7c-9099-f424d30e0d5c'),
	('112c52c3-b5dd-4c68-a1a6-7357ca5dc2a4', '40ec38a7-40fb-40d8-8dee-9e15b816dc66'),
	('4d2ed48b-72eb-496b-b6a3-1011429c96c0', 'cf8c8cd6-445d-49b6-95ed-37c397b9479e'),
	('5ec295bf-e88a-418c-a392-5378274c6029', 'cf8c8cd6-445d-49b6-95ed-37c397b9479e'),
	('7ce02f71-9c55-4cc5-8937-37e307e01191', 'cf8c8cd6-445d-49b6-95ed-37c397b9479e'),
	('9107bc7f-9eea-4319-9fe4-bbd41a7e0633', 'cf8c8cd6-445d-49b6-95ed-37c397b9479e'),
	('c087cc58-32a8-4899-b77b-1db9575c8b5f', 'cf8c8cd6-445d-49b6-95ed-37c397b9479e');

-- Volcando estructura para tabla dentassistdb.aspnetusers
CREATE TABLE IF NOT EXISTS `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetusers: ~9 rows (aproximadamente)
INSERT INTO `aspnetusers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`) VALUES
	('112c52c3-b5dd-4c68-a1a6-7357ca5dc2a4', 'Maria.a@dentassist.cl', 'MARIA.A@DENTASSIST.CL', 'Maria.a@dentassist.cl', 'MARIA.A@DENTASSIST.CL', 0, 'AQAAAAIAAYagAAAAEDmvVI5Rdx6+Xl8TTi3NP49MsK5rOGMAPXLapYxVozYjXxZ0ww2XZltZ7VgkMXIm6A==', 'FNDZUAIFWGTU4OYAFQQFZM7QO74RIDYK', 'b6c86bb4-8915-40c7-90b4-e7b564382d75', NULL, 0, 0, NULL, 1, 0),
	('4d2ed48b-72eb-496b-b6a3-1011429c96c0', 'Deer@dentassist.cl', 'DEER@DENTASSIST.CL', 'Deer@dentassist.cl', 'DEER@DENTASSIST.CL', 0, 'AQAAAAIAAYagAAAAEKON9bxlwGkpD2QFHGf3YFpdLANwmLJT/hfi00bIrc/ObYXyuZ2DOJCRMbn2J/XDEQ==', 'XZWDXJAIB7NFCIWQL55DE2BVMB4SQP6G', '8f87dc23-94c4-4c9c-986a-2692ac609bd5', NULL, 0, 0, NULL, 1, 0),
	('5ec295bf-e88a-418c-a392-5378274c6029', 'Javi@dentassist.cl', 'JAVI@DENTASSIST.CL', 'Javi@dentassist.cl', 'JAVI@DENTASSIST.CL', 0, 'AQAAAAIAAYagAAAAEOM/NKP1HzyBzc210HPH4fGnuIYgdkh7leMiR/Pq1jH9ReNJD7dTm0T8FZtmESdcoQ==', 'BHWSZPP4LQAVFGYGFF575QBYJVEHNR4S', '7952f649-2c45-4727-ac44-07c3795c2681', NULL, 0, 0, NULL, 1, 0),
	('7ce02f71-9c55-4cc5-8937-37e307e01191', 'Orellana@sonrisaplena.cl', 'ORELLANA@SONRISAPLENA.CL', 'Orellana@sonrisaplena.cl', 'ORELLANA@SONRISAPLENA.CL', 0, 'AQAAAAIAAYagAAAAEFeR3peUfMCrBn8+WAFR9P+yesRCLznHCSmaElvlZNUw49IhoXT1+4B/ZtaZjHflbw==', 'JY47CRXIQMS4N35SRR3WIGG6DEYS6KMV', '1e0385a9-5d3f-49c7-8e20-4810d72f591d', NULL, 0, 0, NULL, 1, 0),
	('850c510c-a1d0-43f9-b479-296e72abf551', 'Eugenio@odo.cl', 'EUGENIO@ODO.CL', 'Eugenio@odo.cl', 'EUGENIO@ODO.CL', 0, 'AQAAAAIAAYagAAAAEGaGaRTIVDRDQUxtemAWm+QfmuRSU5AYwb+cf6ifYWyAavi+OLm1QDTtUlo96qVi8g==', 'DLRY2ZRD3ZQFLTW3E3UK2DTSRACJZ2NS', '447ca0a9-c52d-4aed-9e8e-1d501a54acf3', NULL, 0, 0, NULL, 1, 0),
	('9107bc7f-9eea-4319-9fe4-bbd41a7e0633', 'j.m@dentassist.cl', 'J.M@DENTASSIST.CL', 'j.m@dentassist.cl', 'J.M@DENTASSIST.CL', 0, 'AQAAAAIAAYagAAAAEPl+/y4J0LksDefe7PwjxnhVbhZPirlrPD983eJlLXiN+m/XbMdvSX8dqReoMb1TeQ==', 'Y4JIAAOLHH4PE4LSYUYHBI5DTGROFSNP', '76e7b65d-a31d-4c95-920e-aba241b42fc5', NULL, 0, 0, NULL, 1, 0),
	('9a758d3c-7d50-4ca2-b88d-0152d06449de', 'Eugenio@odon.cl', 'EUGENIO@ODON.CL', 'Eugenio@odon.cl', 'EUGENIO@ODON.CL', 0, 'AQAAAAIAAYagAAAAEADztjqx2jfz/KPnhZjRtkAIszATaUeFJ77SEZq34HDreq16GUIMuTe/BRjuRxTFtQ==', 'CLGJ5TTQ4IGBF2STHUDQLWNW6DHDGVEH', 'b1b16613-1d49-4498-a04d-266ff238575c', NULL, 0, 0, NULL, 1, 0),
	('c087cc58-32a8-4899-b77b-1db9575c8b5f', 'Pool@Sonrisaplena.cl', 'POOL@SONRISAPLENA.CL', 'Pool@Sonrisaplena.cl', 'POOL@SONRISAPLENA.CL', 0, 'AQAAAAIAAYagAAAAEMNaw69z+xGhbPdjsvx3UpOnjS106hBJAIG8Kb9QYGS8uhLjqq3nzGAblicvvhmiHQ==', 'LXAIQ5FXIPFFIIR7MKNCTXECJYDQOYNS', 'cc2fdc23-aa0e-44d0-bb5a-e7349af74950', NULL, 0, 0, NULL, 1, 0),
	('e51c34e0-1f11-4947-8212-ea9226703cf6', 'admin@dentassist.com', 'ADMIN@DENTASSIST.COM', 'admin@dentassist.com', 'ADMIN@DENTASSIST.COM', 0, 'AQAAAAIAAYagAAAAEOeDr4niGKx4sRo+5YA3Qacj051KwKUgt30OtUru4uxaLiTYXe3m6exFZ3x+VdNRIg==', 'ZVOAJTP4GGRNW6RNSHHXHRO5I7GWEIHL', 'a70946a5-ceed-4de6-8a22-b14c5b28d11f', NULL, 0, 0, NULL, 1, 0);

-- Volcando estructura para tabla dentassistdb.aspnetusertokens
CREATE TABLE IF NOT EXISTS `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.aspnetusertokens: ~0 rows (aproximadamente)

-- Volcando estructura para tabla dentassistdb.configuraciones
CREATE TABLE IF NOT EXISTS `configuraciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NombreClinica` varchar(150) NOT NULL,
  `Direccion` varchar(200) NOT NULL,
  `Telefono` varchar(30) NOT NULL,
  `EmailContacto` varchar(80) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.configuraciones: ~1 rows (aproximadamente)
INSERT INTO `configuraciones` (`Id`, `NombreClinica`, `Direccion`, `Telefono`, `EmailContacto`) VALUES
	(1, 'Sonrisa plena', 'O\'Carrol, 63, Rancagua', '722782930', 'Info@SonrisaPlena.cl');

-- Volcando estructura para tabla dentassistdb.odontologo
CREATE TABLE IF NOT EXISTS `odontologo` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Rut` varchar(12) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Especialidad` varchar(100) NOT NULL,
  `Direccion` varchar(100) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Email` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.odontologo: ~3 rows (aproximadamente)
INSERT INTO `odontologo` (`Id`, `Rut`, `Nombre`, `Apellido`, `Especialidad`, `Direccion`, `Telefono`, `Email`) VALUES
	(3, '21ef4245', 'Javiera', 'Marchant', 'Cirujano', 'Calle 123', '+56977004701', 'j.m@dentassist.cl'),
	(4, '211574674', 'Francis', 'Deer', 'Dentista', 'Calle123', '977004791', 'Deer@dentassist.cl'),
	(5, '775433220', 'André', 'Orellana', 'Cirujano Dentista', 'Calle 234', '988113696', 'Orellana@sonrisaplena.cl');

-- Volcando estructura para tabla dentassistdb.odontologos
CREATE TABLE IF NOT EXISTS `odontologos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Rut` varchar(12) NOT NULL,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Especialidad` varchar(100) NOT NULL,
  `Direccion` varchar(200) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Email` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.odontologos: ~0 rows (aproximadamente)

-- Volcando estructura para tabla dentassistdb.pacientes
CREATE TABLE IF NOT EXISTS `pacientes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(50) NOT NULL,
  `Apellido` varchar(50) NOT NULL,
  `Rut` varchar(12) NOT NULL,
  `Direccion` varchar(100) NOT NULL,
  `FechaNacimiento` datetime(6) NOT NULL,
  `Telefono` varchar(15) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `OdontologoId` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`Id`),
  KEY `IX_Pacientes_OdontologoId` (`OdontologoId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.pacientes: ~3 rows (aproximadamente)
INSERT INTO `pacientes` (`Id`, `Nombre`, `Apellido`, `Rut`, `Direccion`, `FechaNacimiento`, `Telefono`, `Email`, `OdontologoId`) VALUES
	(4, 'Rodrigo Andres', 'Carrasco Araya', '9987547k', 'calle123', '2002-10-10 00:00:00.000000', '987542476', 'roroc@gmail.com', 0),
	(5, 'Felipe', 'Araya', '182292230', 'O\'carrol 5', '2025-10-10 00:00:00.000000', '987542476', 'Fliara@gmail.com', 3),
	(6, 'Eduardo', 'Inostroza', '127958653', 'calle123', '1975-05-05 00:00:00.000000', '912348728', 'EdoInos@gmail.com', 3);

-- Volcando estructura para tabla dentassistdb.pasotratamientos
CREATE TABLE IF NOT EXISTS `pasotratamientos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlanTratamientoId` int(11) NOT NULL,
  `TratamientoId` int(11) NOT NULL,
  `Descripcion` varchar(200) DEFAULT NULL,
  `FechaEstimada` date NOT NULL,
  `Estado` varchar(20) NOT NULL,
  `Precio` decimal(18,2) DEFAULT NULL,
  `ObservacionesClinicas` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Paso_Plan` (`PlanTratamientoId`),
  KEY `IX_Paso_Trat` (`TratamientoId`),
  CONSTRAINT `FK_Paso_PlanTrat` FOREIGN KEY (`PlanTratamientoId`) REFERENCES `plantratamientos` (`Id`),
  CONSTRAINT `FK_Paso_Tratamientos` FOREIGN KEY (`TratamientoId`) REFERENCES `tratamientos` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.pasotratamientos: ~4 rows (aproximadamente)
INSERT INTO `pasotratamientos` (`Id`, `PlanTratamientoId`, `TratamientoId`, `Descripcion`, `FechaEstimada`, `Estado`, `Precio`, `ObservacionesClinicas`) VALUES
	(4, 4, 4, 'Profilaxis dental', '2025-06-30', 'Realizado', 25000.00, 'Limpieza supragingival con cepillo y pasta profiláctica.'),
	(5, 4, 6, 'Barniz de flúor', '2025-07-05', 'Realizado', 10000.00, 'Refuerzo de esmalte; paciente con riesgo moderado.'),
	(6, 4, 4, 'Profilaxis dental', '2025-07-07', 'Realizado', 25000.00, 'Limpieza supragingival con cepillo y pasta profiláctica.'),
	(7, 4, 17, 'Implante dental', '2025-08-08', 'pendiente', 500000.00, 'Colocación de implante y corona; carga diferida a 3 meses.');

-- Volcando estructura para tabla dentassistdb.plantratamientos
CREATE TABLE IF NOT EXISTS `plantratamientos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PacienteId` int(11) NOT NULL,
  `FechaCreacion` datetime NOT NULL DEFAULT current_timestamp(),
  `Observaciones` varchar(500) NOT NULL,
  `Precio` decimal(18,2) DEFAULT NULL,
  `TratamientoId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `PacienteId` (`PacienteId`),
  KEY `FK_PlanTratamientos_Tratamientos` (`TratamientoId`),
  CONSTRAINT `FK_PlanTratamientos_Tratamientos` FOREIGN KEY (`TratamientoId`) REFERENCES `tratamientos` (`Id`),
  CONSTRAINT `plantratamientos_ibfk_1` FOREIGN KEY (`Id`) REFERENCES `tratamientos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `plantratamientos_ibfk_2` FOREIGN KEY (`PacienteId`) REFERENCES `pacientes` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.plantratamientos: ~1 rows (aproximadamente)
INSERT INTO `plantratamientos` (`Id`, `PacienteId`, `FechaCreacion`, `Observaciones`, `Precio`, `TratamientoId`) VALUES
	(4, 5, '2025-06-20 19:29:00', 'Evaluación de tejidos blandos; sin hallazgos patológicos.', 20000.00, 1);

-- Volcando estructura para tabla dentassistdb.recepcionistas
CREATE TABLE IF NOT EXISTS `recepcionistas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Password` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.recepcionistas: ~1 rows (aproximadamente)
INSERT INTO `recepcionistas` (`Id`, `Nombre`, `Email`, `Password`) VALUES
	(3, 'Maria Andrade', 'Maria.a@dentassist.cl', 'Admin123!');

-- Volcando estructura para tabla dentassistdb.tratamientos
CREATE TABLE IF NOT EXISTS `tratamientos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(200) NOT NULL,
  `Descripcion` varchar(500) DEFAULT NULL,
  `Precio` decimal(18,2) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.tratamientos: ~16 rows (aproximadamente)
INSERT INTO `tratamientos` (`Id`, `Nombre`, `Descripcion`, `Precio`) VALUES
	(1, 'Examen clínico inicial ', 'Inspección y palpación de dientes y tejidos para diagnóstico preliminar.', 20000.00),
	(2, 'Radiografía bite‐wing', 'Captura de imagen de caras proximales posteriores para detectar caries interproximales.', 15000.00),
	(3, 'Radiografía panorámica', 'Toma radiográfica de toda la arcada y estructuras óseas circundantes.', 30000.00),
	(4, 'Profilaxis dental', 'Limpieza profesional de placa y sarro supragingival con instrumentos manuales y pastas.', 25000.00),
	(5, 'Sellado de fosas y fisuras', 'Aplicación de resina selladora en surcos para prevenir caries en molares y premolares.', 15000.00),
	(6, 'Barniz de flúor', 'Recubrimiento tópico de flúor para fortalecer el esmalte y prevenir desmineralización.', 10000.00),
	(7, 'Detartraje manual', 'Raspado manual del sarro supragingival con curetas y gubias.', 20000.00),
	(8, 'Detartraje ultrasónico', 'Desbridamiento de placa y sarro supragingival y subgingival mediante punta ultrasónica.', 30000.00),
	(9, 'Restauración con resina (cavidad pequeña)', 'Reconstrucción estética de cavidades superficiales con resina compuesta.', 35000.00),
	(10, 'Restauración con resina (cavidad mediana)', 'Restauración de cavidades de mayor tamaño con modelado y pulido de resina.', 45000.00),
	(11, 'Restauración con amalgama	', 'Reparación de cavidades posteriores usando amalgama metálica de alta resistencia.', 40000.00),
	(12, 'Tratamiento endodóntico molar', 'Desinfección y obturación de canales en molares multirradiculares.', 120000.00),
	(13, 'Extracción dental simple', 'Remoción de diente sin retención ósea ni complicaciones quirúrgicas.', 50000.00),
	(15, 'Corona cerámica', 'Restauración definitiva de diente con corona de porcelana para función y estética.', 200000.00),
	(16, 'Puente fijo de 3 unidades', 'Reemplazo de pieza ausente con prótesis fija anclada en dos coronas.', 600000.00),
	(17, 'Implante dental', 'Inserción de tornillo de titanio en hueso y colocación de corona definitiva.', 500000.00);

-- Volcando estructura para tabla dentassistdb.turnos
CREATE TABLE IF NOT EXISTS `turnos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PacienteId` int(11) NOT NULL,
  `OdontologoId` int(11) NOT NULL,
  `FechaHora` datetime NOT NULL,
  `Duracion` int(11) NOT NULL,
  `Observaciones` longtext NOT NULL,
  `Estado` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.turnos: ~3 rows (aproximadamente)
INSERT INTO `turnos` (`Id`, `PacienteId`, `OdontologoId`, `FechaHora`, `Duracion`, `Observaciones`, `Estado`) VALUES
	(6, 4, 4, '2025-06-18 10:30:00', 30, '-', 4),
	(7, 5, 3, '2025-06-20 02:36:36', 40, '-', 0),
	(8, 6, 3, '2025-06-19 10:00:46', 50, '-', 1);

-- Volcando estructura para tabla dentassistdb.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla dentassistdb.__efmigrationshistory: ~14 rows (aproximadamente)
INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
	('20250614052949_InicialMariaDb', '9.0.6'),
	('20250614060330_AgregarIdentity', '9.0.6'),
	('20250614073045_AgregarPlanes', '9.0.6'),
	('20250615000139_AgregarPasswordOdontologo', '9.0.6'),
	('20250615001237_CrearRecepcionista', '9.0.6'),
	('20250615002349_CrearTablaTratamientos', '9.0.6'),
	('20250615003750_CrearConfiguracion', '9.0.6'),
	('20250615011423_AgregaRolOdontologo', '9.0.6'),
	('20250615035718_QuitaPasswordDeOdontologo', '9.0.6'),
	('20250616180617_CrearTablaOdontologo', '9.0.6'),
	('20250616211911_TurnoCompleto', '9.0.6'),
	('20250617021110_CrearTablaTurnos', '9.0.6'),
	('20250617073618_ReiniciaTurnos', '9.0.6'),
	('20250617074044_CrearTurnos', '9.0.6');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;

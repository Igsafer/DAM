CREATE DATABASE IF NOT EXISTS `tpv`/*!40100 DEFAULT CHARACTER SET latin1 */;
USE `tpv`;
-- MySQL dump 10.13  Distrib 8.0.32, for Win64 (x86_64)
--
-- Host: localhost    Database: tpv
-- ------------------------------------------------------
-- Server version	5.7.38-log

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
-- Table structure for table `cliente`
--

DROP TABLE IF EXISTS `cliente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nombrecompleto` varchar(100) NOT NULL,
  `nombre` varchar(50) NOT NULL,
  `apellidos` varchar(50) NOT NULL,
  `email` varchar(50) NOT NULL,
  `direccion` varchar(50) NOT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente`
--

LOCK TABLES `cliente` WRITE;
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` VALUES (1,'Evaristo Rey de la Baraja','Evaristo','Rey de la Baraja','evaristorey@gmail.com','C/Falsa 123'),(2,'Rubén Cacho Sánchez','Rubén','Cacho Sánchez','rubencsanchez@gmail.com','C/Privilegiado 25'),(3,'Natalia Dumala','Natalia','Dumala','ndumala@gmal.com','C/ Polonia 55'),(4,'Jesús Quesada','Jesús','Quesada','jquesada@gmail.com','C/ Momentos 2'),(5,'Homero Lápida','Homero','Lápida','homerolapidasim@gmail.com','Avenida Sim 5'),(6,'Julián Palacios','Julián','Palacios','jpalacios@gmail.com','C/ El Sitio 4'),(7,'Emilio Delgado','Emilio','Delgado','emiliodelgado@gmail.com','C/ Desengaño 21'),(8,'Luis Brazofuertre','Luis','Brazofuertre','larmstrong@gmail.com','C/ Mundo Maravilloso 63'),(9,'Mari Carmen Cañizares','Mari Carmen','Cañizares','mcanizares@gmail.com','C/ Calcetines 44'),(10,'Gregorio Antúnez','Gregorio','Antúnez','gantunez@gmail.com','C/ La Virgen 32'),(11,'Victoria De La Vega','Victoria','De La Vega','vdelavega@gmail.com','Avenida Calvario 1'),(12,'Mónica Salazar','Mónica','Salazar','msalazar@gmail.com','C/ Alicante 21'),(13,'Aniceto Papandujo','Aniceto','Papandujo','apapandujo@gmail.com','C/ Alzira 40');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cliente_has_oferta`
--

DROP TABLE IF EXISTS `cliente_has_oferta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente_has_oferta` (
  `cliente` int(11) NOT NULL,
  `oferta` int(11) NOT NULL,
  PRIMARY KEY (`cliente`,`oferta`),
  KEY `oferta` (`oferta`),
  CONSTRAINT `cliente_has_oferta_ibfk_1` FOREIGN KEY (`oferta`) REFERENCES `oferta` (`codigo`),
  CONSTRAINT `cliente_has_oferta_ibfk_2` FOREIGN KEY (`cliente`) REFERENCES `cliente` (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente_has_oferta`
--

LOCK TABLES `cliente_has_oferta` WRITE;
/*!40000 ALTER TABLE `cliente_has_oferta` DISABLE KEYS */;
/*!40000 ALTER TABLE `cliente_has_oferta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `oferta`
--

DROP TABLE IF EXISTS `oferta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `oferta` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(200) DEFAULT NULL,
  `periodo` varchar(50) NOT NULL,
  `ubicacion` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oferta`
--

LOCK TABLES `oferta` WRITE;
/*!40000 ALTER TABLE `oferta` DISABLE KEYS */;
/*!40000 ALTER TABLE `oferta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permiso`
--

DROP TABLE IF EXISTS `permiso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permiso` (
  `codigo` int(11) NOT NULL,
  `descripcion` varchar(200) NOT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permiso`
--

LOCK TABLES `permiso` WRITE;
/*!40000 ALTER TABLE `permiso` DISABLE KEYS */;
INSERT INTO `permiso` VALUES (1,'Introducir ventas'),(2,'Devolución de ventas'),(3,'Introducir productos/clientes'),(4,'Modificar/eliminar productos/clientes'),(5,'Introducir usuarios'),(6,'Modificar/eliminar usuarios'),(7,'Cambiar contraseñas');
/*!40000 ALTER TABLE `permiso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `producto`
--

DROP TABLE IF EXISTS `producto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `producto` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(200) NOT NULL,
  `precio` double NOT NULL,
  `ubicacion` varchar(50) DEFAULT NULL,
  `cantidad` int(11) NOT NULL,
  `tipo` int(11) NOT NULL,
  `oferta` int(11) DEFAULT NULL,
  `imagen` varchar(80) NOT NULL,
  `iva` int(2) NOT NULL,
  PRIMARY KEY (`codigo`),
  KEY `tipo` (`tipo`),
  KEY `oferta` (`oferta`),
  CONSTRAINT `producto_ibfk_1` FOREIGN KEY (`tipo`) REFERENCES `tipoproducto` (`codigo`),
  CONSTRAINT `producto_ibfk_2` FOREIGN KEY (`oferta`) REFERENCES `oferta` (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `producto`
--

LOCK TABLES `producto` WRITE;
/*!40000 ALTER TABLE `producto` DISABLE KEYS */;
INSERT INTO `producto` VALUES (1,'Estrella Galicia',2.5,NULL,98,1,NULL,'/Recursos/Imagenes/estrellagalicia.png',21),(2,'Túria',2.5,NULL,102,1,NULL,'/Recursos/Imagenes/turia.jpg',21),(3,'Voll Damm',3,NULL,102,1,NULL,'/Recursos/Imagenes/volldamm.png',21),(4,'Loki',3.5,NULL,78,2,NULL,'/Recursos/Imagenes/loki.png',21),(5,'Odín',3,NULL,30,2,NULL,'/Recursos/Imagenes/odin.png',21),(6,'Hela',3.5,NULL,80,2,NULL,'/Recursos/Imagenes/hela.png',21),(7,'Limonzio',7,NULL,96,3,NULL,'/Recursos/Imagenes/limonzio.jpg',21),(8,'Ruso blanco',7,NULL,67,3,NULL,'/Recursos/Imagenes/rusoblanco.png',21),(9,'Cóctel',8.5,NULL,42,3,NULL,'/Recursos/Imagenes/coctel.jpg',21),(10,'Botella 33cl',1,NULL,73,4,NULL,'/Recursos/Imagenes/agua33cl.jpg',21),(11,'Vichy Catalán',2,NULL,25,4,NULL,'/Recursos/Imagenes/vichycatalan.jpg',21),(12,'Coca-Cola',2,NULL,291,5,NULL,'/Recursos/Imagenes/cocacola.jpg',21),(13,'Nestea',2,NULL,50,5,NULL,'/Recursos/Imagenes/nestea.jpg',21),(14,'Sprite',2,NULL,96,5,NULL,'/Recursos/Imagenes/sprite.jpg',21),(15,'Tónica Schweppes',2,NULL,64,6,NULL,'/Recursos/Imagenes/tonicaoriginal.jpg',21),(16,'Ginger Ale',2,NULL,54,6,NULL,'/Recursos/Imagenes/gingerale.jpg',21),(17,'San Miguel 0,0',1.5,NULL,19,7,NULL,'/Recursos/Imagenes/sanmiguel.png',21),(18,'Estrella Galicia 0,0',1.5,NULL,26,7,NULL,'/Recursos/Imagenes/estrellagalicia00.png',21),(19,'Red Bull',2.5,NULL,29,8,NULL,'/Recursos/Imagenes/redbull.png',21),(20,'Monster',2,NULL,47,8,NULL,'/Recursos/Imagenes/monster.jpg',21),(21,'Ramón Bilbao',2.5,'',96,9,NULL,'/Recursos/Imagenes/ramonbilbao.png',21),(22,'Sangre de Judas',3,'',84,9,NULL,'/Recursos/Imagenes/sangredejudas.jpg',21),(23,'Malavida',2.2,'',106,9,NULL,'/Recursos/Imagenes/malavida.png',21),(24,'Protos',3,'',78,9,NULL,'/Recursos/Imagenes/protos.png',21),(25,'Medio',5,'',40,10,NULL,'/Recursos/Imagenes/mediobocadillo.jpg',21),(26,'Entero',7.5,'',38,10,NULL,'/Recursos/Imagenes/bocadilloentero.jpg',21),(27,'Especial',8,'',14,10,NULL,'/Recursos/Imagenes/bocadilloespecial.jpg',21),(28,'Hot Dog',4,'',32,10,NULL,'/Recursos/Imagenes/hotdog.jpg',21),(29,'Solo',1,'',37,11,NULL,'/Recursos/Imagenes/cafesolo.png',21),(30,'Con leche',1.2,'',35,11,NULL,'/Recursos/Imagenes/cafeconleche.jpg',21),(31,'Capuccino',1.5,'',40,11,NULL,'/Recursos/Imagenes/cafecapuccino.jpg',21),(32,'Descafeinado',1,'',40,11,NULL,'/Recursos/Imagenes/cafedescafeinado.jpg',21);
/*!40000 ALTER TABLE `producto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol`
--

DROP TABLE IF EXISTS `rol`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rol` (
  `codigo` int(11) NOT NULL,
  `nombre` varchar(20) NOT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol`
--

LOCK TABLES `rol` WRITE;
/*!40000 ALTER TABLE `rol` DISABLE KEYS */;
INSERT INTO `rol` VALUES (1,'Gerente'),(2,'Encargado'),(3,'Empleado');
/*!40000 ALTER TABLE `rol` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rol_has_permiso`
--

DROP TABLE IF EXISTS `rol_has_permiso`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rol_has_permiso` (
  `rol` int(11) NOT NULL,
  `permiso` int(11) NOT NULL,
  PRIMARY KEY (`rol`,`permiso`),
  KEY `permiso` (`permiso`),
  CONSTRAINT `rol_has_permiso_ibfk_1` FOREIGN KEY (`rol`) REFERENCES `rol` (`codigo`),
  CONSTRAINT `rol_has_permiso_ibfk_2` FOREIGN KEY (`permiso`) REFERENCES `permiso` (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rol_has_permiso`
--

LOCK TABLES `rol_has_permiso` WRITE;
/*!40000 ALTER TABLE `rol_has_permiso` DISABLE KEYS */;
INSERT INTO `rol_has_permiso` VALUES (1,1),(2,1),(3,1),(1,2),(2,2),(1,3),(2,3),(3,3),(1,4),(2,4),(1,5),(2,5),(1,6),(1,7),(2,7),(3,7);
/*!40000 ALTER TABLE `rol_has_permiso` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipoproducto`
--

DROP TABLE IF EXISTS `tipoproducto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipoproducto` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `imagen` varchar(80) NOT NULL,
  PRIMARY KEY (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipoproducto`
--

LOCK TABLES `tipoproducto` WRITE;
/*!40000 ALTER TABLE `tipoproducto` DISABLE KEYS */;
INSERT INTO `tipoproducto` VALUES (1,'Cervezas','/Recursos/Imagenes/cerveza.jpg'),(2,'Hidromieles','/Recursos/Imagenes/hidromiel.png'),(3,'Copas','/Recursos/Imagenes/copa.jpg'),(4,'Aguas','/Recursos/Imagenes/agua.jpg'),(5,'Refrescos','/Recursos/Imagenes/refresco.jpg'),(6,'Tónicas','/Recursos/Imagenes/tonica.jpg'),(7,'Cervezas 0,0','/Recursos/Imagenes/cerveza00.jpg'),(8,'Energéticas','/Recursos/Imagenes/energeticas.jpg'),(9,'Vinos','/Recursos/Imagenes/vino.jpg'),(10,'Bocadillos','/Recursos/Imagenes/bocadillos.jpg'),(11,'Cafés','/Recursos/Imagenes/cafes.png');
/*!40000 ALTER TABLE `tipoproducto` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellidos` varchar(50) NOT NULL,
  `login` varchar(50) NOT NULL,
  `contraseña` varchar(50) NOT NULL,
  `rol` int(11) NOT NULL,
  PRIMARY KEY (`codigo`),
  KEY `rol` (`rol`),
  CONSTRAINT `usuario_ibfk_1` FOREIGN KEY (`rol`) REFERENCES `rol` (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuario`
--

LOCK TABLES `usuario` WRITE;
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` VALUES (1,'Gerente','Gerencio','gerente','gerente',1),(2,'Encagardo','Encargado','encargado','encargado',2),(3,'Empleado','Asalariado','empleado','empleado',3),(4,'Antonio','Mundánez','mundanez','mundanez',3),(5,'Pío','Baroja','pio','pio',3),(6,'Miguel','De Unamuno','unamuno','unamuno',2),(7,'Antonio','Machado','machado','machado',2),(8,'Ignacio','Sanz Ferri','iggy322','iggy322',1),(9,'Ramon María','Del Valle-Inclán','valleinclan','valleinclan',2),(10,'Jacinto','Benavente','benavente','benavente',2),(11,'Ángel','Ganivet','ganivet','ganivet',3),(12,'Vicente','Blasco Ibáñez','vblasco','vblasco',3),(13,'Luis','Ruiz Contreras','contreras','contreras',2),(14,'Concha','Espina','espina','espina',2),(15,'Carmen','De Burgos','deburgos','deburgos',3),(16,'Carmen','Karr','karr','karr',3);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `venta`
--

DROP TABLE IF EXISTS `venta`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `venta` (
  `codigo` int(11) NOT NULL AUTO_INCREMENT,
  `fecha` datetime DEFAULT NULL,
  `tipocobro` varchar(20) DEFAULT NULL,
  `total` double NOT NULL,
  `mensaje` varchar(50) DEFAULT NULL,
  `usuario` int(11) NOT NULL,
  `cliente` int(11) DEFAULT NULL,
  PRIMARY KEY (`codigo`),
  KEY `usuario` (`usuario`),
  KEY `cliente` (`cliente`),
  CONSTRAINT `venta_ibfk_1` FOREIGN KEY (`usuario`) REFERENCES `usuario` (`codigo`),
  CONSTRAINT `venta_ibfk_2` FOREIGN KEY (`cliente`) REFERENCES `cliente` (`codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `venta`
--

LOCK TABLES `venta` WRITE;
/*!40000 ALTER TABLE `venta` DISABLE KEYS */;
INSERT INTO `venta` VALUES (1,'2023-02-22 22:54:35','Efectivo',10,NULL,1,12),(14,'2023-05-04 09:31:45','Efectivo',38.5,'',1,8),(15,'2023-04-14 23:32:10','Bizum',16.4,'',1,11),(16,'2023-05-05 01:32:41','Efectivo',13,'',1,3),(17,'2023-04-14 01:43:11','Bizum',55.3,'',1,5),(18,'2023-06-02 18:24:58','Bizum',13.2,'',1,1),(19,'2023-03-08 18:25:25','Tarjeta',8.5,'',1,6),(20,'2023-06-02 18:25:51','Tarjeta',11,'',1,10),(21,'2023-04-12 18:31:34','Bizum',49,'',1,3),(22,'2023-05-29 18:33:12','Tarjeta',120,'',1,2),(23,'2023-05-13 18:39:14',NULL,68.5,'',1,7),(24,'2023-06-07 18:40:36','Efectivo',41.5,'',1,11),(25,'2023-02-28 18:44:38',NULL,35.5,'',1,13),(26,'2023-06-02 18:45:24','Bizum',62.8,'',1,9),(27,'2023-05-28 18:56:33',NULL,16.7,'',1,3),(28,'2023-05-04 18:56:48','Efectivo',15,'',1,5),(29,'2023-05-10 19:01:26','Bizum',25.5,'',1,4),(30,'2023-04-06 19:01:45','Tarjeta',15.5,'',1,7),(31,'2023-06-02 19:02:09','Efectivo',81,'',1,6);
/*!40000 ALTER TABLE `venta` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `venta_producto`
--

DROP TABLE IF EXISTS `venta_producto`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `venta_producto` (
  `venta` int(11) NOT NULL,
  `producto` int(11) NOT NULL,
  `cantidad` int(11) DEFAULT NULL,
  `preciototal` double DEFAULT NULL,
  PRIMARY KEY (`venta`,`producto`),
  KEY `producto` (`producto`),
  CONSTRAINT `venta_producto_ibfk_1` FOREIGN KEY (`venta`) REFERENCES `venta` (`codigo`),
  CONSTRAINT `venta_producto_ibfk_2` FOREIGN KEY (`producto`) REFERENCES `producto` (`codigo`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `venta_producto`
--

LOCK TABLES `venta_producto` WRITE;
/*!40000 ALTER TABLE `venta_producto` DISABLE KEYS */;
INSERT INTO `venta_producto` VALUES (14,3,1,3),(14,4,1,3.5),(14,5,1,3),(14,8,4,28),(14,10,1,1),(15,18,4,6),(15,23,2,4.4),(15,24,2,6),(16,16,1,2),(16,17,1,1.5),(16,18,1,1.5),(16,19,1,2.5),(16,21,1,2.5),(16,22,1,3),(17,16,2,4),(17,17,2,3),(17,18,2,3),(17,19,2,5),(17,21,2,5),(17,22,3,9),(17,23,3,6.6000000000000005),(17,24,2,6),(17,26,1,7.5),(17,28,1,4),(17,29,1,1),(17,30,1,1.2),(18,23,6,13.2),(19,16,2,4),(19,17,2,3),(19,18,1,1.5),(20,21,2,5),(20,22,2,6),(21,12,2,4),(21,13,1,2),(21,14,1,2),(21,15,2,4),(21,16,1,2),(21,18,5,7.5),(21,26,1,7.5),(21,27,2,16),(21,28,1,4),(22,12,60,120),(23,1,1,2.5),(23,2,1,2.5),(23,5,1,3),(23,6,1,3.5),(23,7,2,14),(23,8,3,21),(23,9,2,17),(23,10,1,1),(23,11,1,2),(23,15,1,2),(24,1,1,2.5),(24,2,1,2.5),(24,3,1,3),(24,4,1,3.5),(24,5,1,3),(24,6,1,3.5),(24,7,1,7),(24,8,1,7),(24,9,1,8.5),(24,10,1,1),(25,11,3,6),(25,12,5,10),(25,13,1,2),(25,14,2,4),(25,15,2,4),(25,16,1,2),(25,17,2,3),(25,19,1,2.5),(25,20,1,2),(26,27,4,32),(26,28,6,24),(26,29,2,2),(26,30,4,4.8),(27,16,2,4),(27,17,1,1.5),(27,18,1,1.5),(27,19,1,2.5),(27,20,1,2),(27,22,1,3),(27,23,1,2.2),(28,3,5,15),(29,3,5,15),(29,6,3,10.5),(30,1,2,5),(30,4,3,10.5),(31,1,13,32.5),(31,2,11,27.5),(31,3,7,21);
/*!40000 ALTER TABLE `venta_producto` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-07 18:47:13

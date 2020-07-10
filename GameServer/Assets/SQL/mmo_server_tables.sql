-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               10.3.10-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             9.5.0.5366
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping structure for table mmo_server.accounts
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE IF NOT EXISTS `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `email` varchar(128) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.characters
DROP TABLE IF EXISTS `characters`;
CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `level` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  KEY `FK_characters_accounts` (`account_id`),
  CONSTRAINT `FK_characters_accounts` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc
DROP TABLE IF EXISTS `npc`;
CREATE TABLE IF NOT EXISTS `npc` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT 'Noname',
  `health` int(11) NOT NULL DEFAULT 0,
  `merchant` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc_entities
DROP TABLE IF EXISTS `npc_entities`;
CREATE TABLE IF NOT EXISTS `npc_entities` (
  `npc_id` int(10) unsigned NOT NULL,
  `position_x` float NOT NULL DEFAULT 0,
  `position_y` float NOT NULL DEFAULT 0,
  `position_z` float NOT NULL DEFAULT 0,
  `heading` float NOT NULL DEFAULT 0,
  KEY `FK_npc_entities_npc` (`npc_id`),
  CONSTRAINT `FK_npc_entities_npc` FOREIGN KEY (`npc_id`) REFERENCES `npc` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

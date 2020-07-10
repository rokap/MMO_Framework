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
-- Dumping structure for table mmo_server.npcs
CREATE TABLE IF NOT EXISTS `npcs` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT 'Noname',
  `health` int(11) NOT NULL DEFAULT 100,
  `prefab` varchar(50) NOT NULL DEFAULT 'Default',
  `merchant` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc_spawn
CREATE TABLE IF NOT EXISTS `npc_spawn` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_id` int(10) unsigned NOT NULL,
  `npc_spawn_group_id` int(10) unsigned DEFAULT NULL,
  `npc_spawn_point_id` int(10) unsigned DEFAULT NULL,
  `position_x` float NOT NULL DEFAULT 0,
  `position_y` float NOT NULL DEFAULT 0,
  `position_z` float NOT NULL DEFAULT 0,
  `heading` float NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  KEY `FK_npc_entities_npc` (`npc_id`),
  KEY `FK_npc_spawn_npc_spawn_points` (`npc_spawn_point_id`),
  KEY `FK_npc_entities_npc_spawn_group` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_entities_npc` FOREIGN KEY (`npc_id`) REFERENCES `npcs` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_npc_entities_npc_spawn_group` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `FK_npc_spawn_npc_spawn_points` FOREIGN KEY (`npc_spawn_point_id`) REFERENCES `npc_spawn_points` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc_spawn_groups
CREATE TABLE IF NOT EXISTS `npc_spawn_groups` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL DEFAULT '0',
  `npc_limit` int(10) unsigned NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc_spawn_groups_entries
CREATE TABLE IF NOT EXISTS `npc_spawn_groups_entries` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_spawn_group_id` int(10) unsigned NOT NULL DEFAULT 1,
  `npc_id` int(10) unsigned NOT NULL DEFAULT 0,
  `chance` decimal(5,2) unsigned NOT NULL DEFAULT 0.00,
  PRIMARY KEY (`id`),
  KEY `FK_npc_spawn_groups_entries_npc` (`npc_id`),
  KEY `FK_npc_spawn_groups_entries_npc_spawn_groups` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_spawn_groups_entries_npc` FOREIGN KEY (`npc_id`) REFERENCES `npcs` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_npc_spawn_groups_entries_npc_spawn_groups` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Data exporting was unselected.
-- Dumping structure for table mmo_server.npc_spawn_points
CREATE TABLE IF NOT EXISTS `npc_spawn_points` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_spawn_group_id` int(10) unsigned NOT NULL,
  `position_x` float NOT NULL DEFAULT 0,
  `position_y` float NOT NULL DEFAULT 0,
  `position_z` float NOT NULL DEFAULT 0,
  `heading` float NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`),
  KEY `FK_npc_spawn_points_npc_spawn_group` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_spawn_points_npc_spawn_group` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

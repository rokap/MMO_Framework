-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.0.21-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             10.2.0.5599
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
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=latin1;

-- Dumping data for table mmo_server.accounts: ~5 rows (approximately)
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` (`id`, `username`, `password`, `email`) VALUES
	(20, 'Jim', '123', 'dave@rokap.com'),
	(21, 'Dave', '123', 'dave@rokap.com'),
	(22, 'Ralph', '123', 'dave@rokap.com'),
	(23, 'Billy', '123', 'dave@rokap.com'),
	(44, 'Pee Pee Pants', 'Password', 'dave@somethingelse.com');
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;

-- Dumping structure for table mmo_server.characters
DROP TABLE IF EXISTS `characters`;
CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `level` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_characters_accounts` (`account_id`),
  CONSTRAINT `FK_characters_accounts` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1;

-- Dumping data for table mmo_server.characters: ~2 rows (approximately)
/*!40000 ALTER TABLE `characters` DISABLE KEYS */;
INSERT INTO `characters` (`id`, `account_id`, `name`, `level`) VALUES
	(3, 44, 'Something Else', 500),
	(4, 23, 'Flappy', 4);
/*!40000 ALTER TABLE `characters` ENABLE KEYS */;

-- Dumping structure for table mmo_server.items
DROP TABLE IF EXISTS `items`;
CREATE TABLE IF NOT EXISTS `items` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL DEFAULT 'NoName',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=latin1;

-- Dumping data for table mmo_server.items: ~3 rows (approximately)
/*!40000 ALTER TABLE `items` DISABLE KEYS */;
INSERT INTO `items` (`id`, `name`) VALUES
	(2, 'Jimmies Broad Sword'),
	(3, 'Dagger'),
	(5, 'Club');
/*!40000 ALTER TABLE `items` ENABLE KEYS */;

-- Dumping structure for table mmo_server.npcs
DROP TABLE IF EXISTS `npcs`;
CREATE TABLE IF NOT EXISTS `npcs` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT 'Noname',
  `health` int(11) NOT NULL DEFAULT '100',
  `prefab` varchar(50) NOT NULL DEFAULT 'Default',
  `merchant` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

-- Dumping data for table mmo_server.npcs: ~11 rows (approximately)
/*!40000 ALTER TABLE `npcs` DISABLE KEYS */;
INSERT INTO `npcs` (`id`, `name`, `health`, `prefab`, `merchant`) VALUES
	(1, 'Skeleton', 100, 'Default', 0),
	(2, 'Zombie', 100, 'Default', 0),
	(3, 'Gremlin', 100, 'Default', 0),
	(5, 'Tusker', 100, 'Default', 0),
	(6, 'Troll', 100, 'Default', 0),
	(7, 'Rat', 100, 'Default', 0),
	(8, 'Snake', 100, 'Default', 0),
	(9, 'Thug', 100, 'Default', 0),
	(10, 'Bee', 100, 'Default', 0),
	(11, 'Spider', 100, 'Default', 0),
	(12, 'Guard', 100, 'Default', 0);
/*!40000 ALTER TABLE `npcs` ENABLE KEYS */;

-- Dumping structure for table mmo_server.npc_spawn
DROP TABLE IF EXISTS `npc_spawn`;
CREATE TABLE IF NOT EXISTS `npc_spawn` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_id` int(10) unsigned NOT NULL,
  `npc_spawn_group_id` int(10) unsigned DEFAULT NULL,
  `npc_spawn_point_id` int(10) unsigned DEFAULT NULL,
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `heading` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_npc_entities_npc` (`npc_id`),
  KEY `FK_npc_spawn_npc_spawn_points` (`npc_spawn_point_id`),
  KEY `FK_npc_entities_npc_spawn_group` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_entities_npc` FOREIGN KEY (`npc_id`) REFERENCES `npcs` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_npc_entities_npc_spawn_group` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE SET NULL ON UPDATE SET NULL,
  CONSTRAINT `FK_npc_spawn_npc_spawn_points` FOREIGN KEY (`npc_spawn_point_id`) REFERENCES `npc_spawn_points` (`id`) ON DELETE SET NULL ON UPDATE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

-- Dumping data for table mmo_server.npc_spawn: ~1 rows (approximately)
/*!40000 ALTER TABLE `npc_spawn` DISABLE KEYS */;
INSERT INTO `npc_spawn` (`id`, `npc_id`, `npc_spawn_group_id`, `npc_spawn_point_id`, `position_x`, `position_y`, `position_z`, `heading`) VALUES
	(2, 1, 1, 1, 0, 0, 0, 0);
/*!40000 ALTER TABLE `npc_spawn` ENABLE KEYS */;

-- Dumping structure for table mmo_server.npc_spawn_groups
DROP TABLE IF EXISTS `npc_spawn_groups`;
CREATE TABLE IF NOT EXISTS `npc_spawn_groups` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL DEFAULT '0',
  `npc_limit` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Dumping data for table mmo_server.npc_spawn_groups: ~2 rows (approximately)
/*!40000 ALTER TABLE `npc_spawn_groups` DISABLE KEYS */;
INSERT INTO `npc_spawn_groups` (`id`, `name`, `npc_limit`) VALUES
	(1, 'Ravenshire Underworld', 5),
	(2, 'Ravenshire Town Guards', 0);
/*!40000 ALTER TABLE `npc_spawn_groups` ENABLE KEYS */;

-- Dumping structure for table mmo_server.npc_spawn_groups_entries
DROP TABLE IF EXISTS `npc_spawn_groups_entries`;
CREATE TABLE IF NOT EXISTS `npc_spawn_groups_entries` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_spawn_group_id` int(10) unsigned NOT NULL DEFAULT '1',
  `npc_id` int(10) unsigned NOT NULL DEFAULT '0',
  `chance` decimal(5,2) unsigned NOT NULL DEFAULT '0.00',
  PRIMARY KEY (`id`),
  KEY `FK_npc_spawn_groups_entries_npc` (`npc_id`),
  KEY `FK_npc_spawn_groups_entries_npc_spawn_groups` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_spawn_groups_entries_npc` FOREIGN KEY (`npc_id`) REFERENCES `npcs` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_npc_spawn_groups_entries_npc_spawn_groups` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Dumping data for table mmo_server.npc_spawn_groups_entries: ~3 rows (approximately)
/*!40000 ALTER TABLE `npc_spawn_groups_entries` DISABLE KEYS */;
INSERT INTO `npc_spawn_groups_entries` (`id`, `npc_spawn_group_id`, `npc_id`, `chance`) VALUES
	(4, 1, 1, 100.00),
	(5, 1, 1, 5.00),
	(6, 2, 12, 100.00);
/*!40000 ALTER TABLE `npc_spawn_groups_entries` ENABLE KEYS */;

-- Dumping structure for table mmo_server.npc_spawn_points
DROP TABLE IF EXISTS `npc_spawn_points`;
CREATE TABLE IF NOT EXISTS `npc_spawn_points` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `npc_spawn_group_id` int(10) unsigned NOT NULL,
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `heading` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_npc_spawn_points_npc_spawn_group` (`npc_spawn_group_id`),
  CONSTRAINT `FK_npc_spawn_points_npc_spawn_group` FOREIGN KEY (`npc_spawn_group_id`) REFERENCES `npc_spawn_groups` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=latin1 ROW_FORMAT=DYNAMIC;

-- Dumping data for table mmo_server.npc_spawn_points: ~4 rows (approximately)
/*!40000 ALTER TABLE `npc_spawn_points` DISABLE KEYS */;
INSERT INTO `npc_spawn_points` (`id`, `npc_spawn_group_id`, `position_x`, `position_y`, `position_z`, `heading`) VALUES
	(1, 1, 0, 0, 0, 0),
	(2, 2, 1, 0, 1, 0),
	(3, 2, 0, 0, 1, 0),
	(4, 2, -1, 0, 1, 0);
/*!40000 ALTER TABLE `npc_spawn_points` ENABLE KEYS */;

-- Dumping structure for table mmo_server.scenery
DROP TABLE IF EXISTS `scenery`;
CREATE TABLE IF NOT EXISTS `scenery` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL DEFAULT 'Noname',
  `prefab` varchar(50) NOT NULL DEFAULT 'Default',
  `description` text,
  `scale_x` float NOT NULL DEFAULT '1',
  `scale_y` float NOT NULL DEFAULT '1',
  `scale_z` float NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Dumping data for table mmo_server.scenery: ~2 rows (approximately)
/*!40000 ALTER TABLE `scenery` DISABLE KEYS */;
INSERT INTO `scenery` (`id`, `name`, `prefab`, `description`, `scale_x`, `scale_y`, `scale_z`) VALUES
	(7, 'Lamp Post', 'lamp_post_001', 'An Old Light Post, Made of Wood and Iron, The Candle Looks Weak and frail', 1, 1, 1),
	(23, 'Chest', 'chest_001', 'Rusty Chest, With a Broken Latch', 10, 10, 10);
/*!40000 ALTER TABLE `scenery` ENABLE KEYS */;

-- Dumping structure for table mmo_server.scenery_spawn_points
DROP TABLE IF EXISTS `scenery_spawn_points`;
CREATE TABLE IF NOT EXISTS `scenery_spawn_points` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `scenery_id` int(10) unsigned NOT NULL,
  `position_x` float NOT NULL DEFAULT '0',
  `position_y` float NOT NULL DEFAULT '0',
  `position_z` float NOT NULL DEFAULT '0',
  `rotation_x` float NOT NULL DEFAULT '0',
  `rotation_y` float NOT NULL DEFAULT '0',
  `rotation_z` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `FK_scenery_spawn_points_scenery` (`scenery_id`),
  CONSTRAINT `FK_scenery_spawn_points_scenery` FOREIGN KEY (`scenery_id`) REFERENCES `scenery` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

-- Dumping data for table mmo_server.scenery_spawn_points: ~6 rows (approximately)
/*!40000 ALTER TABLE `scenery_spawn_points` DISABLE KEYS */;
INSERT INTO `scenery_spawn_points` (`id`, `scenery_id`, `position_x`, `position_y`, `position_z`, `rotation_x`, `rotation_y`, `rotation_z`) VALUES
	(2, 7, -9.6, 0.5, -1.08, 0, 0, 0),
	(4, 7, -6.88, 0.5, -8.91, 0, 0, 0),
	(5, 7, -0.13, 0.5, 1.84, 0, 0, 0),
	(6, 7, -3.17, 0.5, -4.72, 0, 0, 0),
	(7, 7, -9.92, 0.5, -15.47, 0, 0, 0),
	(14, 23, -8.45, 5, -12.55, 0, 0, 0);
/*!40000 ALTER TABLE `scenery_spawn_points` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;

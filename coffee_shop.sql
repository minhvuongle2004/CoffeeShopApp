-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.0.41 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             12.6.0.6765
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for coffee_shop
CREATE DATABASE IF NOT EXISTS `coffee_shop` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `coffee_shop`;

-- Dumping structure for table coffee_shop.category
CREATE TABLE IF NOT EXISTS `category` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.category: ~7 rows (approximately)
INSERT INTO `category` (`id`, `name`) VALUES
	(1, 'Cà phê'),
	(2, 'Trà sữa'),
	(3, 'Nước ép'),
	(4, 'Bánh ngọt'),
	(6, 'Trà lạnh'),
	(7, 'Sinh tố'),
	(8, 'Trà');

-- Dumping structure for table coffee_shop.inventory
CREATE TABLE IF NOT EXISTS `inventory` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `stock` int NOT NULL DEFAULT '0',
  `unit` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.inventory: ~5 rows (approximately)
INSERT INTO `inventory` (`id`, `name`, `stock`, `unit`, `updated_at`) VALUES
	(1, 'Cà phê hạt', 10, 'kg', '2025-03-15 07:42:24'),
	(2, 'Sữa đặc', 20, 'hộp', '2025-03-15 07:42:24'),
	(3, 'Trân châu đen', 15, 'kg', '2025-03-15 07:42:24'),
	(4, 'Nước cam nguyên chất', 10, 'quả', '2025-03-20 10:08:44'),
	(6, 'Dưa hấu', 20, 'kg', '2025-03-20 10:08:32');

-- Dumping structure for table coffee_shop.menu
CREATE TABLE IF NOT EXISTS `menu` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `category_id` int NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `image` text COLLATE utf8mb4_general_ci,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `category_id` (`category_id`),
  CONSTRAINT `menu_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `category` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.menu: ~14 rows (approximately)
INSERT INTO `menu` (`id`, `name`, `category_id`, `price`, `image`, `created_at`) VALUES
	(18, 'Nước dưa hấu', 3, 50000.00, 'Image/dưa hấu.jpg', '2025-03-22 08:49:34'),
	(19, 'Cà phê muối', 1, 35000.00, 'Image/caphemuoi.png', '2025-03-26 08:22:47'),
	(20, 'Cà phê sữa', 1, 25000.00, 'image/caphesua.png', '2025-03-26 08:30:04'),
	(21, 'Cà phê đen', 1, 25000.00, 'image/caphenau.png', '2025-03-26 08:32:19'),
	(22, 'Nước ép cam', 3, 40000.00, 'Image/nuocepcam.png', '2025-03-26 08:32:49'),
	(23, 'Nước ép táo', 1, 40000.00, 'Image/nuoceptao.png', '2025-03-26 08:33:03'),
	(24, 'Sinh tố bơ', 7, 50000.00, 'Image/sinhtobo.png', '2025-03-26 08:33:30'),
	(25, 'Sinh tố xoài', 7, 50000.00, 'Image/sinhtoxoa.png', '2025-03-26 08:33:56'),
	(26, 'Tiramisu', 4, 35000.00, 'Image/tiramisu.png', '2025-03-26 08:34:19'),
	(27, 'Sữa chua cà phê', 1, 50000.00, 'Image/suachuacaphe.png', '2025-03-26 08:34:40'),
	(28, 'Trà cam vàng mật ong', 8, 35000.00, 'Image/trâcmvangsenman.png', '2025-03-26 08:35:27'),
	(29, 'Trà xoài kem cheese', 8, 50000.00, 'Image/traxoakemcheese.png', '2025-03-26 08:35:51'),
	(30, 'Trà sữa trân châu đường đen', 2, 40000.00, 'Image/tranchauduongden.png', '2025-03-26 08:36:30'),
	(31, 'Trà sữa cốm', 2, 50000.00, 'Image/trấucom.png', '2025-03-26 08:36:46'),
	(32, 'Trà nhài hoa cúc', 8, 35000.00, 'Image/tranhaihoacuc.png', '2025-03-26 08:37:06');

-- Dumping structure for table coffee_shop.orders
CREATE TABLE IF NOT EXISTS `orders` (
  `id` int NOT NULL AUTO_INCREMENT,
  `table_id` int NOT NULL,
  `user_id` int NOT NULL,
  `total_price` decimal(10,2) NOT NULL DEFAULT '0.00',
  `total_guest` int DEFAULT '1',
  `status` enum('pending','paid','canceled') COLLATE utf8mb4_general_ci DEFAULT 'pending',
  `start_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_time` timestamp NULL DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `table_id` (`table_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`table_id`) REFERENCES `tables` (`id`) ON DELETE CASCADE,
  CONSTRAINT `orders_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.orders: ~1 rows (approximately)
INSERT INTO `orders` (`id`, `table_id`, `user_id`, `total_price`, `total_guest`, `status`, `start_time`, `end_time`, `created_at`) VALUES
	(45, 7, 5, 85000.00, 2, 'pending', '2025-05-10 11:55:59', NULL, '2025-05-10 11:55:59');

-- Dumping structure for table coffee_shop.order_details
CREATE TABLE IF NOT EXISTS `order_details` (
  `id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `menu_id` int NOT NULL,
  `quantity` int NOT NULL DEFAULT '1',
  `subtotal` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `order_id` (`order_id`),
  KEY `menu_id` (`menu_id`),
  CONSTRAINT `order_details_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`) ON DELETE CASCADE,
  CONSTRAINT `order_details_ibfk_2` FOREIGN KEY (`menu_id`) REFERENCES `menu` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=58 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.order_details: ~2 rows (approximately)
INSERT INTO `order_details` (`id`, `order_id`, `menu_id`, `quantity`, `subtotal`) VALUES
	(56, 45, 26, 1, 35000.00),
	(57, 45, 27, 1, 50000.00);

-- Dumping structure for table coffee_shop.payments
CREATE TABLE IF NOT EXISTS `payments` (
  `id` int NOT NULL AUTO_INCREMENT,
  `order_id` int NOT NULL,
  `method` enum('cash','card','e-wallet') COLLATE utf8mb4_general_ci NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `paid_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `order_id` (`order_id`),
  CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.payments: ~3 rows (approximately)

-- Dumping structure for table coffee_shop.shifts
CREATE TABLE IF NOT EXISTS `shifts` (
  `id` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `user_id` int NOT NULL DEFAULT (0),
  `start_time` datetime NOT NULL,
  `end_time` datetime DEFAULT NULL,
  `opening_cash` double NOT NULL DEFAULT (0),
  `closed_cash` double DEFAULT '0',
  `total_cash` double NOT NULL DEFAULT (0),
  `total_bill` int NOT NULL DEFAULT (0),
  `status` enum('open','closed') COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'open',
  `session` enum('morning','afternoon','evening') COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'morning',
  PRIMARY KEY (`id`),
  KEY `FK_shifts_users` (`user_id`),
  CONSTRAINT `FK_shifts_users` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Quản lý ca\r\n';

-- Dumping data for table coffee_shop.shifts: ~6 rows (approximately)
INSERT INTO `shifts` (`id`, `user_id`, `start_time`, `end_time`, `opening_cash`, `closed_cash`, `total_cash`, `total_bill`, `status`, `session`) VALUES
	('50257987397', 5, '2025-05-10 09:49:58', '2025-05-10 09:52:23', 2000000, 0, 0, 0, 'closed', 'morning'),
	('52517654272', 5, '2025-05-10 09:55:33', '2025-05-10 10:26:13', 2000000, 150000, 270000, 2, 'closed', 'morning'),
	('52837743718', 5, '2025-05-09 01:05:07', '2025-05-09 01:14:48', 2000000, 0, 0, 0, 'closed', 'afternoon'),
	('56566214034', 5, '2025-05-10 11:26:18', '2025-05-10 17:42:41', 2000000, 0, 125000, 1, 'closed', 'morning'),
	('58477986231', 5, '2025-05-10 17:46:13', NULL, 2000000, 170000, 170000, 1, 'open', 'morning'),
	('58656985957', 5, '2025-05-10 10:27:38', '2025-05-10 10:28:19', 2000000, 135000, 135000, 1, 'closed', 'morning');

-- Dumping structure for table coffee_shop.tables
CREATE TABLE IF NOT EXISTS `tables` (
  `id` int NOT NULL AUTO_INCREMENT,
  `table_name` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `status` enum('full','empty') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT 'empty',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.tables: ~13 rows (approximately)
INSERT INTO `tables` (`id`, `table_name`, `status`) VALUES
	(1, 'A1', 'empty'),
	(2, 'A2', 'empty'),
	(3, 'A3', 'empty'),
	(4, 'A4', 'empty'),
	(5, 'A5', 'empty'),
	(6, 'A6', 'empty'),
	(7, 'A7', 'full'),
	(8, 'A8', 'empty'),
	(9, 'A9', 'empty'),
	(10, 'A10', 'empty'),
	(11, 'A11', 'empty'),
	(12, 'B1', 'empty'),
	(13, 'B2', 'empty'),
	(14, 'B3', 'empty');

-- Dumping structure for table coffee_shop.table_sessions
CREATE TABLE IF NOT EXISTS `table_sessions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `table_id` int NOT NULL,
  `user_id` int NOT NULL,
  `start_time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_time` timestamp NULL DEFAULT NULL,
  `status` enum('active','completed') COLLATE utf8mb4_general_ci DEFAULT 'active',
  PRIMARY KEY (`id`),
  KEY `table_id` (`table_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `table_sessions_ibfk_1` FOREIGN KEY (`table_id`) REFERENCES `tables` (`id`) ON DELETE CASCADE,
  CONSTRAINT `table_sessions_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=44 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.table_sessions: ~0 rows (approximately)

-- Dumping structure for table coffee_shop.users
CREATE TABLE IF NOT EXISTS `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `fullname` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `username` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `role` enum('admin','cashier','staff') COLLATE utf8mb4_general_ci NOT NULL,
  `phone` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dumping data for table coffee_shop.users: ~3 rows (approximately)
INSERT INTO `users` (`id`, `fullname`, `username`, `password`, `role`, `phone`, `created_at`) VALUES
	(5, 'Lê Minh Vương', 'minhvuong', '$2a$11$LvJuD3M3.UFuQGAM7BSCyeDJKeKHzcRw0j0M7VXdiIhLhzXi44K/y', 'staff', '0912345678', '2025-04-16 09:28:28'),
	(6, 'Lê Minh Vương', 'admin', '$2a$11$JjUgTn1xLBp/CHMEJakBie/WdE0SCrEPawZy7TORUaYGupc/XHTRK', 'admin', '0912345678', '2025-04-16 14:40:51'),
	(9, 'Trần Văn Lương', 'vanluong', '$2a$11$msXJWbIe1DnnJYGU9vSnzeXHwtehtnQEULAkIOFUgxXC2fItdf98y', 'cashier', '0123456789', '2025-04-18 06:39:02'),
	(11, 'Trương Mỹ Hoa', 'truonghoa', '$2a$11$WlzF.G2GvsHlycAPXhpAseZdugk0udAz4Tex.n5FCaZuenYeiy5Ri', 'staff', '1234566543', '2025-05-04 13:28:44');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;

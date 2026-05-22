-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 22, 2026 at 12:29 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `network_monitoring`
--

-- --------------------------------------------------------

--
-- Table structure for table `alerts`
--

CREATE TABLE `alerts` (
  `Id` int(11) NOT NULL,
  `AlertMessage` text NOT NULL,
  `Timestamp` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `alerts`
--

INSERT INTO `alerts` (`Id`, `AlertMessage`, `Timestamp`) VALUES
(1, '[ALERT!!!]: Critical issue with Main_Office_Router! Message: Прекъснато захранване (Power Outage)', '2026-05-22 13:22:40'),
(2, '[ALERT!!!]: Critical issue with Main_Office_Router! Message: Прекъснато захранване (Power Outage)', '2026-05-22 13:25:13');

-- --------------------------------------------------------

--
-- Table structure for table `devicelogs`
--

CREATE TABLE `devicelogs` (
  `Id` int(11) NOT NULL,
  `LogMessage` text NOT NULL,
  `Timestamp` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `devicelogs`
--

INSERT INTO `devicelogs` (`Id`, `LogMessage`, `Timestamp`) VALUES
(1, '[22-May-26 1:22:40 PM] - Main_Office_Router: Прекъснато захранване (Power Outage)', '2026-05-22 13:22:40'),
(2, '[22-May-26 1:25:13 PM] - Main_Office_Router: Прекъснато захранване (Power Outage)', '2026-05-22 13:25:13');

-- --------------------------------------------------------

--
-- Table structure for table `devices`
--

CREATE TABLE `devices` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `IpAddress` varchar(50) NOT NULL,
  `IsOnline` tinyint(1) DEFAULT 1,
  `Latency` int(11) DEFAULT 0,
  `DeviceTypeId` int(11) DEFAULT NULL,
  `LastChecked` datetime DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `devices`
--

INSERT INTO `devices` (`Id`, `Name`, `IpAddress`, `IsOnline`, `Latency`, `DeviceTypeId`, `LastChecked`) VALUES
(1, 'Printer_1', '172.16.0.116', 1, 74, 3, '2026-05-22 13:25:13'),
(2, 'AP_2', '192.168.10.197', 1, 97, 4, '2026-05-22 13:25:13'),
(3, 'Printer_3', '172.16.0.213', 0, 114, 3, '2026-05-22 13:25:13'),
(4, 'Switch_4', '10.0.0.138', 0, 9, 2, '2026-05-22 13:25:13'),
(5, 'Switch_5', '10.0.0.189', 0, 89, 2, '2026-05-22 13:25:13'),
(6, 'Switch_6', '10.0.0.67', 1, 35, 2, '2026-05-22 13:25:13'),
(7, 'Printer_7', '172.16.0.93', 1, 22, 3, '2026-05-22 13:25:13'),
(8, 'AP_8', '192.168.10.23', 1, 39, 4, '2026-05-22 13:25:13'),
(9, 'AP_9', '192.168.10.177', 1, 103, 4, '2026-05-22 13:25:13'),
(10, 'Switch_10', '10.0.0.23', 1, 94, 2, '2026-05-22 13:25:13'),
(11, 'Switch_11', '10.0.0.151', 1, 59, 2, '2026-05-22 13:25:13'),
(12, 'Printer_12', '172.16.0.215', 1, 60, 3, '2026-05-22 13:25:13'),
(13, 'AP_13', '192.168.10.3', 1, 22, 4, '2026-05-22 13:25:13'),
(14, 'Switch_14', '10.0.0.28', 1, 74, 2, '2026-05-22 13:25:13'),
(15, 'Switch_15', '10.0.0.189', 1, 75, 2, '2026-05-22 13:25:13'),
(16, 'Printer_16', '172.16.0.235', 1, 32, 3, '2026-05-22 13:25:13'),
(17, 'Switch_17', '10.0.0.68', 1, 32, 2, '2026-05-22 13:25:13'),
(18, 'Router_18', '192.168.1.238', 1, 85, 1, '2026-05-22 13:25:13'),
(19, 'Switch_19', '10.0.0.123', 0, 78, 2, '2026-05-22 13:25:13'),
(20, 'Printer_20', '172.16.0.18', 0, 15, 3, '2026-05-22 13:25:13'),
(21, 'AP_21', '192.168.10.11', 1, 6, 4, '2026-05-22 13:25:13'),
(22, 'Router_22', '192.168.1.163', 1, 31, 1, '2026-05-22 13:25:13'),
(23, 'Printer_23', '172.16.0.64', 1, 86, 3, '2026-05-22 13:25:13'),
(24, 'Router_24', '192.168.1.150', 1, 81, 1, '2026-05-22 13:25:13'),
(25, 'AP_25', '192.168.10.231', 1, 66, 4, '2026-05-22 13:25:13'),
(26, 'AP_26', '192.168.10.30', 1, 69, 4, '2026-05-22 13:25:13'),
(27, 'Switch_27', '10.0.0.111', 1, 90, 2, '2026-05-22 13:25:13'),
(28, 'Router_28', '192.168.1.76', 1, 56, 1, '2026-05-22 13:25:13'),
(29, 'Printer_29', '172.16.0.14', 0, 59, 3, '2026-05-22 13:25:13'),
(30, 'AP_30', '192.168.10.68', 1, 114, 4, '2026-05-22 13:25:13');

-- --------------------------------------------------------

--
-- Table structure for table `devicetypes`
--

CREATE TABLE `devicetypes` (
  `Id` int(11) NOT NULL,
  `TypeName` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `devicetypes`
--

INSERT INTO `devicetypes` (`Id`, `TypeName`) VALUES
(4, 'AccessPoint'),
(3, 'Printer'),
(1, 'Router'),
(2, 'Switch');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `alerts`
--
ALTER TABLE `alerts`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `devicelogs`
--
ALTER TABLE `devicelogs`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `devices`
--
ALTER TABLE `devices`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `DeviceTypeId` (`DeviceTypeId`);

--
-- Indexes for table `devicetypes`
--
ALTER TABLE `devicetypes`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `TypeName` (`TypeName`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `alerts`
--
ALTER TABLE `alerts`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `devicelogs`
--
ALTER TABLE `devicelogs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `devices`
--
ALTER TABLE `devices`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `devicetypes`
--
ALTER TABLE `devicetypes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `devices`
--
ALTER TABLE `devices`
  ADD CONSTRAINT `devices_ibfk_1` FOREIGN KEY (`DeviceTypeId`) REFERENCES `devicetypes` (`Id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

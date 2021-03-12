-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 12-03-2021 a las 21:25:47
-- Versión del servidor: 10.4.17-MariaDB
-- Versión de PHP: 8.0.2

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `webtask`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `orders`
--

CREATE TABLE `orders` (
  `OrderId` int(11) NOT NULL,
  `ProductID` int(11) DEFAULT NULL,
  `OrderUserId` int(11) NOT NULL,
  `DateCreated` datetime NOT NULL,
  `State` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `orders`
--

INSERT INTO `orders` (`OrderId`, `ProductID`, `OrderUserId`, `DateCreated`, `State`, `Quantity`) VALUES
(1, 1, 1001, '2019-04-26 00:00:00', 0, 1),
(2, 1, 1002, '2020-04-21 00:00:00', 0, 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `product`
--

CREATE TABLE `product` (
  `ProductID` int(11) NOT NULL,
  `Name` text DEFAULT NULL,
  `CreatedByUser` int(11) NOT NULL,
  `Description` varchar(100) DEFAULT NULL,
  `Slug` text DEFAULT NULL,
  `Price` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `product`
--

INSERT INTO `product` (`ProductID`, `Name`, `CreatedByUser`, `Description`, `Slug`, `Price`) VALUES
(1, 'Mouse', 1000, 'Mouse, Color Red', '1OWYBU203527', 12),
(2, 'Mouse', 1000, 'Mouse, Color Blue', 'BFAIVX203527', 18),
(3, 'Motherboard', 1001, 'Asus ROG Maximus Hero VI', 'AKCDKG203527', 251),
(4, 'Keyboard', 1002, 'Gamer keyboard', 'EQYB5Q203527', 10);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `products`
--

CREATE TABLE `products` (
  `ProductCardId` int(11) NOT NULL,
  `ProductID` int(11) DEFAULT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `products`
--

INSERT INTO `products` (`ProductCardId`, `ProductID`, `Quantity`) VALUES
(1, 1, 3),
(2, 2, 1),
(3, 3, 2),
(4, 4, 10);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `FirstName` text DEFAULT NULL,
  `LastName` text DEFAULT NULL,
  `Email` text DEFAULT NULL,
  `Username` text DEFAULT NULL,
  `Password` text DEFAULT NULL,
  `Role` text DEFAULT NULL,
  `Token` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `Email`, `Username`, `Password`, `Role`, `Token`) VALUES
(1000, 'Daimel', 'Rodriguez Torres', 'hackerprodlive@gmail.com', 'Hackerprod', 'loops', 'Admin', NULL),
(1001, 'Dayron', 'Alfaro Gonzales', 'Jst4rk@gmail.com', 'Jhon', 'jhon', 'Seller', NULL),
(1002, 'Julio', 'Garcia Hernandez', 'stickm4n@gmail.com', 'Stick', 'stick', 'Client', NULL);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`OrderId`),
  ADD KEY `IX_Orders_ProductID` (`ProductID`);

--
-- Indices de la tabla `product`
--
ALTER TABLE `product`
  ADD PRIMARY KEY (`ProductID`);

--
-- Indices de la tabla `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`ProductCardId`),
  ADD KEY `IX_Products_ProductID` (`ProductID`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `orders`
--
ALTER TABLE `orders`
  MODIFY `OrderId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `product`
--
ALTER TABLE `product`
  MODIFY `ProductID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `products`
--
ALTER TABLE `products`
  MODIFY `ProductCardId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1003;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `FK_Orders_Product_ProductID` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);

--
-- Filtros para la tabla `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `FK_Products_Product_ProductID` FOREIGN KEY (`ProductID`) REFERENCES `product` (`ProductID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

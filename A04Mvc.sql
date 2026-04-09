
GO

CREATE DATABASE Catalogdb;
USE Catalogdb;


CREATE TABLE Resident (
    residentid nchar(5) PRIMARY KEY,
    residentname NVARCHAR(30),
    residentaddress nvarchar(50),
    catalogid nchar(5)
);


CREATE TABLE Items (
    itemid nchar(5) PRIMARY KEY,
    itemname NVARCHAR(30),
    itemquantity int,
    itemtype NVARCHAR(30)
);

CREATE TABLE CatalogTable(
    catalogid nchar(5) PRIMARY KEY,
);

CREATE TABLE CatalogItem(
    catalogid nchar(5),
    itemid char(5) PRIMARY KEY,
    itemname NVARCHAR(30),
    itemtype NVARCHAR(30),
    itemvalue Int
);

DROP TABLE CatalogItem;

GO
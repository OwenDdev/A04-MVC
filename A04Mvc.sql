
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

create table Residentuser(  
    residentid nchar(5) PRIMARY KEY,
    username NVARCHAR(30),
    userpassword NVARCHAR(30)
);

alter table CatalogItem
Add foreign key (catalogid) references CatalogTable(catalogid);

--Default data--
-- 1. Create the Catalogs first
INSERT INTO CatalogTable (catalogid) VALUES ('C-101');
INSERT INTO CatalogTable (catalogid) VALUES ('C-102');

-- 2. Create the Residents and link them to the catalogs
INSERT INTO Resident (residentid, residentname, residentaddress, catalogid) 
VALUES ('R-001', 'John Doe', '123 Main St', 'C-101');

INSERT INTO Resident (residentid, residentname, residentaddress, catalogid) 
VALUES ('R-002', 'Jane Smith', '456 Oak Ave', 'C-102');

-- 3. Create the Login Accounts for the Residents
-- We will need this later for your Data Isolation requirement!
INSERT INTO Residentuser (residentid, username, userpassword) 
VALUES ('R-001', 'johndoe', 'password123');

INSERT INTO Residentuser (residentid, username, userpassword) 
VALUES ('R-002', 'janesmith', 'password123');

-- 4. Add items to John Doe's Catalog (C-101)
INSERT INTO CatalogItem (catalogid, itemid, itemname, itemtype, itemvalue)
VALUES ('C-101', 'I-001', '4K Television', 'Electronics', 1500);

INSERT INTO CatalogItem (catalogid, itemid, itemname, itemtype, itemvalue)
VALUES ('C-101', 'I-002', 'Leather Sofa', 'Furniture', 800);

-- 5. Add an item to Jane Smith's Catalog (C-102)
INSERT INTO CatalogItem (catalogid, itemid, itemname, itemtype, itemvalue)
VALUES ('C-102', 'I-003', 'Gaming Laptop', 'Electronics', 2000);
--Checking data
select * from dbo.CatalogItem;
GO
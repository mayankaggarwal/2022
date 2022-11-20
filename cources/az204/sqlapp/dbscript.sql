CREATE TABLE Products
(
	ProductID int,
	ProductName VARCHAR(1000),
	Quantity int
)

INSERT INTO Products VALUES
(1,'Mobile',100),
(2,'Laptop',200),
(3,'Tabs',300);

SELECT ProductID,ProductName,Quantity FROM Products;
CREATE TABLE [OrderProduct]
(
    [OrderID] INT,
    [ProductID] INT,
    Quantity INT,
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES [Order](Id),
    FOREIGN KEY (ProductID) REFERENCES Product(Id)
);
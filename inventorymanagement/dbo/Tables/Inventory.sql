CREATE TABLE [dbo].[Inventory] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [ItemId]         INT            NULL,
    [StockQuantity]  INT            NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [ItemPrice]      FLOAT (53)     NULL,
    [SupplierId]     INT            NULL,
    [DeleteStatus]   INT            NULL,
    [CreatedDate]    DATETIME       NULL,
    [LastUpdateDate] DATETIME       NULL,
    [OrderDate]      DATETIME       NULL,
    CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Inventory_Item] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([Id]),
    CONSTRAINT [FK_Inventory_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [dbo].[Supplier] ([Id])
);




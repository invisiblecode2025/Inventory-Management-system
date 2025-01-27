CREATE TABLE [dbo].[Item] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [CategoryId]     INT            NOT NULL,
    [DeleteStatus]   INT            NULL,
    [LastUpdateDate] DATETIME       NULL,
    [CreatedDate]    DATETIME       NULL,
    [Notes]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id])
);


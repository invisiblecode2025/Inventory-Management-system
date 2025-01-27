CREATE TABLE [dbo].[Supplier] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (250) NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [ContactInfo]    NVARCHAR (MAX) NULL,
    [Email]          NVARCHAR (250) NULL,
    [DeleteStatus]   INT            NULL,
    [CreatedDate]    DATETIME       NULL,
    [LastUpdateDate] DATETIME       NULL,
    CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([Id] ASC)
);


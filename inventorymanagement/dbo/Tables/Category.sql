CREATE TABLE [dbo].[Category] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (150) NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [DeleteStatus]   INT            NULL,
    [LastUpdateDate] DATETIME       NULL,
    [CreatedDate]    DATETIME       NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED ([Id] ASC)
);


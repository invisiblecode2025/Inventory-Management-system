CREATE TABLE [dbo].[Users] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UserName]       NVARCHAR (150) NULL,
    [Password]       NVARCHAR (150) NULL,
    [DeleteStatus]   INT            NULL,
    [LastUpdateDate] DATETIME       NULL,
    [CreatedDate]    DATETIME       NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


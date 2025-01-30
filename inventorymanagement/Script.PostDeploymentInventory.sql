/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (1, N'Metals', N'start', 0, CAST(N'2025-01-30T16:47:27.640' AS DateTime), CAST(N'2025-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (2, N'Furniture', N'Furniture Cat', 0, CAST(N'2025-01-29T16:24:17.430' AS DateTime), CAST(N'2025-01-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (1003, N'Leather', N'vv', 0, CAST(N'2025-01-22T02:05:09.063' AS DateTime), CAST(N'2025-01-22T02:05:09.063' AS DateTime))
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (1004, N'Fibers', N'p', 0, CAST(N'2025-01-22T02:10:54.133' AS DateTime), CAST(N'2025-01-22T02:10:54.133' AS DateTime))
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (1005, N'Chemicals', N'cc', 0, CAST(N'2025-01-22T03:59:18.063' AS DateTime), CAST(N'2025-01-22T03:59:18.063' AS DateTime))
GO
INSERT [dbo].[Category] ([Id], [Name], [Description], [DeleteStatus], [LastUpdateDate], [CreatedDate]) VALUES (1006, N'Minerals', N'Minerals-Minerals-Minerals', 0, CAST(N'2025-01-22T04:03:39.607' AS DateTime), CAST(N'2025-01-22T04:03:39.607' AS DateTime))
GO

GO
INSERT [dbo].[Item] ([Id], [Name], [Description], [CategoryId], [DeleteStatus], [LastUpdateDate], [CreatedDate], [Notes]) VALUES (4, N'Item1', N'Description for Item1', 1, 0, CAST(N'2025-01-29T16:23:26.480' AS DateTime), CAST(N'2025-01-08T13:24:19.840' AS DateTime), NULL)
GO
INSERT [dbo].[Item] ([Id], [Name], [Description], [CategoryId], [DeleteStatus], [LastUpdateDate], [CreatedDate], [Notes]) VALUES (9, N'Item2', N'Description for Item2', 1, 0, CAST(N'2025-01-30T16:47:27.640' AS DateTime), CAST(N'2025-01-08T15:27:29.250' AS DateTime), NULL)
GO

INSERT [dbo].[Supplier] ([Id], [Name], [Description], [ContactInfo], [Email], [DeleteStatus], [CreatedDate], [LastUpdateDate]) VALUES (1, N'Zayed', N'Electronic Supplier', N'233234 2 -3 2432423- 34234- 324 234', N'zayed.elreedy@gmail.com', 0, CAST(N'2025-05-01T00:00:00.000' AS DateTime), CAST(N'2025-01-29T16:22:37.273' AS DateTime))
GO
INSERT [dbo].[Supplier] ([Id], [Name], [Description], [ContactInfo], [Email], [DeleteStatus], [CreatedDate], [LastUpdateDate]) VALUES (2, N'Logey', N'Wood', N'435555553453  - 43543534- 43534543', N'smart@gmai.com', 0, CAST(N'2025-01-01T00:00:00.000' AS DateTime), CAST(N'2025-01-30T16:47:27.640' AS DateTime))
GO
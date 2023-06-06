CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateOfIssue] [DATETIME] NOT NULL,
	[DueDate] [DATETIME] NOT NULL,
	[Discount] INT,
	[Description] [ntext] NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)

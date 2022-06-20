CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[SKU] [nvarchar](50) NOT NULL,
	[Price] [int] NOT NULL,
	[Currency] [nvarchar](3) NOT NULL,
	[Description] [ntext] NULL,
	[AmountInStock] [int] NULL,
	[MinStock] [int] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)


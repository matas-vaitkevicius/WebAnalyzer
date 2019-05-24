USE [WebAnalyzer]
GO

/****** Object:  Table [dbo].[SpatialAnalysis]    Script Date: 24/05/2019 13:14:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SpatialAnalysis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NULL,
	[RentId] [int] NULL,
	[Point] [geography] NULL,
	[RentsIn1kRadiusCount] [int] NULL,
	[SalesIn1kRadiusCount] [int] NULL,
	[RentsIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn1kRadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn500RadiusCount] [int] NULL,
	[SalesIn500RadiusCount] [int] NULL,
	[RentsIn500RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn500RadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn200RadiusCount] [int] NULL,
	[SalesIn200RadiusCount] [int] NULL,
	[RentsIn200RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn200RadiusAvgSqM] [decimal](12, 6) NULL,
	[RentsIn100RadiusCount] [int] NULL,
	[SalesIn100RadiusCount] [int] NULL,
	[RentsIn100RadiusAvgSqM] [decimal](12, 6) NULL,
	[SalesIn100RadiusAvgSqM] [decimal](12, 6) NULL,
 CONSTRAINT [PK_SpatialAnalysis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[SpatialAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_SpatialAnalysis_Rent] FOREIGN KEY([RentId])
REFERENCES [dbo].[Rent] ([Id])
GO

ALTER TABLE [dbo].[SpatialAnalysis] CHECK CONSTRAINT [FK_SpatialAnalysis_Rent]
GO

ALTER TABLE [dbo].[SpatialAnalysis]  WITH CHECK ADD  CONSTRAINT [FK_SpatialAnalysis_Sale] FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sale] ([Id])
GO

ALTER TABLE [dbo].[SpatialAnalysis] CHECK CONSTRAINT [FK_SpatialAnalysis_Sale]
GO



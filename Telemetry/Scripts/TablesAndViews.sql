IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblDataItems]') AND type in (N'U'))
DROP TABLE [dbo].[tblDataItems]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblValidSummaryType]') AND type in (N'U'))
DROP TABLE [dbo].[tblValidSummaryType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblValidDataType]') AND type in (N'U'))
DROP TABLE [dbo].[tblValidDataType]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblValidSummaryType](
	[ID] [int] NOT NULL,
	[SummaryType] [varchar](10) NOT NULL,
	 CONSTRAINT [PK__tblSummaryType] PRIMARY KEY CLUSTERED ([ID] ASC)
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[tblValidDataType](
	[ID] [int] NOT NULL,
	[DataType] [varchar](50) NOT NULL,
	[SQLDataType] [varchar](50) NOT NULL,
	 CONSTRAINT [PK__tblValidDataType] PRIMARY KEY CLUSTERED ([ID] ASC)
) ON [PRIMARY]

GO
SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tblDataItems](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [varchar](50) NOT NULL,
	[Tag] [varchar](10) NOT NULL,
	[CANTag] [varchar](8) NULL,
	[CANByteOffset] int NULL,
	[CANDataType] int NULL,
	[SummaryType] [int] NOT NULL,
	IsCANValue AS (CASE WHEN CANTag IS NOT NULL THEN 1 ELSE 0 END),
	[Description] [varchar](50) NOT NULL,
	[DisplayFormat] [varchar](50) NOT NULL,
	[DataType] [int] NOT NULL,
	[NoCharting] [bit] NOT NULL,
	 CONSTRAINT [PK__tblDataItems] PRIMARY KEY CLUSTERED ([ID] ASC)
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tblDataItems]  WITH CHECK ADD CONSTRAINT [SummaryType_Key] FOREIGN KEY([SummaryType])
REFERENCES [dbo].[tblValidSummaryType] ([ID])
GO

ALTER TABLE [dbo].[tblDataItems]  WITH CHECK ADD CONSTRAINT [DataType_Key] FOREIGN KEY([DataType])
REFERENCES [dbo].[tblValidDataType] ([ID])
GO

ALTER TABLE [dbo].[tblDataItems]  WITH CHECK ADD CONSTRAINT [CANDataType_Key] FOREIGN KEY([CANDataType])
REFERENCES [dbo].[tblValidDataType] ([ID])
GO

ALTER TABLE [dbo].[tblDataItems] WITH CHECK ADD CONSTRAINT [FieldName_unique] UNIQUE([FieldName])
GO

ALTER TABLE [dbo].[tblDataItems] WITH CHECK ADD CONSTRAINT [Tag_unique] UNIQUE([Tag])
GO

SET ANSI_PADDING OFF
GO



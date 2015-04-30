CREATE DATABASE [Books]

GO

USE [Books]

GO

CREATE TABLE [dbo].[Books](
	[Id] [varchar](255)  NOT NULL,
	[ISBN] [varchar](10) NOT NULL,
	[Title] [varchar](255) NOT NULL,
	[URL] [varchar](MAX) ,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
 ([Id] ASC)
)

GO

SET ANSI_PADDING OFF
GO
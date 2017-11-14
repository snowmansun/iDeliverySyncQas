CREATE TABLE [dbo].[CrashLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[USER_ID] [varchar](20) NULL,
	[DATE] [datetime] NULL,
	[LOGINFO] [varchar](max) NULL,
	[CUSTOMER_ID] [numeric](10, 0) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[CrashLog] ADD  DEFAULT (getdate()) FOR [DATE]
GO



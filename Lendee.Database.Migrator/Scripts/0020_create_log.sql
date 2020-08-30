create table [lendee].[logs](
	[id] [int] IDENTITY(1,1) primary key,
	[date] [datetime2] NOT NULL,
	[level] [nvarchar](50) NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[logger] [nvarchar](250) NULL,
	[callsite] [nvarchar](max) NULL,
	[exception] [nvarchar](max) NULL)
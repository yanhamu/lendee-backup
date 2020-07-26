create table [lendee].[contracts](
	id bigint identity(100,1) primary key,
	[name] nvarchar(100) null,
	[type] int not null,
	currency int not null,
	note nvarchar(400),
	lendee bigint null foreign key references [lendee].[legal_entities](id),
	lender bigint null foreign key references [lendee].[legal_entities](id)
)
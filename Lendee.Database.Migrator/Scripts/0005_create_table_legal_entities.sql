create table core.legal_entities(
	id bigint identity(100,1) primary key,
	firstname nvarchar(60),
	lastname nvarchar(60),
	company_name nvarchar(60),
	email nvarchar(40),
	phone nvarchar(40),
	bank_account_number nvarchar(20),
	note nvarchar(400),
	identifying_number nvarchar(20),
	tax_identifying_number nvarchar(20),
)
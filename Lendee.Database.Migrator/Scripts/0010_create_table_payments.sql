create table lendee.payments(
	id bigint identity(100,1) primary key,
	amount decimal(11,3) not null,
	paid_at datetime2 not null,
	contract_id bigint foreign key references lendee.contracts(id) not null
)
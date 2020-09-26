create table lendee.payment_dues(
	id bigint identity(100,1) primary key,
	contract_id bigint foreign key references lendee.contracts(id) not null,
	amount decimal(19,3) not null,
	due date not null
)
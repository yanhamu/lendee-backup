create table core.payments(
	id bigint identity(100,1) primary key,
	amount decimal(11,3) not null,
	paid_at datetime2 not null,
	contract_id bigint foreign key references core.contracts(id) not null
)
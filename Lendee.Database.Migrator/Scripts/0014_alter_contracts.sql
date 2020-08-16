create table lendee.contract_drafts(
contract_id bigint primary key foreign key references lendee.contracts(id),
step int not null
)
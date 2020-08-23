alter table lendee.repayments 
add 
contract_type int not null, 
amount decimal(12,3) null

alter table lendee.repayments drop column amount_data
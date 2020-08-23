alter table lendee.repayments 
add 
amount_data nvarchar(1000) not null, 
due_date date not null

alter table lendee.repayments drop column amount
alter table lendee.repayments drop column paid_at
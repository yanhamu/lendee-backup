alter table [lendee].[contracts] add 
payment_term_type int not null,
payment_amount decimal(9,2),
valid_from date,
valid_until date
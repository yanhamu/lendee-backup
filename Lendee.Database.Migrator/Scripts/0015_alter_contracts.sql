alter table lendee.contracts 
add fee decimal(11,3) null
go
update lendee.contracts set rent_type = 3 where [type] = 3

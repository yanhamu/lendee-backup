alter table lendee.contracts 
add rent_type int null
go
update lendee.contracts set rent_type = 3 where [type] = 3

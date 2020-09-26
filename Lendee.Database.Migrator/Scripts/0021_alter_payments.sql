alter table [lendee].payments
add received_at date not null

alter table [lendee].payments drop column due
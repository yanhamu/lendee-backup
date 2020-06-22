create table users.users(
	id uniqueidentifier primary key,
	username nvarchar(100),
	[password] nvarchar(100)
)
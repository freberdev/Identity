CREATE TABLE [dbo].[User]
(
	[Id] NVARCHAR (450) NOT NULL PRIMARY KEY references AspNetUsers(Id),
	[FirstName] NVARCHAR (32) NOT NULL,
	[LastName] NVARCHAR (32) NOT NULL,
)

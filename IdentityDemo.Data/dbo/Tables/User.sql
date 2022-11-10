CREATE TABLE [dbo].[User]
(
	[Id] NVARCHAR (450) NOT NULL PRIMARY KEY references AspNetUsers(Id)
)

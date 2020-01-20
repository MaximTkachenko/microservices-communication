-- create

CREATE DATABASE UsersDb
GO

USE UsersDb
GO

CREATE TABLE [dbo].[Users](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Email] [varchar](100) NOT NULL,
    [Name] [varchar](100) NOT NULL,
    [CustomerId] [bigint] NOT NULL,
    [CustomerName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Claims](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [UserId] [bigint] NOT NULL,
    [ClaimType] [varchar](100) NOT NULL,
    [ClaimValue] [varchar](100) NOT NULL,
    [ClaimValueType] [varchar](100) NOT NULL,
 CONSTRAINT [PK_Claims] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Claims]  WITH CHECK ADD  CONSTRAINT [FK_Claims_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

-- seed

INSERT INTO [Users] (Email, Name, CustomerId, CustomerName)
VALUES ('oblomov86@gmail.com', 'mtkachenko', 1, 'Arcadia')

INSERT INTO [Claims] (UserId, ClaimType, ClaimValue, ClaimValueType)
VALUES (1, 'x-userId', 1, 'integer'),
(1, 'x-customerId', 1, 'integer')
GO
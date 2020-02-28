IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [Email] nvarchar(300) NOT NULL,
    [Name] nvarchar(500) NOT NULL,
    [CustomerId] bigint NOT NULL,
    [CustomerName] nvarchar(300) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200228081120_Initial', N'3.1.1');

GO


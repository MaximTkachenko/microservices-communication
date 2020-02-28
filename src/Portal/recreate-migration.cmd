echo off

rem delete all migrations
rmdir /s /q Migrations

rem create new migration
dotnet ef migrations add Initial

rem create sql file for migration
dotnet ef migrations script -o Migrations\Initial.sql
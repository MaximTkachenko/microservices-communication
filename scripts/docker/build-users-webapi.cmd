echo off
set version=%1
set current=%cd%

cd ..
cd ..
cd src

docker build -f Users.WebApi/Dockerfile -t users-api-app:%version% .

cd %current%
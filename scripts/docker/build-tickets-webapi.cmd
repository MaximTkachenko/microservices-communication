echo off
set version=%1
set current=%cd%

cd ..
cd ..
cd src

docker build -f Tickets.WebApi/Dockerfile -t tickets-api-app:%version% .

cd %current%
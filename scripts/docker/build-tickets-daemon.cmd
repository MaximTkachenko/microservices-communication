echo off
set version=%1
set current=%cd%

cd ..
cd ..
cd src

docker build -f Tickets.Daemon/Dockerfile -t tickets-daemon-app:%version% .

cd %current%
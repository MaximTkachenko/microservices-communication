echo off
set version=%1
set current=%cd%

cd ..
cd ..
cd src

docker build -f Portal/Dockerfile -t portal-app:%version% .

cd %current%
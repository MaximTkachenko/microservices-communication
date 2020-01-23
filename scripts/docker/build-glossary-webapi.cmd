echo off
set version=%1
set current=%cd%

cd ..
cd ..
cd src

docker build -f Glossary.WebApi/Dockerfile -t glossary-api-app:%version% .

cd %current%
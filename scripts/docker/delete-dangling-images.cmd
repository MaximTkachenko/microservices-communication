echo off

for /f "delims=" %%i in ('docker images -f "dangling=true" -q') do ^
docker rmi %%i
@echo off
mode con: cols=30 lines=20

dotnet run --configuration Release --project "./src" --property:WarningLevel=0 

pause
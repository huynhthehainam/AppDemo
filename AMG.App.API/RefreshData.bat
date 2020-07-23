@echo off
dotnet ef database drop -f
dotnet ef migrations remove
dotnet ef migrations add aa
dotnet ef database update
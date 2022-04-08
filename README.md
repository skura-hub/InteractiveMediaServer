## Technology:
- Database: postgreSQL
- Visual Studio
- .Net Core 5

## Database Settings:
Backend.Api/appsettings.json

## Migration
Project: Backend.Infrastructure
  1. Set Backend.Api as starting project
  2. Tools -> NuGetPackageManager->Package Manager Console
  3. Change DefaultProject: Backend.Infrastructure
  4. Write in console:
```sh
Add-Migration InitalContext -context ApplicationDbContext 
Update-Database -context ApplicationDbContext
```

Update:
```sh
Update-Database -context ApplicationDbContext
```

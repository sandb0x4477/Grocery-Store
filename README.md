# Grocery Store in ASP.Net Core 2.1 MVC

[Live Demo](https://www.wbinkowski.site/gs/)

`username: test@test.com; password: Pass123@`

![Grocery Store](preview/preview.gif)

### Instructions

#### Modify connection string in 'appsettings.json'

```
dotnet restore

dotnet build

dotnet ef migrations add "mymigration"

dotnet ef database update
```

##### Run script 'Populate_DB.sql' inside Database

`dotnet run`

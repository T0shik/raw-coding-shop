# migrations
```shell script
dotnet ef migrations add <name> -c ApplicationDbContext -s ../RawCoding.Shop.UI -o Migrations/
```

# update
```shell script
dotnet ef database update -c ApplicationDbContext -s ../RawCoding.Shop.UI
```

# script
```shell script
dotnet ef migrations script -i -o "script.sql" -c ApplicationDbContext -s ../RawCoding.Shop.UI
```

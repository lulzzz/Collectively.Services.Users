dotnet restore --source "https://api.nuget.org/v3/index.json" --source "https://www.myget.org/F/collectively/api/v3/index.json" --no-cache
dotnet pack "Collectively.Services.Users.Shared" -o .

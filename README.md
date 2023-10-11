# MixApp

MixApp web app, powered by blazor

```bash
yarn # OR npm install
mv ./node_modules/ ./MixApp.Shared/wwwroot/lib

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -v 8.0.100-rc.2.23502.2
rm dotnet-install.sh

dotnet workload restore
dotnet restore
dotnet watch run --project MixApp.Shared/MixApp.csproj
```

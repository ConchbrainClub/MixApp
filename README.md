# MixApp

[![Build](https://github.com/ConchbrainClub/MixApp/actions/workflows/main.yml/badge.svg)](https://github.com/ConchbrainClub/MixApp/actions/workflows/main.yml)

MixApp web app, powered by blazor

```bash
yarn # OR npm install
mv ./node_modules/ ./MixApp.Web/wwwroot/lib

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -v 8.0.302
rm dotnet-install.sh

dotnet workload restore
dotnet restore
dotnet watch run --project MixApp.Web/ixApp.Web.csproj
```

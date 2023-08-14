# MixApp

MixApp web app, powered by blazor

```bash
yarn # OR npm install
mv ./node_modules/ ./MixApp/wwwroot/lib

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -v 8.0.100-preview.7.23376.3
rm dotnet-install.sh

dotnet restore
dotnet watch run --project MixApp/MixApp.csproj
```

npm install
mv ./node_modules/ ./MixApp.Web/wwwroot/lib

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh -v 8.0.301 --install-dir ~/cli

~/cli/dotnet workload restore
~/cli/dotnet restore
~/cli/dotnet publish -c Release

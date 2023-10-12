npm install
mv ./node_modules/ ./MixApp.Shared/wwwroot/lib

wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh -v 8.0.100-rc.2.23502.2 --install-dir ~/cli

~/cli/dotnet workload restore
~/cli/dotnet restore
~/cli/dotnet publish -c Release

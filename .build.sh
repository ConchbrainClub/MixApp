npm install
mv ./node_modules/ ./MixApp.Shared/wwwroot/lib
wget https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh
chmod +x ./dotnet-install.sh
./dotnet-install.sh -v 8.0.100-rc.1.23463.5
dotnet publish -c Release
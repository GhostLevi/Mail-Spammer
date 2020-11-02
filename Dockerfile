FROM mcr.microsoft.com/dotnet/core/runtime:2.2

WORKDIR /app

COPY ./MailSpammer/App/bin/Release/netcoreapp2.2/publish/ .

RUN dir

ENTRYPOINT ["dotnet", "App.dll"]
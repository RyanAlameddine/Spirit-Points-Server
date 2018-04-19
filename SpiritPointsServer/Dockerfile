FROM microsoft/aspnetcore:1.1 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:1.1 AS build
WORKDIR /src
COPY *.sln ./
COPY SpiritPointsServer/SpiritPointsServer.csproj SpiritPointsServer/
RUN dotnet restore
COPY . .
WORKDIR /src/SpiritPointsServer
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SpiritPointsServer.dll"]

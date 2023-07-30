FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Xtee.Connector/Xtee.Connector.csproj", "Xtee.Connector/"]
RUN dotnet restore "Xtee.Connector/Xtee.Connector.csproj"
COPY . .
WORKDIR "/src/Xtee.Connector"
RUN dotnet build "Xtee.Connector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xtee.Connector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xtee.Connector.dll"]

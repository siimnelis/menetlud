FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Xtee.Teavitus.Connector/Xtee.Teavitus.Connector.csproj", "Xtee.Teavitus.Connector/"]
RUN dotnet restore "Xtee.Teavitus.Connector/Xtee.Teavitus.Connector.csproj"
COPY . .
WORKDIR "/src/Xtee.Teavitus.Connector"
RUN dotnet build "Xtee.Teavitus.Connector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Xtee.Teavitus.Connector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Xtee.Teavitus.Connector.dll"]

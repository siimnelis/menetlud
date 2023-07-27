FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TeavitusTeenus.Host/TeavitusTeenus.Host.csproj", "TeavitusTeenus.Host/"]
RUN dotnet restore "TeavitusTeenus.Host/TeavitusTeenus.Host.csproj"
COPY . .
WORKDIR "/src/TeavitusTeenus.Host"
RUN dotnet build "TeavitusTeenus.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeavitusTeenus.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeavitusTeenus.Host.dll"]

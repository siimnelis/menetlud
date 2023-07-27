FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Menetlus.API/Menetlus.API.csproj", "Menetlus.API/"]
RUN dotnet restore "Menetlus.API/Menetlus.API.csproj"
COPY . .
WORKDIR "/src/Menetlus.API"
RUN dotnet build "Menetlus.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Menetlus.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menetlus.API.dll"]

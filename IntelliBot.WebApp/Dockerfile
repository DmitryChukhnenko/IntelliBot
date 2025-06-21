FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "IntelliBot.WebApp.csproj"
RUN dotnet build "IntelliBot.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IntelliBot.WebApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/keys && chmod 777 /app/keys 
EXPOSE 80
HEALTHCHECK --interval=30s --timeout=3s \
    CMD curl -f http://localhost:80/ || exit 1
ENTRYPOINT ["dotnet", "IntelliBot.WebApp.dll"]
# Étape de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["./HumanEvolution.csproj", "./"]
RUN dotnet restore

COPY . .
RUN dotnet publish "HumanEvolution.csproj" -c Release -o /app/out

# Étape finale : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "HumanEvolution.dll"]

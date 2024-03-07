# Étape de construction
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copie du fichier .sln
COPY Films.sln ./

# Copie des fichiers .csproj dans leurs répertoires respectifs
COPY Film/Film.csproj Film/
COPY Models/Models.csproj Models/
COPY Migration/Migration.csproj Migration/
COPY RunMigration/RunMigration.csproj RunMigration/
COPY Services/Services.csproj Services/
COPY Repository/Repository.csproj Repository/
COPY Test/UnitTest.csproj Test/
COPY IntegrationTest/IntegrationTest.csproj IntegrationTest/
# Répétez pour tous les autres projets nécessaires

# Restauration des dépendances
RUN dotnet restore

# Copie des autres fichiers sources et construction de l'application
COPY . ./
RUN dotnet publish Film/Film.csproj -c Release -o out

# Étape d'exécution
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Film.dll"]

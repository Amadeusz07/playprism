FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/TournamentService/Playprism.Services.TournamentService.API/", "Services/Playprism.Services.TournamentService.API/"]
COPY ["Services/TournamentService/Playprism.Services.TournamentService.BLL/", "Services/Playprism.Services.TournamentService.BLL/"]
COPY ["Services/TournamentService/Playprism.Services.TournamentService.DAL/", "Services/Playprism.Services.TournamentService.DAL/"]
COPY ["Services/TournamentService/Playprism.Services.TournamentService.UnitTests/", "Services/Playprism.Services.TournamentService.UnitTests/"]
RUN dotnet restore "Services/Playprism.Services.TournamentService.API/Playprism.Services.TournamentService.API.csproj"
COPY . .
WORKDIR "/src/Services/Playprism.Services.TournamentService.API"
RUN dotnet build "Playprism.Services.TournamentService.API.csproj" -c Release -o /app/build

FROM build as test
WORKDIR "/src/Services/Playprism.Services.TournamentService.UnitTests"
RUN dotnet test

FROM build AS publish
WORKDIR /src/Services/Playprism.Services.TournamentService.API
RUN dotnet publish "Playprism.Services.TournamentService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Playprism.Services.TournamentService.API.dll"]

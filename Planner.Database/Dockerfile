FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planner.Database/Planner.Database.csproj", "Planner.Database/"]
RUN dotnet restore "Planner.Database/Planner.Database.csproj"
COPY . .
WORKDIR "/src/Planner.Database"
RUN dotnet build "Planner.Database.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planner.Database.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.Database.dll"]

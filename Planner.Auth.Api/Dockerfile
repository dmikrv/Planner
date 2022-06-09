FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planner.Auth.Api/Planner.Auth.Api.csproj", "Planner.Auth.Api/"]
RUN dotnet restore "Planner.Auth.Api/Planner.Auth.Api.csproj"
COPY . .
WORKDIR "/src/Planner.Auth.Api"
RUN dotnet build "Planner.Auth.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planner.Auth.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.Auth.Api.dll"]

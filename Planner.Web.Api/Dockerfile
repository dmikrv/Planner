FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Planner.Web.Api/Planner.Web.Api.csproj", "Planner.Web.Api/"]
RUN dotnet restore "Planner.Web.Api/Planner.Web.Api.csproj"
COPY . .
WORKDIR "/src/Planner.Web.Api"
RUN dotnet build "Planner.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Planner.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Planner.Web.Api.dll"]

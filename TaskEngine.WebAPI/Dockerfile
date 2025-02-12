FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskEngine.WebAPI/TaskEngine.WebAPI.csproj", "TaskEngine.WebAPI/"]
COPY ["TaskEngine.Application/TaskEngine.Application.csproj", "TaskEngine.Application/"]
COPY ["TaskEngine.Domain/TaskEngine.Domain.csproj", "TaskEngine.Domain/"]
COPY ["TaskEngine.Infrastructure/TaskEngine.Infrastructure.csproj", "TaskEngine.Infrastructure/"]
RUN dotnet restore "./TaskEngine.WebAPI/TaskEngine.WebAPI.csproj"
COPY . .
WORKDIR "/src/TaskEngine.WebAPI"
RUN dotnet build "./TaskEngine.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "./TaskEngine.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskEngine.WebAPI.dll"]
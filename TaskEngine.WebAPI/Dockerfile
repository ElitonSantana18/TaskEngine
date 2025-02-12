# Etapa base (antes de mudar para o usuário 'app')
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

## Copiar o script wait-for-it.sh para o container
#COPY wait-for-it.sh /app/wait-for-it.sh
#
## Executar chmod como root para garantir permissões corretas
#RUN chmod +x /app/wait-for-it.sh

# Definir o usuário 'app' depois de garantir que o arquivo tenha permissões corretas
USER app
WORKDIR /app
EXPOSE 5000

# Continuar com o resto do Dockerfile
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

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "./TaskEngine.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskEngine.WebAPI.dll"]

# Alterar ENTRYPOINT para usar o script wait-for-it.sh
#ENTRYPOINT ["sh", "-c", "/app/wait-for-it.sh rabbitmq:5672 -- /app/wait-for-it.sh mongodb:27017 -- dotnet TaskEngine.WebAPI.dll"]

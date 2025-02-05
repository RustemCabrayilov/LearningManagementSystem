FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Update the path to include the relative directory from Solution Items to the project
COPY ["Presentation/LearningManagementSystem.API/LearningManagementSystem.API.csproj", "Presentation/LearningManagementSystem.API/"]
COPY ["Core/LearningManagementSystem.Application/LearningManagementSystem.Application.csproj", "Core/LearningManagementSystem.Application/"]
COPY ["Core/LearningManagementSystem.Domain/LearningManagementSystem.Domain.csproj", "Core/LearningManagementSystem.Domain/"]
COPY ["Infrastructure/LearningManagementSystem.Infrastructure/LearningManagementSystem.Infrastructure.csproj", "Infrastructure/LearningManagementSystem.Infrastructure/"]
COPY ["Infrastructure/LearningManagementSystem.Persistence/LearningManagementSystem.Persistence.csproj", "Infrastructure/LearningManagementSystem.Persistence/"]
COPY ["Infrastructure/LearningManagementSystem.BLL/LearningManagementSystem.BLL.csproj", "Infrastructure/LearningManagementSystem.BLL/"]
COPY ["UI/LearningManagementSystem.UI/LearningManagementSystem.UI.csproj", "UI/LearningManagementSystem.UI/"]
RUN dotnet restore "Presentation/LearningManagementSystem.API/LearningManagementSystem.API.csproj"
COPY .. .
WORKDIR "/src/Presentation/LearningManagementSystem.API"
RUN dotnet build "LearningManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LearningManagementSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LearningManagementSystem.API.dll"]

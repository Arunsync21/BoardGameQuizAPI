# Use Linux-based .NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use Linux-based .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore dependencies
COPY ["BoardGameQuizAPI/BoardGameQuizAPI.csproj", "BoardGameQuizAPI/"]
RUN dotnet restore "BoardGameQuizAPI/BoardGameQuizAPI.csproj"

# Copy entire application and build
COPY . .
WORKDIR "/src/BoardGameQuizAPI"
RUN dotnet build "BoardGameQuizAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BoardGameQuizAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoardGameQuizAPI.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Prediction/Prediction.API/Prediction.API.csproj", "Services/Prediction/Prediction.API/"]
RUN dotnet restore "Services/Prediction/Prediction.API/Prediction.API.csproj"
COPY . .
WORKDIR "/src/Services/Prediction/Prediction.API"
RUN dotnet build "Prediction.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Prediction.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Prediction.API.dll"]

# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /src

ARG BUILD_CONFIGURATION=Release

COPY EmployeeManagementSystem.sln ./
COPY EmployeeManagementSystem/EmployeeManagementSystem.csproj EmployeeManagementSystem/

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore EmployeeManagementSystem/EmployeeManagementSystem.csproj

COPY EmployeeManagementSystem/. ./EmployeeManagementSystem/

WORKDIR /src/EmployeeManagementSystem
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish EmployeeManagementSystem.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends libreoffice fonts-dejavu fontconfig libgdiplus \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish ./

RUN mkdir -p /app/GeneratedLetters /app/GeneratedPayslips /app/wwwroot/uploads/assets

ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    LIBREOFFICE_PATH=/usr/bin/soffice

EXPOSE 8080

ENTRYPOINT ["dotnet", "EmployeeManagementSystem.dll"]

# Étape 1 — Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY TodoApi_AsN/*.csproj ./TodoApi_AsN/
RUN dotnet restore ./TodoApi_AsN/TodoApi_AsN.csproj

COPY TodoApi_AsN/. ./TodoApi_AsN/
RUN dotnet publish ./TodoApi_AsN/TodoApi_AsN.csproj -c Release -o out

# Étape 2 — Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "TodoApi_AsN.dll"]

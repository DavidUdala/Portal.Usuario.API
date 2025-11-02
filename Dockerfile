#Stage 1: Build 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY "Portal.Usuario.API/Portal.Usuario.API.csproj" "Portal.Usuario.API/"
COPY "Portal.Usuario.Application/Portal.Usuario.Application.csproj" "Portal.Usuario.API/"
COPY "Portal.Usuario.Core/Portal.Usuario.Core.csproj" "Portal.Usuario.API/"
COPY "Portal.Usuario.Infrastructure/Portal.Usuario.Infrastructure.csproj" "Portal.Usuario.API/"

RUN dotnet restore "Portal.Usuario.API/Portal.Usuario.API.csproj"

COPY . .

WORKDIR /src/Portal.Usuario.API

RUN dotnet publish "Portal.Usuario.API.csproj" -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:9080
EXPOSE 9080
ENTRYPOINT ["dotnet", "Portal.Usuario.API.dll"]


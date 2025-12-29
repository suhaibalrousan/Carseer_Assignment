# Multi-stage Dockerfile for Vehicle Catalog Application

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY [\"Carseer.sln\", \"./\"]
COPY [\"src/VehicleCatalog.Domain/VehicleCatalog.Domain.csproj\", \"src/VehicleCatalog.Domain/\"]
COPY [\"src/VehicleCatalog.Application/VehicleCatalog.Application.csproj\", \"src/VehicleCatalog.Application/\"]
COPY [\"src/VehicleCatalog.Infrastructure/VehicleCatalog.Infrastructure.csproj\", \"src/VehicleCatalog.Infrastructure/\"]
COPY [\"src/VehicleCatalog.Web/VehicleCatalog.Web.csproj\", \"src/VehicleCatalog.Web/\"]

# Restore dependencies
RUN dotnet restore \"Carseer.sln\"

# Copy all source code
COPY . .

# Build the application
WORKDIR \"/src/src/VehicleCatalog.Web\"
RUN dotnet build \"VehicleCatalog.Web.csproj\" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish \"VehicleCatalog.Web.csproj\" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose ports
EXPOSE 80
EXPOSE 443

# Copy published application
COPY --from=publish /app/publish .

# Set environment variable
ENV ASPNETCORE_URLS=http://+:80

# Entry point
ENTRYPOINT [\"dotnet\", \"VehicleCatalog.Web.dll\"]

# Etapa 1: Build y Publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj de los 4 proyectos y restaurar dependencias
COPY ["SubastaYa.Domain/SubastaYa.Domain.csproj", "SubastaYa.Domain/"]
COPY ["SubastaYa.Application/SubastaYa.Application.csproj", "SubastaYa.Application/"]
COPY ["SubastaYa.Infrastructure/SubastaYa.Infrastructure.csproj", "SubastaYa.Infrastructure/"]
COPY ["SubastaYa.Api/SubastaYa.Api.csproj", "SubastaYa.Api/"]

RUN dotnet restore "SubastaYa.Api/SubastaYa.Api.csproj"

# Copiar el código fuente y compilar
COPY SubastaYa.Domain/ SubastaYa.Domain/
COPY SubastaYa.Application/ SubastaYa.Application/
COPY SubastaYa.Infrastructure/ SubastaYa.Infrastructure/
COPY SubastaYa.Api/ SubastaYa.Api/

WORKDIR /src/SubastaYa.Api
RUN dotnet publish "SubastaYa.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "SubastaYa.Api.dll"]

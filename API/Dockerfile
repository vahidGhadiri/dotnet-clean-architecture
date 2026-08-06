FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY API.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

RUN groupadd -r appgroup && useradd -r -g appgroup appuser

COPY --from=build /app/publish .

RUN mkdir -p /app/Data && chown -R appuser:appgroup /app

USER appuser
EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]



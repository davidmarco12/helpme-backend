FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["helpme.csproj", "./"]
RUN dotnet restore "./helpme.csproj"
COPY . .



WORKDIR /src
RUN dotnet build "helpme.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "helpme.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "helpme.dll"]

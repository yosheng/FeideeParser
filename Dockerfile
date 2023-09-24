FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /docker
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore src/FeideeParser.Web/FeideeParser.Web.csproj --configfile ./NuGet.Config
WORKDIR src/FeideeParser.Web
RUN dotnet build FeideeParser.Web.csproj -c Release

FROM build AS publish
RUN dotnet publish FeideeParser.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV ASPNETCORE_URLS http://*:5000
ENV TZ Asia/Shanghai

ENTRYPOINT ["dotnet", "FeideeParser.Web.dll"]



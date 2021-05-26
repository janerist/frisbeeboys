﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Frisbeeboys.Web/Frisbeeboys.Web.csproj", "Frisbeeboys.Web/"]
RUN dotnet restore "Frisbeeboys.Web/Frisbeeboys.Web.csproj"
COPY src/ .
WORKDIR "/src/Frisbeeboys.Web"
RUN dotnet build "Frisbeeboys.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Frisbeeboys.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Frisbeeboys.Web.dll"]
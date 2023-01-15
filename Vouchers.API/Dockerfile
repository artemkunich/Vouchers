#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vouchers.API/Vouchers.API.csproj", "Vouchers.API/"]
COPY ["Vouchers.Persistence/Vouchers.Persistence.csproj", "Vouchers.Persistence/"]
COPY ["Vouchers.Auth/Vouchers.Auth.csproj", "Vouchers.Auth/"]
COPY ["Vouchers.Domains/Vouchers.Domains.csproj", "Vouchers.Domains/"]
COPY ["Vouchers.Core/Vouchers.Core.csproj", "Vouchers.Core/"]
COPY ["Vouchers.Application/Vouchers.Application.csproj", "Vouchers.Application/"]
COPY ["Vouchers.Identities/Vouchers.Identities.csproj", "Vouchers.Identities/"]
COPY ["Vouchers.Values/Vouchers.Values.csproj", "Vouchers.Values/"]
RUN dotnet restore "Vouchers.API/Vouchers.API.csproj"
COPY . .
WORKDIR "/src/Vouchers.API"
RUN dotnet build "Vouchers.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vouchers.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vouchers.API.dll"]
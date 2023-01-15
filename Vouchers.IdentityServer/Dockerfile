#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vouchers.IdentityServer/Vouchers.IdentityServer.csproj", "Vouchers.IdentityServer/"]
RUN dotnet restore "Vouchers.IdentityServer/Vouchers.IdentityServer.csproj"
COPY . .
WORKDIR "/src/Vouchers.IdentityServer"
RUN dotnet build "Vouchers.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vouchers.IdentityServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vouchers.IdentityServer.dll"]
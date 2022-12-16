FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Vouchers.MinimalAPI/Vouchers.MinimalAPI.csproj", "Vouchers.MinimalAPI/"]
RUN dotnet restore "Vouchers.MinimalAPI/Vouchers.MinimalAPI.csproj"
COPY . .
WORKDIR "/src/Vouchers.MinimalAPI"
RUN dotnet build "Vouchers.MinimalAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vouchers.MinimalAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vouchers.MinimalAPI.dll"]

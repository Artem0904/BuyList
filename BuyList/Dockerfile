# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ["BuyList/BuyList.csproj", "BuyList/"]
RUN dotnet restore "BuyList/BuyList.csproj"

# copy everything else and build app
COPY . .
WORKDIR /source/BuyList
RUN dotnet publish -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BuyList.dll"]
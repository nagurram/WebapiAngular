#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR ./source
COPY . .
RUN dotnet restore "./LSDataApi/LSDataApi/LSDataApi.csproj" --disable-parallel

RUN dotnet publish "./LSDataApi/LSDataApi/LSDataApi.csproj" -c release -o /app --no-restore


#Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000 1433

ENTRYPOINT ["dotnet", "LSDataApi.dll"]

#docker build --rm -t dataapi:latest .
#docker run --rm -p 5000:5000 -p 5001:5001 -e ASPNETCORE_HTTP_PORT=https://+:5001 -e ASPNETCORE_URLS=http://+:5000 dataapi
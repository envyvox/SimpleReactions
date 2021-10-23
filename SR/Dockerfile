FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Copy everything else and build
COPY SR ./SR
COPY SR.Data ./SR.Data
COPY SR.Services ./SR.Services

RUN dotnet restore SR/SR.csproj
RUN dotnet publish SR/*.csproj -c Release -o out

# Build runtime image
WORKDIR /
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY --from=build-env ./out .
ENTRYPOINT ["dotnet", "SR.dll"]

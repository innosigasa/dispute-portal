FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DisputePortal.Application/DisputePortal.Application.csproj DisputePortal.Application/
COPY DisputePortal.Persistence/DisputePortal.Persistence.csproj DisputePortal.Persistence/
COPY DisputePortal.Api/DisputePortal.Api.csproj DisputePortal.Api/
RUN dotnet restore DisputePortal.Api/DisputePortal.Api.csproj

COPY DisputePortal.Application/ DisputePortal.Application/
COPY DisputePortal.Persistence/ DisputePortal.Persistence/
COPY DisputePortal.Api/ DisputePortal.Api/
RUN dotnet publish DisputePortal.Api/DisputePortal.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "DisputePortal.Api.dll"]

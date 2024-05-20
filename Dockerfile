FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

WORKDIR /source

RUN dotnet tool install --version 8.0.5 --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

COPY Directory.Build.props Directory.Packages.props global.json ./

# copy csproj and restore as distinct layers
COPY src/ ./src

RUN dotnet restore ./src/Web/Web.csproj -a $TARGETARCH

RUN dotnet build ./src/Web/Web.csproj --no-restore --configuration Release

RUN dotnet publish ./src/Web/Web.csproj --configuration Release --no-restore  -o /app

RUN dotnet ef migrations bundle --project src/Infrastructure/ --startup-project src/Web/

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

EXPOSE 8080

WORKDIR /app

COPY --from=build /app .
COPY --from=build /source/efbundle ./

USER $APP_UID

ENTRYPOINT ["./ActivityManager.Web"]
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH

WORKDIR /source

RUN dotnet tool install --version 8.0.5 --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

COPY Directory.Build.props Directory.Packages.props global.json ActivityManager.sln ./

COPY ["src/Web/Web.csproj", "src/Web/Web.csproj"]
COPY ["src/Domain/Domain.csproj", "src/Domain/Domain.csproj"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/Infrastructure.csproj"]
COPY ["src/Application/Application.csproj", "src/Application/Application.csproj"]

COPY ["tests/Application.UnitTests/Application.UnitTests.csproj", "tests/Application.UnitTests/Application.UnitTests.csproj"]
COPY ["tests/Domain.UnitTests/Domain.UnitTests.csproj", "tests/Domain.UnitTests/Domain.UnitTests.csproj"]
COPY ["tests/Web.AcceptanceTests/Web.AcceptanceTests.csproj", "tests/Web.AcceptanceTests/Web.AcceptanceTests.csproj"]
COPY ["tests/Application.FunctionalTests/Application.FunctionalTests.csproj", "tests/Application.FunctionalTests/Application.FunctionalTests.csproj"]
COPY ["tests/Infrastructure.IntegrationTests/Infrastructure.IntegrationTests.csproj", "tests/Infrastructure.IntegrationTests/Infrastructure.IntegrationTests.csproj"]

RUN dotnet restore ActivityManager.sln -a $TARGETARCH

# copy csproj and restore as distinct layers
COPY src/ src/

# RUN dotnet restore ./src/Web/Web.csproj -a $TARGETARCH

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
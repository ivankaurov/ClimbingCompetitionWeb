FROM microsoft/dotnet:2.0.0-runtime
ARG source
WORKDIR /app
COPY ${source} .
ENTRYPOINT [ "dotnet", "Climbing.Web.MigratorService.dll" ]
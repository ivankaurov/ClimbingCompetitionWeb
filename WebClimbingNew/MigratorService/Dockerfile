FROM microsoft/dotnet:2.1-runtime
ARG source
WORKDIR /app
COPY ${source} .
ENTRYPOINT [ "dotnet", "Climbing.Web.MigratorService.dll" ]
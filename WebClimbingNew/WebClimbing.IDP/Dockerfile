FROM microsoft/dotnet:2.1-aspnetcore-runtime
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source} .
ENTRYPOINT [ "dotnet", "Climbing.Web.IDP.dll" ]
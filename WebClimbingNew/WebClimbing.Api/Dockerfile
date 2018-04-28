FROM microsoft/aspnetcore:2.0.7
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source} .
ENTRYPOINT [ "dotnet", "Climbing.Web.Api.dll" ]
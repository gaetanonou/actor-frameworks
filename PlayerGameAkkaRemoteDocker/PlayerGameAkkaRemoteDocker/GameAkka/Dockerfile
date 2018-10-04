FROM microsoft/dotnet:2.1.1-aspnetcore-runtime

COPY ./DockerBuild ./app

EXPOSE 8080

WORKDIR /app

ENTRYPOINT  ["dotnet", "GameAkka.dll"]

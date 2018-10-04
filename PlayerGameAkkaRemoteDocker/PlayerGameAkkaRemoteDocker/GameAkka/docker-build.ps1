#First build to local docker build directory
dotnet publish -c Release -o DockerBuild

#Build image based on Dockerfile
docker build -t "demo/gameserver:latest" -t "aucxis.azurecr.io/demo/gameserver:latest" .

rm -Force -Recurse DockerBuild
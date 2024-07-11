## AspNetCore Microservice

## prepare enviroment
* Install donet core version in file 'global.json'
* Visual studio 2022
* docker desktop
---

## How to run the project

Run command for build project

```Power shell
dotnet build
```

Go to the folder contain file `docker-compose`

1. Using docker-compose
```Power shell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```

## Local Environment (Docker container):

## Docker application urls
- Portainer: http://localhost:9000 - username: admin ; pass: admin123456789
- Kibana: http://localhost:5601 - username: elastic ; pass: admin
- RabbitMQ: http://locahost - username: guest ; pass: guest

2. Using visual studio 2022
- Open sln file
- Run Compound to start multi projects
---

## Developement environment

## Production environment

---

## Package reference

## Install environment

- https://dotnet.microsoft.com/download/dotnet/6.0
- https://visualstudio.microsoft.com/

## Reference url

## Docker command

- docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans -build

## Useful commands

- ASPNETCORE_ENVIRONMENT=Production dotnet ef database update
- dotnet watch run --environment "Developement"
- dotnet restore
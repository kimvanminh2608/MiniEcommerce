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

## Application URLs - Local Environment (Docker container):
- Product API: http://localhost:6002/api/products
- Customer API: http://localhost:6003/api/customers
- Basket API: http://localhost:6004/api/baskets

## Docker application urls
- Portainer: http://localhost:9000 - username: admin ; pass: admin123456789
- Kibana: http://localhost:5601 - username: elastic ; pass: admin
- RabbitMQ: http://locahost - username: guest ; pass: guest

2. Using visual studio 2022
- Open sln file
- Run Compound to start multi projects
---

## Developement environment
- Product API: http://locahost:5002/api/products
- Customer API: http://localhost:5003/api/customers
- Basket API: http://localhost:5004/api/baskets
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
- Migration Command:
- add-migration "Init_OrderDb" -p Ordering.Infrastructure -s Ordering.API -o Persistence\Migrations
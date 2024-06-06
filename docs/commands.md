

## REDIS

- Pobranie obrazu
```
docker pull redis
```

- Uruchomienie kontenera na podstawie obrazu
```
docker run --name sages-redis -d -p 6379:6379 redis
```

- Uruchomienie klienta REDIS w kontenerze
```
docker exec -it sages-redis redis-cli

```

- Podstawowe polecenia REDIS
```
PING
SET message "Hello World!"
GET message
KEYS *
```

- Hash
```
HINCRBY cart:user:123 product:1 1 i
```

## Rabbit MQ

- Uruchomienie kontenera
```
 docker run --name sages-message-broker --hostname ecommerce-mq --restart always -p 5672:5672 -p 15672:15672  -e  RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password -d  rabbitmq:3-management-alpine
 ```
 
- Strona do zarządzania:
 `http://localhost:15672/`
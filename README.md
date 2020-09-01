# GameMarketAPI

## Overview

This API allows vendors to add their games with keys and clients to buy them

## What is done
- **GameMarketAPI**
    - Basic CRUD operations to work with Games and GameKeys
    - Buying games via operations over Payment
    - JWT-based authentication for both client and vendor 
    - [OpenAPI Specification](https://app.swaggerhub.com/apis/thinkingabouther2/game-market_api/1.2.3)
    - [Exception handling middleware](https://github.com/thinkingabouther/game-market-API/blob/master/GameMarketAPI/Controllers/ErrorController.cs)
    that [logs](https://github.com/thinkingabouther/game-market-API/blob/master/GameMarketAPI/Services/ExceptionLoggingService/RedisLoggingService.cs) unpredicted exception using Redis
    - [Notifying services](https://github.com/thinkingabouther/game-market-API/tree/master/GameMarketAPI/Services/NotifyingService) for both vendor (HTTP) and client (Email). 
    [Hash](https://github.com/thinkingabouther/game-market-API/blob/master/GameMarketAPI/Services/NotifyingService/VendorNotifyingService.csv) is sent to the vendor in order to verify the message on the vendor side. 
    Hash is computed based on the body of the request + the secret that might be configured in appconfig.json
    - [Dockerfile](https://github.com/thinkingabouther/game-market-API/blob/master/Dockerfile) and [docker-compose](https://github.com/thinkingabouther/game-market-API/blob/master/docker-compose.yml)

- **VendorNotifier**    
  - Separate service that uses RabbitMQ to receive messages from the main API and resend messages in case the request was unsuccessful
  - [Message consumer](https://github.com/thinkingabouther/game-market-API/blob/master/VendorNotifier/MessageConsumer.cs) that receives the message from API processes the message using [Requester](https://github.com/thinkingabouther/game-market-API/blob/master/VendorNotifier/Requesters/PostRequester.cs). 
  - [Message postprocessor](https://github.com/thinkingabouther/game-market-API/tree/master/VendorNotifier/FailurePostProcessors) that resends the message back to the queue and to Requester using DLX
  - Custom growing TTL for message that was rejected
  
# What might be done in the future
- Proper deployment (API and Notifier as separate services)
- Payment session expiration time
- Replacing EF Core with ADO.NET in order to provide query caching (i.e. Redis in-memory cache)

## Requirements
- .NET Core 3 or newer
- RabbitMQ 3 or newer
- Redis 6 or newer

## Usage
### Using Docker
```
docker-compose up
```
The app will be listening on *http://.:5000*

It this case you won't be able to receive POST requests to vendor URL

### Using .NET CLI
1. Run API:
```
dotnet run --project GameMarketAPI
```
2. Open another terminal instance and run Notifier
```
dotnet run --project VendorNotifier
```


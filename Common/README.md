# Common
- Reusable code and configurations across all microservices are added here

## Set up
- For setting up a microservice to use Common package, please follow this [commit](https://github.com/AbdulmajeedAlOmari/Aishow/commit/c197e62bc0e7a39d42132e3b13df9ef5181508da).

## Provides
- `HttpCommonExceptionFilter`: Formats exceptions to be readable for API users.
- `UseAishowSentry`: Preconfigured Sentry client integration extension with custom exception filter for **ApiException** to not be included in Sentry issues.
- `MessageBroker`: Message broker interface to use RabbitMQ or any other message brokers.
- `IServiceClient`: Interface contains all exposed endpoints for microservices to be reused easily in all microservices. 
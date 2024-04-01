---
title: HttpMediator Packages
---

# HTTP Mediator packages

| Package | Description |
| ------- | ----------- |
| [HttpMediator.Client](./HttpMediator/HttpMediator.Client.md)                                   | Client implementation of Tyne's HTTP Mediator. |
| [HttpMediator.Client.FluentValidation](./HttpMediator/HttpMediator.Client.FluentValidation.md) | Fluent Validation middleware for Tyne's client HTTP Mediator. |
| [HttpMediator.Core](./HttpMediator/HttpMediator.Core.md)                                       | Core library of Tyne's HTTP Mediator. |
| [HttpMediator.Server](./HttpMediator/HttpMediator.Server.md)                                   | Server implementation of Tyne's HTTP Mediator. |
| [HttpMediator.Server.FluentValidation](./HttpMediator/HttpMediator.Server.FluentValidation.md) | Fluent Validation middleware for Tyne's server HTTP Mediator. |
| [HttpMediator.Server.MediatR](./HttpMediator/HttpMediator.Server.MediatR.md)                   | MediatR middleware for Tyne's server HTTP Mediator. |

# Overview
Tyne's HTTP Mediator is an end-to-end Client/Server implementation of the Mediator pattern.

Clients can inject [IHttpMediator](xref:Tyne.HttpMediator.Client.IHttpMediator), which will send requests to the server to be processed.

The general flow is as follows:

```mermaid
flowchart TD
    subgraph Client
        UserComponent[Component.razor] -->|SendAsync| IHttpMediator
        IHttpMediator --> ClientMiddlewarePipeline
    end

    ClientMiddlewarePipeline[[Middleware pipeline]] -. (HTTP request) .-> ServerHttpRequestHandler

    subgraph Server
        ServerHttpRequestHandler[HTTP endpoint] --> ServerMiddlewarePipeline
        ServerMiddlewarePipeline[[Middleware pipeline]] --> ServerHandler
        ServerHandler[Request handler]
    end
```

## Middleware
HTTP Mediator middleware is similar to ASP.NET Core's implementation.
Middlewares are registered in the order they will be executed during configuration
(e.g. [client](./HttpMediator/HttpMediator.Client.md), [server](./HttpMediator/HttpMediator.Server.md)).
Their purpose is to generically handle requests, such as validating them or handling exceptions to return an error result.

The client pipeline is responsible for taking the user's request and sending it to the server.
The server pipeline is responsible for handling requests from an endpoint.

### Terminal middleware
Terminal middlewares are the final steps in a pipeline, and execute the request. For example, the HTTP sender middleware on the client is terminal as it is responsible for sending the request, and transforming it into a response.

### Short-circuiting middleware
Some middlewares can short circuit the pipeline. This allows a middleware to return without executing the rest of the pipeline, such as when validating a request.

For example, the server can validate the request. If valid, the request handler is invoked. Otherwise, the pipeline can short circuit and return a bad result:

```mermaid
sequenceDiagram
    participant Pipeline
    participant ValidationMiddleware as Validation middleware
    participant RequestHandler as Request handler

    note over Pipeline,RequestHandler: Example valid request
    Pipeline->>+ValidationMiddleware: Invoke(request)
    ValidationMiddleware->>+RequestHandler: Invoke(request)
    RequestHandler->>-ValidationMiddleware: Return result
    ValidationMiddleware->>-Pipeline: Return result

    note over Pipeline,RequestHandler: Example invalid request
    Pipeline->>+ValidationMiddleware: Invoke(request)
    note over ValidationMiddleware: Request is invalid,<br/>short circuit and return
    ValidationMiddleware->>-Pipeline: Return bad result
```


### End-to-end request example
```mermaid
sequenceDiagram
    box Client
    participant Component as Component.razor
    participant IHttpMediator
    participant ClientPipeline as Client pipeline
    participant ClientExceptionHandlerMiddleware as Exception handler middleware
    participant ClientValidationMiddleware as Validation middleware
    participant ClientSenderMiddleware as Sender middleware
    end

    box Server
    participant ServerEndpoint as HTTP endpoint
    participant ServerPipeline as Server pipeline
    participant ServerExceptionHandlerMiddleware as Exception handler middleware
    participant ServerValidationMiddleware as Validation middleware
    participant ServerMediatrMiddleware as MediatR middleware
    participant ServerRequestHandler as Request handler
    end

    Component->>+IHttpMediator: Send request
    IHttpMediator->>+ClientPipeline: Run pipeline
    ClientPipeline->>+ClientExceptionHandlerMiddleware: 
    ClientExceptionHandlerMiddleware->>+ClientValidationMiddleware: 
    ClientValidationMiddleware->>+ClientSenderMiddleware: 

    ClientSenderMiddleware-->>ServerEndpoint: HTTP request

    ServerEndpoint->>+ServerPipeline: Run pipeline
    ServerPipeline->>+ServerExceptionHandlerMiddleware: 
    ServerExceptionHandlerMiddleware->>+ServerValidationMiddleware: 
    ServerValidationMiddleware->>+ServerMediatrMiddleware: 
    ServerMediatrMiddleware->>+ServerRequestHandler: Send MediatR request

    ServerRequestHandler->>-ServerMediatrMiddleware: MediatR result
    ServerMediatrMiddleware->>-ServerValidationMiddleware: 
    ServerValidationMiddleware->>-ServerExceptionHandlerMiddleware: 
    ServerExceptionHandlerMiddleware->>-ServerPipeline: 
    ServerPipeline->>-ServerEndpoint: Pipeline result

    ServerEndpoint-->>ClientSenderMiddleware: HTTP response

    ClientSenderMiddleware->>-ClientValidationMiddleware: 
    ClientValidationMiddleware->>-ClientExceptionHandlerMiddleware: 
    ClientExceptionHandlerMiddleware->>-ClientPipeline: 
    ClientPipeline->>-IHttpMediator: Pipeline result
    IHttpMediator->>-Component: Result
```

# Request type
Request types need to implement [IHttpRequestBase](xref:Tyne.HttpMediator.IHttpRequestBase`1). This gives Tyne metadata about how the client and server should communicate, and acts as a marker so that the mediator knows what return type it should expect.

# Request encoding
`GET` and `DELETE` requests serialise the request to the URL. `POST`, `PUT`, and `PATCH` request serialise the request to the body.

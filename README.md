# AspNetCoreWithReact
Sample application that show architectural design for building AspNet Core 7 RESTful minimal APIs with React as frontend. The application also demonstrates [Dapr](https://dapr.io/) integration with AspNet Core 7 RESTful minimal APIs.
Following Dapr building blocks are demonstrated

1) Service Invocation
2) State Management
3) Pub/Sub

## Prerequisites
1) [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
2) [Dapr CLI] (https://docs.dapr.io/getting-started/install-dapr-cli/)
3) MongoDb Atlas : The application uses the free version of MongoDb Atlas. You can create a free account [here](https://www.mongodb.com/cloud/atlas/register)

## Projects
All projects are located in the src folder. The projects are as follows
1) AuthApi : This is the Web API project, it has the endpoints of `Login` and `Token`. The login endpoint uses the cookie based authentication whereas the token based used the JWT based authentication. The `Logout` endpoint deletes the cookie from the session.
2) BackendForFrontEnd: This project uses AspNetCore 7 minimal API features of the group routing. This project will be the entry point for the front end react application. This project communicates to the downstream API like `CustomerApi` via Dapr service invocation. It also uses Dapr State management building block for saving the token in the Redis. This project also uses Swagger advanced functionality use JWT authentication. In the Swagger UI it will prompt for JWT token.
3) CustomerApi: This project uses all the latest features of AspNet Core 7 such as filters and group routes. The project follows vertical slicing approach where all feature of the project as within the specific folder. eg `CreateCustomer`, `GetAllCustomers` etc. The project also demonstrates the usage of [Generic Repository](https://github.com/goldytech/AspNetCoreWithReact/blob/main/src/Api/CustomerApi/Common/MongoDbServices/MongoRepository.cs) pattern with MongoDb. The repositiory class creates a high level abstraction with the Mongodb driver, so a .NET developer doesn't need to learn the native query syntax of the MongoDb. The repository uses standard Linq methods. For managing the validations it uses the AspnetCore 7 filter feature.
4) OrdersApi: This Web api project demonstrates the usage of Dapr Pub Sub building block. Redis streams and RabbitMQ both are configured as message brokers. The configuration files of both are located under `dapr-components` folder.Just by changing the `PubSubName` variable value you can change the underlying message broker without changing the single line of code. This is the true power of Dapr.
5) SignalR.Notifications: This project leverages SignalR websockets functionality to send ui updates to the React app.
It is SignalR server with a [Hub](https://github.com/goldytech/AspNetCoreWithReact/blob/main/src/Api/SignalR.Notifications/NotificationHub.cs) in it. If the server wants to send a notification to the client , it invokes the hub method called `UpdateUI`.
6) pyevtsub: This is a python project that demonstrates the usage of Dapr Pub Sub building block. It is a simple python script that subscribes to the `ordersubmitted` topic and prints the message on the console. The python script is configured to use RabbitMQ as message broker. The configuration file is located under `dapr-components` folder.So now when an Order is Submitted , the event gets published and it is listened by two listeners , one is .NET Api (OrdersApi) and another is python project (pyevtsub).
7) react-app: This is frontend project which was created using [create-react-app](https://reactjs.org/docs/create-a-new-react-app.html). It uses TypeScript instead of JavaScript. It also leverages other features of React like
* React Router
* React Hooks
* React Query
* React Functional Components
It also uses Signalr client library for establishing the socket connection with the Server (SignalR.Notifications)
# Coolector.Services.Users

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Coolector.Services.Users.svg?branch=master)](https://travis-ci.org/noordwind/Coolector.Services.Users)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Coolector.Services.Users.svg?branch=develop)](https://travis-ci.org/noordwind/Coolector.Services.Users/branches)

####**Keep your commune clean in just a few clicks.**

**What is Coolector?**
----------------

Have you ever felt unhappy or even angry about the litter left on the streets or in the woods? Or the damaged things that should've been fixed a long time ago, yet the city council might not even be aware of them?

**Coolector** is an open source & cross-platform solution that provides applications and a services made for all of the inhabitants to make them even more aware about keeping the community clean. 
Within a few clicks you can greatly improve the overall tidiness of the place where you live in. 

**Coolector** may help you not only to quickly submit a new remark about the pollution or broken stuff, but also to browse the already sent remarks and help to clean them up if you feel up to the task of keeping your neighborhood a clean place.

**Coolector.Services.Users**
----------------

The **Coolector.Services.Users** is a service responsible for handling the user authentication & authorization, editing the account, changing and resetting password and other types of operations that are related to the user account. 

In order to access the available commands, events, DTOs and operation codes make use of the **Coolector.Services.Users.Shared** package.

**Quick start**
----------------

## Docker way

Coolector is built as a set of microservices, therefore the easiest way is to run the whole system using the *docker-compose*.

Clone the [Coolector.Docker](https://github.com/noordwind/Coolector.Docker) repository and run the *start.sh* script:

```
git clone https://github.com/noordwind/Coolector.Docker
./start.sh
```

Once executed, you shall be able to access the following services:

|Name               |URL                                                  |Repository 
|-------------------|-----------------------------------------------------|-----------------------------------------------------------------------------------------------
|API                |[http://localhost:5000](http://localhost:5000)       |[Coolector.Api](https://github.com/noordwind/Coolector.Api) 
|Mailing            |[http://localhost:10005](http://localhost:10005)     |[Coolector.Services.Mailing](https://github.com/noordwind/Coolector.Services.Mailing) 
|Operations         |[http://localhost:10000](http://localhost:10000)     |[Coolector.Services.Operations](https://github.com/noordwind/Coolector.Services.Operations)
|Remark             |[http://localhost:10002](http://localhost:10002)     |[Coolector.Services.Remarks](https://github.com/noordwind/Coolector.Services.Remarks)
|SignalR            |[http://localhost:15000](http://localhost:15000)     |[Coolector.Services.SignalR](https://github.com/noordwind/Coolector.Services.SignalR) 
|Statistics         |[http://localhost:10006](http://localhost:10006)     |[Coolector.Services.Statistics](https://github.com/noordwind/Coolector.Services.Statistics)
|Storage            |[http://localhost:10000](http://localhost:10000)     |[Coolector.Services.Storage](https://github.com/noordwind/Coolector.Services.Storage)
|Supervisor         |[http://localhost:11000](http://localhost:11000)     |[Coolector.Services.Supervisor](https://github.com/noordwind/Coolector.Services.Supervisor)
|**Users**          |**[http://localhost:10001](http://localhost:10001)** |**[Coolector.Services.Users](https://github.com/noordwind/Coolector.Services.Users)** 
|Web                |[http://localhost:9000](http://localhost:9000)       |[Coolector.Web](https://github.com/noordwind/Coolector.Web) 

## Classic way

In order to run the **Coolector.Services.Users** you need to have installed:
- [.NET Core](https://dotnet.github.io)
- [MongoDB](https://www.mongodb.com)
- [RabbitMQ](https://www.rabbitmq.com)

Clone the repository and start the application via *dotnet run* command:

```
git clone https://github.com/noordwind/Coolector.Services.Users
cd Coolector.Services.Users/Coolector.Services.Users
dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/coolector/api/v3/index.json --no-cache
dotnet run
```

Now you should be able to access the service under the [http://localhost:10001](http://localhost:10001). 

Please note that the following solution will only run the Users Service which is merely one of the many parts required to run properly the whole Coolector system.

**Configuration**
----------------

Please edit the *appsettings.json* file in order to use the custom application settings. To configure the docker environment update the *dockerfile* - if you would like to change the exposed port, you need to also update it's value that can be found within *Program.cs*.
For the local testing purposes the *.local* or *.docker* configuration files are being used (for both *appsettings* and *dockerfile*), so feel free to create or edit them.

**Tech stack**
----------------
- **[.NET Core](https://dotnet.github.io)** - an open source & cross-platform framework for building applications using C# language.
- **[Nancy](http://nancyfx.org)** - an open source framework for building HTTP API.
- **[MongoDB](https://github.com/mongodb/mongo-csharp-driver)** - an open source library for integration with [MongoDB](https://www.mongodb.com) database.
- **[RawRabbit](https://github.com/pardahlman/RawRabbit)** - an open source library for integration with [RabbitMQ](https://www.rabbitmq.com) service bus.

**Solution structure**
----------------
- **Coolector.Services.Users** - core and executable project via *dotnet run* command.
- **Coolector.Services.Users.Shared** - shared package containing events, commands, DTOs & operation codes.
- **Coolector.Services.Users.Tests** - unit & integration tests executable via *dotnet test* command.
- **Coolector.Services.Users.Tests.EndToEnd** - End-to-End tests executable via *dotnet test* command.
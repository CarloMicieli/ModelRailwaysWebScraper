# ModelRailways Web Scraper

Just another web scraper for model railways info.

## How to run

First, let's run the rabbit mq to make Mass Transit (and the application happy):

```
$ docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Now, let's run the application:

```
$ dotnet run -p Src/ModelRailwayWorker
```

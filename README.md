## vs-code

### Prerequisites:

Install [.NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

Install [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/)

### Loose Agenda:

Play with C# and create a WebAPI application


### Step by Step

#### Setup playground

Create a directory for this exercise

Open Visual Studio 2019

Create a new project > ASP.NET Core Web Application

Name: dotnet-csharp-webapi
Location: C:\dev\non-zero-days\dotnet-csharp-webapi
Solution Name: Leave default

From the top dropdowns, select ASP.NET Core 5.0

Select ASP.NET Core Web API

On the right make certain that 'Configure for HTTPS' and 'Enable OpenAPI support' are checked

Click Create

#### Exploring the default template

The ASP.NET Core 5.0 Web API template implements a single GET endpoint with service discoverability/documentation implemented via Swagger. 
Note and review the following files:
- WeatherForecast.cs
  - Contract for the templated endpoint
- Controllers/WeatherForecastController.cs
  - Entry point for the templated endpoint
- Program.cs
  - Entry point for the application
- Startup.cs
  - Application Configuration is implemented here

Along your header there should be a green triangle indicating your selected Debug Profile. Select the dropdown icon for that Debug Profile icon and select dotnet_csharp_webapi then start debugging by clicking the icon.

Navigate to 'https://localhost:5001/swagger/index.html' then note the GET /WeatherForecast option. This is Swagger and it can be used to interact with your endpoint.

Stop Debugging by typing Shift+F5 in Visual Studio


#### Creating a GET endpoint

Right-click the Controllers folder and select Add > New Item... 
Search for and select API Controller - Empty
Enter the name NonZeroController.cs then select Add
You should now have an empty controller

```
namespace dotnet_csharp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonZeroController : ControllerBase
    {
    }
}
```

Let's add a new method to that controller which will return a string.

```
    public class NonZeroController : ControllerBase
    {
        public string NonZeroMessage()
        {
            return "Congratulations on your Non-Zero Day!";
        }
    }
```

Now let's decorate that method with some API attributes
```
    public class NonZeroController : ControllerBase
    {
        [HttpGet]
        [Route("motd")]
        public string NonZeroMessage()
        {
            return "Congratulations on your Non-Zero Day!";
        }
    }
```

Start debugging as per above.

Navigate to 'https://localhost:5001/swagger/index.html' then note the GET /api/NonZero/motd option.

Click the /api/NonZero/motd item then click "Try it out" followed by "Execute"

Note the Response Body includes our expected string.

Stop Debugging by typing Shift+F5 in Visual Studio


#### ConcurrentDictionary

Let's implement a dictionary in our code that can be called into and adjusted at runtime.

Create a new class called MessageRepository.cs and populate it with the following code:
```
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace dotnet_csharp_webapi
{
    public class MessageRepository
    {
        IDictionary<string, string> _dictionary = new ConcurrentDictionary<string, string>();

        public string GetMessage()
        {
            if (_dictionary.ContainsKey("motd"))
            {
                return _dictionary["motd"];
            }

            return "No motd entered.";
        }

        public string SetMessage(string input)
        {
            _dictionary["motd"] = input;
            return _dictionary["motd"];
        }
    }
}

```

Right-click the MessageRepository class name and select Quick Actions and Refactorings then select Extract Interface. Click OK to select the defaults.

Now let's adjust the Startup.cs ConfigureServices method to be able to manage the lifetime of this MessageRepository class.
```
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnet_csharp_webapi", Version = "v1" });
            });

            services.AddSingleton<IMessageRepository, MessageRepository>();
        }

```


Let's adjust our NonZeroController.cs to have this MessageRepository dependency injected in it's constructor.
```
using Microsoft.AspNetCore.Mvc;

using System;

namespace dotnet_csharp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonZeroController : ControllerBase
    {
        private readonly IMessageRepository _repository;

        public NonZeroController(IMessageRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Route("motd")]
        public string NonZeroMessage()
        {
            return _repository.GetMessage();
        }
    }
}

```

Now if we run our code and call the endpoint we will see "No motd entered." 

Let's add a way to adjust that message of the day next.


#### Creating a PUT endpoint

We want a way to enter or edit the message of the day without having to update the code. Let's create a PUT endpoint to accomplish this.
**Note - Some developers use the POST HTTP action for this goal. I suggest being pragmatic by being consistent within your projects with your own decisions for what verb to use.**

Let's create a new method.

```
using Microsoft.AspNetCore.Mvc;

using System;

namespace dotnet_csharp_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonZeroController : ControllerBase
    {
        private readonly IMessageRepository _repository;

        public NonZeroController(IMessageRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Route("motd")]
        public string NonZeroMessage()
        {
            return _repository.GetMessage();
        }

        [HttpPut]
        [Route("motd")]
        public string SetMessageOfTheDay(string input)
        {
            return _repository.SetMessage(input);
        }
    }
}

```

Now if you run your code and navigate to 'https://localhost:5001/swagger/index.html' you will see two motd endpoints. Try out the following
- Call the GET endpoint and note the default motd is returned.
- Call the PUT endpoint with the input "Non-Zero Day!" and note the message should be returned to you indicating success.
- Call the GET endpoint again and note that the previous PUT call has adjusted the message.
- Stop then restart debugging and note that the value has not persisted between debug sessions.


## Additional Reading
- [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

Check out the links in the description for additional reading

Congratulations on a non-zero day!

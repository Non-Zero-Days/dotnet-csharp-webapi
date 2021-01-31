using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

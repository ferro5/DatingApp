using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatingApp.API.Controllers
{
    public class TCController : ControllerBase
    {
        protected IMapper _mapper;
        protected ILogger<TCController> _logger;
        protected Config _config;

        public TCController(IMapper mapper, ILogger<TCController> logger,IOptions<Config> config)
        {
            _mapper = mapper;
            _logger = logger;
            _config = config.Value;
        }
    }
}

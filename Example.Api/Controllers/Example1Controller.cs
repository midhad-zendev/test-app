using System.Collections.Generic;
using Example.Api.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Example.Api.Models;
using Example.Db.Interfaces;

namespace Example.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class Example1Controller : BaseController
    {

        private readonly ILogger<Example1Controller> _logger;
        private readonly IExample1CollectionService _example1Service;
        public Example1Controller(IDbContext dbContext, ILogger<Example1Controller> logger, IExample1CollectionService example1Service) : base(dbContext)
        {
            _logger = logger;
            _example1Service = example1Service;
        }

        /// <summary>
        /// Get a list
        /// </summary>
        [HttpGet]
        [Produces("application/json", Type = typeof(IEnumerable<Example1Model>))]
        [ProducesResponseType(typeof(IEnumerable<Example1Model>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [ProducesResponseType(typeof(ErrorResponse), 502)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public  IEnumerable<Example1Model> GetSomeList()
        {
            var exampleList = _example1Service.ListExamples();
            return exampleList;
        }

        


    }
}

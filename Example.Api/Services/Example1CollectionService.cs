using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Example.Api.Core;
using Example.Api.Models;

namespace Example.Api.Services
{
 
    public class Example1CollectionService : IExample1CollectionService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<Example1CollectionService> _logger;
        public Example1CollectionService(IMapper mapper, ILogger<Example1CollectionService> logger)
        {
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// List Sensors
        /// </summary>
        //public async Task<IEnumerable<SensorsModel>> ListSensorsAsync()
        //{
        //    _logger.LogTrace($"List Sensors Request received");
        //    DynamoDBContext dbContext = new DynamoDBContext(_dynamoDb);
        //    List<SensorsModel> docsList = new List<SensorsModel>();
        //    Stopwatch stopWatch = new Stopwatch();

        //    #region Get data from Amazon
        //    try
        //    {
        //        stopWatch.Start();
        //        _logger.LogTrace($"Prepared query using sensors table");
        //        docsList = await dbContext.ScanAsync<SensorsModel>(null).GetRemainingAsync();
        //        _logger.LogTrace($"Finished query returning:{docsList.Count} records.");

        //    }
        //    catch (AmazonServiceException ase)
        //    {
        //        _logger.LogError("Could not complete operation: {0} {1}", ase.Message, ase.StackTrace);
        //        throw new HttpResponseException($"Could not complete operation RequestId:{ase.RequestId} ErrorType:{ase.ErrorType} ErrorCode:{ase.ErrorCode}", ase.Message, (int)ase.StatusCode);
        //    }
        //    catch (AmazonClientException ace)
        //    {
        //        _logger.LogError("Error occurred communicating with DynamoDB: {0} {1}", ace.Message, ace.StackTrace);
        //        throw new HttpResponseException($"Error occurred communicating with external service", ace.Message, 502);
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError("Gateway error: {0} {1}", e.Message, e.StackTrace);
        //        throw new HttpResponseException("Gateway error", e.Message, 502);
        //    }
        //    finally
        //    {
        //        stopWatch.Stop();
        //        TimeSpan tsa = stopWatch.Elapsed;
        //        _logger.LogTrace($"Fetch all reqested records finished, count:{docsList.Count}, duration:{string.Format("{0:00}h:{1:00}m:{2:00}s.{3:00}ms", tsa.Hours, tsa.Minutes, tsa.Seconds, tsa.Milliseconds)}");
        //    }
        //    #endregion
        //    return docsList;
        //}

    
        public IEnumerable<Example1Model> ListExamples()
        {
            List<Example1Model> result = new List<Example1Model> { new Example1Model { Id = "123", Name = "Test1" }, new Example1Model { Id = "3456", Name = "Test3456" } };
            _logger.LogDebug($"result!");
            return result;
            //throw new NotImplementedException();
        }
    }
}

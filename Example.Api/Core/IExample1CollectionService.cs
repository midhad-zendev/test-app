using System.Collections.Generic;
using Example.Api.Models;

namespace Example.Api.Core
{
    public interface IExample1CollectionService
    {
        IEnumerable<Example1Model> ListExamples();
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Example.Api.Helper;

namespace Example.Test
{
    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void WherePreviousTest()
        {
            //Arrange
            var numbers = new[] { 1, 5, 8, 7, 12, 12, 8, 5 };
            var expected = new[] { 1, 5, 8, 7, 12 };
            //Act
            var actual = numbers.WherePrevious((first, second) => second != first).ToArray();

            //Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    
    }
}

using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            Foo foo = new Foo() { Id = 42, Name = "Foo", LastName = "Full foo name", Address = "Minsk" };
            Bar bar;

            mapGenerator.AddPropertyMap(nameof(foo.Address), nameof(bar.Location));
            mapGenerator.AddPropertyMap(nameof(foo.Name), nameof(bar.FullName));
            var mapper = mapGenerator.Generate<Foo, Bar>();

            bar = mapper.Map(foo);

            Assert.AreEqual(foo.Address, bar.Location);
            Assert.AreEqual(foo.Name, bar.FullName);
        }
    }
}

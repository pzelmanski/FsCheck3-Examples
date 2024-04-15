using FsCheck;
using FsCheck.Fluent;
using Xunit.Abstractions;

namespace FsCheck3PBTests.ComplexTypes;

public static class ComplexTypesGenerators
{
    private static readonly List<string> FruitNames = new List<string> { "Apple", "Banana", "Cherry", "Elderberry" };
    private static readonly List<string> ToolNames = new List<string> { "Axe", "Hammer", "Screwdriver", "Wrench" };

    public static Gen<OrderStatus> OrderStatusGenerator()
    {
        return Gen.Elements<OrderStatus>(OrderStatus.New, OrderStatus.InProgress, OrderStatus.Done);
    }

    // Generator returning a name from two lists, with weight attached.
    public static Gen<String> NameGenerator()
    {
        return Gen.Frequency(
            (3, Gen.Elements<String>(FruitNames)),
            (1, Gen.Elements<String>(ToolNames))
        );
    }

    public static Gen<decimal> PriceGenerator()
    {
        return Gen.Choose(1, 100).Select(x => x * 1.0m);
    }

    public static Gen<Order> OrderGenerator()
    {
        return NameGenerator().SelectMany(name =>
        {
            return Gen.Choose(1, 10).Select(qty =>
                new Order(Guid.NewGuid(),
                    name,
                    OrderStatusGenerator().Sample(1, 1).Single(),
                    qty,
                    qty * PriceGenerator().Sample(1, 1).Single())
            );
        });
    }

    public static Arbitrary<Order> OrderArb() =>
        Arb.From(OrderGenerator());
}

public class ComplexTypesTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ComplexTypesTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void SingleOrderTest()
    {
        var prop = Prop.ForAll<Order>(order =>
        {
            _testOutputHelper.WriteLine(order.ToString());
            Assert.True(order.Price >= 1);
        });
        prop.Check(Config.QuickThrowOnFailure
                // Specify the generator class for the test
            .WithArbitrary(new [] { typeof(ComplexTypesGenerators) }));
    }

    [Fact]
    public void OrdersListTest()
    {
        // Having generator for a complex type, we have a generation of a list of such types for free.
        Prop.ForAll<List<Order>>(orders =>
            {
                _testOutputHelper.WriteLine(orders.Count.ToString());
                // Some asserts
            })
            .Check(Config.QuickThrowOnFailure
                .WithArbitrary(new[] { typeof(ComplexTypesGenerators) }));
    }
}
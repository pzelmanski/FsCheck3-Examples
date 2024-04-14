using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace FsCheck3PBTests;

public record PositiveInt(int Value);

public static class SimpleGenerators
{
    // Generator, used to actually generate the values
    public static Gen<PositiveInt> GetPositiveInt() =>
        ArbMap.Default.GeneratorFor<int>().Where(x => x >= 0).Select(x => new PositiveInt(x));
    
    // In order to use the generator in the property-based tests, we need to create an Arbitrary instance
    // This is a function used to generate PositiveInt parameters of PositiveInt_ShouldBePositive test
    public static Arbitrary<PositiveInt> PositiveIntArb() =>
        Arb.From(GetPositiveInt());

    // It is possible to override the default arbitrary generator for a simple type.
    // Without this function, default arb for int would generate any int, including negative ones.
    public static Arbitrary<int> OverrideIntArb() =>
        ArbMap.Default.GeneratorFor<int>().Where(x => x >= 0).ToArbitrary();
}

public class PropertyBasedTests
{
    [Property(Arbitrary = new[] { typeof(SimpleGenerators) })]
    public Property PositiveInt_ShouldBePositive(PositiveInt p)
    {
        
        return (p.Value >= 0).ToProperty();
    }
    
    [Property(Arbitrary = new[] { typeof(SimpleGenerators) })]
    public Property Test2(int p)
    {
        return (p >= 0).ToProperty();
    }
    
    [Fact]
    public void AnotherWayOfRunningTests()
    {
        var prop = Prop.ForAll<PositiveInt>(p => p.Value >= 0);
        prop.Check(Config.Default);
    }
}

public class SomeUnitTests
{
    // It is possible to use generators in a regular unit tests. It might be useful if you
    // already have a generator and you don't want to run it as a PBT (for any reason).
    // However I feel like there is no reason to do that, because why not just use PBT?
    [Fact]
    public void T1()
    {
        var positiveInt = SimpleGenerators.GetPositiveInt().Sample(1).First();
        Assert.True(positiveInt.Value >= 0);
    }
}
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace FsCheck3PBTests.SimpleTypes;

public class SimpleTypesTests
{
    [Property(Arbitrary = new[] { typeof(SimpleTypesGenerators) })]
    public Property PositiveInt_ShouldBePositive(PositiveInt p)
    {
        return (p.Value >= 0).ToProperty();
    }
    
    [Property(Arbitrary = new[] { typeof(SimpleTypesGenerators) })]
    public Property Generator_OverridingInt(int p)
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
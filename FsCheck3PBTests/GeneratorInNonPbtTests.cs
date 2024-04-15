using FsCheck.Fluent;
using FsCheck3PBTests.SimpleTypes;

namespace FsCheck3PBTests;

public class GeneratorInNonPbtTests
{
    // It is possible to use generators in a regular unit tests. It might be useful if you
    // already have a generator and you don't want to run it as a PBT (for any reason).
    // However I feel like there is no reason to do that, because why not just use PBT?
    [Fact]
    public void NonPbtUnitTest()
    {
        var positiveInt = SimpleTypesGenerators.GetPositiveInt().Sample(numberOfSamples: 1, size: 1).First();
        Assert.True(positiveInt.Value >= 0);
    }
}
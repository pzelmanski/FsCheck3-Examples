using FsCheck;
using FsCheck.Fluent;

namespace FsCheck3PBTests.SimpleTypes;

public static class SimpleTypesGenerators
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

    // When there are two generators generating the same type, the one being higher is used.
    // This means that the generator below is ignored and `OverrideIntArb` from above is used.
    public static Arbitrary<int> AnotherOverrideIntArb()
    {
        return ArbMap.Default.GeneratorFor<int>().Where(x => x <= 0).ToArbitrary();
    }
}
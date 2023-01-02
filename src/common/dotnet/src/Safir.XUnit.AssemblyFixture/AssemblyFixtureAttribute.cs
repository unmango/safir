using JetBrains.Annotations;

namespace Safir.XUnit.AssemblyFixture;

// https://github.com/xunit/samples.xunit/blob/5dc1d35a63c3394a8678ac466b882576a70f56f6/AssemblyFixtureExample/AssemblyFixtureAttribute.cs

[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class AssemblyFixtureAttribute : Attribute
{
    public AssemblyFixtureAttribute(Type fixtureType)
    {
        FixtureType = fixtureType;
    }

    public Type FixtureType { get; }
}


[PublicAPI]
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class AssemblyFixtureAttribute<T> : AssemblyFixtureAttribute
{
    public AssemblyFixtureAttribute() : base(typeof(T)) { }
}

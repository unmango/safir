namespace Safir.Manager.EndToEndTests;

[CollectionDefinition(Name)]
public class ManagerServiceCollection : ICollectionFixture<ManagerServiceFixture>
{
    public const string Name = "Manager Service";
}

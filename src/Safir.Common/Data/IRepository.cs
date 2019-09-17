namespace Safir.Common.Data
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}

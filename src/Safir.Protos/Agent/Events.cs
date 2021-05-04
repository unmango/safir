using Safir.Messaging;

// ReSharper disable CheckNamespace
namespace Safir.Agent.Protos
{
    public partial class FileCreated : IEvent { }
    
    public partial class FileChanged : IEvent { }
    
    public partial class FileDeleted : IEvent { }
    
    public partial class FileRenamed : IEvent { }
}

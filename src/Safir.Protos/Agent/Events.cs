// ReSharper disable CheckNamespace

using Safir.Messaging;

namespace Safir.Agent.Protos
{
    public partial class FileCreated : IEvent { }
    
    public partial class FileChanged : IEvent { }
    
    public partial class FileDeleted : IEvent { }
    
    public partial class FileRenamed : IEvent { }
}

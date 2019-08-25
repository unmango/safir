using System;

namespace Safir.Common.Domain
{
    public abstract class EntityMetadata
    {
        protected EntityMetadata()
        {
            CreatedBy = string.Empty;
            UpdatedBy = string.Empty;
        }

        public DateTimeOffset CreatedOn { get; protected internal set; }

        public string CreatedBy { get; protected internal set; }

        public DateTimeOffset UpdatedOn { get; protected internal set; }

        public string UpdatedBy { get; protected internal set; }
    }
}

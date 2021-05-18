using System.Collections.Generic;
using JetBrains.Annotations;

namespace Safir.Manager.Configuration
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ManagerOptions
    {
        public const string DefaultPostgresDatabase = "postgres";

        public const string DefaultSqliteDatabase = "manager.db";
        
        public string DataDirectory { get; set; } = string.Empty;
        
        public bool IsSelfContained { get; set; }

        public string PostgresPassword { get; set; } = string.Empty;
        
        public string PostgresUsername { get; set; } = string.Empty;
        
        public string? PostgresDatabase { get; set; }
        
        public string Redis { get; set; } = string.Empty;
        
        public string? SqliteDatabase { get; set; }
    }
}

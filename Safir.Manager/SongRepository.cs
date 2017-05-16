using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir.Manager
{
    internal class SongRepository : IDisposable
    {
        private readonly SQLiteConnection _songdb;

        public SongRepository(SQLiteConnection conn)
        {
            _songdb = conn;
            _songdb.Open();
        }

        public void Dispose()
        {
            _songdb.Close();
        }
    }
}

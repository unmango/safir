using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Safir
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {

            });
        }
    }
}

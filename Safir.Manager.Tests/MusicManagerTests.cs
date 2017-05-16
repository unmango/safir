using Safir.Manager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Safir.Manager.Tests
{
    public class MusicManagerTests
    {
        [Fact]
        public void TestMethod1()
        {
            var man = new MusicManager("");
            man.RemoveSongs(new List<Song>().ToArray());
        }
    }
}

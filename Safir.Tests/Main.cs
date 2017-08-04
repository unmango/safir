// <copyright file="Main.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Tests
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using Xunit;

    public class Main
    {
        public void Test() {            
        }

        [Fact(Skip = "Utility and not really a test case")]
        public void ReplaceKeyWithKeyName() {
            var file = File.ReadAllLines(@"C:\Users\Erik\Source\Repos\Safir\Safir\Resources\Icons\Icons.xaml");
            for (int i = 0; i < file.Length; i++) {
                var line = file[i];
                var match = Regex.Match(line, "x:Key=\"(.*?)\"");
                if (match.Success) {
                    var val = match.Value;
                    var vals = val.Split('"');
                    var savedVal = vals[1];
                    file[i] = line.Replace(val, $"x:Key=\"{savedVal}\" x:Name=\"{savedVal}\"");
                }
            }
            File.WriteAllLines(@"C:\Users\Erik\Source\Repos\Safir\Safir\Resources\Icons\IconsTemp.xaml", file);
        }

        [Fact(Skip = "Utility and not really a test case")]
        public void ReplaceDoubleLineWithOne() {
            var file = File.ReadAllText(@"C:\Users\Erik\Source\Repos\Safir\Safir\Resources\Icons\Icons.xaml");
            var lines = Regex.Split(Regex.Replace(file, @"(?:\r\n|[\r\n])", "\n"), @"\n{2,}");
            File.WriteAllLines(@"C:\Users\Erik\Source\Repos\Safir\Safir\Resources\Icons\IconsTemp.xaml", lines);
        }

        [Fact(Skip = "Utility and not really a test case")]
        public void ConcatXamlFiles() {
            var root = @"C:\Users\Erik\Pictures\Resources\VS2015 Image Library\2015_VSIcon";
            var outDir = @"C:\Users\Erik\Pictures\Resources\VS2015 Image Library\ConcatResults";
            var count = 1;
            Concat(root, outDir, ref count);
        }

        [Fact(Skip = "")]
        public void ConcatVisualStudioXamlFiles(string newRootDir = "") {
            var vsRootDir = @"C:\Users\Erik\Pictures\Resources\VS2015 Image Library\2015_VSIcon";
            var vsNewRootDir = string.IsNullOrEmpty(newRootDir) ? @"C:\Users\Erik\Pictures\Resources\VS2015 Image Library\2015_VSIcon" : newRootDir;
            var subFiles = Directory.GetFiles(vsRootDir);
            var subDirs = Directory.GetDirectories(vsRootDir);
            if (subDirs.Length > 1)
                ConcatVisualStudioXamlFiles();

        }

        [Fact(Skip = "")]
        private void Concat(string rootDir, string outDir, ref int count) {
            var rootDirs = Directory.GetDirectories(rootDir);
            var rootFiles = Directory.GetFiles(rootDir);
            foreach (var dir in Directory.GetDirectories(rootDir)) {
                Console.WriteLine(count++);
                var files = Directory.GetFiles(dir);
                foreach (var file in files) {
                    if (file.Contains(".xaml")) {
                        var text = File.ReadAllText(file);
                        text = text.Replace("<Viewbox", $"<Viewbox x:Key=\"{Path.GetFileNameWithoutExtension(file)}\"");
                        var temp2 = Path.GetDirectoryName(dir);
                        var temp3 = Path.GetDirectoryName(outDir);
                        var temp = Path.Combine(temp3, temp2);
                        File.AppendAllText(Path.Combine(outDir, Path.GetPathRoot(dir)), text + '\n');
                    }
                }
                Concat(dir, outDir, ref count);
            }
        }
    }
}

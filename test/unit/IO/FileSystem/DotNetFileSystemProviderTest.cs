using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.IO.FileSystem;
using Xunit;
using System.IO;
using System.Reflection;

namespace RapidCore.UnitTests.IO.FileSystem
{
    public class DotNetFileSystemProviderTest
    {
        private DotNetFileSystemProvider _fileSystem;

        public DotNetFileSystemProviderTest()
        {
            _fileSystem = new DotNetFileSystemProvider();
        }

        [Fact]
        public void Can_ShowCWD()
        {
            var cwd = _fileSystem.Cwd();
            Assert.NotNull(cwd);
            Assert.NotEmpty(cwd);
        }

        [Fact]
        public void Can_ListDirectory()
        {
            var assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);

            var localFilesPath = Path.Combine(assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files");
            var files = _fileSystem.List(localFilesPath);

            Assert.NotNull(files);
            Assert.NotEmpty(files);
            Assert.Contains(Path.Combine(localFilesPath, $"CsvFile1.csv"), files);
            Assert.Contains(Path.Combine(localFilesPath, $"TextFile1.txt"), files);   
        }

        [Fact]
        public void Can_ListDirectoryWithSearchPattern()
        {
            var assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);

            var localFilesPath = Path.Combine(assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files");
            var files = _fileSystem.List(localFilesPath, "*.csv");

            Assert.NotNull(files);
            Assert.NotEmpty(files);
            Assert.Contains(Path.Combine(localFilesPath, $"CsvFile1.csv"), files);
            Assert.DoesNotContain(Path.Combine(localFilesPath, $"TextFile1.txt"), files);
        }
    }
}

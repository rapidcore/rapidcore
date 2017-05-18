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
        private string _assemblyPath;
        private string _filesPath;
        private string _originFilesPath;

        public DotNetFileSystemProviderTest()
        {
            _fileSystem = new DotNetFileSystemProvider();
            _assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);
            _filesPath = Path.Combine(_assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files");
            _originFilesPath = Path.Combine(_assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}OriginFolder");
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
            var files = _fileSystem.List(_filesPath);

            Assert.NotNull(files);
            Assert.NotEmpty(files);
            Assert.Contains(Path.Combine(_filesPath, $"CsvFile1.csv"), files);
            Assert.Contains(Path.Combine(_filesPath, $"TextFile1.txt"), files);   
        }

        [Fact]
        public void Can_ListDirectoryWithSearchPattern()
        {
            var files = _fileSystem.List(_filesPath, "*.csv");

            Assert.NotNull(files);
            Assert.NotEmpty(files);
            Assert.Contains(Path.Combine(_filesPath, $"CsvFile1.csv"), files);
            Assert.DoesNotContain(Path.Combine(_filesPath, $"TextFile1.txt"), files);
        }

        [Fact]
        public void Can_MoveFileToFolder()
        {
            var srcFile = Path.Combine(_originFilesPath, "FileToMove.txt");
            var dstFile = Path.Combine(_filesPath, "FileToMove.txt");
            _fileSystem.Move(srcFile, dstFile);

            var srcFolder = Directory.EnumerateFiles(_originFilesPath);
            var dstFolder = Directory.EnumerateFiles(_filesPath);

            Assert.DoesNotContain(srcFile, srcFolder);
            Assert.Contains(dstFile, dstFolder);

            _fileSystem.Move(dstFile, srcFile);

            Assert.Contains(srcFile, srcFolder);
            Assert.DoesNotContain(dstFile, dstFolder);

        }
    }
}

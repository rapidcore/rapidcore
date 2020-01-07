using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using RapidCore.IO.FileSystem;
using Xunit;

namespace UnitTests.Core.IO.FileSystem
{
    public class DotNetFileSystemProviderTest
    {
        private DotNetFileSystemProvider _fileSystem;
        private string _assemblyPath;
        // the full path: e.g. C:/.../.../bin/debug/IO/FileSystem/Files
        private string _filesPath;
        // the local IO/FileSystem/OriginFolder
        private string _originFilesPath;
        // the local IO/FileSystem/Files
        private string _localFilesFolder;
        // textFile1.txt
        private string _textFileName;
        // CsvFile1.csv
        private string _csvFileName;
        // FileToMove.txt
        private string _fileToMoveName;

        public DotNetFileSystemProviderTest()
        {
            _fileSystem = new DotNetFileSystemProvider();
            _assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);

            _textFileName = "TextFile1.txt";
            _csvFileName = "CsvFile1.csv";
            _fileToMoveName = "FileToMove.txt";

            _localFilesFolder = $"Core{Path.DirectorySeparatorChar}IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files";
            _filesPath = Path.Combine(_assemblyPath, _localFilesFolder);
            _originFilesPath = Path.Combine(_assemblyPath, $"Core{Path.DirectorySeparatorChar}IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}OriginFolder");
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
            Assert.Contains(Path.Combine(_filesPath, _csvFileName), files);
            Assert.Contains(Path.Combine(_filesPath, _textFileName), files);   
        }

        [Fact]
        public void Can_ListDirectoryWithSearchPattern()
        {
            var files = _fileSystem.List(_filesPath, "*.csv");

            Assert.NotNull(files);
            Assert.NotEmpty(files);
            Assert.Contains(Path.Combine(_filesPath, _csvFileName), files);
            Assert.DoesNotContain(Path.Combine(_filesPath, _textFileName), files);
        }

        [Fact]
        public void Can_MoveFileToFolder()
        {
            var srcFile = Path.Combine(_originFilesPath, _fileToMoveName);
            var dstFile = Path.Combine(_filesPath, _fileToMoveName);
            _fileSystem.Move(srcFile, dstFile);

            var srcFolder = Directory.EnumerateFiles(_originFilesPath);
            var dstFolder = Directory.EnumerateFiles(_filesPath);

            Assert.DoesNotContain(srcFile, srcFolder);
            Assert.Contains(dstFile, dstFolder);

            _fileSystem.Move(dstFile, srcFile);

            Assert.Contains(srcFile, srcFolder);
            Assert.DoesNotContain(dstFile, dstFolder);
        }

        [Fact]
        public void Can_CombinePaths()
        {
            Assert.Equal(_fileSystem.CombinePath(_assemblyPath, _localFilesFolder), Path.Combine(_assemblyPath, _localFilesFolder));
        }

        [Fact]
        public void Can_OpenReadAFile()
        {
            var fileContent = "my content";

            var stream = _fileSystem.OpenReadFile(Path.Combine(_filesPath, _textFileName));
            string content = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            Assert.Equal(content, fileContent);

        }

        [Fact]
        public void Can_LoadContentOfAFile()
        {
            var fileContent = "my content";
            var returnedContent = _fileSystem.LoadContent(Path.Combine(_filesPath, _textFileName));
            Assert.Equal(returnedContent, fileContent);
        }

        [Fact]
        public void Can_GetFileName()
        {
            var path = Path.Combine(_filesPath, _textFileName);
            var filename = _fileSystem.GetFileName(path);

            Assert.Equal(filename, _textFileName);
        }

        [Fact]
        public void Can_DetermineIfDirectoryExists()
        {
            Assert.True(_fileSystem.DirectoryExists(_filesPath));
            Assert.False(_fileSystem.DirectoryExists(Path.Combine(_assemblyPath, "FakePath")));
        }

        [Fact]
        public void Can_CreateDirectory()
        {
            var path = Path.Combine(_filesPath, "tmpDir");
            _fileSystem.CreateDirectory(path);

            Assert.True(Directory.Exists(path));
            Directory.Delete(path);
        }

        [Fact]
        public void Can_WriteAllLinesToAFile()
        {
            var text = new List<string>
            {
                "my text",
                "lines"
            };
            var path = Path.Combine(_filesPath, "testFile.txt");
            _fileSystem.WriteAllLines(path, text);
            
            Assert.Equal(text, File.ReadAllLines(path));
            File.Delete(path);
        }
    }
}

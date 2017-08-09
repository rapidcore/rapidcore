using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.IO.FileSystem;
using Xunit;
using System.IO;
using System.Reflection;
using FakeItEasy;

namespace RapidCore.UnitTests.IO.FileSystem
{
    public class SftpFileSystemProviderTest
    {

        private readonly ISftpClient _fakeSftpClient;

        private SftpFileSystemProvider _fileSystem;
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

        public SftpFileSystemProviderTest()
        {
            _assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);
            _textFileName = "TextFile1.txt";
            _csvFileName = "CsvFile1.csv";
            _fileToMoveName = "FileToMove.txt";

            _localFilesFolder = $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files";
            _filesPath = Path.Combine(_assemblyPath, _localFilesFolder);
            _originFilesPath = Path.Combine(_assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}OriginFolder");

            _fakeSftpClient = A.Fake<ISftpClient>();
            A.CallTo(() => _fakeSftpClient.GetWorkingDirectory()).Returns("not empty");
            var fakeStrings = new List<string>{"string1", "string2"};
            _fileSystem = new SftpFileSystemProvider(_fakeSftpClient);
        }

        [Fact]
        public void Can_ShowCWD()
        {
            var cwd = _fileSystem.Cwd();
            Assert.NotNull(cwd);
            Assert.NotEmpty(cwd);
            Assert.Equal("not empty", cwd);
        }

      [Fact]
        public void Can_ListDirectory()
        {
            var files = _fileSystem.List(_filesPath);

            A.CallTo(() => _fakeSftpClient.ListDirectory(_filesPath, null)).MustHaveHappened(Repeated.Exactly.Once);

        }

        [Fact]
        public void Can_ListDirectoryWithSearchPattern()
        {
            var files = _fileSystem.List(_filesPath, @".*\.csv$");

            A.CallTo(() => _fakeSftpClient.ListDirectory(_filesPath, null)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Can_MoveFileToFolder()
        {
            var srcFile = Path.Combine(_originFilesPath, _fileToMoveName);
            var dstFile = Path.Combine(_filesPath, _fileToMoveName);
            _fileSystem.Move(srcFile, dstFile);

            A.CallTo(() => _fakeSftpClient.RenameFile(srcFile, dstFile)).MustHaveHappened(Repeated.Exactly.Once);
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

            A.CallTo(() => _fakeSftpClient.OpenRead(Path.Combine(_filesPath, _textFileName))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Can_LoadContentOfAFile()
        {
            var fileContent = "my content";
            var returnedContent = _fileSystem.LoadContent(Path.Combine(_filesPath, _textFileName));
            A.CallTo(() => _fakeSftpClient.ReadAllText(Path.Combine(_filesPath, _textFileName))).MustHaveHappened(Repeated.Exactly.Once);  
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
            _fileSystem.DirectoryExists(_filesPath);
            A.CallTo(() => _fakeSftpClient.Exists(_filesPath)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void Can_CreateDirectory()
        {
            var path = Path.Combine(_filesPath, "tmpDir");
            _fileSystem.CreateDirectory(path);
            A.CallTo(() => _fakeSftpClient.CreateDirectory(path)).MustHaveHappened(Repeated.Exactly.Once);

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

            A.CallTo(() => _fakeSftpClient.BeginUploadFile(A<Stream>._, path)).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
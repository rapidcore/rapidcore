using System;
using System.Collections.Generic;
using System.Text;
using RapidCore.IO.FileSystem;
using Xunit;
using System.IO;
using System.Reflection;
using FakeItEasy;
using System.Linq;

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
        
        // FileToMove.txt
        private string _fileToMoveName;

        public SftpFileSystemProviderTest()
        {
            _assemblyPath = Path.GetDirectoryName(typeof(DotNetFileSystemProviderTest).GetTypeInfo().Assembly.Location);
            _textFileName = "TextFile1.txt";
            _fileToMoveName = "FileToMove.txt";

            _localFilesFolder = $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}Files";
            _filesPath = Path.Combine(_assemblyPath, _localFilesFolder);
            _originFilesPath = Path.Combine(_assemblyPath, $"IO{Path.DirectorySeparatorChar}FileSystem{Path.DirectorySeparatorChar}OriginFolder");

            _fakeSftpClient = A.Fake<ISftpClient>();
            A.CallTo(() => _fakeSftpClient.GetWorkingDirectory()).Returns("not empty");
            var fakeSftpFile = A.Fake<ISftpFile>();
            fakeSftpFile.Name = "111111";

            var fakeSftpFile2 = A.Fake<ISftpFile>();
            fakeSftpFile2.Name = "111111.csv";

            var fakeSftpFiles = new List<ISftpFile>{fakeSftpFile, fakeSftpFile2};
            A.CallTo(() => _fakeSftpClient.ListDirectory(_filesPath, null)).Returns(fakeSftpFiles);

            A.CallTo(() => _fakeSftpClient.OpenRead(Path.Combine(_filesPath, _textFileName))).Returns(new MemoryStream(Encoding.UTF8.GetBytes("my content")));
            A.CallTo(() => _fakeSftpClient.ReadAllText(Path.Combine(_filesPath, _textFileName))).Returns("my content");
            A.CallTo(() => _fakeSftpClient.Exists(_filesPath)).Returns(true);

            
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
            var files = _fileSystem.List(_filesPath).ToList();

            A.CallTo(() => _fakeSftpClient.ListDirectory(_filesPath, null)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Equal(2, files.Count);
            Assert.Equal("111111", files.First());
            Assert.Equal("111111.csv", files.Last());
        }

        [Fact]
        public void Can_ListDirectoryWithSearchPattern()
        {
            var files = _fileSystem.List(_filesPath, @".*\.csv$").ToList();

            A.CallTo(() => _fakeSftpClient.ListDirectory(_filesPath, null)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Equal(1, files.Count);
            Assert.Equal("111111.csv", files.First());
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
            string content = new StreamReader(stream, Encoding.UTF8).ReadToEnd();
        
            A.CallTo(() => _fakeSftpClient.OpenRead(Path.Combine(_filesPath, _textFileName))).MustHaveHappened(Repeated.Exactly.Once);
            Assert.Equal(content, fileContent);
        }

        [Fact]
        public void Can_LoadContentOfAFile()
        {
            var returnedContent = _fileSystem.LoadContent(Path.Combine(_filesPath, _textFileName));
            A.CallTo(() => _fakeSftpClient.ReadAllText(Path.Combine(_filesPath, _textFileName))).MustHaveHappened(Repeated.Exactly.Once);

            var fileContent = "my content";
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
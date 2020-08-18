using System;
using System.IO;
using System.Threading.Tasks;
using RapidCore.IO;
using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.IO
{
    public class MimeTyperTests
    {
        private readonly MimeTyper _mimeTyper;
        private readonly string _basePath;

        public MimeTyperTests()
        {
            _mimeTyper = new MimeTyper();
            
            _basePath = Path.Combine(
                System.Environment.CurrentDirectory,
                GetType().NamespaceWithoutRoot().Replace('.', Path.DirectorySeparatorChar),
                "FilesForMimeTyperTests"
            );
        }
        
        [Theory]
        [InlineData("empty.zip", "application/zip")]
        [InlineData("squirrel.doc", "application/msword")]
        [InlineData("squirrel.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("squirrel.gif", "image/gif")]
        [InlineData("squirrel.jpg", "image/jpeg")]
        [InlineData("squirrel.odt", "application/vnd.oasis.opendocument.text")]
        [InlineData("squirrel.pdf", "application/pdf")]
        [InlineData("squirrel.png", "image/png")]
        [InlineData("squirrel.zip", "application/zip")]
        [InlineData("unknown.bin", "application/octet-stream")]
        [InlineData("nuts.heic", "image/heic")]
        [InlineData("oneplus5t.jpg", "image/jpeg")]
        [InlineData("samsungs10.jpg", "image/jpeg")]
        [InlineData("samsungs10motion.jpg", "image/jpeg")]
        [InlineData("squirrel.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public async Task GetMimeType(string filename, string expected)
        {
            var fullFileName = Path.Combine(_basePath, filename);
            
            var bytes = await File.ReadAllBytesAsync(fullFileName);

            var actual = _mimeTyper.GetMimeType(bytes, filename);
            
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("nuts.heic", "image/heic")]
        [InlineData("empty.zip", "application/zip")]
        [InlineData("squirrel.doc", "application/msword")]
        [InlineData("squirrel.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("squirrel.gif", "image/gif")]
        [InlineData("squirrel.jpg", "image/jpeg")]
        [InlineData("squirrel.odt", "application/vnd.oasis.opendocument.text")]
        [InlineData("squirrel.pdf", "application/pdf")]
        [InlineData("squirrel.png", "image/png")]
        [InlineData("squirrel.zip", "application/zip")]
        [InlineData("unknown.bin", "application/octet-stream")]
        [InlineData("oneplus5t.jpg", "image/jpeg")]
        [InlineData("samsungs10.jpg", "image/jpeg")]
        [InlineData("samsungs10motion.jpg", "image/jpeg")]
        [InlineData("squirrel.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public async Task GetMimeTypeFromBase64(string filename, string expected)
        {
            var fullFileName = Path.Combine(_basePath, filename);
            
            var bytes = await File.ReadAllBytesAsync(fullFileName);

            var base64 = Convert.ToBase64String(bytes);

            var actual = _mimeTyper.GetMimeTypeFromBase64(base64, filename);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        [InlineData("empty.zip", "application/zip")]
        [InlineData("squirrel.doc", "application/msword")]
        [InlineData("squirrel.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        [InlineData("squirrel.gif", "image/gif")]
        [InlineData("squirrel.jpg", "image/jpeg")]
        [InlineData("squirrel.odt", "application/vnd.oasis.opendocument.text")]
        [InlineData("squirrel.pdf", "application/pdf")]
        [InlineData("squirrel.png", "image/png")]
        [InlineData("squirrel.zip", "application/zip")]
        [InlineData("unknown.bin", "application/octet-stream")]
        [InlineData("nuts.heic", "image/heic")]
        [InlineData("oneplus5t.jpg", "image/jpeg")]
        [InlineData("samsungs10.jpg", "image/jpeg")]
        [InlineData("samsungs10motion.jpg", "image/jpeg")]
        [InlineData("squirrel.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public void GetMimeTypeFromFilename(string filename, string expected)
        {
            var actual = _mimeTyper.GetMimeTypeFromFilename(filename);
            
            Assert.Equal(expected, actual);
        }
        
        [Theory]
        // valid
        [InlineData("squirrel.gif", true)]
        [InlineData("squirrel.jpg", true)]
        [InlineData("squirrel.png", true)]
        [InlineData("nuts.heic", true)]
        [InlineData("oneplus5t.jpg", true)]
        [InlineData("samsungs10.jpg", true)]
        [InlineData("samsungs10motion.jpg", true)]
        // invalid
        [InlineData("empty.zip", false)]
        [InlineData("squirrel.doc", false)]
        [InlineData("squirrel.docx", false)]
        [InlineData("squirrel.odt", false)]
        [InlineData("squirrel.pdf", false)]
        [InlineData("squirrel.zip", false)]
        [InlineData("unknown.bin", false)]
        [InlineData("squirrel.xlsx", false)]
        public async Task IsMimeTypeOneOfTheseFromBase64(string filename, bool expected)
        {
            var fullFileName = Path.Combine(_basePath, filename);
            
            var bytes = await File.ReadAllBytesAsync(fullFileName);

            var base64 = Convert.ToBase64String(bytes);
            
            var allowedMimeTypes = new []
            {
                "image/gif",
                "image/jpeg",
                "image/png",
                "image/heic"
            };

            var actual = _mimeTyper.IsMimeTypeOneOfTheseFromBase64(base64, filename, allowedMimeTypes);
            
            Assert.Equal(expected, actual);
        }
    }
}
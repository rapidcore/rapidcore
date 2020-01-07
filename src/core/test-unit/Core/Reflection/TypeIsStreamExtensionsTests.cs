using System;
using System.IO;
using RapidCore.Reflection;
using Xunit;

namespace UnitTests.Core.Reflection
{
    public class TypeIsStreamExtensionsTests
    {
        [Theory]
        // yes
        [InlineData(typeof(Stream), true)]
        [InlineData(typeof(MemoryStream), true)]
        [InlineData(typeof(FileStream), true)]
        [InlineData(typeof(StreamVictim), true)]
        [InlineData(typeof(CustomStreamVictim), true)]
        // no
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int), false)]
        [InlineData(typeof(TypeIsStreamExtensionsTests), false)]
        public void IsStream(Type type, bool expected)
        {
            Assert.Equal(expected, type.IsStream());
        }

        #region Victims
        public class StreamVictim : Stream
        {
            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => throw new NotImplementedException();

            public override bool CanSeek => throw new NotImplementedException();

            public override bool CanWrite => throw new NotImplementedException();

            public override long Length => throw new NotImplementedException();

            public override long Position { get; set; }
        }
        
        public class CustomStreamVictim : StreamVictim
        {
            
        }
        #endregion
    }
}
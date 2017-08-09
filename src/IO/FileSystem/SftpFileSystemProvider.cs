using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Renci.SshNet;

namespace RapidCore.IO.FileSystem
{
    public class SftpFileSystemProvider : IFileSystemProvider, IDisposable
    {
        private ISftpClient _sftpClient;

        public SftpFileSystemProvider(ISftpClient sftpClient)
        {
            _sftpClient = sftpClient;
        }

        /// <summary>
        /// List the files of the given remote path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <param name="searchPattern">
        /// optional search pattern filtering
        /// </param>
        /// <returns>
        /// the filenames
        /// </returns>
        public IEnumerable<string> List(string path, string searchPattern)
        {
            var regex = new Regex(searchPattern);
            return List(path)?.Where(file => regex.IsMatch(file));
        }

        /// <summary>
        /// List the files of the given remote path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// the filenames
        /// </returns>
        public IEnumerable<string> List(string path)
        {
            var response = new List<string>();
            var sftpFiles = _sftpClient.ListDirectory(path);
            foreach (var f in sftpFiles)
            {
                response.Add(f.Name);
            }

            return response;
        }

        /// <summary>
        /// Gets the current working directory of the remote sftp client
        /// </summary>
        /// <returns>
        /// the current working directory
        /// </returns>
        public string Cwd()
        {
            return _sftpClient.GetWorkingDirectory();
        }

        /// <summary>
        /// Moves a remote file
        /// </summary>
        /// <param name="sourceFile">
        /// The source file
        /// </param>
        /// <param name="destFile">
        /// The destination file
        /// </param>
        public void Move(string sourceFile, string destFile)
        {
            _sftpClient.RenameFile(sourceFile, destFile);
        }

        /// <summary>
        /// Writes lines to a remote file
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <param name="contents">
        /// the lines to write
        /// </param>
        public void WriteAllLines(string path, IEnumerable<string> contents)
        {
            using (Stream s = GenerateStreamFromStrings(contents))
            {
                _sftpClient.BeginUploadFile(s, path);
            }
        }

        /// <summary>
        /// Combine two string into a path.
        /// </summary>
        /// <param name="path1">
        /// the first path
        /// </param>
        /// <param name="path2">
        /// the second path
        /// </param>
        /// <returns>
        /// the combined path
        /// </returns>
        public string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Opens an existing remote sftp file for reading
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// Stream of the file
        /// </returns>
        public Stream OpenReadFile(string path)
        {
            return _sftpClient.OpenRead(path);
        }

        /// <summary>
        /// Load content from the given file
        /// </summary>
        /// <param name="filePath">
        /// the file path
        /// </param>
        /// <returns>
        /// the file content
        /// </returns>
        public string LoadContent(string filePath)
        {
            return _sftpClient.ReadAllText(filePath);
        }

        /// <summary>
        /// Get the file name and extension of the specified path string
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// the filename
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public bool DirectoryExists(string path)
        {
            return _sftpClient.Exists(path);
        }

        /// <summary>
        /// Determines whether the given path is
        ///  an existing remote directory
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        public void CreateDirectory(string path)
        {
            _sftpClient.CreateDirectory(path);
        }

        public void Dispose()
        {
            _sftpClient?.Dispose();
        }

        /// <summary>
        /// Generates a Stream from list of strings
        /// </summary>
        /// <param name="contents">
        /// list of strings
        /// </param>
        /// <returns>
        /// the streamn
        /// </returns>
        private Stream GenerateStreamFromStrings(IEnumerable<string> contents)
        {
            var s = string.Join(string.Empty, contents);

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
  }
}

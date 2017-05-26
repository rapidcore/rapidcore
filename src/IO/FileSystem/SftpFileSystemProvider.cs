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
        private string _host;
        private string _username;
        private string _password;

        private SftpClient _sftpClient;

        public SftpFileSystemProvider(string host, string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
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
            return List(path).Where(file => regex.IsMatch(file));
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
            var sftpFiles = GetSftpClient().ListDirectory(path);
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
            return GetSftpClient().WorkingDirectory;
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
            GetSftpClient().RenameFile(sourceFile, destFile);
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
                GetSftpClient().BeginUploadFile(s, path);
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
            return Path.Combine(path1, path2).Replace("\\", "/");
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
            return GetSftpClient().OpenRead(path);
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
            return GetSftpClient().ReadAllText(filePath);
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
            return GetSftpClient().Exists(path);
        }

        /// <summary>
        /// Determines whether the given path is
        ///  an existing remote directory
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        public FileSystemInfo CreateDirectory(string path)
        {
            GetSftpClient().CreateDirectory(path);
            return new FileInfo(path);
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

        /// <summary>
        /// authenticates and returns an sftp client connection
        /// via ssh.NET
        /// </summary>
        /// <returns>
        /// the sftp client
        /// </returns>
        private SftpClient GetSftpClient()
        {
            if (_sftpClient == null || !_sftpClient.IsConnected)
            {
                var connectionInfo = new ConnectionInfo(
                    _host,
                    _username,
                    new PasswordAuthenticationMethod(_username, _password));

                _sftpClient = new SftpClient(connectionInfo);
                _sftpClient.Connect();
            }

            return _sftpClient;
        }
    }
}

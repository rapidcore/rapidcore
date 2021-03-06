using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;

namespace RapidCore.IO.FileSystem
{
  public class SftpClient : ISftpClient
  {
    private Renci.SshNet.SftpClient _client;
    private readonly string _host;
    private readonly string _username;
    private readonly string _password;
    private bool disposed = false;

    public SftpClient(string host, string username, string password)
    {
      _host = host;
      _username = username;
      _password = password;
    }

    public string GetWorkingDirectory()
    {
      return GetSftpClient().WorkingDirectory;
    }

    public IAsyncResult BeginUploadFile(Stream input, string path)
    {
      return GetSftpClient().BeginUploadFile(input, path);
    }

    public void CreateDirectory(string path)
    {
      GetSftpClient().CreateDirectory(path);
    }

    protected virtual void Dispose(bool disposing)
    {
      // Do not dispose this object multiple times
      if (this.disposed)
      {
        return;
      }
      
      // If the method has been called by user code then it is safe to access objects
      if (disposing)
      {
        _client?.Dispose();
      }
        
      // Mark this object as disposed (so it does not happen twice)
      this.disposed = true;
    }
    
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public bool Exists(string path)
    {
      return GetSftpClient().Exists(path);
    }

    public IEnumerable<ISftpFile> ListDirectory(string path, Action<int> listCallback = null)
    {
      var realSftpFiles = GetSftpClient().ListDirectory(path);
      var result = new List<ISftpFile>();

      foreach(var file in realSftpFiles)
      {
        result.Add((ISftpFile) file);
      }
      return result;
    }

    public Stream OpenRead(string path)
    {
      return GetSftpClient().OpenRead(path);
    }

    public string ReadAllText(string path)
    {
      return GetSftpClient().ReadAllText(path);
    }

    public void RenameFile(string oldPath, string newPath)
    {
      GetSftpClient().RenameFile(oldPath, newPath);
    }

    /// <summary>
    /// authenticates and returns an sftp client connection
    /// via ssh.NET
    /// </summary>
    /// <returns>
    /// the sftp client
    /// </returns>
    private Renci.SshNet.SftpClient GetSftpClient()
    {
      if (_client == null || !_client.IsConnected)
      {
        var connectionInfo = new ConnectionInfo(
            _host,
            _username,
            new PasswordAuthenticationMethod(_username, _password));

        _client = new Renci.SshNet.SftpClient(connectionInfo);
        _client.Connect();
      }

      return _client;
    }
  }
}
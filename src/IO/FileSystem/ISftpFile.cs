using System;
using Renci.SshNet.Sftp;

public interface ISftpFile
    {
        SftpFileAttributes Attributes { get; }
        string FullName { get; }
        bool GroupCanExecute { get; set; }
        bool GroupCanRead { get; set; }
        bool GroupCanWrite { get; set; }
        int GroupId { get; set; }
        bool IsBlockDevice { get; }
        bool IsCharacterDevice { get; }
        bool IsDirectory { get; }
        bool IsNamedPipe { get; }
        bool IsRegularFile { get; }
        bool IsSocket { get; }
        bool IsSymbolicLink { get; }
        DateTime LastAccessTime { get; set; }
        DateTime LastAccessTimeUtc { get; set; }
        DateTime LastWriteTime { get; set; }
        DateTime LastWriteTimeUtc { get; set; }
        long Length { get; }
        string Name { get; set;}
        bool OthersCanExecute { get; set; }
        bool OthersCanRead { get; set; }
        bool OthersCanWrite { get; set; }
        bool OwnerCanExecute { get; set; }
        bool OwnerCanRead { get; set; }
        bool OwnerCanWrite { get; set; }
        int UserId { get; set; }

        void Delete();
        void MoveTo(string destFileName);
        void SetPermissions(short mode);
        string ToString();
        void UpdateStatus();
    }
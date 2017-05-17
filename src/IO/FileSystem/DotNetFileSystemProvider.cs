using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RapidCore.IO.FileSystem
{
    public class DotNetFileSystemProvider : IFileSystemProvider
    {
        public FileStream OpenReadFile(string path)
        {
            return File.OpenRead(path);
        }

        Stream IFileSystemProvider.OpenReadFile(string path)
        {
            return OpenReadFile(path);
        }

        /// <summary>
        /// Determines whether the given path is actually a csproj file
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// whether it is a cs.proj file
        /// </returns>
        public bool IsCsProjectFile(string path)
        {
            return File.Exists(path) && path.EndsWith(".csproj");
        }

        /// <summary>
        /// List the files of the given path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <param name="extension">
        /// optional file extension filtering
        /// </param>
        /// <returns>
        /// the filenames
        /// </returns>
        public IEnumerable<string> List(string path, string extension)
        {
            return Directory.EnumerateFiles(path, extension);
        }

        /// <summary>
        /// List the files of the given path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// the filenames
        /// </returns>
        public IEnumerable<string> List(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        /// <summary>
        /// Gets the current working directory of the running application
        /// </summary>
        /// <returns>
        /// the current working directory
        /// </returns>
        public string Cwd()
        {
            return Directory.GetCurrentDirectory();
        }

        public void Move(string sourceFile, string destFile)
        {
            File.Move(sourceFile, destFile);
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
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Get the file name and extension of the specified path string
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// the filename and extension
        /// </returns>
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// Determines whether the given path is
        ///  an existing directory
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// whether the directory exists or not
        /// </returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Creates a directory
        /// </summary>
        /// <param name="path">
        /// the path to create the directory in
        /// </param>
        /// <returns>
        /// the directory info
        /// </returns>
        public FileSystemInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}

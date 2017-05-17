using System.Collections.Generic;
using System.IO;

namespace RapidCore.IO.FileSystem
{
    public interface IFileSystemProvider
    {
        /// <summary>
        /// List the items in a given path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <param name="extension">
        /// the files extension
        /// </param>
        /// <returns>
        /// the items in the path
        /// </returns>
        IEnumerable<string> List(string path, string extension = "*.*");

        /// <summary>
        /// Get the current working directory
        /// </summary>
        /// <returns>
        /// The current working directory
        /// </returns>
        string Cwd();

        /// <summary>
        /// Moves a file
        /// </summary>
        /// <param name="sourceFile">
        /// The source file
        /// </param>
        /// <param name="destFile">
        /// The destination file
        /// </param>
        void Move(string sourceFile, string destFile);

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
        string CombinePath(string path1, string path2);

        /// <summary>
        /// Opens an existing file for reading
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// FileStream of the file
        /// </returns>
        FileStream OpenReadFile(string path);

        /// <summary>
        /// Determines whether the given path is actually a csproj file or a path
        /// </summary>
        /// <param name="path">
        /// the path
        /// </param>
        /// <returns>
        /// whether it is a cs.proj file
        /// </returns>
        bool IsCsProjectFile(string path);

        /// <summary>
        /// Loads all the content from the given file path as a string
        /// </summary>
        /// <param name="filePath">
        /// the file path
        /// </param>
        /// <returns>
        /// the content
        /// </returns>
        string LoadContent(string filePath);
    }
}

---
title: FileSystemProvider
tags: [getting_started]
keywords:
summary: "RapidCore FileSystemProvider is here."
sidebar: rapidcore_sidebar
permalink: rapidcore_filesystemprovider.html
folder: mongo
---
Register your preferred FileSystemProvider in your container:
```
                x.For<IFileSystemProvider>().Use(new DotNetFileSystemProvider());

```

## Available implementations of IFileSystemProvider

The following implementations of IFileSystemProvider are available in `RapidCore.IO.FileSystem`.

### DotNetFileSystemProvider

For proxy usage of the `System.IO` from .NET Standard.

### SftpFileSystemProvider

For usage of a file system available through SFTP.

## Methods

### IEnumerable<string> List(string path, string searchPattern)

Summary: List the items in a given path

Parameters: 

    - path: the path
    - searchPattern: a search pattern, i.e. file extensions: "*.csv"

Returns: The files in the path

### IEnumerable<string> List(string path)

Summary: List the items in a given path

Parameters: 

    - path: the path

Returns: The files in the path

### string Cwd()

Summary: Get the current working directory

Parameters: 

Returns: The current working directory

### void Move(string sourceFile, string destFile)

Summary: Moves a file

Parameters: 

    - sourceFile: The source file
    - destFile: The destination file

Returns:

### string CombinePath(string path1, string path2);

Summary: Combines two string into a path.

Parameters: 

    - path1: the first path
    - path2: the second path

Returns:

### Stream OpenReadFile(string path)

Summary: Opens an existing file for reading

Parameters: 

    - path: the path

Returns: Stream of the file

### string LoadContent(string filePath);

Summary: Loads all the content from the given file path as a string

Parameters: 

    - path: the file path

Returns: the content

### string GetFileName(string path);

Summary: Get the file name and extension of the specified path string

Parameters: 

    - path: the path

Returns: The filename and extension

### bool DirectoryExists(string path);

Summary: Determines whether the given path is an existing directory

Parameters: 

    - path: the path

Returns: true if the directory exists, false otherwise.

### FileSystemInfo CreateDirectory(string path);

Summary: Creates a directory

Parameters: 

    - path: the path to create the directory in

Returns: The directory info
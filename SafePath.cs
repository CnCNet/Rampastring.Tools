using System.Linq;
using System.IO;
using System;
using System.Runtime.InteropServices;

namespace Rampastring.Tools;

/// <summary>
/// Provides safe cross platform Path handling.
/// </summary>
public static class SafePath
{
    /// <summary>
    /// Safely combines multiple directory paths for all platforms.
    /// The first path can be a relative or absolute path.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths.</param>
    /// <returns>The combined directory path of all <paramref name="paths"/>, with trailing separator character and with leading separator character if the path has a root.</returns>
    public static string CombineDirectoryPath(params string[] paths)
    {
        return Combine(true, paths);
    }

    /// <summary>
    /// Safely combines multiple directory paths with a file name for all platforms.
    /// The first path can be a relative or absolute path.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths with a file name.</param>
    /// <returns>The combined directory path and file name of all <paramref name="paths"/> with leading separator character if the path has a root.</returns>
    public static string CombineFilePath(params string[] paths)
    {
        return Combine(false, paths);
    }

    /// <summary>
    /// Safely combines multiple directory paths for all platforms.
    /// The first path can be a relative or absolute path.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths.</param>
    /// <returns>A <see cref="DirectoryInfo"/> instance representing the combined directory path of all <paramref name="paths"/>.</returns>
    public static DirectoryInfo GetDirectory(params string[] paths)
    {
        return new DirectoryInfo(CombineDirectoryPath(paths));
    }

    /// <summary>
    /// Safely combines multiple directory paths with a file name for all platforms.
    /// The first path can be a relative or absolute path.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths with a file name.</param>
    /// <returns>A <see cref="FileInfo"/> instance representing the combined directory path and file name of all <paramref name="paths"/>.</returns>
    public static FileInfo GetFile(params string[] paths)
    {
        return new FileInfo(CombineFilePath(paths));
    }

    /// <summary>
    /// Safely delete a file represented by multiple directory paths with a file name for all platforms.
    /// Does not throw an exception if the file or directory path does not exist.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths with a file name.</param>
    public static void DeleteFileIfExists(params string[] paths)
    {
        var fileInfo = new FileInfo(CombineFilePath(paths));

        if (fileInfo.Exists)
            fileInfo.Delete();
    }

    /// <summary>
    /// Safely delete a directory represented by multiple directory paths for all platforms.
    /// Does not throw an exception if the directory path does not exist.
    /// </summary>
    /// <param name="recursive">true to remove directories, subdirectories, and files in path; otherwise, false.</param>
    /// <param name="paths">Ordered list of directory paths.</param>
    public static void DeleteDirectoryIfExists(bool recursive, params string[] paths)
    {
        var directoryInfo = new DirectoryInfo(CombineFilePath(paths));

        if (directoryInfo.Exists)
            directoryInfo.Delete(recursive);
    }

    /// <summary>
    /// Safely retrieves the name of a directory represented by multiple directory paths and a file name for all platforms.
    /// </summary>
    /// <param name="paths">Ordered list of directory paths and a file name.</param>
    /// <returns>The name (not path) of the directory a given file resides in.</returns>
    public static string GetFileDirectoryName(params string[] paths)
    {
        var fileInfo = new FileInfo(CombineFilePath(paths));

        return fileInfo.DirectoryName;
    }

    private static string Combine(bool isDirectoryPath, params string[] paths)
    {
        if (paths is null)
            return null;
    
        string[] normalizedPaths = paths
            .Where(path => !string.IsNullOrWhiteSpace(path))
            .Select(GetPath)
            .ToArray();
    
        if (normalizedPaths.Length == 0)
            return null;
    
        string combinedPath = Path.Combine(normalizedPaths);
        string root = Path.GetPathRoot(combinedPath) ?? string.Empty;
    
        string result = combinedPath.TrimEnd(
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar);
    
        if (result.Length < root.Length)
            result = root;
    
        if (isDirectoryPath &&
            !result.EndsWith(
                Path.DirectorySeparatorChar.ToString(),
                StringComparison.Ordinal))
        {
            result += Path.DirectorySeparatorChar;
        }
    
        return result;
    }

    private static string GetPath(string path)
    {
        return FormattableString.Invariant($"{path?
            .Replace("///", "/")
            .Replace("//", "/")
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace("\\\\\\", "\\")
            .Replace("\\\\", "\\")
            .Replace('\\', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar)
            .TrimStart(Path.AltDirectorySeparatorChar)
            .TrimEnd(Path.DirectorySeparatorChar)
            .TrimEnd(Path.AltDirectorySeparatorChar)}{Path.DirectorySeparatorChar}");
    }
}

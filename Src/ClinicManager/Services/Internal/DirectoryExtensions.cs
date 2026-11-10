using System;
using System.IO;

public static class DirectoryExtensions
{
    /// <summary>
    /// Ensures that every directory in the supplied path exists, creating any that are missing.
    /// </summary>
    /// <param name="path">The directory path to create.</param>
    /// <returns>A <see cref="DirectoryInfo"/> for the full directory path.</returns>
    /// <exception cref="ArgumentNullException">Path is null.</exception>
    /// <exception cref="ArgumentException">Path is empty, whitespace, or contains invalid characters.</exception>
    /// <exception cref="PathTooLongException">Path exceeds the system-defined maximum length.</exception>
    /// <exception cref="NotSupportedException">Path contains a colon in an invalid position.</exception>
    /// <exception cref="IOException">A file already exists at the path, or the path cannot be created.</exception>
    /// <exception cref="UnauthorizedAccessException">The caller lacks permission to create the directory.</exception>
    public static DirectoryInfo EnsureDirectory(this string path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty or whitespace.", nameof(path));

        if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException($"Path contains invalid characters: '{path}'.", nameof(path));

        // Throws on malformed paths (e.g. misplaced colons, too long).
        string fullPath = Path.GetFullPath(path);

        // Already exists: nothing to do, no exception.
        if (Directory.Exists(fullPath))
            return new DirectoryInfo(fullPath);

        if (File.Exists(fullPath))
            throw new IOException($"A file already exists at '{fullPath}'.");

        // Creates all missing directories in the path; no-op for ones that already exist.
        return Directory.CreateDirectory(fullPath);
    }
}

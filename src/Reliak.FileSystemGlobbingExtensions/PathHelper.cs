using System;
using System.IO;

namespace Reliak.FileSystemGlobbingExtensions
{
    public static class PathHelper
    {
        // From http://stackoverflow.com/a/703292
        public static string GetRelativePath(string filename, string baseDirectory)
        {
            Uri pathUri = new Uri(filename);

            if (!baseDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                baseDirectory += Path.DirectorySeparatorChar;
            }

            Uri folderUri = new Uri(baseDirectory);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static string EnsureValidExtension(string extension)
        {
            if( string.IsNullOrWhiteSpace(extension) )
            {
                return "";
            }

            extension = extension.Trim();

            return extension.StartsWith(".") ? extension : $".{extension}";
        }
    }
}
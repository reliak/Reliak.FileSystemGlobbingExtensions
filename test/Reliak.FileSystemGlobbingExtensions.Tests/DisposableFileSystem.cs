using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reliak.FileSystemGlobbingExtensions.Tests
{
    public class DisposableFileSystem : IDisposable
    {
        private readonly string _tempRootPath;

        public DisposableFileSystem()
        {
            _tempRootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        }

        public string RootDirectory => _tempRootPath;

        public DisposableFileSystem CreateFile(string relativeFilePath, string content = "")
        {
            var fullPath = Path.GetFullPath(Path.Combine(_tempRootPath, relativeFilePath));

            CreateDirectoryIfNotExist(Path.GetDirectoryName(fullPath));

            File.WriteAllText(fullPath, content);
            return this;
        }

        private void CreateDirectoryIfNotExist(string fullDirectoryPath)
        {
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }
        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(_tempRootPath, true);
            }
            catch 
            { } // ignore
        }
    }
}
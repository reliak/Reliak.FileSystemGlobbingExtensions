﻿using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Reliak.FileSystemGlobbingExtensions.Tests
{
    public class GlobMatcherTestFixture : IDisposable
    {
        private readonly DisposableFileSystem _filesystem;

        public GlobMatcherTestFixture()
        {
            _filesystem = new DisposableFileSystem();

            _filesystem.CreateFile("a.txt");
            _filesystem.CreateFile("dummy.csv");
            _filesystem.CreateFile("a.csv");
            _filesystem.CreateFile(@"sub\x.csv");
            _filesystem.CreateFile(@"sub\b.cs");
            _filesystem.CreateFile(@"sub\test.xml");
            _filesystem.CreateFile(@"sub\foo.cs");
            _filesystem.CreateFile(@"sub\{someguid}.cs");
            _filesystem.CreateFile(@"sub\sub2\bar.csv");
            _filesystem.CreateFile(@"sub\sub2\xs.exe");
        }

        public DisposableFileSystem FileSystem => _filesystem;

        public void Dispose()
        {
            _filesystem.Dispose();
        }
    }

    public class GlobMatcherTest : IClassFixture<GlobMatcherTestFixture>
    {
        private readonly string _baseDirectory;

        public GlobMatcherTest(GlobMatcherTestFixture fixture)
        {
            _baseDirectory = fixture.FileSystem.RootDirectory;
        }

        [Fact]
        public void Test_Expansion_And_Exclusion()
        {
            // Act
            var matches = GlobMatcher.FindMatches(_baseDirectory, @"**/*.{csv,xml}", "!**/{a,bar}.csv").ToArray();

            // Assert
            Assert.Equal(3, matches.Length);
            Assert.True(matches.Any(f => f.Path == "dummy.csv" && f.Stem == "dummy.csv"));
            Assert.True(matches.Any(f => f.Path == "sub/x.csv" && f.Stem == "sub/x.csv"));
            Assert.True(matches.Any(f => f.Path == "sub/test.xml" && f.Stem == "sub/test.xml"));
        }

        [Fact]
        public void Test_Expansion_Escape()
        {
            // Act
            var matches = GlobMatcher.FindMatches(_baseDirectory, @"sub/\{someguid\}.cs").ToArray();

            // Assert
            Assert.Equal(1, matches.Length);
            Assert.True(matches.Any(f => f.Path == "sub/{someguid}.cs" && f.Stem == "{someguid}.cs"));
        }

        [Fact]
        public void Test_AbsolutePath()
        {
            // Arrange
            var absoluteFilepath = Path.Combine(_baseDirectory, @"sub\**\*.exe");

            // Act
            var matches = GlobMatcher.FindMatches(_baseDirectory, absoluteFilepath).ToArray();

            // Assert
            Assert.Equal(1, matches.Length);
            Assert.True(matches.Any(f => f.Path == "sub/sub2/xs.exe" && f.Stem == "sub2/xs.exe"));
        }
    }
}
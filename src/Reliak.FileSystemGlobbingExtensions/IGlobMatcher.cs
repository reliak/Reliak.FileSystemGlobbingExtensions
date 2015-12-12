using Microsoft.Extensions.FileSystemGlobbing;
using System.Collections.Generic;

namespace Reliak.FileSystemGlobbingExtensions
{
    public interface IGlobMatcher
    {
        IEnumerable<FilePatternMatch> FindMatches(string baseDirectory, params string[] globPatterns);
    }
}
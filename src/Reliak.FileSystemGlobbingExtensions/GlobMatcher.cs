using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Minimatch.DNX;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reliak.FileSystemGlobbingExtensions
{
    public class GlobMatcher
    {
        private const string GLOB_EXCLUDE_IDENTIFIER = "!";

        public static IEnumerable<FilePatternMatch> FindMatches(string baseDirectory, params string[] globPatterns)
        {
            globPatterns = ExpandGlobPatterns(globPatterns).ToArray();

            var matcher = new Matcher();

            foreach(var pattern in globPatterns)
            {
                var isExclude = pattern.StartsWith(GLOB_EXCLUDE_IDENTIFIER);
                var finalPattern = isExclude ? pattern.Substring(1) : pattern;

                // support absolute paths and convert them to relative pathes
                finalPattern = Path.IsPathRooted(finalPattern) ? PathHelper.GetRelativePath(finalPattern, baseDirectory) : finalPattern;

                if (isExclude)
                {
                    matcher.AddExclude(finalPattern);
                }
                else
                {
                    matcher.AddInclude(finalPattern);
                }
            }
            
            return matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(baseDirectory))).Files;
        }
        
        private static IEnumerable<string> ExpandGlobPatterns(params string[] globPatterns)
        {
            return globPatterns.SelectMany(f => Minimatcher.BraceExpand(f, new Options()))
                               .Select(f => f.Replace("\\{", "{").Replace("\\}", "}")); // normalize escaped braces
        }
    }
}
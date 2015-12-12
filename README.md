# Reliak.FileSystemGlobbingExtensions
Extends Microsoft.Extensions.FileSystemGlobbing with the functionality to expand braces (e.g. `*.{jpg,png}`) and to allow glob excludes to be defined in the glob pattern itself (e.g. `!**/*.exe` to exclude .exe files)

### Installation / NuGet package
Install via NuGet Packagemanager:
```
PM> Install-Package Reliak.FileSystemGlobbingExtensions
```

### Exemplary usage
```
var globMatcher = new DefaultGlobMatcher();
// include all .jpg and .png files, except for files with the name "404.png"
var matches = globMatcher.FindMatches(@"c:\someDirectory", "**/*.{jpg,png}", "!**/404.png");
// do something with matches
```

### License
https://opensource.org/licenses/MIT
using System;
using System.IO;

namespace DotNpm.Internals {

    internal static class StringExtensions {

        public static DirectoryInfo ToAbsoluteDirectory(this string directory) => Path.IsPathRooted(directory) ? new DirectoryInfo(directory) : new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, directory));
    }
}
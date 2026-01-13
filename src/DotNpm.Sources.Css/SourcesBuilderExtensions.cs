using System;

namespace DotNpm {

    public static class SourcesBuilderExtensions {

        public static SourcesBuilder AddCssFile(this SourcesBuilder b, string fileName, Action<CssSourceFileBuilder> builder, out BuiltInCssSourceFile file) => b.AddFile(fileName, builder, out file);
    }
}
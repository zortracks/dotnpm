using System;

namespace DotNpm {

    public static class SourcesBuilderExtensions {

        public static SourcesBuilder AddCssFile(this SourcesBuilder b, string fileName, Action<CssSourceFileBuilder> builder, out ISourceFileReference<BuiltInCssSourceFile> sourceFileReference) {
            var cssSourceFileBuilder = new CssSourceFileBuilder(b._services, b._baseDirectory, fileName);

            builder.Invoke(cssSourceFileBuilder);
            b.Files.Add(serviceProvider => cssSourceFileBuilder.GetSourceFile(serviceProvider));
            sourceFileReference = cssSourceFileBuilder.GetSourceFileReference();

            return b;
        }
    }
}
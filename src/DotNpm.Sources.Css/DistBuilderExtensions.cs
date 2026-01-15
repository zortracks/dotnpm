using System;

namespace DotNpm {

    public static class DistBuilderExtensions {

        public static DistBuilder AddCssFile(this DistBuilder b, string fileName, Action<CssOutputFileBuilder> builder, out OutputFileReference<CssOutputFile> outputFileReference) {
            var cssOutputFileBuilder = new CssOutputFileBuilder(fileName, b._baseDirectory);

            builder.Invoke(cssOutputFileBuilder);
            b.FilesFactory.Add(serviceProvider => cssOutputFileBuilder.GetOutputFile(serviceProvider));
            outputFileReference = cssOutputFileBuilder.GetOutputFileReference();

            return b;
        }
    }
}
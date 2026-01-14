using System;

namespace DotNpm {

    public static class DistBuilderExtensions {

        public static DistBuilder AddCssFile(this DistBuilder b, string fileName, Action<CssOutputFileBuilder> builder, out IOutputFileReference<CssOutputFile> outputFileReference) {
            var cssOutputFileBuilder = new CssOutputFileBuilder(fileName);

            builder.Invoke(cssOutputFileBuilder);
            b.FilesFactory.Add(serviceProvider => cssOutputFileBuilder.GetOutputFile(serviceProvider));
            outputFileReference = cssOutputFileBuilder.GetOutputFileReference();

            return b;
        }
    }
}
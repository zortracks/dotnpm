namespace DotNpm.Services {

    public sealed class PackageService {
        //public Task PrepareAsync(DirectoryInfo baseDirectory, CancellationToken cancellationToken) {
        //    _logger.LogDebug("Preparing package '{0}'", Name);

        //    return Task.Run(async () => {
        //        await WritePackage(baseDirectory);
        //        await InstallAsync(baseDirectory, Name, cancellationToken);
        //    }).ContinueWith(task => {
        //        if (task.Status == TaskStatus.RanToCompletion)
        //            _logger.LogDebug("Package '{0}' prepared successfully.", Name);
        //        else
        //            _logger.LogError(task.Exception, "Package '{0}' preparation failed.", Name);
        //    });
        //}

        //private Task WritePackage(DirectoryInfo baseDirectory) {
        //    _logger.LogDebug("Writing package '{0}'", Name);

        //    if (!baseDirectory.Exists)
        //        baseDirectory.Create();

        //    var options = new JsonSerializerOptions() {
        //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        //    };

        //    options.Converters.Add(new Dependency.DependencyConverter());
        //    options.Converters.Add(new BuiltInScript.BuiltInScriptConverter());

        //    return File.WriteAllTextAsync(Path.Combine(baseDirectory.FullName, "package.json"), JsonSerializer.Serialize(this, options));
        //}
    }
}
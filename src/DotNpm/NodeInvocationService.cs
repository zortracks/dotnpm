using CliWrap;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNpm {

    public sealed class NodeInvocationService {
        private readonly ILogger<NodeInvocationService> _logger;

        public NodeInvocationService(ILogger<NodeInvocationService> logger) {
            _logger = logger;
        }

        public NodeInvocationContext CreateContext(DirectoryInfo directory) => new NodeInvocationContext(_logger, directory);

        public sealed class NodeInvocationContext {
            private readonly DirectoryInfo _directory;
            private readonly ILogger<NodeInvocationService> _logger;
            private readonly ConcurrentQueue<Tuple<LogLevel, string>> _logs = new();

            public NodeInvocationContext(ILogger<NodeInvocationService> logger, DirectoryInfo directory) {
                this._logger = logger;
                _directory = directory;
            }

            public Task InstallAsync() => Invoke(command => command.WithArguments("install"));

            public Task RunAsync(string scriptName) {
                throw new NotImplementedException();
            }

            private Task Invoke(Func<Command, Command> command) {
                return command.Invoke(Cli.Wrap("npm"))
                    .WithWorkingDirectory(_directory.FullName)
                    .WithValidation(CommandResultValidation.ZeroExitCode)
                    .WithStandardOutputPipe(PipeTarget.ToDelegate(line => _logs.Enqueue(Tuple.Create(LogLevel.Information, line))))
                    .WithStandardErrorPipe(PipeTarget.ToDelegate(line => _logs.Enqueue(Tuple.Create(LogLevel.Error, line))))
                    .ExecuteAsync()
                    .Task.ContinueWith(task => {
                        if (task.Status == TaskStatus.RanToCompletion)
                            _logger.LogDebug("Node invocation finished.\n\t{0}", string.Join("\n\t", _logs.ToArray().Select(log => log.Item2.Trim())));
                        else
                            _logger.LogError("Node invocation failed.\n\t{0}", string.Join("\n\t", _logs.ToArray().Select(log => $"{log.Item1} -> {log.Item2.Trim()}")));
                    });
            }
        }
    }
}
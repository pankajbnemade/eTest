using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ERP.Models.Logger
{
    public class SeriLogProvider : ILoggerProvider
    {
        private IDictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();

        public ILogger CreateLogger(string name)
        {
            if (!_loggers.ContainsKey(name))
            {
                lock (_loggers)
                {
                    if (!_loggers.ContainsKey(name))
                    {
                        _loggers[name] = new SeriLogAdapter();
                    }
                }
            }
            return _loggers[name];
        }

        public void Dispose() => _loggers.Clear();
    }
}

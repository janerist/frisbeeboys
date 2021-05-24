using System;
using Npgsql.Logging;
using Serilog.Events;

namespace Frisbeeboys.Web.Data
{
    public class NpgsqlSerilogProvider : INpgsqlLoggingProvider
    {
        public NpgsqlLogger CreateLogger(string name)
        {
            return new NpgsqlSerilogLogger();
        }

        private class NpgsqlSerilogLogger : NpgsqlLogger
        {
            public override bool IsEnabled(NpgsqlLogLevel level)
            {
                return Serilog.Log.IsEnabled(ToSerilogLevel(level));
            }

            public override void Log(NpgsqlLogLevel level, int connectorId, string msg, Exception? exception = null)
            {
                var serilogLevel = ToSerilogLevel(level);
            
                if (exception != null)
                {
                    Serilog.Log.Write(serilogLevel, exception, msg, connectorId);    
                }
                else
                {
                    Serilog.Log.Write(serilogLevel, msg, connectorId);
                }
            }

            private LogEventLevel ToSerilogLevel(NpgsqlLogLevel level) =>
                level switch
                {
                    NpgsqlLogLevel.Trace => LogEventLevel.Verbose,
                    NpgsqlLogLevel.Debug => LogEventLevel.Debug,
                    NpgsqlLogLevel.Info => LogEventLevel.Information,
                    NpgsqlLogLevel.Warn => LogEventLevel.Warning,
                    NpgsqlLogLevel.Error => LogEventLevel.Error,
                    NpgsqlLogLevel.Fatal => LogEventLevel.Fatal,
                    _ => LogEventLevel.Information
                };
        }
    }
}
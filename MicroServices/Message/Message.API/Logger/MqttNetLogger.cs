using MQTTnet.Diagnostics;

namespace Message.API.Logger;
public class MqttNetLogger(ILogger<MqttNetLogger>logger) : IMqttNetLogger
{
    private readonly ILogger<MqttNetLogger> _logger = logger;
    public bool IsEnabled => throw new NotImplementedException();

    public void Publish(MqttNetLogLevel logLevel, string source, string message, object[] parameters, Exception exception)
    {
        LogLevel level = ConvertMqttNetLogLevelToLogLevel(logLevel);
        _logger.Log(level, exception, message, parameters);
    }

    private LogLevel ConvertMqttNetLogLevelToLogLevel(MqttNetLogLevel level)
    {
        // Convert MQTTnet log level to Microsoft.Extensions.Logging log level
        switch (level)
        {
            case MqttNetLogLevel.Verbose:
                return LogLevel.Trace;
            case MqttNetLogLevel.Info:
                return LogLevel.Information;
            case MqttNetLogLevel.Warning:
                return LogLevel.Warning;
            case MqttNetLogLevel.Error:
                return LogLevel.Error;
            default:
                return LogLevel.Information;
        }
    }
}

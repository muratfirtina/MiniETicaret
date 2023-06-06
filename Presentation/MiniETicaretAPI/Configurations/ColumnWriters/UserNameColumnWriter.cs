using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace MiniETicaretAPI.Configurations.ColumnWriters;

public class UserNameColumnWriter: ColumnWriterBase
{
    public UserNameColumnWriter() : base(NpgsqlDbType.Varchar)
    {
    }

    public override object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        var (userName,value) = logEvent.Properties.FirstOrDefault(p => p.Key == "userName");
        return value?.ToString() ?? null;
    }
}
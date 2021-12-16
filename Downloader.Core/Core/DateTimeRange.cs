namespace Downloader.Core.Core;

public struct DateTimeRange
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateTimeRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static DateTimeRange FromUtcToday(TimeSpan diff)
    {
        return FromDiff(DateTime.UtcNow.Date, diff);
    }

    public static DateTimeRange FromDiff(DateTime start, TimeSpan diff)
    {
        var end = start - diff;
        return new DateTimeRange(start <= end ? start : end, start <= end ? end : start);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace QMEditor;

public class DebugTimer {

    private List<TimestampLog> _timestamps;
    private DateTime creationDateTime;

    public DebugTimer() {
        _timestamps = new List<TimestampLog>();
        creationDateTime = DateTime.Now;
    }

    public static DebugTimer Track(Action action) {
        var debugTimer = new DebugTimer();
        action.Invoke();
        debugTimer.Timestamp("Operation lasted");
        return debugTimer;
    }

    public TimestampLog Timestamp(string timestampInfo = null) {
        _timestamps.Add(new TimestampLog(timestampInfo ?? string.Empty));
        return _timestamps.Last();
    }

    public TimestampLog[] GetAllTimestamps() => _timestamps.ToArray();

    public void Log() => _timestamps.Last().Log();

    public void LogAll(bool logTotal = true) {
        DateTime prevDateTime = creationDateTime;
        foreach (TimestampLog timestamp in _timestamps) {
            Console.WriteLine($"Section {timestamp.Info} completed in {timestamp.DateTime - prevDateTime}");
            prevDateTime = timestamp.DateTime;
        }
        if (logTotal)
            Console.WriteLine($"Total exec time: {prevDateTime-creationDateTime}");
    }

    public class TimestampLog {
        public readonly DateTime DateTime;
        public readonly string Info;
        public TimestampLog(string timestampInfo) {
            DateTime = DateTime.Now;
            Info = timestampInfo;
        }
        public void Log() {
            Console.WriteLine($"{Info}: {DateTime}");
        }
    }

}
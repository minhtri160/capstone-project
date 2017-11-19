using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APMS.Business.Dictionary
{
    public enum AccountType : int
    {
        Admin = 0,
        User = 1
    }

    public enum OperatorType : int
    {
        GreaterThanOrEqual = 1, // >=
        GreaterThan = 2,        // >
        LessThanOrEqual = 3,    // <=
        LessThan = 4            // <
    }

    public enum NotificationState : int
    {
        Unread = 0,
        Read = 1
    }

    public enum DeviceState : int
    {
        Off = 0,
        Startup = 1,
        On = 2,
        ShuttingDown = 3
    }

    public enum SensorType : int
    {
        Voltage = 0,
        Current = 1,
        Temperature = 2,
        Humidity = 3,
        Smoke = 4,
        Fire = 5,
        Motion = 6
    }

    public enum WarningState : int
    {
        Normal = 0,
        HighVolta = 1,
        LowVolta = 2,
        OverCurrent = 3,
        FanSlow = 4,
        LampNotWork = 5,
        GasLeakage = 6,
        OverTemp = 7,
        TooMoist = 8,
        HasSmoke = 9,
        HasFire = 10,
        LowTemp = 11,
        TooDry = 12,
        HasMotion = 13,
        Others = 14
    }
}

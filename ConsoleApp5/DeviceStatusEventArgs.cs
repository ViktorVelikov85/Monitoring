using System;

public class DeviceStatusEventArgs : EventArgs
{
    public string DeviceName { get; }
    public string Message { get; }
    public DateTime Time { get; }

    public DeviceStatusEventArgs(string name, string message)
    {
        DeviceName = name;
        Message = message;
        Time = DateTime.Now;
    }
}
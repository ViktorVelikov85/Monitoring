using System;

public class LogService
{
    public void OnStatusChanged(object sender, DeviceStatusEventArgs e)
    {
        Console.WriteLine($"[LOG]: {e.Time} - {e.DeviceName}: {e.Message}");
    }
}
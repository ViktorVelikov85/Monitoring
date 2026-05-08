using System;

public class AlertService
{
    public void OnStatusChanged(object sender, DeviceStatusEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ALERT!!!]: Critical issue with {e.DeviceName}! Sending notification...");
        Console.ResetColor();
    }
}
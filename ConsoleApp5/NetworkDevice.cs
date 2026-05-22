using System;

public abstract class NetworkDevice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public bool IsOnline { get; set; } = true;
    public int Latency { get; set; } // В милисекунди (ms) - за LINQ
    public string DeviceType { get; set; } // Рутер, Суич, Принтер, AP
    public DateTime LastChecked { get; set; } = DateTime.Now;

    public event EventHandler<DeviceStatusEventArgs> StatusChanged;

    public void SimulateFailure(string reason)
    {
        IsOnline = false;
        Latency = 0;
        StatusChanged?.Invoke(this, new DeviceStatusEventArgs(Name, reason));
    }
}
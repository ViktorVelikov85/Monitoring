using System;

public abstract class NetworkDevice
{
    public string Name { get; set; }
    public string IpAddress { get; set; }
    public bool IsOnline { get; set; } = true;

    // Дефинираме събитието
    public event EventHandler<DeviceStatusEventArgs> StatusChanged;

    // Метод, който ще извикваме, когато нещо се развали
    public void SimulateFailure(string reason)
    {
        IsOnline = false;
        // Проверяваме дали някой слуша за това събитие и го задействаме
        StatusChanged?.Invoke(this, new DeviceStatusEventArgs(Name, reason));
    }
}
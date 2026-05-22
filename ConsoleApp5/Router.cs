using System;

public class Router : NetworkDevice
{
    public int ActivePorts { get; set; }
    public event EventHandler<DeviceStatusEventArgs> PortOverloaded;

    public Router() { DeviceType = "Router"; }

    public void ConnectNewDevice()
    {
        ActivePorts++;
        if (ActivePorts > 25)
        {
            PortOverloaded?.Invoke(this, new DeviceStatusEventArgs(Name, "Critical: Too many active connections! Port overloaded."));
        }
    }
}
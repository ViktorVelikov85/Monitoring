public class Router : NetworkDevice
{
    public int ActivePorts { get; set; }

    // ВТОРО СЪБИТИЕ: Специално за рутера
    public event EventHandler<DeviceStatusEventArgs> PortOverloaded;

    public void ConnectNewDevice()
    {
        ActivePorts++;
        if (ActivePorts > 25) // Да кажем, че лимитът е 25
        {
            PortOverloaded?.Invoke(this, new DeviceStatusEventArgs(Name, "Critical: Too many active connections! Port overloaded."));
        }
    }
}
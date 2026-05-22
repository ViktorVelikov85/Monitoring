public class Switch : NetworkDevice
{
    public int TotalPorts { get; set; } = 24;
    public Switch() { DeviceType = "Switch"; }
}
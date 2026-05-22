public class AccessPoint : NetworkDevice
{
    public int ConnectedUsers { get; set; } = 15;
    public AccessPoint() { DeviceType = "AccessPoint"; }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class NetworkMonitor
{
    public List<NetworkDevice> Devices { get; set; } = new List<NetworkDevice>();

    // 1. LINQ: Устройства, които са offline
    public List<NetworkDevice> GetOfflineDevices()
    {
        return Devices.Where(d => !d.IsOnline).ToList();
    }

    // 2. LINQ: Топ 5 устройства с най-висока латентност
    public List<NetworkDevice> GetTop5HighestLatency()
    {
        return Devices.OrderByDescending(d => d.Latency).Take(5).ToList();
    }

    // 3. LINQ: Групиране по тип устройство
    public void ShowGroupedByType()
    {
        var grouped = Devices.GroupBy(d => d.DeviceType);
        foreach (var group in grouped)
        {
            Console.WriteLine($"Тип: {group.Key} (Брой: {group.Count()})");
            foreach (var d in group)
            {
                Console.WriteLine($"  - {d.Name} (IP: {d.IpAddress}, Latency: {d.Latency}ms, Online: {d.IsOnline})");
            }
        }
    }

    // 4. LINQ: Сортиране по IP адрес (Правилно числово сортиране, а не текстово!)
    public List<NetworkDevice> GetDevicesSortedByIp()
    {
        return Devices
            .OrderBy(d => Version.TryParse(d.IpAddress, out var v) ? v : new Version(0, 0, 0, 0))
            .ToList();
    }

    // 5. LINQ: Справка за проблемни устройства за избран ден
    public List<NetworkDevice> GetProblemDevicesByDay(DateTime day)
    {
        return Devices.Where(d => (!d.IsOnline || d.Latency > 50) && d.LastChecked.Date == day.Date).ToList();
    }

    // ФАЙЛОВЕ: Експортиране на данни в CSV формат (Изискване по задание)
    public void ExportToCsv(string filePath)
    {
        try
        {
            List<string> lines = new List<string> { "Id,Name,IpAddress,DeviceType,IsOnline,Latency" };
            foreach (var d in Devices)
            {
                lines.Add($"{d.Id},{d.Name},{d.IpAddress},{d.DeviceType},{d.IsOnline},{d.Latency}");
            }
            File.WriteAllLines(filePath, lines);
            Console.WriteLine($"[ФАЙЛ]: Успешен експорт на {Devices.Count} устройства в '{filePath}'");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[ГРЕШКА ФАЙЛ - EXPORT]: Неуспешно записване на CSV: {ex.Message}");
        }
    }
}
using System;
using System.Text;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Network Monitoring System v3.0 - Final Release";

        DatabaseService dbService = new DatabaseService();
        NetworkMonitor monitor = new NetworkMonitor();

        Console.WriteLine("=== СИСТЕМА ЗА МОНИТОРИНГ СТАРТИРАНА (ФИНАЛНА ВЕРСИЯ) ===\n");

        // 1. ГЕНЕРИРАНЕ НА 30 ЗАПИСА
        Console.WriteLine("[БД]: Напълване на базата данни с 30 реални записа...");
        dbService.ClearDevicesTable();

        Random rand = new Random();
        for (int i = 1; i <= 30; i++)
        {
            NetworkDevice device;
            int typeChoice = rand.Next(1, 5);

            if (typeChoice == 1) device = new Router { Name = $"Router_{i}", IpAddress = $"192.168.1.{rand.Next(2, 254)}" };
            else if (typeChoice == 2) device = new Switch { Name = $"Switch_{i}", IpAddress = $"10.0.0.{rand.Next(2, 254)}" };
            else if (typeChoice == 3) device = new Printer { Name = $"Printer_{i}", IpAddress = $"172.16.0.{rand.Next(2, 254)}" };
            else device = new AccessPoint { Name = $"AP_{i}", IpAddress = $"192.168.10.{rand.Next(2, 254)}" };

            device.Latency = rand.Next(5, 120);
            device.IsOnline = rand.Next(1, 11) > 2;

            dbService.InsertDevice(device);
        }

        // Зареждане на устройствата
        monitor.Devices = dbService.GetAllDevices();

        // 2. ИЗПЪЛНЕНИЕ НА LINQ ЗАДАЧИТЕ
        PrintSectionHeader("LINQ 1: ОФЛАЙН УСТРОЙСТВА");
        foreach (var d in monitor.GetOfflineDevices()) Console.WriteLine($"[OFFLINE]: {d.Name} ({d.DeviceType})");

        PrintSectionHeader("LINQ 2: ТОП 5 ЛАТЕНТНОСТ");
        foreach (var d in monitor.GetTop5HighestLatency()) Console.WriteLine($"{d.Name} -> {d.Latency}ms");

        PrintSectionHeader("LINQ 3: ГРУПИРАНЕ ПО ТИП");
        monitor.ShowGroupedByType();

        PrintSectionHeader("LINQ 4: ПРАВИЛНО СОРТИРАНЕ ПО IP (ПЪРВИТЕ 5)");
        foreach (var d in monitor.GetDevicesSortedByIp().Take(5)) Console.WriteLine($"{d.IpAddress} - {d.Name}");

        PrintSectionHeader("LINQ 5: ПРОБЛЕМНИ УСТРОЙСТВА ДНЕС");
        Console.WriteLine($"Брой проблемни устройства: {monitor.GetProblemDevicesByDay(DateTime.Today).Count}");

        // 3. ТЕСТ НА ФАЙЛОВ ЕКСПОРТ (CSV)
        PrintSectionHeader("ТЕСТ НА ИЗИСКВАНЕТО ЗА ЕКСПОРТ НА ДАННИ");
        monitor.ExportToCsv("exported_devices.csv");

        // 4. ТЕСТ НА ВАЛИДАЦИЯТА И ИЗКЛЮЧЕНИЯТА
        PrintSectionHeader("ТЕСТ НА ИЗКЛЮЧЕНИЯТА И ВАЛИДАЦИЯТА НА ДАННИ");
        try
        {
            Console.WriteLine("Опит за вкарване на невалидно устройство (празно име)...");
            var badDevice = new Router { Name = "", IpAddress = "" };
            dbService.InsertDevice(badDevice);
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[УСПЕШНА ВАЛИДАЦИЯ]: Системата улови грешката навреме: {ex.Message}");
            Console.ResetColor();
        }

        // 5. ДЕМОНСТРАЦИЯ НА СЪБИТИЯ
        PrintSectionHeader("ДЕМОНСТРАЦИЯ НА СЪБИТИЯ (ЛОГВАНЕ И АЛЕРТ)");
        var testRouter = new Router { Name = "Core_Edge_Router", IpAddress = "10.0.0.1" };
        var logService = new LogService();
        var alertService = new AlertService();

        testRouter.StatusChanged += logService.OnStatusChanged;
        testRouter.StatusChanged += alertService.OnStatusChanged;

        testRouter.SimulateFailure("Хардуерен срив на захранващия блок");

        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("Проектът премина финалните тестове!");
        Console.ReadKey();
    }

    static void PrintSectionHeader(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"--- {title} ---");
        Console.ResetColor();
    }
}
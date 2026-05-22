using System;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Network Monitoring System v2.0 - Real MySQL Connection";

        DatabaseService dbService = new DatabaseService();
        NetworkMonitor monitor = new NetworkMonitor();

        Console.WriteLine("=== СИСТЕМА ЗА МОНИТОРИНГ СТАРТИРАНА (РЕАЛНА БД XAMPP) ===\n");

        // 1. ИЗЧИСТВАНЕ И ГЕНЕРИРАНЕ НА ПОНЕ 30 ЗАПИСА В MySQL (Изискване по задание)
        Console.WriteLine("[БД]: Генериране на 30 тестови записа в реалната MySQL база данни...");
        dbService.ClearDevicesTable();

        Random rand = new Random();
        for (int i = 1; i <= 30; i++)
        {
            NetworkDevice device;
            int typeChoice = rand.Next(1, 5); // 1 до 4

            if (typeChoice == 1) device = new Router { Name = $"Router_{i}", IpAddress = $"192.168.1.{rand.Next(2, 254)}" };
            else if (typeChoice == 2) device = new Switch { Name = $"Switch_{i}", IpAddress = $"10.0.0.{rand.Next(2, 254)}" };
            else if (typeChoice == 3) device = new Printer { Name = $"Printer_{i}", IpAddress = $"172.16.0.{rand.Next(2, 254)}" };
            else device = new AccessPoint { Name = $"AP_{i}", IpAddress = $"192.168.10.{rand.Next(2, 254)}" };

            device.Latency = rand.Next(5, 120); // латентност между 5 и 120 ms
            device.IsOnline = rand.Next(1, 11) > 2; // 80% шанс да са онлайн, останалите офлайн

            dbService.InsertDevice(device); // Запис в реалната база данни!
        }
        Console.WriteLine("[БД]: Успешно записани 30 устройства в таблица `Devices`.\n");

        // 2. ЧЕТЕНЕ НА ДАННИТЕ ОТ MySQL И ЗАРЕЖДАНЕ В LINQ МОНИТОРА
        monitor.Devices = dbService.GetAllDevices();

        // 3. ИЗПЪЛНЕНИЕ НА LINQ ЗАДАЧИТЕ ОТ СЦЕНАРИЯ
        PrintSectionHeader("LINQ 1: ОФЛАЙН УСТРОЙСТВА");
        var offline = monitor.GetOfflineDevices();
        foreach (var d in offline) Console.WriteLine($"[OFFLINE]: {d.Name} ({d.DeviceType})");

        PrintSectionHeader("LINQ 2: ТОП 5 НАЙ-ВИСОКА ЛАТЕНТНОСТ");
        var top5 = monitor.GetTop5HighestLatency();
        foreach (var d in top5) Console.WriteLine($"{d.Name} -> {d.Latency}ms");

        PrintSectionHeader("LINQ 3: ГРУПИРАНЕ ПО ТИП УСТРОЙСТВО");
        monitor.ShowGroupedByType();

        PrintSectionHeader("LINQ 4: СОРТИРАНЕ ПО IP АДРЕС (ПЪРВИТЕ 5)");
        var sortedIp = monitor.GetDevicesSortedByIp().Take(5);
        foreach (var d in sortedIp) Console.WriteLine($"{d.IpAddress} - {d.Name}");

        PrintSectionHeader("LINQ 5: СПРАВКА ЗА ПРОБЛЕМНИ УСТРОЙСТВА ДНЕС");
        var problems = monitor.GetProblemDevicesByDay(DateTime.Today);
        Console.WriteLine($"Намерени проблемни устройства за днес: {problems.Count}");

        // 4. ТЕСТ НА СЪБИТИЯТА И ЗАПИСА ИМ В MySQL + ФАЙЛ
        PrintSectionHeader("ДЕМОНСТРАЦИЯ НА СЪБИТИЕ И СИНХРОНЕН ЗАПИС В MySQL И ФАЙЛ");
        var myRouter = new Router { Name = "Main_Office_Router", IpAddress = "192.168.1.1" };
        var logger = new LogService();
        var alerter = new AlertService();

        myRouter.StatusChanged += logger.OnStatusChanged;
        myRouter.StatusChanged += alerter.OnStatusChanged;

        Console.WriteLine("Предизвикване на повреда...");
        myRouter.SimulateFailure("Прекъснато захранване (Power Outage)");
        dbService.UpdateDevice(myRouter); // UPDATE операция

        // 5. ДЕМОНСТРАЦИЯ НА ОБРАБОТКА НА ИЗКЛЮЧЕНИЕ ПРИ СРИВ НА БД
        PrintSectionHeader("ДЕМОНСТРАЦИЯ НА ОБРАБОТКА НА ИЗКЛЮЧЕНИЕ (БД ГРЕШКА)");
        Console.WriteLine("Следва симулация на грешна операция за демонстриране на try-catch блока:");
        dbService.DeleteDeviceByName(null); // Предизвиква безопасно улавяне на логика в метода

        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("Седмица 3 приключи успешно! Всичко е записано в MySQL (XAMPP).");
        Console.WriteLine("Натиснете произволен клавиш за изход...");
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
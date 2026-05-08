using System;
using System.Text; // Нужно е за Encoding

class Program
{
    static void Main(string[] args)
    {
        // Оправяме кодирането за кирилица
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        // Настройка на заглавието
        Console.Title = "Network Monitoring System v1.0";
        Console.WriteLine("=== СИСТЕМА ЗА МОНИТОРИНГ СТАРТИРАНА ===\n");

        // 1. ИНИЦИАЛИЗАЦИЯ
        var myRouter = new Router
        {
            Name = "Main_Office_Router",
            IpAddress = "192.168.1.1",
            ActivePorts = 24
        };

        var officePrinter = new Printer
        {
            Name = "HP_LaserJet_Office",
            IpAddress = "192.168.1.50"
        };

        var logger = new LogService();
        var alerter = new AlertService();

        // 2. АБОНИРАНЕ
        myRouter.StatusChanged += logger.OnStatusChanged;
        myRouter.StatusChanged += alerter.OnStatusChanged;
        myRouter.PortOverloaded += logger.OnStatusChanged;
        myRouter.PortOverloaded += alerter.OnStatusChanged;

        officePrinter.OutOfPaper += logger.OnStatusChanged;
        officePrinter.OutOfPaper += alerter.OnStatusChanged;

        // ---------------------------------------------------------
        // 3. ТЕСТ 1: Рутер
        // ---------------------------------------------------------
        PrintSectionHeader("ТЕСТ 1: ПРЕТОВАРВАНЕ НА ПОРТ");
        Console.WriteLine("Натиснете [Enter], за да добавите нови устройства...");
        Console.ReadLine();

        myRouter.ConnectNewDevice(); // 25
        myRouter.ConnectNewDevice(); // 26 -> Събитие!

        // ---------------------------------------------------------
        // 4. ТЕСТ 2: Принтер
        // ---------------------------------------------------------
        PrintSectionHeader("ТЕСТ 2: ПРОВЕРКА НА ХАРТИЯТА");
        Console.WriteLine("Стартиране на печат...");

        for (int i = 0; i < 11; i++)
        {
            officePrinter.PrintPage();
        }

        // ---------------------------------------------------------
        // 5. ТЕСТ 3: Повреда
        // ---------------------------------------------------------
        PrintSectionHeader("ТЕСТ 3: КРИТИЧНА ПОВРЕДА");
        Console.WriteLine("Натиснете [Enter] за симулация на спиране на тока...");
        Console.ReadLine();

        myRouter.SimulateFailure("Прекъснато захранване (Power Outage)");

        // КРАЙ
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("Тестовете приключиха успешно.");
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
using System;

public class Printer : NetworkDevice
{
    public int PaperLevel { get; set; } = 10; // Започваме с 10 листа

    // Второ събитие, специфично за принтера
    public event EventHandler<DeviceStatusEventArgs> OutOfPaper;

    public void PrintPage()
    {
        if (PaperLevel > 0)
        {
            PaperLevel--;
            Console.WriteLine($"[Printer]: Printing... Sheets left: {PaperLevel}");
        }
        else
        {
            // Предизвикваме събитието, ако няма хартия
            OutOfPaper?.Invoke(this, new DeviceStatusEventArgs(Name, "Printer is out of paper!"));
        }
    }
}
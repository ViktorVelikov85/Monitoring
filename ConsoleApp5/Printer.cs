using System;

public class Printer : NetworkDevice
{
    public int PaperLevel { get; set; } = 10;
    public event EventHandler<DeviceStatusEventArgs> OutOfPaper;

    public Printer() { DeviceType = "Printer"; }

    public void PrintPage()
    {
        if (PaperLevel > 0)
        {
            PaperLevel--;
            Console.WriteLine($"[Printer]: Printing... Sheets left: {PaperLevel}");
        }
        else
        {
            OutOfPaper?.Invoke(this, new DeviceStatusEventArgs(Name, "Printer is out of paper!"));
        }
    }
}
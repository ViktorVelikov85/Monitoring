using System;
using System.IO;

public class LogService
{
    private string filePath = "network_logs.txt";
    private DatabaseService db = new DatabaseService();

    public void OnStatusChanged(object sender, DeviceStatusEventArgs e)
    {
        string logLine = $"[{e.Time}] - {e.DeviceName}: {e.Message}";
        Console.WriteLine($"[LOG]: {logLine}");

        // ТЕСТ ФАЙЛОВЕ И ИЗКЛЮЧЕНИЯ
        try
        {
            File.AppendAllText(filePath, logLine + Environment.NewLine);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[ГРЕШКА ФАЙЛ]: {ex.Message}");
        }

        // РЕАЛЕН ЗАПИС В MySQL (Таблица DeviceLogs)
        db.InsertLog(logLine);
    }
}
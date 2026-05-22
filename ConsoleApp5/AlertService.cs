using System;

public class AlertService
{
    private DatabaseService db = new DatabaseService();

    public void OnStatusChanged(object sender, DeviceStatusEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string alertMsg = $"[ALERT!!!]: Critical issue with {e.DeviceName}! Message: {e.Message}";
        Console.WriteLine(alertMsg);
        Console.ResetColor();

        // РЕАЛЕН ЗАПИС В MySQL (Таблица Alerts)
        db.InsertAlert(alertMsg);
    }
}
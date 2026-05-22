using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient; // Използва NuGet пакета

public class DatabaseService
{
    // Connection string за стандартен XAMPP
    private string connectionString = "Server=localhost;Database=network_monitoring;Uid=root;Pwd=;";

    // 1. CREATE: Добавяне на устройство в MySQL
    public void InsertDevice(NetworkDevice device)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                int typeId = GetDeviceTypeId(device.DeviceType);

                string query = "INSERT INTO Devices (Name, IpAddress, IsOnline, Latency, DeviceTypeId) VALUES (@Name, @IpAddress, @IsOnline, @Latency, @TypeId)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", device.Name);
                    cmd.Parameters.AddWithValue("@IpAddress", device.IpAddress);
                    cmd.Parameters.AddWithValue("@IsOnline", device.IsOnline ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Latency", device.Latency);
                    cmd.Parameters.AddWithValue("@TypeId", typeId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[БД ГРЕШКА - INSERT]: Неуспешен запис на устройство! Детайли: {ex.Message}");
            Console.ResetColor();
        }
    }

    // 2. READ: Извличане на всички устройства от MySQL (Нужно за LINQ заявките ни)
    public List<NetworkDevice> GetAllDevices()
    {
        List<NetworkDevice> devices = new List<NetworkDevice>();
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT d.Id, d.Name, d.IpAddress, d.IsOnline, d.Latency, t.TypeName FROM Devices d JOIN DeviceTypes t ON d.DeviceTypeId = t.Id";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string type = reader["TypeName"].ToString();
                        NetworkDevice dev = null;

                        // Полиморфизъм според релацията в базата данни
                        if (type == "Router") dev = new Router();
                        else if (type == "Switch") dev = new Switch();
                        else if (type == "Printer") dev = new Printer();
                        else if (type == "AccessPoint") dev = new AccessPoint();

                        if (dev != null)
                        {
                            dev.Id = Convert.ToInt32(reader["Id"]);
                            dev.Name = reader["Name"].ToString();
                            dev.IpAddress = reader["IpAddress"].ToString();
                            dev.IsOnline = Convert.ToInt32(reader["IsOnline"]) == 1;
                            dev.Latency = Convert.ToInt32(reader["Latency"]);
                            devices.Add(dev);
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[БД ГРЕШКА - ЧЕТЕНЕ]: Срив при извличане на данни! {ex.Message}");
            Console.ResetColor();
        }
        return devices;
    }

    // 3. UPDATE: Обновяване на състоянието в MySQL
    public void UpdateDevice(NetworkDevice device)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Devices SET IsOnline = @IsOnline, Latency = @Latency WHERE Name = @Name";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IsOnline", device.IsOnline ? 1 : 0);
                    cmd.Parameters.AddWithValue("@Latency", device.Latency);
                    cmd.Parameters.AddWithValue("@Name", device.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"[БД ГРЕШКА - UPDATE]: {ex.Message}");
        }
    }

    // 4. DELETE: Изтриване от MySQL
    public void DeleteDeviceByName(string name)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Devices WHERE Name = @Name";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"[БД ГРЕШКА - DELETE]: {ex.Message}");
        }
    }

    // Запис в таблица DeviceLogs през MySQL
    public void InsertLog(string message)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO DeviceLogs (LogMessage) VALUES (@Msg)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Msg", message);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"[БД ГРЕШКА - LOG]: Неуспешен запис на лог: {ex.Message}");
        }
    }

    // Запис в таблица Alerts през MySQL
    public void InsertAlert(string message)
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Alerts (AlertMessage) VALUES (@Msg)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Msg", message);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"[БД ГРЕШКА - LOG]: Неуспешен запис на лог: {ex.Message}");
        }
    }

    // Помощен метод за изчистване на таблицата (за целите на теста)
    public void ClearDevicesTable()
    {
        try
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SET FOREIGN_KEY_CHECKS = 0; TRUNCATE TABLE Devices; SET FOREIGN_KEY_CHECKS = 1;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn)) { cmd.ExecuteNonQuery(); }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"[БД ГРЕШКА - LOG]: Неуспешен запис на лог: {ex.Message}");
        }
    }

    private int GetDeviceTypeId(string typeName)
    {
        if (typeName == "Router") return 1;
        if (typeName == "Switch") return 2;
        if (typeName == "Printer") return 3;
        if (typeName == "AccessPoint") return 4;
        return 1;
    }
}
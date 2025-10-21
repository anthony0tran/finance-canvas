using LiteDB;

namespace FinanceCanvasAPI.Context;

public static class LiteDbFactory
{
    public static LiteDatabase Create(string password)
    {
        var dataDir = Environment.GetEnvironmentVariable("LITEDB_DATA_DIR") 
                      ?? (OperatingSystem.IsWindows() 
                          ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FinanceCanvasAPI", "data") 
                          : "/data");
        Directory.CreateDirectory(dataDir);

        var connectionString = new ConnectionString
        {
            Filename = Path.Combine(dataDir, "secure.ldb"),
            Password = password
        };
        return new LiteDatabase(connectionString);
    }
}
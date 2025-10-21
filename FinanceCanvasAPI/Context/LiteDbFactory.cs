using LiteDB;

namespace FinanceCanvasAPI.Context;

public static class LiteDbFactory
{
    public static LiteDatabase Create(string password)
    {
        var dataDir = Path.Combine(Environment.CurrentDirectory, "data");
        Directory.CreateDirectory(dataDir);
        
        var connectionString = new ConnectionString
        {
            Filename = Path.Combine(dataDir, "secure.ldb"),
            Password = password
        };
        return new LiteDatabase(connectionString);
    }
}
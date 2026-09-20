namespace ClinicManager.Application.FunctionalTests;

public static class TestDatabaseFactory
{
    public static async Task<ITestDatabase> CreateAsync()
    {
#if (UsePostgreSQL)
        var database = new PostgreSQLTestcontainersTestDatabase();
#elif (UseSqlServer)
        var database = new SqlTestcontainersTestDatabase();
#else
        var database = new SqliteTestDatabase();
#endif

        await database.InitialiseAsync();

        return database;
    }
}

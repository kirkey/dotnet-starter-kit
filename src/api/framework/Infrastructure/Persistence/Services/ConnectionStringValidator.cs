using Microsoft.Data.SqlClient;
using Npgsql;

namespace FSH.Framework.Infrastructure.Persistence.Services;
internal sealed class ConnectionStringValidator(IOptions<DatabaseOptions> dbSettings, ILogger<ConnectionStringValidator> logger) : IConnectionStringValidator
{
    private readonly DatabaseOptions _dbSettings = dbSettings.Value;

    public bool TryValidate(string connectionString, string? dbProvider = null)
    {
        if (string.IsNullOrWhiteSpace(dbProvider))
        {
            dbProvider = _dbSettings.Provider;
        }

        try
        {
            switch (dbProvider.ToUpperInvariant())
            {
                case DbProviders.PostgreSQL:
                    _ = new NpgsqlConnectionStringBuilder(connectionString);
                    break;
                
                case DbProviders.MySQL:
                    _ = new MySqlConnector.MySqlConnectionStringBuilder(connectionString);
                    break;

                case DbProviders.MSSQL:
                    _ = new SqlConnectionStringBuilder(connectionString);
                    break;
            }

            return true;
        }
        catch (Exception ex)
        {
#pragma warning disable S6667 // Logging in a catch clause should pass the caught exception as a parameter.
            logger.LogError("Connection String Validation Exception : {Error}", ex.Message);
#pragma warning restore S6667 // Logging in a catch clause should pass the caught exception as a parameter.
            return false;
        }
    }
}

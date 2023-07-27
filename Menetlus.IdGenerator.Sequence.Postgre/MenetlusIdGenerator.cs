using Menetlus.Domain;
using Npgsql;

namespace Menetlus.IdGenerator.Sequence.Postgre;

public class MenetlusIdGenerator : IMenetlusIdGenerator
{
    private string ConnectionString { get; }
    
    public MenetlusIdGenerator(string connectionString)
    {
        ConnectionString = connectionString;
    }
    
    public int GetNext()
    {
        using var dataSource = NpgsqlDataSource.Create(ConnectionString);

        using var cmd = dataSource.CreateCommand("SELECT NEXTVAL('order_id_seq')");
        using var reader = cmd.ExecuteReader();
        
        reader.Read();
        return reader.GetInt32(0);
    }
}
namespace Work.ApiModels;

public class DbConfig
{
    public DbConfig(String connectionString)
    {
        ConnectionString = connectionString;
    }
    public String ConnectionString { get; set; }
}
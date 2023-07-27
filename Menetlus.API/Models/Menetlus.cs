namespace Menetlus.API.Models;

public class Menetlus
{
    public required int Id { get; set; }
    public required Avaldaja Avaldaja { get; set; }
    public required Staatus Staatus { get; set; }
    public required Vastus Vastus { get; set; }
}
using Menetlus.API.Models;

namespace Menetlus.API.Messages;

public class CreateMenetlusRequest
{
    public Avaldaja Avaldaja { get; set; }
    public string Kusimus { get; set; }
    public string Markus { get; set; }
}
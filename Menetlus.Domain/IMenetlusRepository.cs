namespace Menetlus.Domain;

public interface IMenetlusRepository
{
    Menetlus GetById(int menetlusId);
    void Add(Menetlus menetlus);
}
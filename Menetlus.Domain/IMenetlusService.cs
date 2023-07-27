namespace Menetlus.Domain;

public interface IMenetlusService
{
    Menetlus GetById(int menetlusId);
    Menetlus Create(Avaldaja avaldaja, string kusimus, string markus);
    void VaataUle(int menetlusId);
    void VotaMenetlusse(int menetlusId);
    void Vasta(int menetlusId, Vastus vastus);
}
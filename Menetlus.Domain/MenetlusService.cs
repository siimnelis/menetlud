namespace Menetlus.Domain;

public class MenetlusService : IMenetlusService
{
    private IMenetlusRepository MenetlusRepository { get; }
    private IMenetlusIdGenerator MenetlusIdGenerator { get; }
    
    public MenetlusService(IMenetlusRepository menetlusRepository, IMenetlusIdGenerator menetlusIdGenerator)
    {
        MenetlusRepository = menetlusRepository;
        MenetlusIdGenerator = menetlusIdGenerator;
    }
    
    public Menetlus GetById(int menetlusId)
    {
        return MenetlusRepository.GetById(menetlusId);
    }

    public Menetlus Create(Avaldaja avaldaja, string kusimus, string markus)
    {
        var menetlus = new Menetlus(MenetlusIdGenerator, avaldaja, kusimus, markus);

        MenetlusRepository.Add(menetlus);
        
        return menetlus;
    }

    public void VaataUle(int menetlusId)
    {
        var menetlus = MenetlusRepository.GetById(menetlusId);
        menetlus.VaataUle();
    }

    public void VotaMenetlusse(int menetlusId)
    {
        var menetlus = MenetlusRepository.GetById(menetlusId);
        menetlus.VotaMenetlusse();
    }

    public void Vasta(int menetlusId, Vastus vastus)
    {
        var menetlus = MenetlusRepository.GetById(menetlusId);
        menetlus.Vasta(vastus);
    }
}
using Menetlus.Domain;
using Menetlus.Domain.Exceptions;

namespace Menetlus.Repository.EntityFrameworkCore;

public class MenetlusRepository : IMenetlusRepository
{
    private MenetlusContext MenetlusContext { get; }
    
    public MenetlusRepository(MenetlusContext menetlusContext)
    {
        MenetlusContext = menetlusContext;
    }
    
    public Domain.Menetlus GetById(int menetlusId)
    {
        var menetlus = MenetlusContext.Menetlused.FirstOrDefault(x => x.Id == menetlusId);

        if (menetlus == null)
            throw new MenetlustEiLeitudException();

        return menetlus;
    }

    public void Add(Domain.Menetlus menetlus)
    {
        MenetlusContext.Menetlused.Add(menetlus);
    }
}
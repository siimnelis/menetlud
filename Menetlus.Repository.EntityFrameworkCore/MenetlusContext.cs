using System.ComponentModel.DataAnnotations.Schema;
using Menetlus.Domain;
using Menetlus.External.Contracts;
using Menetlus.External.Contracts.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Menetlus.Repository.EntityFrameworkCore;

public class MenetlusContext : DbContext
{
    private string ConnectionString { get; }
    private MenetlejaContext MenetlejaContext { get; }
    
    public MenetlusContext(string connectionString, MenetlejaContext menetlejaContext)
    {
        ConnectionString = connectionString;
        MenetlejaContext = menetlejaContext;
    }
    
    public DbSet<Domain.Menetlus> Menetlused { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("order_id_seq");
        
        var menetlusBuilder = modelBuilder.Entity<Domain.Menetlus>();

        menetlusBuilder.ToTable("menetlus");
        menetlusBuilder.HasKey(x => x.Id);
        menetlusBuilder.Property(x => x.Id).HasDefaultValueSql("nextval('order_id_seq')");
        menetlusBuilder.Property(x => x.Avaldaja).HasColumnType("jsonb");
        menetlusBuilder.Property(x => x.Kusimus).IsRequired();
        menetlusBuilder.Property(x => x.Markus);
        menetlusBuilder.Property(x => x.Staatus).IsRequired();
        menetlusBuilder.Property(x => x.Vastus).IsRequired();
        menetlusBuilder.Ignore(x => x.Events);

        var outboxBuilder = modelBuilder.Entity<OutboxMessage>();

        outboxBuilder.ToTable("outbox");
        outboxBuilder.HasKey(x => x.Id);
        outboxBuilder.Property(x => x.Payload).HasColumnType("jsonb");
        outboxBuilder.Property(x => x.RoutingKey);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var events = Menetlused.Local.SelectMany(menetlus => menetlus.Events).ToList();

        foreach (var @event in events)
        {
            var externalEvent = @event.Map();
            
            Outbox.Add(new OutboxMessage
            {
                Payload = new Envelope
                {
                    Event = externalEvent,
                    Menetleja = MenetlejaContext?.Menetleja.Map()
                },
                RoutingKey = externalEvent.GetType().Name
            });
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
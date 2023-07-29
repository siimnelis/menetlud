using FluentAssertions;
using Menetlus.Domain.Exceptions;
using Moq;
using Xunit;

namespace Menetlus.Domain.Tests;

public class MenetlusTests
{
    [Fact]
    public void Constructor_AndmedOnKorrektsed_LoobMenetluse()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);

        menetlus.Avaldaja.Should().BeSameAs(avaldaja);
        menetlus.Kusimus.Should().Be("Kas toimib?");
        menetlus.Markus.Should().Be("");
        menetlus.Staatus.Should().Be(Staatus.Ootel);
        menetlus.Vastus.Should().Be(Vastus.Puudub);
    }

    [Fact]
    public void InternalConstructor_LoobMenetluseOigeteAndmetega()
    {
        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(1, avaldaja, "Kusimus?", "Markus", Staatus.Tagasilukatud, Vastus.Puudub);

        menetlus.Id.Should().Be(1);
        menetlus.Avaldaja.Should().BeSameAs(avaldaja);
        menetlus.Kusimus.Should().Be("Kusimus?");
        menetlus.Markus.Should().Be("Markus");
        menetlus.Staatus.Should().Be(Staatus.Tagasilukatud);
        menetlus.Vastus.Should().Be(Vastus.Puudub);
    }

    [Fact]
    public void VaataUle_MenetlusOnLoodudJaOnOotel_MenetlusLahebUlevaatamiselOlekusse()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        
        menetlus.VaataUle();

        menetlus.Staatus.Should().Be(Staatus.Ulevaatmisel);
    }
    
    [Fact]
    public void VaataUle_MenetlusOnLoodudJaEiOleOotel_Throws()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();

        var act = () =>
        {
            menetlus.VaataUle();
        };

        act.Should().Throw<MenetlusEiOleOotelException>();
    }
    
    [Fact]
    public void VotaMenetlusse_MenetlusOnLoodudJaOnUlevaatamiselMarkustEiOleLisatud_MenetlusLahebMenetlusesOlekusse()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();
        
        menetlus.VotaMenetlusse();

        menetlus.Staatus.Should().Be(Staatus.Menetluses);
    }
    
    [Fact]
    public void VotaMenetlusse_MenetlusOnLoodudJaOnUlevaatamiselMarkustOnLisatud_MenetlusLahebTagasilukatudOlekusse()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", "markus");
        menetlus.VaataUle();
        
        menetlus.VotaMenetlusse();

        menetlus.Staatus.Should().Be(Staatus.Tagasilukatud);
        menetlus.Vastus.Should().Be(Vastus.Puudub);
    }
    
    [Fact]
    public void VotaMenetlusse_MenetlusOnLoodudJaEiOleUlevaatamisel_ThrowsMenetlusEiOleUlevaatamiselException()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", "markus");

        var act = () =>
        {
            menetlus.VotaMenetlusse();
        };

        act.Should().Throw<MenetlusEiOleUlevaatamiselException>();
    }
    
    [Fact]
    public void Vasta_MenetlusOnLoodudJaOnMenetluses_MenetlusLopetakse()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();
        menetlus.VotaMenetlusse();
        
        menetlus.Vasta(Vastus.Jah);

        menetlus.Staatus.Should().Be(Staatus.Lopetatud);
        menetlus.Vastus.Should().Be(Vastus.Jah);
    }
    
    [Fact]
    public void Vasta_MenetlusOnLoodudJaOnMenetlusesVastusVigane_ThrowsVastusViganeException()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();
        menetlus.VotaMenetlusse();

        var act = () =>
        {
            menetlus.Vasta((Vastus)4);
        };

        act.Should().Throw<ViganeVastusException>();

    }
    
    [Fact]
    public void Vasta_MenetlusOnLoodudJaOnMenetlusesVastusPuudub_ThrowsVastusPuudubException()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();
        menetlus.VotaMenetlusse();

        var act = () =>
        {
            menetlus.Vasta(Vastus.Puudub);
        };

        act.Should().Throw<VastusPuudubException>();

    }
    
    [Fact]
    public void Vasta_MenetlusOnLoodudJaLoppenud_ThrowsMenetlusOnLoppenudException()
    {
        var menetlusIdGeneratorMock = new Mock<IMenetlusIdGenerator>();
        menetlusIdGeneratorMock.Setup(x => x.GetNext()).Returns(1);

        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");
        var menetlus = new Menetlus(menetlusIdGeneratorMock.Object, avaldaja, "Kas toimib?", null);
        menetlus.VaataUle();
        menetlus.VotaMenetlusse();
        menetlus.Vasta(Vastus.Ei);
        
        var act = () =>
        {
            menetlus.Vasta(Vastus.Jah);
        };

        act.Should().Throw<MenetlusOnLoppenudException>();

    }
}
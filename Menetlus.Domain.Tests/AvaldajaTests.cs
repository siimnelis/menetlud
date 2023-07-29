using FluentAssertions;
using Menetlus.Domain.Exceptions;
using Xunit;

namespace Menetlus.Domain.Tests;

public class AvaldajaTests
{
    [Fact]
    public void Constructor_AndmedOnKorrektsed_LoobAvaldaja()
    {
        var avaldaja = new Avaldaja("Peeter", "Tamm", "123456789");

        avaldaja.Eesnimi.Should().Be("Peeter");
        avaldaja.Perenimi.Should().Be("Tamm");
        avaldaja.Isikukood.Should().Be("123456789");
    }
    
    [Fact]
    public void Constructor_EesnimiPuudub_ThrowsEesnimiPuudubException()
    {
        var act = () =>
        {
            new Avaldaja("", "Tamm", "123456789");
        };

        act.Should().Throw<EesnimiPuudubException>();
    }
    
    [Fact]
    public void Constructor_PerenimiPuudub_ThrowsPerenimiPuudubException()
    {
        var act = () =>
        {
            new Avaldaja("Peeter", "", "123456789");
        };

        act.Should().Throw<PerenimiPuudubException>();
    }
    
    [Fact]
    public void Constructor_IsikukoodPuudub_ThrowsIsikukoodPuudubException()
    {
        var act = () =>
        {
            new Avaldaja("Peeter", "Tamm", "");
        };

        act.Should().Throw<IsikukoodPuudubException>();
    }
}
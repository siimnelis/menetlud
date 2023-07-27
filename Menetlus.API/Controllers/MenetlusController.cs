using Menetlus.API.Extensions;
using Menetlus.API.Messages;
using Menetlus.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vastus = Menetlus.API.Models.Vastus;

namespace Menetlus.API.Controllers;

[ApiController]
[Route("menetlus")]
public class MenetlusController : ControllerBase
{

    private IMenetlusService MenetlusService { get; }
    
    public MenetlusController(IMenetlusService menetlusService)
    {
        MenetlusService = menetlusService;
    }

    /// <summary>
    /// Tagastab menetluse.
    /// </summary>
    /// <response code="200">Tagastab menetluse.</response>
    /// <response code="400">Menetlust ei leitud.</response>
    [HttpGet("{menetlusId}")]
    public Models.Menetlus Get(int menetlusId)
    {
        return MenetlusService.GetById(menetlusId).Map();
    }
    
    /// <summary>
    /// Loob uue menetluse.
    /// </summary>
    /// <response code="201">Tagastab loodud menetluse.</response>
    /// <response code="400">Eesnimi puudub. Isikukood puudub. Perenimi puudub. Küsimus puudub.</response>
    [HttpPost]
    public Models.Menetlus Create([FromBody]CreateMenetlusRequest request)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        return MenetlusService.Create(request.Avaldaja.Map(), request.Kusimus, request.Markus).Map();
    }

    /// <summary>
    /// Muudab menetluse staatuse ootel olekust ülevaatamisel olekusse.
    /// </summary>
    /// <response code="204">Menetluse staatus muudeti.</response>
    /// <response code="400">Menetlus ei ole ootel.</response>
    [Authorize]
    [HttpPost("{menetlusId}/:vaataUle")]
    public void VaataUle(int menetlusId)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
        MenetlusService.VaataUle(menetlusId);
    }
    
    /// <summary>
    /// Muudab menetluse staatuse ülevaatamise olekust menetlus olekusse.
    /// Kui märkus on lisatud lõpetab menetluse ja muudab oleku tagasi lükatuks.
    /// </summary>
    /// <response code="204">Menetluse staatus muudeti.</response>
    /// <response code="400">Menetlus ei ole ülevaatamise.</response>
    [Authorize]
    [HttpPost("{menetlusId}/:votaMenetlusse")]
    public void VotaMenetlusse(int menetlusId)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
        MenetlusService.VotaMenetlusse(menetlusId);
    }
    
    /// <summary>
    /// Lõpetab menetluse vastusega.
    /// </summary>
    /// <response code="204">Menetluse staatus muudeti.</response>
    /// <response code="400">Menetlus on lõppenud. Ei ole menetluses. Vastus puudub(saadeti väärtus 0(Puudub)). Vastus on vigane(ei ole 1(Jah) või 2(Ei)).</response>
    [Authorize]
    [HttpPost("{menetlusId}/:vasta")]
    public void Vasta(int menetlusId, [FromBody]Vastus vastus)
    {
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
        MenetlusService.Vasta(menetlusId, (Domain.Vastus)vastus);
    }
}
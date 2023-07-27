using Menetlus.API.Extensions;
using Menetlus.API.Messages;
using Menetlus.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Avaldaja = Menetlus.API.Models.Avaldaja;
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

    [HttpGet("{menetlusId}")]
    public Models.Menetlus Get(int menetlusId)
    {
        return MenetlusService.GetById(menetlusId).Map();
    }
    
    [HttpPost]
    public Models.Menetlus Create([FromBody]CreateMenetlusRequest request)
    {
        return MenetlusService.Create(request.Avaldaja.Map(), request.Kusimus, request.Markus).Map();
    }

    [Authorize]
    [HttpPost("{menetlusId}/:vaataUle")]
    public void VaataUle(int menetlusId)
    {
        MenetlusService.VaataUle(menetlusId);
    }
    
    [Authorize]
    [HttpPost("{menetlusId}/:votaMenetlusse")]
    public void VotaMenetlusse(int menetlusId)
    {
        MenetlusService.VotaMenetlusse(menetlusId);
    }
    
    [Authorize]
    [HttpPost("{menetlusId}/:vasta")]
    public void Vasta(int menetlusId, [FromBody]Vastus vastus)
    {
        MenetlusService.Vasta(menetlusId, (Domain.Vastus)vastus);
    }
}
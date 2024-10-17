using Microsoft.AspNetCore.Mvc;
using Round.Common;
using Round.Services.Banks.Domain;

namespace Round.Services.Banks.Controllers;

[ApiController]
[Route("api/banks")]
public class BanksController : BaseController
{
    [HttpGet]
    public IEnumerable<Bank> GetBanksAsync()
    {
        return new[]
        {
            // TODO: This should also include country, etc.
            new Bank(new Guid("5c488c1a-d3d3-4334-9c23-da8339c804bf"), "Barclays", ""),
            new Bank(new Guid("e06f803a-6c0b-46bd-b75b-f944602a4baa"), "NatWest", ""),
            new Bank(new Guid("3e75f3ab-7582-4815-a695-cf7e3f788cbe"), "Starling", ""),
            new Bank(new Guid("c973f479-a668-4a47-8d5a-c54e7900b3d7"), "Monzo", ""),
            new Bank(new Guid("dadf26da-4854-4ab1-9b3d-167faeed126a"), "HSBC", "")
        };
    }
}
using BusinessLogic.Interfaces;
using BusinessLogic.Models.PurchaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService purchaseService;
        public PurchaseController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        [AllowAnonymous]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await purchaseService.GetAllAsync());
        }

        [AllowAnonymous]
        [HttpGet("getbyid/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await purchaseService.GetByIdAsync(id));
        }

        [AllowAnonymous]
        [HttpPut("create")]
        public async Task<IActionResult> Create([FromForm] BasePurchaseModel creationModel, int userId)
        {
            return Ok(await purchaseService.CreateAsync(creationModel, userId));
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await purchaseService.DeleteAsync(id);
            return Ok();
        }
    }
}

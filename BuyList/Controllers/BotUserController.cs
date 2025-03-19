using BusinessLogic.Interfaces;
using BusinessLogic.Models.PurchaseModels;
using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuyList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotUserController : ControllerBase
    {
        private readonly IBotUserService botUserService;

        public BotUserController(IBotUserService botUserService)
        {
            this.botUserService = botUserService;
        }

        [AllowAnonymous]
        [HttpGet("get/all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await botUserService.GetAllAsync());
        }

        [AllowAnonymous]
        [HttpGet("get/byid/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await botUserService.GetByIdAsync(id));
        }

        [AllowAnonymous]
        [HttpGet("get/bychatid/{chatId:int}")]
        public async Task<IActionResult> GetByChatId([FromRoute] int chatId)
        {
            return Ok(await botUserService.GetByChatIdAsync(chatId));
        }

        [AllowAnonymous]
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await botUserService.DeleteAsync(id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("deletebychatid/{chatId:long}")]
        public async Task<IActionResult> DeleteByChatId([FromRoute] long chatId)
        {
            await botUserService.DeleteAsync(chatId);
            return Ok();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using videocalling_blazor.Server.Services;

namespace videocalling_blazor.Server.Controllers
{
    [
        ApiController,
        Route("/api/twilio")
    ]
    public class TwilioController : ControllerBase
    {
        [HttpGet("token")]
        
        public IActionResult GetToken(
            [FromServices] TwilioService twilioService) =>
             new JsonResult(twilioService.GetTwilioJwt(User.Identity.Name));

        [HttpGet("rooms")]
       
        public async Task<IActionResult> GetRooms(
            [FromServices] TwilioService twilioService) =>
            new JsonResult(await twilioService.GetAllRoomsAsync());
    }
}

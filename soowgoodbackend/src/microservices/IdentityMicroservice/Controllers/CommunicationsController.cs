using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityMicroservice.Data;
using IdentityMicroservice.Model;
using Azure.Communication.Identity;
using Azure;
using Azure.Communication;
using Azure.Communication.Chat;

namespace IdentityMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationsController : ControllerBase
    {
        private readonly IdentityMicroserviceContext _context;
        private readonly CommunicationIdentityClient _client;

        public CommunicationsController(IdentityMicroserviceContext context)
        {
            _context = context;
        }

        // GET: api/Communications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Communication>>> GetCommunication()
        {
            return await _context.Communication.ToListAsync();
        }

        [HttpPost("AccessToken")]
        public async Task GetAsync(string name)
        {
            try
            {
                string connectionString = "endpoint=https://soowgoodcommunication.communication.azure.com/;accesskey=iTvxbyqoauNHIcreCQx28AnWwUtSLGUloXYo6jY3d9ogY77ob5iEleVmspsy7kKP52ymHmv5R8lOAmdEo9Z8sg==";
                var client = new CommunicationIdentityClient(connectionString);

                var identityResponse = await client.CreateUserAsync();
                var identity = identityResponse.Value;

                var tokenResponse = await client.GetTokenAsync(identity, scopes: new[] { CommunicationTokenScope.VoIP });
                var token = tokenResponse.Value.Token;
                var expiresOn = tokenResponse.Value.ExpiresOn;

                //InitializeCallAgent(token);

                //await RefreshAccessTokensAsync(identity, client);
                //await DeleteIdentity(identity, client);
            }
            catch (Exception ex)
            {
            }
        }



        //private void InitializeCallAgent(string token)
        //{
        //    const string initialToken = "your-initial-token";
        //    Uri apiUrl = new Uri("https://yourendpoint.dev.communication.azure.net");
        //    const string identity = "your user id"; //i.e. 8:acs:1b5cc06b-f352-4571-b1e6-d9b259b7c776_00000006-6224-f48d-b274-5aaaaaaaaaaa";

        //    var communicationUserCredential = new CommunicationUserCredential(
        //        initialToken: initialToken,
        //        refreshProactively: true,
        //        tokenRefresher: cancellationToken => fetchNewTokenForCurrentUser(identity).GetAwaiter().GetResult(),
        //        asyncTokenRefresher: cancellationToken => fetchNewTokenForCurrentUser(identity));

        //    var chatClient = new ChatClient(apiUrl, communicationUserCredential);

        //}

        private async Task DeleteIdentity(CommunicationUserIdentifier identity, CommunicationIdentityClient client)
        {
            await client.DeleteUserAsync(identity);
        }

        private async Task RefreshAccessTokensAsync(CommunicationUserIdentifier identity, CommunicationIdentityClient client)
        {
            var identityToRefresh = new CommunicationUserIdentifier(identity.Id);
            var tokenResponse = await client.GetTokenAsync(identityToRefresh, scopes: new[] { CommunicationTokenScope.VoIP });
        }



        //private void RefreshAccessTokens()
        //{

        //}

        // GET: api/Communications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Communication>> GetCommunication(string id)
        {
            var communication = await _context.Communication.FindAsync(id);

            if (communication == null)
            {
                return NotFound();
            }

            return communication;
        }

        // PUT: api/Communications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunication(string id, Communication communication)
        {
            if (id != communication.Id)
            {
                return BadRequest();
            }

            _context.Entry(communication).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Communications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Communication>> PostCommunication(Communication communication)
        {
            _context.Communication.Add(communication);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommunication", new { id = communication.Id }, communication);
        }

        // DELETE: api/Communications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Communication>> DeleteCommunication(string id)
        {
            var communication = await _context.Communication.FindAsync(id);
            if (communication == null)
            {
                return NotFound();
            }

            _context.Communication.Remove(communication);
            await _context.SaveChangesAsync();

            return communication;
        }

        private bool CommunicationExists(string id)
        {
            return _context.Communication.Any(e => e.Id == id);
        }
    }
}

using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administration.Email;
using Shared.Models.Response;

namespace Service.Api.Modules
{
    [Authorize]
    public class EmailProviderController : ApiControllerBase
    {
        private readonly IEmailProviderConfiguration _emailProviderConfiguration;


        public EmailProviderController(IEmailProviderConfiguration emailProviderConfiguration)
        {
            _emailProviderConfiguration = emailProviderConfiguration;
        }

        [HttpGet(Name = "GetProviderSettings")]
        public async Task<List<EmailProviderSettings>> GetProviderSettings()
        {
            return await _emailProviderConfiguration.GetSettings();
        }

        [HttpPost(Name = "ProviderConnectionTest")]
        public async Task<SuccessResponse> ProviderConnectionTest([FromBody] EmailProviderSettings providerSettings)
        {
            var response = new SuccessResponse { Success = await _emailProviderConfiguration.TestConnection(providerSettings) };

            return response;
        }

        [HttpPost(Name = "EstablishProviderConnection")]
        public async Task<SuccessResponse> EstablishProviderConnection([FromBody] EmailProviderSettings providerSettings)
        {
            var response = new SuccessResponse { Success = await _emailProviderConfiguration.OnEstablishConnection(providerSettings) };

            return response;
        }


        [HttpPost("{connectionId}", Name = "DeleteProviderConnection")]
        public async Task<SuccessResponse> DeleteProviderConnection([FromQuery] int connectionId)
        {
            var response = new SuccessResponse { Success = await _emailProviderConfiguration.DeleteConnection(connectionId) };

            return response;
        }


    }
}

using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Models.Settings;

namespace Service.Api
{
    [Route("api/[controller]/[action]/")]
    public class MaintananceControllerBase : ControllerBase
    {
        private readonly IEmailClient _emailClient;
        private readonly IOptions<AppSettingsModel> _appSettings;
        public IEmailClient EmailClient => _emailClient;


        public MaintananceControllerBase(IOptions<AppSettingsModel> appSettings, IEmailClient emailClient)
        {
            _emailClient = emailClient;
            _appSettings = appSettings;
        }

        [NonAction]
        public async Task SendMail(string to, string subject, string body)
        {
            var settings = _appSettings.Value;

            await _emailClient.SendMailViaSmtp(settings.SmtpServer, settings.SmtpPort, settings.SystemEmail, settings.SystemEmailPassword, to, subject, body);
        }


    }
}

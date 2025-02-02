using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Shared.Email
{
    public class EmailSubjectEntity: AEntityBase
    {
        public string EmailSubject { get; set; } = string.Empty;
    }
}

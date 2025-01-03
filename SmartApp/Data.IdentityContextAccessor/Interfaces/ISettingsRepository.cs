using Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ContextAccessor.Interfaces
{
    public interface ISettingsRepository: IDisposable
    {
        public RepositoryBase<EmailAccountEntity> EmailAccountRepository { get; }
    }
}

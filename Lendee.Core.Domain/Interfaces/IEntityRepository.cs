using Lendee.Core.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lendee.Core.Domain.Interfaces
{
    public interface IEntityRepository
    {
        Task<IEnumerable<LegalEntity>> List();
        Task<LegalEntity> Find(long id);
        Task Save();
        LegalEntity Add(LegalEntity entity);
    }
}
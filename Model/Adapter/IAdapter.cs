using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iConto.Model.Adapter
{
    public interface IAdapter
    {
        Task<EntityType> FindOne<EntityType>(string id);

        Task<List<EntityType>> FindAll<EntityType>();

        Task<List<EntityType>> Filter<EntityType>(List<KeyValuePair<string, string>> query);

        Task<List<EntityType>> FindMany<EntityType>(long[] ids);
    }
}

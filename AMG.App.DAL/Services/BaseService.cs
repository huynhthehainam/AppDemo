using AMG.App.DAL.Databases;
using Microsoft.Extensions.Caching.Distributed;

namespace AMG.App.DAL.Services
{
    public class BaseService
    {
        protected DatabaseContext context;
        protected IDistributedCache distributedCache;
        public BaseService(DatabaseContext context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }
    }
}
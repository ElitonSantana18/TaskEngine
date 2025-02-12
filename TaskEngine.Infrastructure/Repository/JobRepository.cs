using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEngine.Domain.Entities;
using TaskEngine.Domain.Interfaces.Repository;
using TaskEngine.Domain.Interfaces.Settings;

namespace TaskEngine.Infrastructure.Repository
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(IMongoDbSettings settings) : base(settings, "Job")
        {

        }
    }
}

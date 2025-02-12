using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskEngine.Domain.Interfaces.Settings;

namespace TaskEngine.Domain.Entities.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskEngine.Domain.Interfaces.Settings
{
    public interface IMongoDbSettings
    {
        string DataBaseName { get; set; }
        string ConnectionString { get; set; }
    }
}

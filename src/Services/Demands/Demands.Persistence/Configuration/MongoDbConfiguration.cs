using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demands.Persistence.Configuration;

public struct MongoDbConfiguration
{
    public string ConnectionString { get; init; }

    public string DemandsDatabaseName { get; init; }

    public string DemandsCollectionName { get; init; }
}

using Identity.Domain.Entities;
using System.Collections.Immutable;

namespace Identity.WebApi.InMemory;

public static class InMemoryResources
{
    private static IEnumerable<Role> _singleCustomer = new List<Role>
    { 
        Role.Customer
    }
    .ToImmutableList();

    public static IEnumerable<Role> SingleCustomer {  get => _singleCustomer; }
}

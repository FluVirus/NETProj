using Identity.Domain.Entities;

namespace Identity.WebApi.InMemory;

public static class InMemoryResources
{
    private static Role[] _singleCustomer = { Role.Customer };

    public static Role[] SingleCustomer {  get => _singleCustomer; }
}

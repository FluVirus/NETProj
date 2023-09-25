using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

public class Role: IdentityRole<int>
{
    public static Role Customer = new Role
    {
        Id = 1,
        Name = "customer",
        NormalizedName = "CUSTOMER"
    };

    public static Role Driver = new Role 
    { 
        Id = 2,
        Name = "driver",
        NormalizedName = "DRIVER"
    };
}

using ITCommunityCRM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Data
{
    public class ITCommunityCRMIdentityDbContext : IdentityDbContext<User>
    {
        public ITCommunityCRMIdentityDbContext(DbContextOptions<ITCommunityCRMIdentityDbContext> options)
            : base(options)
        {
        }
    }
}

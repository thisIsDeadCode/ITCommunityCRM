using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ITCommunityCRM.Data
{
    public class ITCommunityCRMIdentityDbContext : IdentityDbContext
    {
        public ITCommunityCRMIdentityDbContext(DbContextOptions<ITCommunityCRMIdentityDbContext> options)
            : base(options)
        {
        }
    }
}

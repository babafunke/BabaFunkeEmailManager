using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BabaFunkeEmailManager.Client.Data
{
    public class BabaFunkeEmailManagerClientContext : IdentityDbContext<IdentityUser>
    {
        public BabaFunkeEmailManagerClientContext(DbContextOptions<BabaFunkeEmailManagerClientContext> options)
            : base(options)
        {
        }
    }
}

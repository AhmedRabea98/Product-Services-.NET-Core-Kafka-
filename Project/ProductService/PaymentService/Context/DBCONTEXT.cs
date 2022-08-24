using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Context
{
    public class DBCONTEXT:DbContext
    {
        public DBCONTEXT()
        {

        }
        public DBCONTEXT(DbContextOptions<DBCONTEXT> options):base(options)
        {

        }
        public DbSet<OrderRequest> Orders { get; set; }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AIS_Transport_Agency.Models;

namespace AIS_Transport_Agency.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SlotBooking>()
                .HasMany(c => c.Users)
                .WithMany(c => c.SlotBookings)
                .UsingEntity<BookingUser>(
                j =>
                {
                    j.HasKey(d => new { d.UserId, d.BookingId });
                });                                     
        }
        public DbSet<SlotBooking> SlotBooking { get; set; } = default!;
        public DbSet<BookingUser> BookingUser { get; set; } = default!; 
        public DbSet<BookingHistory> BookingHistory  { get; set; } = default!; 
    }
}
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using ToDoListApp.Models;


namespace ToDoListApp.Database
{
    public class ToDoContext : DbContext
    {
        private IDbConnection DbConnection { get; }

        public ToDoContext(DbContextOptions<ToDoContext> options, IConfiguration configuration) : base(options)
        {
            DbConnection = new SqlConnection(configuration.GetConnectionString("P3Referential"));
        }

        public DbSet<ToDoModel> ToDoList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConnection.ConnectionString, providerOptions => providerOptions.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<OrderLine>(entity =>
            {
                entity.HasIndex(e => e.OrderId)
                    .HasName("IX_OrderLineEntity_OrderEntityId");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderLineEntity_OrderEntity_OrderEntityId").OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderLine)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderLine__Produ__52593CB8").OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

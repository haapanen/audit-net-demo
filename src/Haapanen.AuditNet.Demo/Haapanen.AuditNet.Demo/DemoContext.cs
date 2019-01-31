using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Haapanen.AuditNet.Demo
{
    public class DemoContext : AuditDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=demo.db");
        }
    }

    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

    public class AuditLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EntityType { get; set; }
        public string AuditData { get; set; }
        public DateTime AuditDate { get; set; }
        public int UserId { get; set; }
    }
}

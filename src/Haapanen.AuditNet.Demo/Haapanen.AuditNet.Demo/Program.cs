using System;
using System.Collections.Generic;
using System.Linq;
using Audit.Core;
using Microsoft.EntityFrameworkCore;

namespace Haapanen.AuditNet.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureAuditLog();

            using (var dbContext = CreateDemoContext())
            {
                var newOrder = new Order
                {
                    Status = "Incomplete",
                    Items = new List<OrderItem>
                    {
                        new OrderItem
                        {
                            Quantity = 7
                        }
                    }
                };
                dbContext.Attach(newOrder);
                dbContext.SaveChanges();

                newOrder.Status = "Delivered";
                dbContext.SaveChanges();
            }
        }

        private static void ConfigureAuditLog()
        {
            Audit.Core.Configuration.Setup().UseEntityFramework(_ => _.AuditTypeMapper(t => typeof(AuditLog))
                .AuditEntityAction<AuditLog>(
                    (ev, entry, entity) =>
                    {
                        entity.AuditData = entry.ToJson();
                        entity.EntityType = entry.EntityType.Name;
                        entity.AuditDate = DateTime.Now;
                        entity.UserId = new Random().Next(100);
                    }).IgnoreMatchedProperties(true));

        }

        private static DemoContext CreateDemoContext()
        {
            return new DemoContext();
        }
    }
}

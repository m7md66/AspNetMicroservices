using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order() {
                       CreatedBy ="fmis",
                   CreatedDate =DateTime.Now,
                   LastModifiedBy="fmis",
                   LastModifiedDate=DateTime.Now,

                    UserName = "swn",
                    FirstName = "Mehmet",
                    LastName = "Ozkaya",
                    EmailAddress = "ezozkme@gmail.com",
                    AddressLine = "Bahcelievler", 
                    Country = "Turkey",
                    State = "California",
                    ZipCode = "12345",
                      CardName = "John Doe",
                    CardNumber = "1234567890123456",
                        Expiration = "12/25",
                    CVV = "123",
                    TotalPrice = 350
                }
                ,



               new Order()
{
                   CreatedBy ="fmis",
                   CreatedDate =DateTime.Now,
                   LastModifiedBy="fmis",
                   LastModifiedDate=DateTime.Now,
    UserName = "JohnDoe",
    TotalPrice = 100,

    FirstName = "John",
    LastName = "Doe",
    EmailAddress = "johndoe@example.com",
    AddressLine = "123 Main Street",
    Country = "United States",
    State = "California",
    ZipCode = "12345",

    CardName = "John Doe",
    CardNumber = "1234567890123456",
    Expiration = "12/25",
    CVV = "123",
    PaymentMethod = 1
}




        };
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Linq;

namespace DatabasefirstNamespace.Models
{
    internal class Program
    {
        static void Main(string[] args)
        {


            using (OrderDbContext context = new OrderDbContext())
            {


                var items = context.Orders.Include(x => x.Customer).Where(x => x.Customer.Country == "Germany").Include(x => x.OrderItems).ThenInclude(x => x.Product).GroupBy(l => l.Customer).Select(x => x.Select(
                    y => new
                    {
                        x.First().Customer.FirstName,
                        x.First().Customer.LastName,
                        Summe = x.Sum(c => c.TotalAmount),
                        OrderListe = x.ToList(),
                    }).First()).ToList().OrderBy(x => x.LastName).Where(x => x.OrderListe != null);

                foreach (var item in items)
                {
                    Console.WriteLine("\n---------------------------------------------------------\n");
                    Console.WriteLine($"Kunde: {item.FirstName} {item.LastName}");
                    Console.WriteLine($"Gesamter Bestellwert: {item.Summe} Euro");
                    Console.WriteLine("\nBestellungen:");
                    Console.WriteLine("\n-------------\n");
                    foreach (var c2 in item.OrderListe)
                    {
                        Console.WriteLine($"Nr.: {c2.OrderNumber} Datum: {c2.OrderDate.ToString("dd/MM/yyyy")} Bestellwert: {c2.TotalAmount} Euro");


                        Console.Write("Artikel: ");
                        foreach (var item1 in c2.OrderItems.ToList())
                        {
                            Console.Write(item1.Product.ProductName);
                            Console.Write(", ");
                        }
                        Console.WriteLine("\n");
                    }

                }

            }

        }
    }
}

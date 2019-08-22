using System;
using System.Linq;
using Performance.EFCore;

namespace Demos
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AdventureWorksContext())
            {
                var customers = db.Customers.ToList();

                foreach (var customer in customers)
                {
                    Console.WriteLine(customer.Store.Name + " " + customer.Territory.Name);
                }
            }

            Console.ReadLine();
        }
    }
}
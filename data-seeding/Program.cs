using System;
using Performance.EFCore;

namespace Demos
{
    public class Program
    {
        private static void Main()
        {
            using (var db = new AdventureWorksContext())
            {
                // Query with a DbFunction
                var vendors = db.Vendor;

                foreach (var vendor in vendors)
                {
                    Console.WriteLine(vendor.Name);
                }
            }

            Console.ReadLine();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main(string[] args)
        {
            Run();

            Console.ReadLine();
        }

        public static async Task Run()
        {
            var client = new ProductsClient();

            var product = await client.GetProductAsync(1);

            Console.WriteLine(product.ProductName);
        }
    }
}

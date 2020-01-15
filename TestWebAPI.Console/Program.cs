namespace TestWebAPI.Console
{
    using System.Linq;

    using TestWebApi.Data.Contexts;

    using Console = System.Console;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            using (var context = new DbFirstContext())
            {
                var testProducts = context.TestProducts.ToList();
                Console.WriteLine($"{testProducts.Count}");
            }

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }
    }
}

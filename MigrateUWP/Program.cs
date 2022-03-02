using System;

namespace MigrateUWP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataContext.DataSeed.Seed<DataContext.FilamentContext>();
        }
    }
}

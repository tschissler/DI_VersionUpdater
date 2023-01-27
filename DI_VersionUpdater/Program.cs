using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedContracts;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        builder.ConfigureServices(
            services =>
                services.AddScoped<ICounterIncreaser, CounterIncreaserV1.CounterIncreaserV1>());

        var host = builder.Build();

        host.Start();
        
        int counter = 1;

        Console.WriteLine("Hello, to this demonstration of using Dependency Injection for live update of funktionality!");
        Console.WriteLine("--------------------------------------------------------------------------------------------");
        Console.WriteLine();
        Console.WriteLine("Press 'i' to increate the counter.");
        Console.WriteLine("Press 'u' to update to new counter logic.");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Lets get started. Her is the initial value of the counter:");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"Current Counter value: {counter}");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.I)
            {
                counter = host.Services.GetRequiredService<ICounterIncreaser>().IncreaseCounter(counter);
            }
            else if (key.Key == ConsoleKey.U)
            {
                var directory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                var latestAssemblyFile = directory.GetFiles("CounterIncreaserV*.dll").ToList().OrderByDescending(f => f.Name).First();
                var assemblyPath = Path.Combine(directory.FullName, latestAssemblyFile.Name);

                //var types = Assembly.LoadFrom(assemblyPath).GetTypes();
                var types = Assembly.Load(File.ReadAllBytes(assemblyPath)).GetTypes();
                builder = Host.CreateDefaultBuilder(args);

                builder.ConfigureServices(
                    services =>
                        services.AddScoped(typeof(ICounterIncreaser), types[0]));
                host = builder.Build();

                host.Start();
            }
        }
    }
}
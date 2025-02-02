using System.Diagnostics;
using Microsoft.Extensions.Configuration;

if (args.Length == 0)
{
    Console.WriteLine("Error: No mode argument provided.");
    return;
}

var mode = args[0];
var hostname = args.Length > 1 ? args[1] : string.Empty;
var username = args.Length > 2 ? args[2] : string.Empty;
var password = args.Length > 3 ? args[3] : string.Empty;

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = builder.Build();

var command = configuration[$"{mode}:command"];

if (string.IsNullOrEmpty(command))
{
    Console.WriteLine($"Error: Mode '{mode}' not found in configuration.");
    return;
}

var argumentsTemplate = configuration[$"{mode}:arguments"];

var arguments = argumentsTemplate?
    .Replace("{password}", password)
    .Replace("{username}", username)
    .Replace("{hostname}", hostname);

var startInfo = new ProcessStartInfo
{
    FileName = command,
    Arguments = arguments
};

var process = new Process();
process.StartInfo = startInfo;
process.Start();
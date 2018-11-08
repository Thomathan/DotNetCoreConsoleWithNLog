using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace DotNetCoreConsoleWithNLog {
	internal class Program {
		public static async Task Main(string[] args) {
			IHost host = new HostBuilder()
				 .ConfigureHostConfiguration(configHost => {
					 configHost.SetBasePath(Directory.GetCurrentDirectory());
					 configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");
				 })
				 .ConfigureAppConfiguration((hostContext, configApp) => {
					 configApp.SetBasePath(Directory.GetCurrentDirectory());
					 configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");
					 configApp.AddJsonFile($"appsettings.json", true);
				 })
				.ConfigureServices((hostContext, services) => {
					ConfigureNLogServices(services);
				})
				.ConfigureLogging((hostContext, configLogging) => {
					configLogging.AddNLog();
				})
				.UseConsoleLifetime()
				.Build();

			await host.RunAsync();
		}

		public static void ConfigureNLogServices(IServiceCollection services) {
			var path = Path.GetFullPath("nlog.config");
			var baseDir = GetBaseDir();

			NLog.LogManager.LoadConfiguration(path);
			NLog.LogManager.Configuration.Variables["basedir"] = baseDir;

			services.AddLogging((x) => x.SetMinimumLevel(LogLevel.Trace));
			var serviceProvider = services.BuildServiceProvider();

			var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
			loggerFactory.AddNLog();
		}

		private static string GetBaseDir() {
			var isWindowsRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
			var isLinuxRuntime = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

			var baseDir = string.Empty;

			if (isLinuxRuntime) {
				baseDir = "/opt/someDir";
			} else if (isWindowsRuntime) {
				baseDir = "C:\\ProgramData\\SomeFolder";
			}

			return baseDir;
		}
	}
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConsoleWithNLog {
	public class ApplicationLifetimeHostedService : IHostedService {
		private readonly IApplicationLifetime appLifetime;
		private readonly ILogger<ApplicationLifetimeHostedService> logger;
		private readonly IHostingEnvironment environment;
		private readonly IConfiguration configuration;

		public ApplicationLifetimeHostedService(
			IConfiguration configuration,
			IHostingEnvironment environment,
			ILogger<ApplicationLifetimeHostedService> logger,
			IApplicationLifetime appLifetime) {
			this.configuration = configuration;
			this.logger = logger;
			this.appLifetime = appLifetime;
			this.environment = environment;
		}

		public Task StartAsync(CancellationToken cancellationToken) {
			this.logger.LogInformation("StartAsync method called.");

			this.appLifetime.ApplicationStarted.Register(OnStarted);
			this.appLifetime.ApplicationStopping.Register(OnStopping);
			this.appLifetime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private void OnStarted() {
			this.logger.LogInformation("OnStarted method called.");

			// Post-startup code goes here
		}

		private void OnStopping() {
			this.logger.LogInformation("OnStopping method called.");

			// On-stopping code goes here
		}

		private void OnStopped() {
			this.logger.LogInformation("OnStopped method called.");

			// Post-stopped code goes here
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			this.logger.LogInformation("StopAsync method called.");

			return Task.CompletedTask;
		}
	}
}

using System;
using System.Linq;
using FluentAssertions;
using HealthChecks.AzureServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Xunit;

namespace UnitTests.HealthChecks.DependencyInjection.AzureServiceBus
{
   using global::Azure.Identity;

    public class azure_service_bus_deadletter_queue_message_threshold_registration_with_token_should
    {
        [Fact]
        public void add_health_check_when_properly_configured()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddAzureServiceBusDeadLetterQueueMessageCountThreshold("cnn", "queueName",new AzureCliCredential());

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("azuredeadletterqueuethreshold");
            check.GetType().Should().Be(typeof(AzureServiceBusDeadLetterQueueMessageCountThresholdHealthCheck));
        }

        [Fact]
        public void add_named_health_check_when_properly_configured()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddAzureServiceBusDeadLetterQueueMessageCountThreshold("cnn", "queueName",new AzureCliCredential(),
                name: "azureservicebusdeadletterqueuemessagethresholdcheck");

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();
            var check = registration.Factory(serviceProvider);

            registration.Name.Should().Be("azureservicebusdeadletterqueuemessagethresholdcheck");
            check.GetType().Should().Be(typeof(AzureServiceBusDeadLetterQueueMessageCountThresholdHealthCheck));
        }

        [Fact]
        public void fail_when_no_health_check_configuration_provided()
        {
            var services = new ServiceCollection();
            services.AddHealthChecks()
                .AddAzureServiceBusQueueMessageCountThreshold(string.Empty, string.Empty,new AzureCliCredential());

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>();

            var registration = options.Value.Registrations.First();

            Assert.Throws<ArgumentNullException>(() => registration.Factory(serviceProvider));
        }
    }
}
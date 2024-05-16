using ApplicationFormApi.Data;
using Microsoft.Azure.Cosmos;

namespace ApplicationFormApi.Extension
{
    public static class ServiceExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Register CosmosClient as a singleton
            services.AddSingleton<CosmosClient>(sp =>
            {
                // Create and configure the CosmosClient instance
                var accountEndpoint = "https://localhost:8081/";
                var authKeyOrResourceToken = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
                return new CosmosClient(accountEndpoint, authKeyOrResourceToken);
            });

            // Other service registrations...
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();

            // Resolve CosmosClient instance
            using var serviceProvider = services.BuildServiceProvider();
            var cosmosClient = serviceProvider.GetRequiredService<CosmosClient>();

            // Initialize database and container
            InitializeDatabaseAndContainer(cosmosClient).GetAwaiter().GetResult();
        }

        private static async Task InitializeDatabaseAndContainer(CosmosClient cosmosClient)
        {
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(
                id: "ApplicationsDb",
                throughput: 400
                );
            _ = await database.CreateContainerIfNotExistsAsync(
                id: "Applications",
                partitionKeyPath: "/id"
                );
            _ = await database.CreateContainerIfNotExistsAsync(
                id: "Questions",
                partitionKeyPath: "/id"
                );
            _ = await database.CreateContainerIfNotExistsAsync(
             id: "PersonalInformation",
             partitionKeyPath: "/id"
             );
            _ = await database.CreateContainerIfNotExistsAsync(
             id: "AdditionalInformation",
             partitionKeyPath: "/applicantId"
             );
        }

    }

}

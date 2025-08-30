using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace SharedSvc.HealthChecks;

public class CustomHealthCheckOptions
{
    public string ServiceName { get; set; } = string.Empty;
    public string? DatabaseConnectionString { get; set; }
    public string? ElasticsearchUri { get; set; }
    public bool EnableDatabaseCheck { get; set; } = true;
    public bool EnableElasticsearchCheck { get; set; } = true;
}


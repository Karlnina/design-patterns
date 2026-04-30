using Microsoft.OpenApi;
using Bancario.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Patrones de Diseño — API Bancaria",
        Version = "v1",
        Description = """
            API didáctica que demuestra los 14 patrones de diseño (sesiones 3-16) en un flujo bancario real.

            **Cómo usar el modo diagnóstico:**
            Agrega el header `X-Debug-Patterns: true` en cualquier POST para ver
            la traza de todos los patrones que participaron en la ejecución.

            **Cuentas de prueba disponibles:** ACC-001 ($20,000) · ACC-002 ($7,000) · ACC-003 ($1,500) · ACC-004 ($800)
            **Monedas soportadas:** USD · EUR · MXN · COP
            **Gateways:** legacy · modern
            **Prioridad:** fast · standard
            """,
        Contact = new OpenApiContact { Name = "Diplomado Patrones .NET 9" }
    });

    c.AddSecurityDefinition("DebugHeader", new OpenApiSecurityScheme
    {
        Name = "X-Debug-Patterns",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Enviar 'true' para incluir la traza de patrones en la respuesta."
    });
});

builder.Services.AddBankingModule();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patrones Bancarios v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Patrones de Diseño — API Bancaria";
    c.DefaultModelsExpandDepth(1);
    c.DefaultModelExpandDepth(2);
});

app.UseHttpsRedirection();
app.MapBankingEndpoints();

app.Run();

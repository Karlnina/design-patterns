using DependencyInyection.Interfaces;
using DependencyInyection.Models;
using DependencyInyection.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de inyección de dependencias
// Sigleton: una sola instancia compartida(puede ser útil para configuraciones o historiales)
builder.Services.AddSingleton<IConfiguracion, TiendaConfiguracion>();
// Transient: una nueva instancia cada vez que se solicita (ideal para estrategias de pago o notificaciones)
builder.Services.AddTransient<IPagoStrategy, PagoPayPal>(); // Puedes cambiar a PagoPayPal para otra estrategia de pago
builder.Services.AddTransient<INotificador, NotificadorEmail>();
builder.Services.AddTransient<INotificador, NotificadorSMS>();
builder.Services.AddSingleton<IHistoricoPedidos, HistoricoPedidos>();
// Procesador de pedidos (puede ser transient o singleton dependiendo de tus necesidades), se regitra sin interfaz porque es una clase concreta que se inyectará directamente
builder.Services.AddTransient<ProcesadorPedido>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/historicos", () =>
{
    // Resolver el histórico de pedidos desde el contenedor de dependencias
    var historico = app.Services.GetRequiredService<IHistoricoPedidos>();
    var pedidos = historico.ObtenerPedidos();

    return Results.Ok(pedidos);
}).WithName("ObtenerHistoricos");

app.MapPost("/pedidos", (PedidoRequest pedidoRequest) =>
{
    // Crear un pedido con el monto proporcionado
    var pedido = new Pedido { Monto = pedidoRequest.Monto };

    // Resolver el procesador de pedidos desde el contenedor de dependencias
    var procesador = app.Services.GetRequiredService<ProcesadorPedido>();

    // Finalizar el pedido utilizando el procesador
    procesador.FinalizarPedido(pedido);

    return Results.Ok(new
    {
        Mensaje = $"Pedido procesado exitosamente con total de {pedido.Monto:C}",
        status = "success"
    });
})
.WithName("GenerarPedido");


app.Run();

public record PedidoRequest(decimal Monto);
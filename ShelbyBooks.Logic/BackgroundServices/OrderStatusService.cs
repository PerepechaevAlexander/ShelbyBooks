using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShelbyBooks.Data;
using ShelbyBooks.Data.Models;

namespace ShelbyBooks.Logic.BackgroundServices;

public class OrderStatusService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderStatusService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Генерим область видимости
            await using var scope = _serviceProvider.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<ShelbyBooksDbContext>();
            
            var order = await GetOrder(db, stoppingToken);
            if (order == null)
            {
                await Task.Delay(15000, stoppingToken);
            }
            else
            {
                await ChangeStatus(order, db, stoppingToken);
            }

        }
    }

    private async Task<Order?> GetOrder(ShelbyBooksDbContext db, CancellationToken cancellationToken)
    {
        var order = await db.Orders.Where(order => order.Status!.Equals("Новый")).FirstOrDefaultAsync(cancellationToken);
        if (order != null) return order;
        order = await db.Orders.Where(order1 => order1.Status!.Equals("Принят")).FirstOrDefaultAsync(cancellationToken);
        if (order != null) return order;
        order = await db.Orders.Where(order2 => order2.Status!.Equals("В сборке")).FirstOrDefaultAsync(cancellationToken);
        return order ?? null;
    }
    
    private async Task ChangeStatus(Order order, ShelbyBooksDbContext db, CancellationToken cancellationToken)
    {
        
        if (order.Status!.Equals("Новый"))
        {
            order.Status = "Принят";
            await db.SaveChangesAsync(cancellationToken);
            await Task.Delay(5000, cancellationToken);
        }
        if (order.Status.Equals("Принят"))
        {
            order.Status = "В сборке";
            await db.SaveChangesAsync(cancellationToken);
            await Task.Delay(5000, cancellationToken);
        }
        if (order.Status.Equals("В сборке"))
        {
            order.Status = "Готов к выдаче";
            await db.SaveChangesAsync(cancellationToken);
            await Task.Delay(5000, cancellationToken);
        }
    }
}
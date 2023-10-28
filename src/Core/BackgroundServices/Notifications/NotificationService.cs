using Core.Domain.Enums.Subscriptions;
using Core.Entities.Cars;
using Core.Entities.Users;
using Core.Queues;
using Core.Services;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain.Enums;
using Shop.Core.Services;

namespace Core.BackgroundServices.Notifications
{
    public class NotificationService : BackgroundService
    {
        private Timer _timer;
        private readonly ILogger<NotificationService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly INewCarsForSaleQueue _immidiateNotificationQueue;
        private bool _dailyAndWeeklyProcessing;

        public NotificationService(IServiceScopeFactory scopeFactory, INewCarsForSaleQueue immidiateNotificationQueue, ILogger<NotificationService> logger)
        {
            _scopeFactory = scopeFactory;
            _immidiateNotificationQueue = immidiateNotificationQueue;
            _timer = new Timer(async _ => await NotifyDailyAndWeeklyUsersAboutNewCarsAsync(), null, TimeSpan.Zero, TimeSpan.FromHours(24));
            _logger = logger;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var anyNewCarsInQueue = await _immidiateNotificationQueue.AnyNewCarsInQueToReadAsync();

                if (anyNewCarsInQueue)
                {
                    while(_immidiateNotificationQueue.TryRead(out var modelName))
                    {
                        await NotifyImmidiateUsersAsync(modelName);
                    }
                }
            }
        }

        private async Task NotifyDailyAndWeeklyUsersAboutNewCarsAsync()
        {

            if (_dailyAndWeeklyProcessing)
            {
                return;
            }

            _dailyAndWeeklyProcessing = true;

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CarShopDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var now = DateTime.UtcNow;
            var lastWeek = now.AddDays(-7);
            var yesterday = now.AddDays(-1);

            try
            {
                var newCarsSinceLastWeek = await dbContext.Cars.Where(_ => _.CreatedUtc >= lastWeek && _.Status.Equals(Status.Sale)).ToListAsync();

                if (!newCarsSinceLastWeek.Any())
                {
                    return;
                }

                var newCarsSinceYesterDay = newCarsSinceLastWeek.Where(_ => _.CreatedUtc >= yesterday).ToList();

                var usersToNotify = await dbContext.Users.Include(_ => _.Subscription)
                    .Where(_ =>
                    (_.Subscription != null
                    && _.Subscription.IsActive
                    && _.EmailVerified
                    && _.SubscribeToNewCars
                    && ((_.Subscription.NotificationInterval.Equals(NotificationInterval.Daily) && _.LastNotified <= yesterday)
                    || (_.Subscription.NotificationInterval.Equals(NotificationInterval.Weekly) && _.LastNotified <= lastWeek)))
                    )
                    .ToListAsync();

                if (!usersToNotify.Any())
                {
                    return;
                }

                var weeklyUsers = usersToNotify.Where(_ => _.Subscription!.NotificationInterval.Equals(NotificationInterval.Weekly));
                var dailyUsers = usersToNotify.Except(weeklyUsers);

                foreach (var user in weeklyUsers)
                {
                    await HandleEmailNotification(emailService, user, newCarsSinceLastWeek, lastWeek);
                    user.LastNotified = now;
                }

                foreach (var user in dailyUsers)
                {
                    await HandleEmailNotification(emailService, user, newCarsSinceYesterDay, yesterday);
                    user.LastNotified = now;
                }

                await dbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Something went wrong when executing NotifyDailyAndWeeklyUsersAboutNewCarsAsync");
            }
            finally
            {
                _dailyAndWeeklyProcessing = false;
            }
        }

        private async Task NotifyImmidiateUsersAsync(string modelName)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CarShopDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                var immidiateUsers = await dbContext.Users.Include(_ => _.Subscription)
                .Where(_ => _.EmailVerified
                    && _.SubscribeToNewCars
                    && _.Subscription != null && _.Subscription.IsActive && _.Subscription.NotificationInterval.Equals(NotificationInterval.Immidiate))
                    .ToListAsync();

                if (!immidiateUsers.Any())
                {
                    return;
                }

                foreach (var user in immidiateUsers)
                {
                    await HandleImmidiateEmailNotification(emailService, user, modelName);
                    user.LastNotified = DateTime.UtcNow;
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something went wrong when executing NotifyImmidiateUsersAsync");
            }
        }

        private async Task HandleEmailNotification(IEmailService emailService, User user, IEnumerable<Car> cars, DateTime fromDate)
        {
            await emailService.SendEmailAsync("PontusAndEric@TheCarShop.com", user.Email, "New cars for sale", EmailMessages.NewCarsNotificationEmail(user.FirstName, cars.Select(_ => _.ModelName), fromDate.ToString("yyyy-MM-dd")));
        }

        private async Task HandleImmidiateEmailNotification(IEmailService emailService, User user, string modelName)
        {
            await emailService.SendEmailAsync("PontusAndEric@TheCarShop.com", user.Email, "New car for sale", EmailMessages.NewCarsImmidiateNotificationEmail(user.FirstName, modelName));
        }
    }
}

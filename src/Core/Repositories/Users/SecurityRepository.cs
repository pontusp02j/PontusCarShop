using AutoMapper;
using Core.Entities.Security;
using Core.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Repositories.Users
{
    public interface ISecurityRepository
    {
        Task AddFailedLoginAttemptAsync(FailedLoginAttempt failedLoginAttempt);
        Task AddRestrictedIpAddressAsync(RestrictedIpAddress restrictedIpAddress);
        Task RemoveFailedLoginAttemptsForIpAsync(string ip);
        Task RemoveRestrictedIpAddressesForIpAsync(string ip);
        Task<List<FailedLoginAttempt>> GetFailedLoginAttemptsByIpAsync(string ip);
        Task<bool> IsIpAddressRestrictedAsync(string ip);
        Task<bool> IsLastFailedAttemptBeforeRestrictionForIp(string ip);
        Task<int> SaveSecurityChangesAsync();
    }

    public class SecurityRepository : RepositoryBase<FailedLoginAttempt>, ISecurityRepository
    {
        public SecurityRepository(CarShopDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task AddFailedLoginAttemptAsync(FailedLoginAttempt failedLoginAttempt)
        {
            Create(failedLoginAttempt);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRestrictedIpAddressAsync(RestrictedIpAddress restrictedIpAddress)
        {
            _dbContext.RestrictedIpAddresses.Add(restrictedIpAddress);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<FailedLoginAttempt>> GetFailedLoginAttemptsByIpAsync(string ip)
        {
            return await _dbContext.FailedLoginAttempts
                .Where(fla => fla.Ip == ip)
                .ToListAsync();
        }


        public async Task<bool> IsIpAddressRestrictedAsync(string ip)
        {
            return await _dbContext.RestrictedIpAddresses
                .AnyAsync(rip => rip.IpAddress == ip && rip.RestrictionExpiresUtc > DateTime.UtcNow);
        }


        public async Task RemoveFailedLoginAttemptsForIpAsync(string ip)
        {
            var failedLoginAttempts = await _dbContext.FailedLoginAttempts
                .Where(fla => fla.Ip == ip)
                .ToListAsync();

            if (failedLoginAttempts.Count > 0)
            {
                _dbContext.FailedLoginAttempts.RemoveRange(failedLoginAttempts);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveRestrictedIpAddressesForIpAsync(string ip)
        {
            var restrictedIps = await _dbContext.RestrictedIpAddresses
                .Where(rip => rip.IpAddress == ip).ToListAsync();

            if (restrictedIps.Any())
            {
                _dbContext.RestrictedIpAddresses.RemoveRange(restrictedIps);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsLastFailedAttemptBeforeRestrictionForIp(string ip)
        {
            return await _dbContext.FailedLoginAttempts.Where(_ => _.Ip == ip && _.AttemptTime >= DateTime.UtcNow.AddMinutes(-3)).CountAsync() > 1;
        }

        public async Task<int> SaveSecurityChangesAsync()
        {
            return await SaveChangesAsync();
        }
    }
}

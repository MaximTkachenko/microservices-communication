using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Portal.Db;

namespace Portal.Services
{
    public class DbTokenCache : IDbTokenCache
    {
        private readonly PortalDb _db;
        private readonly IHttpContextAccessor _httpAccessor;

        public DbTokenCache(PortalDb db,
            IHttpContextAccessor httpAccessor)
        {
            _db = db;
            _httpAccessor = httpAccessor;
        }

        public void ConfigureCache(IConfidentialClientApplication application)
        {
            application.UserTokenCache.SetBeforeAccessAsync(OnBeforeAccessAsync);
            application.UserTokenCache.SetAfterAccessAsync(OnAfterAccessAsync);
        }

        public Task RemoveCacheAsync()
        {
            //todo
            return Task.CompletedTask;
        }

        private async Task OnAfterAccessAsync(TokenCacheNotificationArgs args)
        {
            if (args.HasStateChanged)
            {
                string cacheKey = GetCacheKey();
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    await WriteCacheBytesAsync(cacheKey, args.TokenCache.SerializeMsalV3());
                }
            }
        }

        private async Task OnBeforeAccessAsync(TokenCacheNotificationArgs args)
        {
            string cacheKey = GetCacheKey();

            if (!string.IsNullOrEmpty(cacheKey))
            {
                byte[] tokenCacheBytes = await ReadCacheBytesAsync(cacheKey);
                if (tokenCacheBytes.Length == 0)
                {
                    return;
                }
                args.TokenCache.DeserializeMsalV3(tokenCacheBytes, shouldClearExistingCache: true);
            }
        }

        private string GetCacheKey() => _httpAccessor.HttpContext.User.GetMsalAccountId();

        private async Task<byte[]> ReadCacheBytesAsync(string cacheKey)
        {
            var existingRecord = await _db.CachedTokens.FirstOrDefaultAsync(x => x.AccountId == cacheKey);
            if (existingRecord == null)
            {
                return Array.Empty<byte>();
            }

            return existingRecord.Token;
        }

        private async Task WriteCacheBytesAsync(string cacheKey, byte[] token)
        {
            var tokenRecord = await _db.CachedTokens.FirstOrDefaultAsync(x => x.AccountId == cacheKey);
            if (tokenRecord == null)
            {
                tokenRecord = new CachedToken
                {
                    AccountId = cacheKey,
                    Timestamp = DateTime.UtcNow
                };
                _db.CachedTokens.Add(tokenRecord);
            }

            tokenRecord.Token = token;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //token is already set, no need to set it again
            }
            catch (DbUpdateException)
            {
                //todo need to check whether it's PK constraint violation
            }
        }
    }
}

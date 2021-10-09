using ERP.DataAccess.Entity;
using ERP.Models.Admin;
using ERP.Models.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.DataAccess.EntityData
{
    public partial class ErpDbContext : IdentityDbContext<ApplicationIdentityUser, ApplicationRole, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public ErpDbContext(
                DbContextOptions<ErpDbContext> options,
                IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// override save changes async
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //try
            //{
            UserSessionModel userSessionModel = SessionExtension.GetComplexData<UserSessionModel>(_session, "UserSession");
            var changeSet = ChangeTracker.Entries<IAuditable>();
            if (null != changeSet)
            {
                foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            if (entry.Entity.GetType().GetProperty("PreparedByUserId") != null) entry.Property("PreparedByUserId").CurrentValue = userSessionModel.UserId;
                            if (entry.Entity.GetType().GetProperty("PreparedDateTime") != null) entry.Property("PreparedDateTime").CurrentValue = DateTime.Now;
                            if (entry.Entity.GetType().GetProperty("UpdatedByUserId") != null) entry.Property("UpdatedByUserId").CurrentValue = userSessionModel.UserId;
                            if (entry.Entity.GetType().GetProperty("UpdatedDateTime") != null) entry.Property("UpdatedDateTime").CurrentValue = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            if (entry.Entity.GetType().GetProperty("UpdatedByUserId") != null) entry.Property("UpdatedByUserId").CurrentValue = userSessionModel.UserId;
                            if (entry.Entity.GetType().GetProperty("UpdatedDateTime") != null) entry.Property("UpdatedDateTime").CurrentValue = DateTime.Now;
                            break;
                        default:
                            // dont' update anything.
                            break;
                    }
                }
            }

            return await base.SaveChangesAsync(true, cancellationToken);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
    }
}
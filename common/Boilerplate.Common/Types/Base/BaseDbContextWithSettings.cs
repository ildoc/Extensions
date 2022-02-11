﻿using Boilerplate.Common.Settings;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Common.Types.Base
{
    public class BaseDbContextWithSettings : BaseDbContext, IDbContextWithSettings
    {
        protected BaseDbContextWithSettings(DbContextOptions options) : base(options) { }

        public DbSet<ApplicationSetting> ApplicationSettings { get; set; }
    }
}

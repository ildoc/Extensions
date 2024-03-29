﻿using Infrastructure.Types.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Settings
{
    public class ApplicationSettingsManager<T> : IApplicationSettingsManager where T : DbContext, IDbContextWithSettings
    {
        private readonly T _context;

        public ApplicationSettingsManager(T context)
        {
            _context = context;
        }

        public string GetSetting(string key, string ifDefault = default) =>
            _context.ApplicationSettings.FirstOrDefault(x => x.Key == key)?.Value ?? ifDefault;

        public async Task<string> SaveSetting(string key, string value)
        {
            var setting = _context.ApplicationSettings.FirstOrDefault(x => x.Key == key);
            if (setting == default)
            {
                setting = new ApplicationSetting { Key = key, Value = value };
                _context.ApplicationSettings.Add(setting);
            }
            else
            {
                setting.Value = value;
                _context.ApplicationSettings.Update(setting);
            }

            await _context.SaveChangesAsync();
            return setting.Value;
        }

        public bool Exists(string key) =>
            GetSetting(key) != default;
    }
}

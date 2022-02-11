using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Boilerplate.Common.Settings;
using Extensions;

namespace Boilerplate.Common.DbPatch
{
    public abstract class PatcherBase : IPatcher
    {
        private readonly IApplicationSettingsManager _settingsManager;
        private readonly Version _dbVersion;
        public abstract Version Version { get; }

        protected PatcherBase(IApplicationSettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            _dbVersion = new Version(_settingsManager.GetSetting(ApplicationSettingsConsts.DBVERSION) ?? Version?.ToString() ?? "0.0.0");
        }

        private async Task<Version> UpdateVersion() =>
            new Version(await _settingsManager.SaveSetting(ApplicationSettingsConsts.DBVERSION, Version.ToString()));

        public async Task<Version> ApplyPatches()
        {
            foreach (var (version, patch) in _patches.Where(x => x.version > _dbVersion && x.version <= Version).OrderBy(x => x.version))
            {
                patch().Wait();
            }

            return await UpdateVersion();
        }

        private readonly List<(Version version, Func<Task> patch)> _patches = new List<(Version version, Func<Task> patch)>();

        public void QueuePatch(Func<Task> patch)
        {
            var regex = Regex.Match(patch.Method.Name, @"^Patch([\d]+)_([\d]+)_([\d]+)$");

            if (!regex.Success)
                throw new Exception("Cannot add patch to queue");

            var version = new Version(regex.Groups[1].Value.ToInt32(), regex.Groups[2].Value.ToInt32(), regex.Groups[3].Value.ToInt32());

            _patches.Add((version, patch));
        }
    }
}

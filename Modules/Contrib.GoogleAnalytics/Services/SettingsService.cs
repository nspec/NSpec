using System;
using System.Linq;
using Contrib.GoogleAnalytics.Models;
using Orchard.Caching;
using Orchard.Data;

namespace Contrib.GoogleAnalytics.Services {
    public class SettingsService : ISettingsService {
        private const string DefaultScript = "";

        private readonly IRepository<SettingsRecord> _repository;
        private readonly ISignals _signals;

        public SettingsService(
            IRepository<SettingsRecord> repository,
            ISignals signals) {
            _repository = repository;
            _signals = signals;
        }

        public SettingsRecord Get() {
            var record = _repository.Table.FirstOrDefault();

            if(record == null) {
                record = new SettingsRecord {
                                Enable = false,
                                Script = DefaultScript
                            };

                _repository.Create(record);
            }

            return record;
        }

        public bool Set(bool enable, string script) {
            var settings = Get();

            settings.Enable = enable;
            settings.Script = script;

            _signals.Trigger("GoogleAnalytics.SettingsChanged");
            
            return true;
        }
    }
}
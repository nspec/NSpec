using System;
using System.Linq;
using System.Collections.Generic;
using Heikura.Orchard.Modules.SyntaxHighlighter.Records;
using Orchard;
using Orchard.Data;
using Orchard.Localization;
using Orchard.UI.Notify;

namespace Heikura.Orchard.Modules.SyntaxHighlighter.Services
{
    public interface ISyntaxHighlighterService : IDependency {
        IEnumerable<string> GetSupportedThemes();
        void SetCurrentTheme(string themeName);
        string GetCurrentTheme();
    }

    public class SyntaxHighlighterService : ISyntaxHighlighterService {
        private readonly IRepository<SettingsRecord> _repository;
        private readonly IRepository<ThemeRecord> _themeRepository;
        private readonly IOrchardServices _services;

        public SyntaxHighlighterService(IRepository<SettingsRecord> repository, IRepository<ThemeRecord> themeRepository, IOrchardServices services) {
            _repository = repository;
            _themeRepository = themeRepository;
            _services = services;
            T = NullLocalizer.Instance;
        }

        public Localizer T {
            get;
            set;
        }

        public IEnumerable<string> GetSupportedThemes() {

            var themes = new List<string>();
            try {
                var themesFromDatabase = _themeRepository.Table.Take(20);

                themes.AddRange(themesFromDatabase.Select(t => t.ThemeName));
            }
            catch (Exception) {
                themes.Add("shThemeDefault.css");
            }

            return themes;
        }

        public void SetCurrentTheme(string themeName) {
            try {
                var current = _repository.Table.SingleOrDefault();

                if (current == null) {
                    current = new SettingsRecord();
                    _repository.Create(current);
                }

                current.CurrentThemeName = themeName;

            }
            catch {
                _services.Notifier.Add(NotifyType.Warning, T("There is problem setting current syntax highlighter theme in database. Try reinstalling the module."));
            }
        }


        public string GetCurrentTheme() {
            try {
                var current = _repository.Table.SingleOrDefault();

                if (current == null) {
                    return "shThemeDefault.css";
                }

                return current.CurrentThemeName;
            }
            catch {
                return "shThemeDefault.css";
            }
        }
    }
}
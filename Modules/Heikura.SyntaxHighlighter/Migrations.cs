using System.Collections.Generic;
using Heikura.Orchard.Modules.SyntaxHighlighter.Records;
using Orchard.Data;
using Orchard.Data.Migration;

namespace Heikura.Orchard.Modules.SyntaxHighlighter {
    public class Migrations : DataMigrationImpl {
        private readonly IRepository<ThemeRecord> _themeRepository;

        public Migrations(IRepository<ThemeRecord> themeRepository) {
            _themeRepository = themeRepository;
        }

        public int Create() {
            SchemaBuilder.CreateTable("SyntaxHighlighterSettingsRecord",
             table => table
                 .Column<int>("Id", column => column.PrimaryKey().Identity())
                 .Column<string>("CurrentThemeName", column => column.WithDefault("shThemeDefault.css").WithLength(100))
             );

            SchemaBuilder.CreateTable("SyntaxHighlighterThemeRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("ThemeName", column => column.WithLength(100))
                    .Column<string>("Author", column => column.WithLength(40))
                );

            return 1;
        }

        public int UpdateFrom1() {
            SchemaBuilder.DropTable("SyntaxHighlighterSettingsRecord");
            SchemaBuilder.DropTable("SyntaxHighlighterThemeRecord");

            SchemaBuilder.CreateTable("SettingsRecord",
             table => table
                 .Column<int>("Id", column => column.PrimaryKey().Identity())
                 .Column<string>("CurrentThemeName", column => column.WithDefault("shThemeDefault.css").WithLength(100))
             );

            SchemaBuilder.CreateTable("ThemeRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<string>("ThemeName", column => column.WithLength(100))
                    .Column<string>("Source", column => column.WithLength(100))
                );

            // seed the initial themes
            var themes = new List<string>() {
                "shThemeDefault.css",
                "shThemeDjango.css",
                "shThemeEclipse.css",
                "shThemeEmacs.css",
                "shThemeFadeToGrey.css",
                "shThemeMDUltra.css",
                "shThemeMidnight.css",
                "shThemeRDark.css"
            };

            foreach (var themeName in themes) {
                var themeRecord = new ThemeRecord() {
                    Source = "http://alexgorbatchev.com/SyntaxHighlighter/",
                    ThemeName = themeName
                };

                _themeRepository.Create(themeRecord);
            }


            return 2;
        }
    }
}
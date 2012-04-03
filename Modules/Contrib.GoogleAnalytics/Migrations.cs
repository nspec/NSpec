using System;
using Orchard.Data.Migration;

namespace Contrib.GoogleAnalytics {
    public class Migrations : DataMigrationImpl {
        public int Create() {
            SchemaBuilder.CreateTable("SettingsRecord",
                table => table
                    .Column<int>("Id", column => column.PrimaryKey().Identity())
                    .Column<bool>("Enable", column => column.NotNull().WithDefault(false))
                    .Column<string>("Script", column => column.NotNull().Unlimited().WithDefault(""))
                );

            return 1;
        }
    }
}

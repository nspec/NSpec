using Orchard.Data.Migration;

namespace Vandelay.Favicon {
    public class Migrations : DataMigrationImpl {
    
        public int Create() {
            SchemaBuilder.CreateTable("FaviconSettingsPartRecord", table => table
                .ContentPartRecord()
                .Column<string>("FaviconUrl")
               );
            return 1;
        }
    }
}
using FluentMigrator;

namespace Excursions.Migrator.Migrations;

[TimestampedMigration(2022, 05, 02, 23, 00)]
public class AddSchema : MigrationBase
{
    public override void Up()
    {
        Create.Schema("excursion");
    }
}
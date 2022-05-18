using FluentMigrator;

namespace Excursions.Migrator.Migrations;

[TimestampedMigration(2022, 05, 02, 23, 01)]
public class AddExcursion : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "excursion",
            "Excursion",
            table =>
            {
                table.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("PK_Excursion").Identity();
                table.WithColumn("Name").AsString(64).NotNullable();
                table.WithColumn("Description").AsString(512).Nullable();
                table.WithColumn("DateTimeUtc").AsDateTime().NotNullable();
                table.WithColumn("PlacesCount").AsInt32().NotNullable();
                table.WithColumn("PricePerPlace").AsDecimal().Nullable();
                table.WithColumn("GuideId").AsString(64).NotNullable();
                table.WithColumn("Status").AsString(64).NotNullable();
                table.WithColumn("CreateDateTimeUtc").AsDateTime().NotNullable();
                table.WithColumn("UpdateDateTimeUtc").AsDateTime().Nullable();
            });
        
        CreateIndexIfNotExists(
            "excursion",
            "Excursion",
            "IX_Excursion_GuideId",
            index => index
                .OnColumn("GuideId").Ascending()
                .WithOptions()
                .NonClustered());
    }
}
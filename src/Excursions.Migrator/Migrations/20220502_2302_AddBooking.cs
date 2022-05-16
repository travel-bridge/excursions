using FluentMigrator;

namespace Excursions.Migrator.Migrations;

[TimestampedMigration(2022, 05, 02, 23, 02)]
public class AddBooking : MigrationBase
{
    public override void Up()
    {
        CreateTableIfNotExists(
            "excursion",
            "Booking",
            table =>
            {
                table.WithColumn("Id").AsInt32().NotNullable().PrimaryKey("PK_Booking").Identity();
                table.WithColumn("TouristId").AsString(64).NotNullable();
                table.WithColumn("Status").AsString(64).NotNullable();
                table.WithColumn("CreateDateTimeUtc").AsDateTime().NotNullable();
                table.WithColumn("UpdateDateTimeUtc").AsDateTime().Nullable();
                table.WithColumn("ExcursionId").AsInt32().NotNullable();
            });
        
        CreateForeignKeyIfNotExists(
            "excursion",
            "Booking",
            "FK_Booking_ExcursionId",
            constraint => constraint
                .FromTable("Booking").InSchema("excursion").ForeignColumn("ExcursionId")
                .ToTable("Excursion").InSchema("excursion").PrimaryColumn("Id"));
        
        CreateIndexIfNotExists(
            "excursion",
            "Booking",
            "IX_Booking_ExcursionId",
            index => index
                .OnColumn("ExcursionId").Ascending()
                .WithOptions()
                .NonClustered());
        
        CreateIndexIfNotExists(
            "excursion",
            "Booking",
            "IX_Booking_TouristId",
            index => index
                .OnColumn("TouristId").Ascending()
                .WithOptions()
                .NonClustered());
        
        CreateIndexIfNotExists(
            "excursion",
            "Booking",
            "IX_Booking_CreateDateTimeUtc",
            index => index
                .OnColumn("CreateDateTimeUtc").Ascending()
                .WithOptions()
                .NonClustered());
    }
}
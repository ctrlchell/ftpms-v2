using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ftpms.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MeasurementDataJson",
                table: "Measurements");

            migrationBuilder.AddColumn<decimal>(
                name: "Ankle",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ArmRound",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Bicep",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BustPoint",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Chest",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTaken",
                table: "Measurements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "GownLength",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Hip",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Inseam",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Measurements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Knee",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Neck",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Measurements",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentMeasurementId",
                table: "Measurements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RoundSleeve",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Shoulder",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SkirtLength",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SleeveLength",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemplateType",
                table: "Measurements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Thigh",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TopLength",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TrouserLength",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnderBust",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Measurements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Waist",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Wrist",
                table: "Measurements",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_CustomerId_TemplateType_Version",
                table: "Measurements",
                columns: new[] { "CustomerId", "TemplateType", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ParentMeasurementId",
                table: "Measurements",
                column: "ParentMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Customers_CustomerId",
                table: "Measurements",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_Measurements_ParentMeasurementId",
                table: "Measurements",
                column: "ParentMeasurementId",
                principalTable: "Measurements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Customers_CustomerId",
                table: "Measurements");

            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_Measurements_ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_CustomerId_TemplateType_Version",
                table: "Measurements");

            migrationBuilder.DropIndex(
                name: "IX_Measurements_ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Ankle",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ArmRound",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Bicep",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "BustPoint",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Chest",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "DateTaken",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "GownLength",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Hip",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Inseam",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Knee",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Neck",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "ParentMeasurementId",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "RoundSleeve",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Shoulder",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "SkirtLength",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "SleeveLength",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "TemplateType",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Thigh",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "TopLength",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "TrouserLength",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "UnderBust",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Waist",
                table: "Measurements");

            migrationBuilder.DropColumn(
                name: "Wrist",
                table: "Measurements");

            migrationBuilder.AddColumn<string>(
                name: "MeasurementDataJson",
                table: "Measurements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

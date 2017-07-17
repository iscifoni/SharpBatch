using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SharpBatch.Tracking.DB.Migrations
{
    public partial class AddExceptionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SB_Tracking_Exception",
                columns: table => new
                {
                    ExceptionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingModelTrackingId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_Tracking_Exception", x => x.ExceptionId);
                    table.ForeignKey(
                        name: "FK_SB_Tracking_Exception_SB_Tracking_Tracking_TrackingModelTrackingId",
                        column: x => x.TrackingModelTrackingId,
                        principalTable: "SB_Tracking_Tracking",
                        principalColumn: "TrackingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SB_Tracking_Exception_TrackingModelTrackingId",
                table: "SB_Tracking_Exception",
                column: "TrackingModelTrackingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SB_Tracking_Exception");
        }
    }
}

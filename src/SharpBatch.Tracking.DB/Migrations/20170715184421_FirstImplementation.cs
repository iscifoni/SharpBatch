using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SharpBatch.Tracking.DB.Migrations
{
    public partial class FirstImplementation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SB_Tracking_Tracking",
                columns: table => new
                {
                    TrackingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BatchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_Tracking_Tracking", x => x.TrackingId);
                    table.UniqueConstraint("AK_SB_Tracking_Tracking_SessionId", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "SB_Tracking_Message",
                columns: table => new
                {
                    MessageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Message = table.Column<string>(type: "varchar(500)", nullable: true),
                    TrackingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_Tracking_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_SB_Tracking_Message_SB_Tracking_Tracking_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "SB_Tracking_Tracking",
                        principalColumn: "TrackingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SB_Tracking_Ping",
                columns: table => new
                {
                    PingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PingData = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrackingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SB_Tracking_Ping", x => x.PingId);
                    table.ForeignKey(
                        name: "FK_SB_Tracking_Ping_SB_Tracking_Tracking_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "SB_Tracking_Tracking",
                        principalColumn: "TrackingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SB_Tracking_Message_TrackingId",
                table: "SB_Tracking_Message",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_SB_Tracking_Ping_TrackingId",
                table: "SB_Tracking_Ping",
                column: "TrackingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SB_Tracking_Message");

            migrationBuilder.DropTable(
                name: "SB_Tracking_Ping");

            migrationBuilder.DropTable(
                name: "SB_Tracking_Tracking");
        }
    }
}

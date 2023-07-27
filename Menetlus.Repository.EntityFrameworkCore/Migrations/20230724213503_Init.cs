using System;
using Menetlus.Domain;
using Menetlus.External.Contracts;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menetlus.Repository.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "order_id_seq");

            migrationBuilder.CreateTable(
                name: "menetlus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('order_id_seq')"),
                    Avaldaja = table.Column<Avaldaja>(type: "jsonb", nullable: false),
                    Kusimus = table.Column<string>(type: "text", nullable: false),
                    Markus = table.Column<string>(type: "text", nullable: false),
                    Staatus = table.Column<int>(type: "integer", nullable: false),
                    Vastus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menetlus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "outbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Payload = table.Column<Envelope>(type: "jsonb", nullable: false),
                    RoutingKey = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "menetlus");

            migrationBuilder.DropTable(
                name: "outbox");

            migrationBuilder.DropSequence(
                name: "order_id_seq");
        }
    }
}

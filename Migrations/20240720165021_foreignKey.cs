using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp1.Migrations
{
    public partial class foreignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Birth = table.Column<DateTime>(type: "date", nullable: false),
                    Phone_Number = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Client_Id);
                });

            migrationBuilder.CreateTable(
                name: "Extra_Services",
                columns: table => new
                {
                    Services_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "varchar(225)", unicode: false, maxLength: 225, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Extra_Se__A74BF87401B9C26D", x => x.Services_Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    Hotel_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Owner = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Stars = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Phone_Number = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    CheckinTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckoutTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.Hotel_Id);
                });

            migrationBuilder.CreateTable(
                name: "Room_Type",
                columns: table => new
                {
                    Type_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room_Typ__FE90DD9E40A19413", x => x.Type_Id);
                });

            migrationBuilder.CreateTable(
                name: "Browsing_Data",
                columns: table => new
                {
                    Browsing_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action_type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Time = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Browsing__6FAAE20268C01713", x => x.Browsing_Id);
                    table.ForeignKey(
                        name: "FK_Browsing_Data_Client",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    TokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Client_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Room_Number = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Hotel_Id = table.Column<int>(type: "int", nullable: false),
                    Type_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Room__353A906E6DAAB576", x => x.Room_Number);
                    table.ForeignKey(
                        name: "FK__Room__Hotel_Id__5070F446",
                        column: x => x.Hotel_Id,
                        principalTable: "Hotel",
                        principalColumn: "Hotel_Id");
                    table.ForeignKey(
                        name: "FK__Room__Type_Id__5165187F",
                        column: x => x.Type_Id,
                        principalTable: "Room_Type",
                        principalColumn: "Type_Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Reservation_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reservation_Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Check_in_date = table.Column<DateTime>(type: "date", nullable: false),
                    Check_out_date = table.Column<DateTime>(type: "date", nullable: false),
                    Total_Price = table.Column<double>(type: "float", nullable: false),
                    Client_Id = table.Column<int>(type: "int", nullable: false),
                    Room_Number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Reservation_Id);
                    table.ForeignKey(
                        name: "FK__Reservati__Clien__5441852A",
                        column: x => x.Client_Id,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                    table.ForeignKey(
                        name: "FK__Reservati__Room___5535A963",
                        column: x => x.Room_Number,
                        principalTable: "Room",
                        principalColumn: "Room_Number");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Notification_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sender_Client_Id = table.Column<int>(type: "int", nullable: false),
                    Receiver_Client_Id = table.Column<int>(type: "int", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false),
                    Reservation_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Notification_Id);
                    table.ForeignKey(
                        name: "FK__Notificat__Recei__60A75C0F",
                        column: x => x.Receiver_Client_Id,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                    table.ForeignKey(
                        name: "FK__Notificat__Reser__5EBF139D",
                        column: x => x.Reservation_Id,
                        principalTable: "Reservation",
                        principalColumn: "Reservation_Id");
                    table.ForeignKey(
                        name: "FK__Notificat__Sende__5FB337D6",
                        column: x => x.Sender_Client_Id,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservation_Services",
                columns: table => new
                {
                    ReservationServices_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reservation_Id = table.Column<int>(type: "int", nullable: false),
                    Services_Id = table.Column<int>(type: "int", nullable: false),
                    Client_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reservat__89ECE05356065195", x => x.ReservationServices_Id);
                    table.ForeignKey(
                        name: "FK__Reservati__Clien__5BE2A6F2",
                        column: x => x.Client_Id,
                        principalTable: "Client",
                        principalColumn: "Client_Id");
                    table.ForeignKey(
                        name: "FK__Reservati__Reser__59FA5E80",
                        column: x => x.Reservation_Id,
                        principalTable: "Reservation",
                        principalColumn: "Reservation_Id");
                    table.ForeignKey(
                        name: "FK__Reservati__Servi__5AEE82B9",
                        column: x => x.Services_Id,
                        principalTable: "Extra_Services",
                        principalColumn: "Services_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Browsing_Data_ClientId",
                table: "Browsing_Data",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "UQ__Client__A9D10534E9128A8D",
                table: "Client",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Receiver_Client_Id",
                table: "Notification",
                column: "Receiver_Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Reservation_Id",
                table: "Notification",
                column: "Reservation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Sender_Client_Id",
                table: "Notification",
                column: "Sender_Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ClientId",
                table: "RefreshTokens",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Client_Id",
                table: "Reservation",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Room_Number",
                table: "Reservation",
                column: "Room_Number");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Services_Client_Id",
                table: "Reservation_Services",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Services_Reservation_Id",
                table: "Reservation_Services",
                column: "Reservation_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_Services_Services_Id",
                table: "Reservation_Services",
                column: "Services_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Hotel_Id",
                table: "Room",
                column: "Hotel_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Room_Type_Id",
                table: "Room",
                column: "Type_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Browsing_Data");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reservation_Services");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Extra_Services");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "Hotel");

            migrationBuilder.DropTable(
                name: "Room_Type");
        }
    }
}

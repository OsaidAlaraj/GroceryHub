using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceryHub.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<int>(
                name: "Address",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CartID",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Advertisment",
                columns: table => new
                {
                    AdvertismentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertismentName = table.Column<string>(nullable: true),
                    AdvertismentPhoto = table.Column<byte[]>(nullable: true),
                    AdvertismentDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisment", x => x.AdvertismentID);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_Cart_AspNetUsers_AppUserID",
                        column: x => x.AppUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: false),
                    Icon = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(nullable: false),
                    Location = table.Column<int>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    Website = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    OpenHour = table.Column<DateTime>(nullable: false),
                    CloseHour = table.Column<DateTime>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Item_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlyerReader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    StoreID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlyerReader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlyerReader_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offer",
                columns: table => new
                {
                    OfferID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    OfferDescription = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false),
                    StoreID = table.Column<int>(nullable: false),
                    ItemID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offer", x => x.OfferID);
                    table.ForeignKey(
                        name: "FK_Offer_Item_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offer_Store_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Store",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ByteWraper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    photo = table.Column<byte[]>(nullable: true),
                    FlyerReaderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ByteWraper", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ByteWraper_FlyerReader_FlyerReaderId",
                        column: x => x.FlyerReaderId,
                        principalTable: "FlyerReader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlyerOffer",
                columns: table => new
                {
                    FlyerOfferId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    StoreID = table.Column<int>(nullable: false),
                    flyerphoto = table.Column<byte[]>(nullable: true),
                    recognized = table.Column<bool>(nullable: false),
                    FlyerReader = table.Column<int>(nullable: true),
                    FlyerOffer = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlyerOffer", x => x.FlyerOfferId);
                    table.ForeignKey(
                        name: "FK_FlyerOffer_FlyerReader_FlyerReader",
                        column: x => x.FlyerReader,
                        principalTable: "FlyerReader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferID = table.Column<int>(nullable: false),
                    CartID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItemID);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartID",
                        column: x => x.CartID,
                        principalTable: "Cart",
                        principalColumn: "CartID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Offer_OfferID",
                        column: x => x.OfferID,
                        principalTable: "Offer",
                        principalColumn: "OfferID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoubleWraper",
                columns: table => new
                {
                    DoubleWraperID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    num = table.Column<double>(nullable: false),
                    FlyerOfferId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoubleWraper", x => x.DoubleWraperID);
                    table.ForeignKey(
                        name: "FK_DoubleWraper_FlyerOffer_FlyerOfferId",
                        column: x => x.FlyerOfferId,
                        principalTable: "FlyerOffer",
                        principalColumn: "FlyerOfferId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ByteWraper_FlyerReaderId",
                table: "ByteWraper",
                column: "FlyerReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_AppUserID",
                table: "Cart",
                column: "AppUserID",
                unique: true,
                filter: "[AppUserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                column: "CartID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_OfferID",
                table: "CartItem",
                column: "OfferID");

            migrationBuilder.CreateIndex(
                name: "IX_DoubleWraper_FlyerOfferId",
                table: "DoubleWraper",
                column: "FlyerOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_FlyerOffer_FlyerReader",
                table: "FlyerOffer",
                column: "FlyerReader");

            migrationBuilder.CreateIndex(
                name: "IX_FlyerReader_StoreID",
                table: "FlyerReader",
                column: "StoreID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryID",
                table: "Item",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_ItemID",
                table: "Offer",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_StoreID",
                table: "Offer",
                column: "StoreID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisment");

            migrationBuilder.DropTable(
                name: "ByteWraper");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "DoubleWraper");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Offer");

            migrationBuilder.DropTable(
                name: "FlyerOffer");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "FlyerReader");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CartID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}

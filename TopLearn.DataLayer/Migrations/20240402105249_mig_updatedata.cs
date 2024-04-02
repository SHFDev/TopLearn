using Microsoft.EntityFrameworkCore.Migrations;

namespace TopLearn.DataLayer.Migrations
{
    public partial class mig_updatedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permission_Permission_ParentID",
                table: "Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Permission",
                table: "Permission");

            migrationBuilder.RenameTable(
                name: "RolePermission",
                newName: "rolepermission");

            migrationBuilder.RenameTable(
                name: "Permission",
                newName: "permission");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_RoleId",
                table: "rolepermission",
                newName: "IX_rolepermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RolePermission_PermissionId",
                table: "rolepermission",
                newName: "IX_rolepermission_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_Permission_ParentID",
                table: "permission",
                newName: "IX_permission_ParentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_rolepermission",
                table: "rolepermission",
                column: "Rp_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_permission",
                table: "permission",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_permission_permission_ParentID",
                table: "permission",
                column: "ParentID",
                principalTable: "permission",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_rolepermission_permission_PermissionId",
                table: "rolepermission",
                column: "PermissionId",
                principalTable: "permission",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_rolepermission_Roles_RoleId",
                table: "rolepermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_permission_permission_ParentID",
                table: "permission");

            migrationBuilder.DropForeignKey(
                name: "FK_rolepermission_permission_PermissionId",
                table: "rolepermission");

            migrationBuilder.DropForeignKey(
                name: "FK_rolepermission_Roles_RoleId",
                table: "rolepermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_rolepermission",
                table: "rolepermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_permission",
                table: "permission");

            migrationBuilder.RenameTable(
                name: "rolepermission",
                newName: "RolePermission");

            migrationBuilder.RenameTable(
                name: "permission",
                newName: "Permission");

            migrationBuilder.RenameIndex(
                name: "IX_rolepermission_RoleId",
                table: "RolePermission",
                newName: "IX_RolePermission_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_rolepermission_PermissionId",
                table: "RolePermission",
                newName: "IX_RolePermission_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_permission_ParentID",
                table: "Permission",
                newName: "IX_Permission_ParentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermission",
                table: "RolePermission",
                column: "Rp_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Permission",
                table: "Permission",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Permission_Permission_ParentID",
                table: "Permission",
                column: "ParentID",
                principalTable: "Permission",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Permission_PermissionId",
                table: "RolePermission",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermission_Roles_RoleId",
                table: "RolePermission",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

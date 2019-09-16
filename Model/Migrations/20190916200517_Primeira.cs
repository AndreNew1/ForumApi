using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class Primeira : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Senha = table.Column<string>(nullable: true),
                    ConfirmaSenha = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publicacaos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Texto = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: true),
                    UsuarioId = table.Column<Guid>(nullable: true),
                    MediaDeVotos = table.Column<float>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacaos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publicacaos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    PublicacaoId = table.Column<Guid>(nullable: false),
                    ComentarioId = table.Column<Guid>(nullable: true),
                    CitacaoId = table.Column<Guid>(nullable: true),
                    UsuarioId = table.Column<Guid>(nullable: false),
                    Mensagem = table.Column<string>(nullable: true),
                    Nota = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_Publicacaos_PublicacaoId",
                        column: x => x.PublicacaoId,
                        principalTable: "Publicacaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Comentarios_CitacaoId",
                        column: x => x.CitacaoId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Comentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "Comentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_CitacaoId",
                table: "Comentarios",
                column: "CitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ComentarioId",
                table: "Comentarios",
                column: "ComentarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacaos_UsuarioId",
                table: "Publicacaos",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Publicacaos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Model;

namespace Model.Migrations
{
    [DbContext(typeof(BancoContexto))]
    partial class BancoContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Model.Comentarios", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CitacaoId");

                    b.Property<Guid?>("ComentariosId");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<string>("Mensagem");

                    b.Property<float?>("Nota");

                    b.Property<Guid?>("PublicacaoId");

                    b.Property<Guid?>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("ComentariosId");

                    b.HasIndex("PublicacaoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("Model.Publicacao", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataCadastro");

                    b.Property<float?>("MediaDeVotos");

                    b.Property<string>("Status");

                    b.Property<string>("Texto");

                    b.Property<string>("Tipo");

                    b.Property<string>("Titulo");

                    b.Property<Guid?>("UsuarioId");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Publicacaos");
                });

            modelBuilder.Entity("Model.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmaSenha");

                    b.Property<DateTime>("DataCadastro");

                    b.Property<string>("Email");

                    b.Property<string>("Nome");

                    b.Property<string>("Senha");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("Model.Comentarios", b =>
                {
                    b.HasOne("Model.Comentarios")
                        .WithMany("Replicas")
                        .HasForeignKey("ComentariosId");

                    b.HasOne("Model.Publicacao")
                        .WithMany("Comentario")
                        .HasForeignKey("PublicacaoId");

                    b.HasOne("Model.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });

            modelBuilder.Entity("Model.Publicacao", b =>
                {
                    b.HasOne("Model.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId");
                });
#pragma warning restore 612, 618
        }
    }
}

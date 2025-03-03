﻿// <auto-generated />
using System;
using FSH.Starter.WebApi.Todo.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FSH.Starter.WebApi.Migrations.MySQL.Todo
{
    [DbContext(typeof(TodoDbContext))]
    [Migration("20240802061357_Add Todo Schema")]
    partial class AddTodoSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("todo")
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("FSH.Starter.WebApi.Todo.Domain.TodoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("LastModified")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifiedBy")
                        .HasColumnType("char(36)");

                    b.Property<string>("Note")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<string>("Title")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Todos", "todo");

                    b.HasAnnotation("Finbuckle:MultiTenant", true);
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskAPI.DomainModel;

namespace TaskAPI.Migrations
{
    [DbContext(typeof(TaskContext))]
    [Migration("20200320120618_addedColumnParentTaskDes")]
    partial class addedColumnParentTaskDes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TaskAPI.DomainModel.ParentTask", b =>
                {
                    b.Property<int>("Parent_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("parent_id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ParentTaskDescription")
                        .HasColumnName("parent_task_details")
                        .HasColumnType("varchar(40)");

                    b.Property<int>("Parent_Task")
                        .HasColumnName("parent_task")
                        .HasColumnType("int");

                    b.HasKey("Parent_ID");

                    b.ToTable("parent_tasks");
                });

            modelBuilder.Entity("TaskAPI.DomainModel.Tasks", b =>
                {
                    b.Property<int>("TaskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("task_id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("end_date")
                        .HasColumnType("timestamp");

                    b.Property<int>("ParentTaskId")
                        .HasColumnName("parent_id")
                        .HasColumnType("integer");

                    b.Property<int>("Priortiy")
                        .HasColumnName("priority")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("start_date")
                        .HasColumnType("timestamp");

                    b.Property<int>("Status")
                        .HasColumnName("status")
                        .HasColumnType("int");

                    b.Property<string>("TaskDeatails")
                        .HasColumnName("task")
                        .HasColumnType("varchar(40)");

                    b.HasKey("TaskId");

                    b.HasIndex("ParentTaskId");

                    b.ToTable("tasks");
                });

            modelBuilder.Entity("TaskAPI.DomainModel.Tasks", b =>
                {
                    b.HasOne("TaskAPI.DomainModel.ParentTask", "ParentTask")
                        .WithMany("Tasks")
                        .HasForeignKey("ParentTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

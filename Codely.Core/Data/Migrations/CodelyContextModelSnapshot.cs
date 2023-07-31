﻿// <auto-generated />
using System;
using Codely.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Codely.Core.Data.Migrations
{
    [DbContext(typeof(CodelyContext))]
    partial class CodelyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Codely.Core.Data.Entities.Example", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Explanation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("explanation");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("input");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("output");

                    b.Property<int>("ProblemId")
                        .HasColumnType("integer")
                        .HasColumnName("problem_id");

                    b.HasKey("Id")
                        .HasName("pk_examples");

                    b.HasIndex("ProblemId")
                        .HasDatabaseName("ix_examples_problem_id");

                    b.ToTable("examples", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Problem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_problems");

                    b.ToTable("problems", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.ProgrammingLanguageVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("ProgrammingLanguage")
                        .HasColumnType("integer")
                        .HasColumnName("programming_language");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_programming_language_versions");

                    b.ToTable("programming_language_versions", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<DateTime?>("UsedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("used_on");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<DateTime>("ValidUntil")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("valid_until");

                    b.HasKey("Id")
                        .HasName("pk_refresh_tokens");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_refresh_tokens_user_id");

                    b.ToTable("refresh_tokens", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Submission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("answer");

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int>("ProblemId")
                        .HasColumnType("integer")
                        .HasColumnName("problem_id");

                    b.Property<int>("ProgrammingLanguageVersionId")
                        .HasColumnType("integer")
                        .HasColumnName("programming_language_version_id");

                    b.Property<int>("SubmissionStatus")
                        .HasColumnType("integer")
                        .HasColumnName("submission_status");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_submissions");

                    b.HasIndex("ProblemId")
                        .HasDatabaseName("ix_submissions_problem_id");

                    b.HasIndex("ProgrammingLanguageVersionId")
                        .HasDatabaseName("ix_submissions_programming_language_version_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_submissions_user_id");

                    b.ToTable("submissions", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.SubmissionTestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("output");

                    b.Property<int>("SubmissionId")
                        .HasColumnType("integer")
                        .HasColumnName("submission_id");

                    b.Property<int>("SubmissionTestCaseStatus")
                        .HasColumnType("integer")
                        .HasColumnName("submission_test_case_status");

                    b.Property<int>("TestCaseId")
                        .HasColumnType("integer")
                        .HasColumnName("test_case_id");

                    b.HasKey("Id")
                        .HasName("pk_submission_test_cases");

                    b.HasIndex("SubmissionId")
                        .HasDatabaseName("ix_submission_test_cases_submission_id");

                    b.HasIndex("TestCaseId")
                        .HasDatabaseName("ix_submission_test_cases_test_case_id");

                    b.ToTable("submission_test_cases", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.TestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Input")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("input");

                    b.Property<string>("Output")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("output");

                    b.Property<int?>("ProblemId")
                        .HasColumnType("integer")
                        .HasColumnName("problem_id");

                    b.HasKey("Id")
                        .HasName("pk_test_cases");

                    b.HasIndex("ProblemId")
                        .HasDatabaseName("ix_test_cases_problem_id");

                    b.ToTable("test_cases", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Archived")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("archived");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Example", b =>
                {
                    b.HasOne("Codely.Core.Data.Entities.Problem", "Problem")
                        .WithMany("Examples")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_examples_problems_problem_id");

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.RefreshToken", b =>
                {
                    b.HasOne("Codely.Core.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_refresh_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Submission", b =>
                {
                    b.HasOne("Codely.Core.Data.Entities.Problem", "Problem")
                        .WithMany()
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_problems_problem_id");

                    b.HasOne("Codely.Core.Data.Entities.ProgrammingLanguageVersion", "ProgrammingLanguageVersion")
                        .WithMany()
                        .HasForeignKey("ProgrammingLanguageVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_programming_language_versions_programming_langu");

                    b.HasOne("Codely.Core.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submissions_users_user_id");

                    b.Navigation("Problem");

                    b.Navigation("ProgrammingLanguageVersion");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.SubmissionTestCase", b =>
                {
                    b.HasOne("Codely.Core.Data.Entities.Submission", "Submission")
                        .WithMany("SubmissionTestCases")
                        .HasForeignKey("SubmissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submission_test_cases_submissions_submission_id");

                    b.HasOne("Codely.Core.Data.Entities.TestCase", "TestCase")
                        .WithMany()
                        .HasForeignKey("TestCaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_submission_test_cases_test_cases_test_case_id");

                    b.Navigation("Submission");

                    b.Navigation("TestCase");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.TestCase", b =>
                {
                    b.HasOne("Codely.Core.Data.Entities.Problem", null)
                        .WithMany("TestCases")
                        .HasForeignKey("ProblemId")
                        .HasConstraintName("fk_test_cases_problems_problem_id");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Problem", b =>
                {
                    b.Navigation("Examples");

                    b.Navigation("TestCases");
                });

            modelBuilder.Entity("Codely.Core.Data.Entities.Submission", b =>
                {
                    b.Navigation("SubmissionTestCases");
                });
#pragma warning restore 612, 618
        }
    }
}

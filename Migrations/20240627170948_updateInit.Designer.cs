﻿// <auto-generated />
using Dydaktycznie.Data.Dydaktycznie.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dydaktycznie.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240627170948_updateInit")]
    partial class updateInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dydaktycznie.Models.QuestionAnswer", b =>
                {
                    b.Property<int>("QuestionAnswerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionAnswerID"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Correct")
                        .HasColumnType("bit");

                    b.Property<int>("QuizQuestionID")
                        .HasColumnType("int");

                    b.HasKey("QuestionAnswerID");

                    b.HasIndex("QuizQuestionID");

                    b.ToTable("QuestionAnswers");
                });

            modelBuilder.Entity("Dydaktycznie.Models.Quiz", b =>
                {
                    b.Property<int>("QuizID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuizID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuizID");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("Dydaktycznie.Models.QuizQuestion", b =>
                {
                    b.Property<int>("QuizQuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuizQuestionID"));

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuizID")
                        .HasColumnType("int");

                    b.HasKey("QuizQuestionID");

                    b.HasIndex("QuizID");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("Dydaktycznie.Models.QuestionAnswer", b =>
                {
                    b.HasOne("Dydaktycznie.Models.QuizQuestion", "QuizQuestion")
                        .WithMany("QuestionAnswers")
                        .HasForeignKey("QuizQuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("QuizQuestion");
                });

            modelBuilder.Entity("Dydaktycznie.Models.QuizQuestion", b =>
                {
                    b.HasOne("Dydaktycznie.Models.Quiz", "Quiz")
                        .WithMany("QuizQuestions")
                        .HasForeignKey("QuizID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("Dydaktycznie.Models.Quiz", b =>
                {
                    b.Navigation("QuizQuestions");
                });

            modelBuilder.Entity("Dydaktycznie.Models.QuizQuestion", b =>
                {
                    b.Navigation("QuestionAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}

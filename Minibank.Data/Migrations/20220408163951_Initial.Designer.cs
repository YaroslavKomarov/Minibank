﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minibank.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Minibank.Data.Migrations
{
    [DbContext(typeof(MinibankContext))]
    [Migration("20220408163951_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Minibank.Data.BankAccounts.BankAccountDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("closing_date");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency_code");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_closed");

                    b.Property<DateTime>("OpeningDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("opening_date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_banc_accounts");

                    b.ToTable("bank_account");
                });

            modelBuilder.Entity("Minibank.Data.MoneyTransfersHistory.MoneyTransferHistoryDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("currency_code");

                    b.Property<string>("FromAccountId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("from_account_id");

                    b.Property<string>("ToAccountId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("to_account_id");

                    b.HasKey("Id")
                        .HasName("pk_money_transfer_histories");

                    b.ToTable("money_transfer_history");
                });

            modelBuilder.Entity("Minibank.Data.Users.UserDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(29)
                        .HasColumnType("character varying(29)")
                        .HasColumnName("login");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("user");
                });
#pragma warning restore 612, 618
        }
    }
}

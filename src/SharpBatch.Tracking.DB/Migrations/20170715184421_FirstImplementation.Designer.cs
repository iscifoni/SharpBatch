﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SharpBatch.Tracking.DB.data;
using System;

namespace SharpBatch.Tracking.DB.Migrations
{
    [DbContext(typeof(TrackingContext))]
    [Migration("20170715184421_FirstImplementation")]
    partial class FirstImplementation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26339")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SharpBatch.Tracking.DB.data.MessagesModel", b =>
                {
                    b.Property<long>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message")
                        .HasColumnType("varchar(500)");

                    b.Property<long>("TrackingId");

                    b.HasKey("MessageId");

                    b.HasIndex("TrackingId");

                    b.ToTable("SB_Tracking_Message");
                });

            modelBuilder.Entity("SharpBatch.Tracking.DB.data.PingsModel", b =>
                {
                    b.Property<long>("PingId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("PingData");

                    b.Property<long>("TrackingId");

                    b.HasKey("PingId");

                    b.HasIndex("TrackingId");

                    b.ToTable("SB_Tracking_Ping");
                });

            modelBuilder.Entity("SharpBatch.Tracking.DB.data.TrackingModel", b =>
                {
                    b.Property<long>("TrackingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BatchName");

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("MachineName");

                    b.Property<Guid>("SessionId");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("State");

                    b.HasKey("TrackingId");

                    b.HasAlternateKey("SessionId");

                    b.ToTable("SB_Tracking_Tracking");
                });

            modelBuilder.Entity("SharpBatch.Tracking.DB.data.MessagesModel", b =>
                {
                    b.HasOne("SharpBatch.Tracking.DB.data.TrackingModel")
                        .WithMany("Messages")
                        .HasForeignKey("TrackingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SharpBatch.Tracking.DB.data.PingsModel", b =>
                {
                    b.HasOne("SharpBatch.Tracking.DB.data.TrackingModel")
                        .WithMany("Pings")
                        .HasForeignKey("TrackingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

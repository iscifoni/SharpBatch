using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SharpBatch.Tracking.DB.data
{
    public partial class TrackingContext:DbContext
    {
        public TrackingContext(DbContextOptions<TrackingContext> options):base(options)
        {
                
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrackingModel>()
                .ToTable("SB_Tracking_Tracking")
                .HasAlternateKey(p=> p.SessionId );

            modelBuilder.Entity<PingsModel>().ToTable("SB_Tracking_Ping");
            modelBuilder.Entity<MessagesModel>().ToTable("SB_Tracking_Message");
            modelBuilder.Entity<ExceptionModel>().ToTable("SB_Tracking_Exception");
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TrackingModel> Trackings { get; set; }
        public DbSet<MessagesModel> Messages { get; set; }
        public DbSet<PingsModel> Pings { get; set; }
        public DbSet<ExceptionModel> Exceptions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpravochnikPP.Models;

namespace SpravochnikPP.Context;

public partial class JvtufokyContext : DbContext
{
    public JvtufokyContext()
    {
    }
    private static JvtufokyContext _context; 
    public static JvtufokyContext GetContext()
    {
        if (_context == null)
        {
            _context = new JvtufokyContext();
        }

        return _context;
    }

    public JvtufokyContext(DbContextOptions<JvtufokyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Kbzu> Kbzus { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=cornelius.db.elephantsql.com;Database=jvtufoky;Username=jvtufoky;password=Nj1kHVeYwC_qDy6FDKOqOhsl_4kfSS7X");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gin")
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("citext")
            .HasPostgresExtension("cube")
            .HasPostgresExtension("dblink")
            .HasPostgresExtension("dict_int")
            .HasPostgresExtension("dict_xsyn")
            .HasPostgresExtension("earthdistance")
            .HasPostgresExtension("fuzzystrmatch")
            .HasPostgresExtension("hstore")
            .HasPostgresExtension("intarray")
            .HasPostgresExtension("ltree")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("pgrowlocks")
            .HasPostgresExtension("pgstattuple")
            .HasPostgresExtension("tablefunc")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp")
            .HasPostgresExtension("xml2");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Categories_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Favorites_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Favorites_productid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Favorites_userid_fkey");
        });

        modelBuilder.Entity<Kbzu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("KBZU_pkey");

            entity.ToTable("KBZU");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cal).HasColumnName("cal");
            entity.Property(e => e.Carbohydrates).HasColumnName("carbohydrates");
            entity.Property(e => e.Categoriesid).HasColumnName("categoriesid");
            entity.Property(e => e.Fats).HasColumnName("fats");
            entity.Property(e => e.Glikindex).HasColumnName("glikindex");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Proteins).HasColumnName("proteins");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Categories).WithMany(p => p.Kbzus)
                .HasForeignKey(d => d.Categoriesid)
                .HasConstraintName("KBZU_categoriesid_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Passwords).HasColumnName("passwords");
            entity.Property(e => e.Patronomic).HasColumnName("patronomic");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_roleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

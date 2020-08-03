using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreRelacije.Server
{
	public class EF : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-75VO5EN\TESTSERVER;Initial Catalog=TestBaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
		}

		public DbSet<OtMA> OtMAs { get; set; }
		public DbSet<OtMB> OtMBs { get; set; }

		public DbSet<OtOA> OtOAs { get; set; }
		public DbSet<OtOB> OtOBs { get; set; }

		public DbSet<MtMA> MtMAs { get; set; }
		public DbSet<MtMB> MtMBs { get; set; }
		public DbSet<MtMA_B> MtMA_Bs { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OtMA>().HasKey(o => o.Id);
			modelBuilder.Entity<OtMB>().HasKey(o => o.Id);

			modelBuilder.Entity<OtOA>().HasKey(o => o.Id);
			modelBuilder.Entity<OtOB>().HasKey(o => o.Id);

			modelBuilder.Entity<OtMA>().HasMany(o => o.listaDrugog);

			modelBuilder.Entity<OtOA>().HasOne(o => o.drugi);

			modelBuilder.Entity<MtMA>().HasKey(o => o.Id);
			modelBuilder.Entity<MtMB>().HasKey(o => o.Id);
			modelBuilder.Entity<MtMA_B>().HasKey(o => new { o.IdA, o.IdB });

			modelBuilder.Entity<MtMA_B>().HasOne(ab => ab.A)
										 .WithMany(a => a.Lista)
										 .HasForeignKey(ab => ab.IdA);
			modelBuilder.Entity<MtMA_B>().HasOne(ab => ab.B)
										 .WithMany(b => b.Lista)
										 .HasForeignKey(ab => ab.IdB);
		}
	}

	public class OtMA
	{
		public int Id { get; set; }
		public List<OtMB> listaDrugog { get; set; } = new List<OtMB>();
	}
	public class OtMB
	{
		public int Id { get; set; }
	}

	public class OtOA
	{
		public int Id { get; set; }

		public OtOB drugi { get; set; }
	}

	public class OtOB
	{
		public int Id { get; set; }
	}

	public class MtMA
	{
		public int Id { get; set; }
		public List<MtMA_B> Lista { get; set; } = new List<MtMA_B>();
	}

	public class MtMB
	{
		public int Id { get; set; }
		public List<MtMA_B> Lista { get; set; } = new List<MtMA_B>();
	}

	public class MtMA_B
	{
		public int IdA { get; set; }
		public int IdB { get; set; }

		public MtMA A { get; set; }
		public MtMB B { get; set; }

		public MtMA_B(MtMA a, MtMB b)
		{
			A = a;
			B = b;
		}
		public MtMA_B() { }
	}
}

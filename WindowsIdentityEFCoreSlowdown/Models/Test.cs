using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WindowsIdentityEFCoreSlowdown.Models {
	public class TestContext : DbContext {
		public TestContext() : base() { }

		public TestContext(DbContextOptions<TestContext> options)
			: base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite("Filename=test.db");
		}

		public DbSet<Test> Tests { get; set; }
	}

	public class Test {
		public int Id { get; set; }
	}
}
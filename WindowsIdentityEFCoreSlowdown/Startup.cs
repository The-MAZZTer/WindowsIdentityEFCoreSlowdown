﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WindowsIdentityEFCoreSlowdown.Models;
using Microsoft.EntityFrameworkCore;

namespace WindowsIdentityEFCoreSlowdown {
	public class Startup {
		public Startup(IConfiguration configuration) {
			this.Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			using (TestContext test = new TestContext()) {
				test.Database.Migrate();

				if (!test.Tests.Any()) {
					test.Tests.AddRange(new[] {
						new Test() { Id = 1 },
						new Test() { Id = 2 },
						new Test() { Id = 3 },
						new Test() { Id = 4 },
						new Test() { Id = 5 }
					});
					test.SaveChanges();
				}
			}
		}
	}
}

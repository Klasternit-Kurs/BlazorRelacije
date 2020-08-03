using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace EFCoreRelacije.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			using (EF db = new EF())
			{
				db.Database.EnsureCreated();
				OtMA a = new OtMA();
				OtMB b1 = new OtMB();
				OtMB b2 = new OtMB();

				a.listaDrugog.Add(b1);
				a.listaDrugog.Add(b2);

				db.OtMAs.Add(a);
				db.OtMBs.Add(b1);
				db.OtMBs.Add(b2);
				db.SaveChanges();

				OtOA oneA = new OtOA();
				OtOB oneB = new OtOB();
				oneA.drugi = oneB;

				db.OtOAs.Add(oneA);
				db.OtOBs.Add(oneB);
				db.SaveChanges();

				MtMA manyA1 = new MtMA();
				MtMA manyA2 = new MtMA();

				MtMB manyB1 = new MtMB();
				MtMB manyB2 = new MtMB();

				manyA1.Lista.Add(new MtMA_B(manyA1, manyB1));
				manyA1.Lista.Add(new MtMA_B(manyA1, manyB2));

				manyA2.Lista.Add(new MtMA_B(manyA2, manyB1));
				manyA2.Lista.Add(new MtMA_B(manyA2, manyB2));

				db.MtMAs.Add(manyA1);
				db.MtMAs.Add(manyA2);
				db.MtMBs.Add(manyB1);
				db.MtMBs.Add(manyB2);
				db.SaveChanges();
			}
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}

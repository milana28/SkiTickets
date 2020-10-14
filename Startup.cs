using System.IO;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using SkiTickets.Domain;
using SkiTickets.Pdf;
using SkiTickets.Utils;
using SkiTickets.Utils.Responses;

namespace SkiTickets
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddMemoryCache();
            services.AddSingleton<IDatabase, Database>();
            services.AddSingleton<IAge, Age>();
            services.AddSingleton<IPerson, Person>();
            services.AddSingleton<ISellingPoint, SellingPoint>();
            services.AddSingleton<ITicketType, TicketType>();
            services.AddSingleton<ITicket, Ticket>();
            services.AddSingleton<ITicketPurchase, TicketPurchase>();
            services.AddSingleton<ITicketUsed, TicketUsed>();
            services.AddSingleton<IStatistic, Statistic>();
            services.AddSingleton<TemplateGenerator>();
            services.AddRazorPages();
            services.AddMvc();
            
            var context = new CustomAssemblyLoadContext(); 
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.so"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            
            // services.AddMvc()
            //     .ConfigureApiBehaviorOptions(options =>
            //     {
                    // options.InvalidModelStateResponseFactory = context =>
                    // {
                    //     var problems = new CustomBadRequest(context);
                    //     return new BadRequestObjectResult(problems);
                    // };
                // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiTickets");
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using VUTTR.Backend.Data;
using VUTTR.Backend.Models;

namespace VUTTR.Backend
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VUTTR Api",
                    Description = "Bossabox challenge API"
                });
            });

            BsonClassMap.RegisterClassMap<Tool>(cm =>
            {
                cm.AutoMap();

                cm.MapIdProperty(x => x.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance);
            });

            services.AddScoped(x => new MongoFactory(Configuration["MongoDb:ConnectionString"], Configuration["MongoDb:DatabaseName"]));

            services.AddScoped<IToolsRespository, MongoToolRepository>(x =>
            {
                return new MongoToolRepository(x.GetRequiredService<MongoFactory>().GetCollection<Tool>("tools"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using CompleteExample.API.ExceptionFilter;
using CompleteExample.Entities;
using CompleteExample.Logic.PipelineBehaviours;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace CompleteExample.API
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
            services.AddDbContext<CompleteExampleDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SchoolContext")));

            services.AddMediatR(Assembly.GetExecutingAssembly(), typeof(Logic.Assy).Assembly);

            services.AddScoped<ICompleteExampleDBContext>(provider => provider.GetService<CompleteExampleDBContext>());

            AssemblyScanner.FindValidatorsInAssembly(typeof(Logic.Assy).Assembly)
                .ForEach(x => services.AddScoped(x.InterfaceType, x.ValidatorType));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddControllers(options =>
            {
                options.Filters.Add<CustomExceptionFilterAttribute>();
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
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using AutoMapper;
using Todo.Api.Middleware.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Todo.Api.Middleware;
using Todo.Core.Repositories;
using Todo.Core.Services;
using Todo.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Http;
using System;

namespace Todo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            this.SetupDatabase(services);
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.Configure<IConfiguration>(this.Configuration);
            services.Configure<PinOptions>(this.Configuration);

            services.AddTransient<IPinService, PinService>();

            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IStoryService, StoryService>();

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo", Version = "v1" });
                options.OperationFilter<SwaggerHeaderFilter>();
            });


            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo V1");
            });
        }

        public virtual void SetupDatabase (IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionStrings:TodoDBContextConnectionString"];
            services.AddDbContext<ToDoContext>(options => options.UseSqlServer(connectionString));
        }

        public class SwaggerHeaderFilter : IOperationFilter
        {
            
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                if (string.Equals(context.ApiDescription.HttpMethod, HttpMethod.Post.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "Pin",
                        In = ParameterLocation.Header,
                        Required = false,
                    });
                }
            }
        }


    }
}

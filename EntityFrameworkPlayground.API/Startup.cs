﻿using AutoMapper;
using EntityFrameworkPlayground.DataAccess;
using EntityFrameworkPlayground.DataAccess.Repositories;
using EntityFrameworkPlayground.DataAccess.Repositories.Interfaces;
using EntityFrameworkPlayground.Service.Authors;
using EntityFrameworkPlayground.Service.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;

namespace EntityFrameworkPlayground.API
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
            services.AddDbContext<BooksContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //Register repositories
            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();

            // Register Strategies
            services.AddTransient<IGetAuthorsStrategy, GetAuthorsStrategy>();
            services.AddTransient<IGetAuthorStrategy, GetAuthorStrategy>();
            services.AddTransient<ICreateAuthorLinksStrategy, CreateAuthorLinksStrategy>();
            services.AddTransient<ICreateAuthorStrategy, CreateAuthorStrategy>();
            services.AddTransient<IDeleteAuthorStrategy, DeleteAuthorStrategy>();

            services.AddTransient<IGetBooksStrategy, GetBooksStrategy>();
            services.AddTransient<IGetBookStrategy, GetBookStrategy>();
            services.AddTransient<ICreateBookLinksStrategy, CreateBookLinksStrategy>();
            services.AddTransient<ICreateBookStrategy, CreateBookStrategy>();
            services.AddTransient<IUpdateBookStrategy, UpdateBookStrategy>();
            services.AddTransient<IDeleteBookStrategy, DeleteBookStrategy>();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            // MVC
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc(setupAction =>
            {
                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.hateoas+json");
                }
            })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

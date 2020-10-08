using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using DAL.Classes;
using Microsoft.EntityFrameworkCore.Proxies;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Services;
using System.IServices;
using DAL.infrastructure;
using DAL.Repositories;
using DAL.IRepositories;
using System.Controllers;

namespace System
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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            //services.AddDbContext<TccContext>(options =>
            //    {
            //    options/*.UseLazyLoadingProxies()*/
            //    .UseSqlServer(Configuration.GetConnectionString("TccConnection"));
            //},ServiceLifetime.Transient);
            try
            {
                services.AddTransient(typeof(DbContext), typeof(TccContext));
                services.AddScoped(typeof(AbstractUnitOfWork), typeof(UnitOfWork));
                services.AddTransient(typeof(ISubjectServices), typeof(SubjectServices));
                services.AddAutoMapper(typeof(Startup));
                services.AddTransient(typeof(IStudentService), typeof(StudentService));
                services.AddTransient(typeof(IStudentSubjectService), typeof(StudentSubjectService));
                services.AddTransient(typeof(IStudentRepositroy), typeof(StudentRepository));
                services.AddTransient(typeof(IExamSemesterRepositroy), typeof(ExamSemesterRepositroy));
                services.AddTransient(typeof(IRepositroy<>), typeof(Repository<>));
                services.AddSingleton<IFileWriter, ImageWriter>();
                //services.AddTransient<IFileWriter, ImageWriter>();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            string securityKey = "this_is_our_supper_long_security_key_for_token_validation_project_2018_09_07$smesk.in";
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //what to validate
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        //setup validate data
                        ValidIssuer = "smesk.in",
                        ValidAudience = "readers",
                        IssuerSigningKey = symmetricSecurityKey
                    };
                });
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .WithExposedHeaders("x-paging"); ;
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

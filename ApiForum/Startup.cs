﻿using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiForum
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
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.DateFormatString = "dd/MM/yyyy";            
            });

            var config = new MapperConfiguration(opts =>
            {
                opts.CreateMap<Usuario, UsuarioToken>()
                .ForMember(d => d.TokenUsuario, s => s.MapFrom(temp => temp.Id));

                opts.CreateMap<TopicoView, Topico>();

                opts.CreateMap<ViewVoto, Votacao>()
                .ForMember(d => d.VotacaoRelacionada, s => s.MapFrom(sorc => sorc.ComentarioId));

                opts.CreateMap<Usuario, UsuarioFake>()
                .ForMember(d => d.Email, s => s.MapFrom(sorc => sorc.Email))
                .ForMember(d => d.Nome, s => s.MapFrom(sorc => sorc.Nome));

                opts.CreateMap<Topico, Topico>()
                .ForMember(d => d.Texto, s => s.Condition(sorc => sorc.Texto != null && sorc.Texto.Length > 49))
                .ForMember(d => d.Titulo, s => s.Condition(sorc => sorc.Titulo != null && sorc.Titulo.Length > 8 && sorc.Titulo.Length < 250))
                .ForMember(d=>d.Status,s=>s.Condition(sorc=>sorc.Status!="fechado"||sorc.Status!="aberto"));
            }) ; 

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Forum" });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V!");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "",
                  template: "{controller=Values}/{id?}");
            });
        }
    }
}
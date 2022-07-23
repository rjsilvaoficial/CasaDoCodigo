using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo
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
            services.AddMvc();
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddTransient<IDataService, DataService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //IServiceProvider permite usar um ApplicationContext para assegurar instanciação do DB
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Pedido}/{action=Carrossel}");
            });

            //Invocar GetService<ApplicationContext>() permite acionar a Prop Database
            //Database possui EnsureCreated() método boleano que garante a criação de banco de dados com base no contexto da aplicação
            //serviceProvider.GetService<ApplicationContext>().Database.EnsureCreated();

            //Invocar GetService<ApplicationContext>() permite acionar a Prop Database
            //Database possui Migrate() método que garante a criação de banco de dados com base no contexto da aplicação
            //serviceProvider.GetService<ApplicationContext>().Database.Migrate();


            //Para criar o db, gere IDataService com metodo InicializaDb(), implemente em uma classe DataService
            //Em DataService crie uma injeção de dependência readonly e tal
            //Implemente void InicializaDb() com chamada de Database.Migrate() a partir de _context

            //Aqui inclua IServiceProvider em Configure() inclua D.I. de IDataService,DataService em ConfigureServices() como transient
            //Chame a partir de serviceProvider a GetService<IDataService>(), com isso é só chamar o InicializaDb()
            //O InicializaDb() vai alimentar o Db, mesmo que já exista!
            serviceProvider.GetService<IDataService>().InicializaDb();


        }
    }
}

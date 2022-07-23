using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Produto>().HasKey(t => t.Id);


            modelBuilder.Entity<Pedido>().HasKey(t => t.Id);

            //1 * N Pedido x Item's (1 Pedido tem muitos 'item'[disponíveis em Itens aqui no programa])
            //Esse conjunto de itens se relaciona a 1 Pedido
            //Cada pedido tem uma lista de Itens relacionada com 1 pedido (cada Item em Itens, se relacionara a 1 pedido)
            modelBuilder.Entity<Pedido>().HasMany(t => t.Itens).WithOne(t => t.Pedido);
            //Cada Pedido tem 1 cadastro, cada um Cadastro tem 1 Pedido e Cadastro não existe sem 1 Pedido
            modelBuilder.Entity<Pedido>().HasOne(t=>t.Cadastro).WithOne(t=>t.Pedido).IsRequired();

            modelBuilder.Entity<ItemPedido>().HasKey(t => t.Id);
            modelBuilder.Entity<ItemPedido>().HasOne(t => t.Pedido);
            modelBuilder.Entity<ItemPedido>().HasOne(t => t.Produto);

            modelBuilder.Entity<Cadastro>().HasKey(t => t.Id);
            modelBuilder.Entity<Cadastro>().HasOne(t => t.Pedido);
        }
    }
}

using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    class DataService : IDataService
    {
        private readonly ApplicationContext _context;
        private readonly IProdutoRepository _produtoRepository;


        public DataService(ApplicationContext context, IProdutoRepository produtoRepository)
        {
            _context = context;
            _produtoRepository = produtoRepository;
        }

        public void InicializaDb()
        {
            //_context.Database.EnsureCreated(); cria o db se não existir mas inviabiliza migrations

            _context.Database.Migrate();


            List<Livro> livros = GetLivros();

            _produtoRepository.SaveProdutos(livros);

        }


        //O método abaixo implementa como LER UM TEXTO PARTINDO DE UM JSON E DEPOIS COMO TRANSFORMÁ-LO EM OBJETOS

        private static List<Livro> GetLivros()
        {
            //Aqui lemos e armazenamos todo um json (que está à nível de projeto) como uma string

            string json = File.ReadAllText("livros.json");

            //Abaixo usamos o JsonConvert do nuget Newtonsoft para desserializar em vários objetos
            //Para isso é necessário especificar entre <> do Deserialize o tipo como List e especificar o tipo da lista
            //E também usar a var que contém esse json a ser deserializado como atb

            List<Livro> livros = JsonConvert.DeserializeObject<List<Livro>>(json);
            return livros;
        }
    }


}

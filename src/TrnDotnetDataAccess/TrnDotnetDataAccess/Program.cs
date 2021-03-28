using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TrnDotnetDataAccess.Entidades;

namespace TrnDotnetDataAccess
{
    class Program
    {
        private static SqlConnection sqlConnection;
        static void Main(string[] args)
        {

            ListarClientes();
            GravarNovoCliente();
            ExcluirCliente();

            GravarNovoProduto();
            ListarProduto();
            Console.ReadKey();
        }
        private static void IniciarConexao()
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbLoja;Integrated Security=True;Connect Timeout=30;";

            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;

            Console.WriteLine(sqlConnection.State);
        }

        private static void GravarNovoCliente()
        {
            IniciarConexao();
            sqlConnection.Open();

            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "insert into Cliente values(@id,@nome,@email,@senha)";

            var cliente = new Cliente("Maria da Silva", "marias274@gmail.com", "123456");

            sqlCommand.Parameters.Add(new SqlParameter("@id", cliente.Id));
            sqlCommand.Parameters.Add(new SqlParameter("@nome", cliente.Nome));
            sqlCommand.Parameters.Add(new SqlParameter("@email", cliente.Email));
            sqlCommand.Parameters.Add(new SqlParameter("@senha", cliente.Senha));

            var qtdRows = sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Cliente cadastrado com sucesso");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();
        }
        private static void ExcluirCliente()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "delete from Cliente where id=@id";

            var clienteId = "795AED5F-E0A0-4004-AA80-41188EDB2DA1";
            sqlCommand.Parameters.Add(new SqlParameter("@id", clienteId));

            var qtdRows = sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Cliente excluído com sucesso");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();


        }
        private static void ListarClientes()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select Id,Nome,Email from Cliente";

            var sqlDataReader = sqlCommand.ExecuteReader();

            List<Cliente> listaClientes = new List<Cliente>();

            while (sqlDataReader.Read())
            {
                Guid id = Guid.Parse(sqlDataReader[0].ToString());
                var cliente = new Cliente(id);
                cliente.Atualizar(sqlDataReader[1].ToString(), sqlDataReader[2].ToString());
                listaClientes.Add(cliente);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            sqlConnection.Dispose();

            foreach (var item in listaClientes)
            {
                Console.WriteLine($"Nome: {item.Nome}  - Email: {item.Email}");
            }


        }
        
        private static void GravarNovoProduto()
        {
            IniciarConexao();
            sqlConnection.Open();

            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "insert into Produto values(@id,@nome,@precoUnitario,@quantidadeEstoque)";

            var produto = new Produto("Notebook", 7000 , 5);

            sqlCommand.Parameters.Add(new SqlParameter("@id", produto.Id));
            sqlCommand.Parameters.Add(new SqlParameter("@nome", produto.Nome));
            sqlCommand.Parameters.Add(new SqlParameter("@precoUnitario", produto.PrecoUnitario));
            sqlCommand.Parameters.Add(new SqlParameter("@quantidadeEstoque", produto.QuantidadeEstoque));

            var qtdRows = sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Produto cadastrado com sucesso");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private static void ListarProduto()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = " select id,nome,preçoUnitario,quantidadeEstoque from Produto";

            var sqlDataReader = sqlCommand.ExecuteReader();

            List<Produto> listaProduto = new List<Produto>();

            while (sqlDataReader.Read())
            {
                Guid id = Guid.Parse(sqlDataReader[0].ToString());
                var produto = new Produto(id);
                produto.Atualizar(sqlDataReader[1].ToString(), 
                    decimal.Parse(sqlDataReader[2].ToString()), 
                    int.Parse(sqlDataReader[3].ToString()));                        
                listaProduto.Add(produto);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            sqlConnection.Dispose();

            foreach (var item in listaProduto)
            {
                Console.WriteLine($"Nome: {item.Nome}  - Preço Unitário: {item.PrecoUnitario} - Quantidade em Estoque: {item.QuantidadeEstoque}");
            }

        
        }
    }

}
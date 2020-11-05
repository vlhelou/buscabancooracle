using System;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Cryptography;

namespace BuscaBanco
{
    class Program
    {
        static void Main(string[] args)
        {

            OracleConnectionStringBuilder cnb = new OracleConnectionStringBuilder();
            string chave = "Like se quiser use o %";
            OracleConnection cn = new OracleConnection("String de conexão");
            cn.Open();
            Console.Title = "Conectou!!!!!!";
            OracleCommand cmd = cn.CreateCommand();
            DataTable dt = new DataTable();
            cmd.CommandText = @"select owner, table_name, num_rows
                    from all_tables 
                    where
                        num_rows>0
                        and num_rows<10000
                        
                    ORDER BY owner, table_name
                    ";

            dt.Load(cmd.ExecuteReader());
            Console.Title = "Busca Tabelas";
            var total = dt.Rows.Count;
            var ct = 0;
            foreach (DataRow ln in dt.Rows)
            {
                ct++;
                //Console.WriteLine($"Owner: {ln["OWNER"].ToString()}, Tabela: {ln["TABLE_NAME"].ToString()}");
                Console.Title = $"{ct}/{total}     {ln["OWNER"].ToString()}.{ln["TABLE_NAME"].ToString()} ";
                cmd.CommandText = $@"select owner, table_name, column_name
                    from sys.all_tab_columns
                    where
                        owner = '{ln["OWNER"].ToString()}'
                        and table_name = '{ln["TABLE_NAME"].ToString()}'
                        and data_type in ('CHAR', 'NCHAR', 'VARCHAR2', 'NVARCHAR2')
                    ";
                DataTable cols = new DataTable();
                cols.Load(cmd.ExecuteReader());
                if (cols.Rows.Count > 0)
                {
                    System.Text.StringBuilder sql = new System.Text.StringBuilder();
                    sql.Append($"select count(*) from {ln["OWNER"].ToString()}.{ln["TABLE_NAME"].ToString()} ");
                    bool primeiro = true;
                    foreach (DataRow coluna in cols.Rows)
                    {
                        if (primeiro)
                        {
                            sql.AppendFormat(" where upper({0}) like '{1}' ", coluna["column_name"].ToString(), chave);
                            primeiro = false;
                        }
                        else
                        {
                            sql.AppendFormat(" or upper({0}) like '{1}' ", coluna["column_name"].ToString(), chave);

                        }


                    }
                    cmd.CommandText = sql.ToString();
                    try
                    {
                        
                        var encontrados = (decimal)cmd.ExecuteScalar();
                        if (encontrados > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{ln["OWNER"].ToString()}.{ln["TABLE_NAME"].ToString()} \r\n \t Encontrados = {encontrados}, Total de registros: {ln["num_rows"].ToString()}");
                        }

                    } catch(Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(ex.Message);
                    }
                    //Console.WriteLine("======>" + coluna["column_name"].ToString());

                }
            }

            dt.Dispose();
            cn.Dispose();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaSeguridad
{
    public class UserDao : UserConnectionToSql
    {
        public bool Login(string NombreUsuario, string Clave)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "select * from tbl_usuarios where NombreUsuario=@NombreUsuario and Clave=@Clave";
                    command.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                    command.Parameters.AddWithValue("@Clave", Clave);
                    command.CommandType = CommandType.Text;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            UserCache.CodigoUsuario = reader.GetInt32(0);
                            UserCache.CodigoRol = reader.GetInt32(1);
                            UserCache.CodigoEmpleado = reader.GetInt32 (2);
                            UserCache.NombreUsuario = reader.GetString(3);
                            UserCache.Clave = reader.GetString(4);
                            UserCache.Estado = reader.GetString(5);
                        }
                        return true;
                    }
                    else
                        return false;
                }
            }
        }
    }
}

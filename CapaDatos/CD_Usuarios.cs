using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        CD_Conexion conn = new CD_Conexion();


        public DataTable MtdConsultarUsuarios()
        {
            string Query = "Select * from tbl_usuarios";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }


        public List<dynamic> MtdListaRol()
        {
            List<dynamic> ListaRol = new List<dynamic>();
            string QueryListaRol = "Select CodigoRol, NombreRol from tbl_roles; ";
            SqlCommand cmd = new SqlCommand(QueryListaRol, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaRol.Add(new
                {
                    Value = (int)reader["CodigoRol"],
                    Text = $"{reader["CodigoRol"]} - {reader["NombreRol"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaRol;
        }


        public List<dynamic> MtdListaEmpleados()
        {
            List<dynamic> ListaEmpleados = new List<dynamic>();
            string QueryListaEmpleados = "Select CodigoEmpleado, Nombre from tbl_empleados; ";
            SqlCommand cmd = new SqlCommand(QueryListaEmpleados, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaEmpleados.Add(new
                {
                    Value = (int)reader["CodigoEmpleado"],
                    Text = $"{reader["CodigoEmpleado"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaEmpleados;
        }
        public void MtdAgregarUsuarios(int CodigoRol, int CodigoEmpleado, string NombreUsuario, double Clave, string Estado)
        {
            string QueryAgregar = "Insert into tbl_usuarios ( CodigoRol, CodigoEmpleado, NombreUsuario, Clave, Estado) values ( @CodigoRol, @CodigoEmpleado, @NombreUsuario, @Clave, @Estado);";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoRol", CodigoRol);
            Sqlcmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            Sqlcmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
            Sqlcmd.Parameters.AddWithValue("@Clave", Clave);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }


        public void MtdEditarUsuarios(int CodigoUsuario, int CodigoRol, int CodigoEmpleado, string NombreUsuario, double Clave, string Estado)
        {
            string QueryAgregar = "Update tbl_usuarios set CodigoRol=@CodigoRol, CodigoEmpleado=@CodigoEmpleado, NombreUsuario=@NombreUsuario, Clave=@Clave, Estado=@Estado Where CodigoUsuario=@CodigoUsuario";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            Sqlcmd.Parameters.AddWithValue("@CodigoRol", CodigoRol);
            Sqlcmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            Sqlcmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
            Sqlcmd.Parameters.AddWithValue("@Clave", Clave);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarUsuarios(int CodigoUsuario)
        {
            string QueryAgregar = "Delete tbl_usuarios where CodigoUsuario=@CodigoUsuario";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Roles
    {
        CD_Conexion conn = new CD_Conexion();

        // Consulta datos de empleados en la base de datos 
        public DataTable MtdConsultarRoles()
        {
            string Query = "Select * from tbl_roles;";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        // Metodo para gregar un nuevo empleado
        public void MtdAgregarRoles(string NombreRol, string TipoPermiso, string Pantalla, string Estado)
        {
            string QueryAgregar = "Insert into tbl_roles (NombreRol, TipoPermiso, Pantalla, Estado) values (@NombreRol, @TipoPermiso, @Pantalla, @Estado)";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@NombreRol", NombreRol);
            Sqlcmd.Parameters.AddWithValue("@TipoPermiso", TipoPermiso);
            Sqlcmd.Parameters.AddWithValue("@Pantalla", Pantalla);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdActualizarRoles(int CodigoRol, string NombreRol, string TipoPermiso, string Pantalla, string Estado)
        {
            string QueryActualizar = "Update tbl_roles set NombreRol=@NombreRol, TipoPermiso=@TipoPermiso, Pantalla=@Pantalla, Estado=@Estado Where CodigoRol=@CodigoRol";
            SqlCommand Sqlcmd = new SqlCommand(QueryActualizar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoRol", CodigoRol);
            Sqlcmd.Parameters.AddWithValue("@NombreRol", NombreRol);
            Sqlcmd.Parameters.AddWithValue("@TipoPermiso", TipoPermiso);
            Sqlcmd.Parameters.AddWithValue("@Pantalla", Pantalla);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        // Metodo para eliminar un nuevo empleado
        public void MtdEliminarRoles(int CodigoRol)
        {
            string QueryAgregar = "Delete tbl_roles where CodigoRol=@CodigoRol";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoRol", CodigoRol);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

    }
}

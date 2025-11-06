using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Proveedores
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarProveedores()
        {
            string Query = "Select * from tbl_proveedores";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarProveedores(string NombreProveedor, int Telefono, string Correo, string Direccion, string Estado)
        {
            string QueryAgregar = "Insert into tbl_proveedores ( Nombre, Telefono, Correo, Direccion, Estado) values ( @Nombre, @Telefono, @Correo, @Direccion, @Estado);";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@Nombre", NombreProveedor);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarProveedores(int CodigoProveedor, string NombreProveedor, int Telefono, string Correo, string Direccion, string Estado)
        {
            string QueryAgregar = "Update tbl_proveedores set Nombre=@Nombre, Telefono=@Telefono, Correo=@Correo, Direccion=@Direccion, Estado=@Estado Where CodigoProveedor=@CodigoProveedor";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            Sqlcmd.Parameters.AddWithValue("@Nombre", NombreProveedor);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarProveedores(int CodigoProveedor)
        {
            string QueryAgregar = "Delete tbl_proveedores where CodigoProveedor=@CodigoProveedor";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
    }
}

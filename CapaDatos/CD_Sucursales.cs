using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Sucursales
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarSucursales()
        {
            string Query = "Select * from tbl_sucursales";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarSucursales(string NombreSucursal, string Direccion, int Telefono, string Correo, int CodigoPostal, string Estado)
        {
            string QueryAgregar = "Insert into tbl_sucursales ( Nombre, Direccion, Telefono, Correo, CodigoPostal ,Estado) values ( @Nombre, @Direccion, @Telefono, @Correo, @CodigoPostal, @Estado);";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@Nombre", NombreSucursal);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@CodigoPostal", CodigoPostal);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarSucursales(int CodigoSucursal, string NombreSucursal, string Direccion, int Telefono, string Correo, int CodigoPostal, string Estado)
        {
            string QueryAgregar = "Update tbl_sucursales set Nombre=@Nombre, Direccion=@Direccion, Telefono=@Telefono, Correo=@Correo, CodigoPostal=@CodigoPostal, Estado=@Estado Where CodigoSucursal=@CodigoSucursal";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            Sqlcmd.Parameters.AddWithValue("@Nombre", NombreSucursal);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@CodigoPostal", CodigoPostal);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarSucursales(int CodigoSucursal)
        {
            string QueryAgregar = "Delete tbl_sucursales where CodigoSucursal=@CodigoSucursal";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }



    }
}

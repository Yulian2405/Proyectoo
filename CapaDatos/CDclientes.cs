using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapaDatos
{
    public class CDclientes
    {
        CD_Conexion conn = new CD_Conexion();

        // Consultar todos los clientes
        public DataTable MtdConsultarClientes()
        {
            string Query = "Select * from tbl_clientes;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        // Agregar cliente
        public void MtdAgregarClientes(string Nombre, string NIT, int Telefono, string Direccion, string Departamento, string TipoCliente, string Estado)
        {
            string Query = "Insert into tbl_clientes (Nombre, NIT, Telefono, Direccion, Departamento, TipoCliente, Estado) values (@Nombre, @NIT, @Telefono, @Direccion, @Departamento, @TipoCliente, @Estado)";
            SqlCommand Sqlcmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@Nombre", Nombre);
            Sqlcmd.Parameters.AddWithValue("@NIT", NIT);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Departamento", Departamento);
            Sqlcmd.Parameters.AddWithValue("@TipoCliente", TipoCliente);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        // Editar cliente
        public void MtdEditarClientes(int CodigoCliente, string Nombre, string NIT, int Telefono, string Direccion, string Departamento, string TipoCliente, string Estado)
        {
            string Query = "Update tbl_clientes set Nombre=@Nombre, NIT=@NIT, Telefono=@Telefono, Direccion=@Direccion, Departamento=@Departamento, TipoCliente=@TipoCliente, Estado=@Estado where CodigoCliente=@CodigoCliente";
            SqlCommand Sqlcmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
            Sqlcmd.Parameters.AddWithValue("@Nombre", Nombre);
            Sqlcmd.Parameters.AddWithValue("@NIT", NIT);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Direccion", Direccion);
            Sqlcmd.Parameters.AddWithValue("@Departamento", Departamento);
            Sqlcmd.Parameters.AddWithValue("@TipoCliente", TipoCliente);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        // Eliminar cliente
        public void MtdEliminarClientes(int CodigoCliente)
        {
            string Query = "Delete tbl_clientes where CodigoCliente=@CodigoCliente";
            SqlCommand Sqlcmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public bool MtdClienteTieneVentas(int codigoCliente)
        {
            string query = "SELECT COUNT(*) FROM tbl_ventas WHERE CodigoCliente = @CodigoCliente";
            SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoCliente", codigoCliente);

            int count = (int)cmd.ExecuteScalar();
            conn.MtdCerrarConexion();

            return count > 0; // true si tiene ventas
        }
    }
}


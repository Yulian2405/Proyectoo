using CapaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Datos
{
    public class CD_Ventas
    {


        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarVentas()
        {
            string Query = "Select * from tbl_ventas";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarVentas(int CodigoCliente, int CodigoSucursal, DateTime FechaVenta, string MetodoPago, string Estado)
        {
            string QueryAgregar = "Insert into tbl_Ventas ( CodigoCliente, CodigoSucursal, FechaVenta, MetodoPago,Estado) values ( @CodigoCliente, @CodigoSucursal, @FechaVenta,  @MetodoPago, @Estado);";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
            Sqlcmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            Sqlcmd.Parameters.AddWithValue("@FechaVenta", FechaVenta);
            Sqlcmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }


        public void MtdEditarVentas(int CodigoVenta, int CodigoCliente, int CodigoSucursal, DateTime FechaVenta,  string MetodoPago, string Estado)

        {
            string QueryAgregar = "Update tbl_Ventas set CodigoCliente=@CodigoCliente, CodigoSucursal=@CodigoSucursal, FechaVenta=@FechaVenta,  MetodoPago=@MetodoPago, Estado=@Estado Where CodigoVenta=@CodigoVenta";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoVenta", CodigoVenta);
            Sqlcmd.Parameters.AddWithValue("@CodigoCliente", CodigoCliente);
            Sqlcmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            Sqlcmd.Parameters.AddWithValue("@FechaVenta", FechaVenta);
            Sqlcmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }


        public void MtdEliminarVentas(int CodigoVenta)
        {
            string QueryAgregar = "Delete tbl_Ventas where CodigoVenta=@CodigoVenta";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoVenta", CodigoVenta);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public List<dynamic> MtdListaClientes()
        {
            List<dynamic> ListaClientes = new List<dynamic>();
            string QueryListaClientes = "Select CodigoCliente, Nombre from tbl_clientes; ";
            SqlCommand cmd = new SqlCommand(QueryListaClientes, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaClientes.Add(new
                {
                    Value = (int)reader["CodigoCliente"],
                    Text = $"{reader["CodigoCliente"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaClientes;
        }

        public List<dynamic> MtdListaSucursal()
        {
            List<dynamic> ListaSucursal = new List<dynamic>();
            string QueryListaSucursal = "Select CodigoSucursal, Nombre from tbl_sucursales; ";
            SqlCommand cmd = new SqlCommand(QueryListaSucursal, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaSucursal.Add(new
                {
                    Value = (int)reader["CodigoSucursal"],
                    Text = $"{reader["CodigoSucursal"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaSucursal;



        }


    }

}  

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Inventarios
    {
        CD_Conexion conn = new CD_Conexion();

        // Metodo que llena el combobox de Empleados    
        public List<dynamic> MtdListaSucursales()
        {
            List<dynamic> ListaSucursales = new List<dynamic>();
            string QueryListaSucursales = "Select CodigoSucursal, Nombre from tbl_sucursales; ";
            SqlCommand cmd = new SqlCommand(QueryListaSucursales, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaSucursales.Add(new
                {
                    Value = (int)reader["CodigoSucursal"],
                    Text = $"{reader["CodigoSucursal"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaSucursales;
        }

        // Metodo que llena el combobox de Empleados    
        public List<dynamic> MtdListaProductos()
        {
            List<dynamic> ListaProductos = new List<dynamic>();
            string QueryListaProductos = "Select CodigoProducto, Nombre from tbl_productos; ";
            SqlCommand cmd = new SqlCommand(QueryListaProductos, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaProductos.Add(new
                {
                    Value = (int)reader["CodigoProducto"],
                    Text = $"{reader["CodigoProducto"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaProductos;
        }

        public DataTable MtdConsultarInventarios()
        {
            string Query = "Select * from tbl_inventarios;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarInventario(int CodigoSucursal, int CodigoProducto, int Cantidad, int StockMinimo, DateTime FechaActualizacion, string Estado)
        {
            string Query = @"Insert into tbl_inventarios (CodigoSucursal, CodigoProducto, Cantidad, StockMinimo, FechaActualizacion, Estado) values (@CodigoSucursal, @CodigoProducto, @Cantidad, @StockMinimo, @FechaActualizacion, @Estado)";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@StockMinimo", StockMinimo);
            cmd.Parameters.AddWithValue("@FechaActualizacion", FechaActualizacion);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        // Metodo que muestra en el combobox la sucursal selecciona en el DataGridView
        public List<dynamic> MtdConsultaSucursalDgv(int CodigoSucursal)
        {
            List<dynamic> ListaDgv = new List<dynamic>();
            string QueryListaDgv = "Select  CodigoSucursal, Nombre from tbl_sucursales where CodigoSucursal=@CodigoSucursal;";
            SqlCommand cmd = new SqlCommand(QueryListaDgv, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaDgv.Add(new
                {
                    Value = Convert.ToInt32(reader["CodigoSucursal"]),
                    Text = $"{reader["CodigoSucursal"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaDgv;
        }

        // Metodo que muestra en el combobox la sucursal selecciona en el DataGridView
        public List<dynamic> MtdConsultaProductoDgv(int CodigoProducto)
        {
            List<dynamic> ListaDgv = new List<dynamic>();
            string QueryListaDgv = "Select  CodigoProducto, Nombre from tbl_productos where CodigoProducto=@CodigoProducto;";
            SqlCommand cmd = new SqlCommand(QueryListaDgv, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaDgv.Add(new
                {
                    Value = Convert.ToInt32(reader["CodigoProducto"]),
                    Text = $"{reader["CodigoProducto"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaDgv;
        }

        public void MtdEditarInventario(int CodigoInventario, int CodigoSucursal, int CodigoProducto, int Cantidad, int StockMinimo, DateTime FechaActualizacion, string Estado)
        {
            string Query = @"Update tbl_inventarios set CodigoSucursal=@CodigoSucursal, CodigoProducto=@CodigoProducto, Cantidad=@Cantidad, StockMinimo=@StockMinimo, FechaActualizacion=@FechaActualizacion, Estado=@Estado where CodigoInventario=@CodigoInventario";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoInventario", CodigoInventario);
            cmd.Parameters.AddWithValue("@CodigoSucursal", CodigoSucursal);
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@StockMinimo", StockMinimo);
            cmd.Parameters.AddWithValue("@FechaActualizacion", FechaActualizacion);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarInventario(int CodigoInventario)
        {
            string Query = "Delete tbl_inventarios where CodigoInventario=@CodigoInventario";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoInventario", CodigoInventario);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
        
    }
}

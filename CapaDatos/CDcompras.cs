using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapaDatos.CDenvios;

namespace CapaDatos
{
    public class CDcompras
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarCompras()
        {
            string Query = "Select * from tbl_compras;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarCompras(int CodigoProveedor, int CodigoProducto, int CodigoEmpleado, DateTime FechaCompra, int Cantidad, decimal Costo, decimal TotalCompra, string Estado)
        {
            string Query = @"Insert into tbl_compras (CodigoProveedor, CodigoProducto, CodigoEmpleado, FechaCompra, Cantidad, Costo, TotalCompra, Estado)
                             values (@CodigoProveedor, @CodigoProducto, @CodigoEmpleado, @FechaCompra, @Cantidad, @Costo, @TotalCompra, @Estado)";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@FechaCompra", FechaCompra);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@Costo", Costo);
            cmd.Parameters.AddWithValue("@TotalCompra", TotalCompra);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarCompras(int CodigoCompra, int CodigoProveedor, int CodigoProducto, int CodigoEmpleado, DateTime FechaCompra, int Cantidad, decimal Costo, decimal TotalCompra, string Estado)
        {
            string Query = @"Update tbl_compras
                             set CodigoProveedor=@CodigoProveedor, CodigoProducto=@CodigoProducto, CodigoEmpleado=@CodigoEmpleado, FechaCompra=@FechaCompra, Cantidad=@Cantidad, Costo=@Costo, TotalCompra=@TotalCompra, Estado=@Estado
                            where CodigoCompra=@CodigoCompra";

            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoCompra", CodigoCompra);
            cmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            cmd.Parameters.AddWithValue("@FechaCompra", FechaCompra);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@Costo", Costo);
            cmd.Parameters.AddWithValue("@TotalCompra", TotalCompra);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarCompras(int CodigoCompra)
        {
            string Query = "Delete tbl_compras where CodigoCompra=@CodigoCompra";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoCompra", CodigoCompra);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
        public List<ItemComboBox> MtdConsultarProducto()
        {
            List<ItemComboBox> lista = new List<ItemComboBox>();
            string query = "SELECT CodigoProducto, Nombre FROM tbl_productos;";
            SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ItemComboBox
                {
                    Value = Convert.ToInt32(reader["CodigoProducto"]),
                    Text = $"{reader["CodigoProducto"]} - {reader["Nombre"]}"
                });
            }

            conn.MtdCerrarConexion();
            return lista;
        }

 
            public List<ItemComboBox> MtdConsultarProveedor()
            {
                List<ItemComboBox> lista = new List<ItemComboBox>();
                string query = "SELECT CodigoProveedor, Nombre FROM tbl_proveedores;";
                SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new ItemComboBox
                    {
                        Value = Convert.ToInt32(reader["CodigoProveedor"]),
                        Text = $"{reader["CodigoProveedor"]} - {reader["Nombre"]}"
                    });
                }

                conn.MtdCerrarConexion();
                return lista;
            }


        public List<ItemComboBox> MtdConsultarEmpleado()
        {
            List<ItemComboBox> ListaDgv = new List<ItemComboBox>();
            string QueryListaDgv = "SELECT CodigoEmpleado, Nombre FROM tbl_empleados;";
            SqlCommand cmd = new SqlCommand(QueryListaDgv, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaDgv.Add(new ItemComboBox
                {
                    Value = Convert.ToInt32(reader["CodigoEmpleado"]),
                    Text = $"{reader["CodigoEmpleado"]} - {(reader["Nombre"])}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaDgv;
        }

        public decimal MtdObtenerPrecioProducto(int CodigoProducto)
        {
            string query = "SELECT Precio FROM tbl_productos WHERE CodigoProducto = @CodigoProducto;";
            SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);

            object result = cmd.ExecuteScalar();
            conn.MtdCerrarConexion();

            if (result != null && result != DBNull.Value)
                return Convert.ToDecimal(result);
            else
                return 0;
        }
    }
}

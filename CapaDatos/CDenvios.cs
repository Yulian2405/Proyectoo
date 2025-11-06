using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDenvios
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarEnvios()
        {
            string Query = "Select * from tbl_envios;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarEnvios(int CodigoVenta, DateTime FechaEnvio, string Direccion, string Vehiculo, decimal CostoEnvio, string Estado)
        {
            string Query = @"Insert into tbl_envios (CodigoVenta, FechaEnvio, Direccion, Vehiculo, CostoEnvio, Estado)
                             values (@CodigoVenta, @FechaEnvio, @Direccion, @Vehiculo, @CostoEnvio, @Estado)";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoVenta", CodigoVenta);
            cmd.Parameters.AddWithValue("@FechaEnvio", FechaEnvio);
            cmd.Parameters.AddWithValue("@Direccion", Direccion);
            cmd.Parameters.AddWithValue("@Vehiculo", Vehiculo);
            cmd.Parameters.AddWithValue("@CostoEnvio", CostoEnvio);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarEnvios(int CodigoEnvio, int CodigoVenta, DateTime FechaEnvio, string Direccion, string Vehiculo, decimal CostoEnvio, string Estado)
        {
            string Query = @"Update tbl_envios
                             set CodigoVenta=@CodigoVenta, FechaEnvio=@FechaEnvio, Direccion=@Direccion, Vehiculo=@Vehiculo, CostoEnvio=@CostoEnvio, Estado=@Estado
                             where CodigoEnvio=@CodigoEnvio";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEnvio", CodigoEnvio);
            cmd.Parameters.AddWithValue("@CodigoVenta", CodigoVenta);
            cmd.Parameters.AddWithValue("@FechaEnvio", FechaEnvio);
            cmd.Parameters.AddWithValue("@Direccion", Direccion);
            cmd.Parameters.AddWithValue("@Vehiculo", Vehiculo);
            cmd.Parameters.AddWithValue("@CostoEnvio", CostoEnvio);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarEnvios(int CodigoEnvio)
        {
            string Query = "Delete tbl_envios where CodigoEnvio=@CodigoEnvio";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEnvio", CodigoEnvio);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
        public class ItemComboBox
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public List<ItemComboBox> MtdConsultaVentasDgv()
        {
            List<ItemComboBox> ListaDgv = new List<ItemComboBox>();
            string QueryListaDgv = "SELECT CodigoVenta, FechaVenta FROM tbl_ventas;";
            SqlCommand cmd = new SqlCommand(QueryListaDgv, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaDgv.Add(new ItemComboBox
                {
                    Value = Convert.ToInt32(reader["CodigoVenta"]),
                    Text = $"{reader["CodigoVenta"]} - {Convert.ToDateTime(reader["FechaVenta"]).ToString("dd/MM/yyyy")}"
                });
            }

            conn.MtdCerrarConexion();
            return ListaDgv;
        }



    }
}

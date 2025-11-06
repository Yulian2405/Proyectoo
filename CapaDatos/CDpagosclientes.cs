using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapaDatos.CDenvios;
using System.Threading;

namespace CapaDatos
{
    public class CDpagosclientes
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarCompras()
        {
            string Query = "SELECT * FROM tbl_pagosClientes;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarPagoClientes (int CodigoEnvio, DateTime Fecha, double Monto, string MetodoPago, string ReferenciaPago, string Estado)
        {
            string Query = @"INSERT INTO tbl_pagosClientes 
                             (CodigoEnvio, Fecha, Monto, MetodoPago, ReferenciaPago, Estado)
                             VALUES (@CodigoEnvio, @Fecha, @Monto, @MetodoPago, @ReferenciaPago, @Estado);";

            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEnvio", CodigoEnvio);
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Monto", Monto);
            cmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            cmd.Parameters.AddWithValue("@ReferenciaPago", ReferenciaPago);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarPagosClientes (int CodigoPago, int CodigoEnvio, DateTime Fecha, double Monto, string MetodoPago, string ReferenciaPago, string Estado)
        {
            string Query = "UPDATE tbl_pagosClientes set CodigoEnvio=@CodigoEnvio, Fecha=@Fecha, Monto=@Monto, MetodoPago=@MetodoPago, ReferenciaPago=@ReferenciaPago, Estado=@Estado WHERE CodigoPago = @CodigoPago";

            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoPago", CodigoPago);
            cmd.Parameters.AddWithValue("@CodigoEnvio", CodigoEnvio);
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Monto", Monto);
            cmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            cmd.Parameters.AddWithValue("@ReferenciaPago", ReferenciaPago);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarPagosClientes (int CodigoPago)
        {
            string Query = "DELETE FROM tbl_pagosClientes WHERE CodigoPago = @CodigoPago;";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoPago", CodigoPago);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
        public List<ItemComboBox> MtdConsultarEnvios()
        {
            List<ItemComboBox> lista = new List<ItemComboBox>();
            string query = "SELECT CodigoEnvio, Direccion FROM tbl_envios";
            SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new ItemComboBox
                {
                    Value = Convert.ToInt32(reader["CodigoEnvio"]),
                    Text = $"{reader["CodigoEnvio"]} - {reader["Direccion"]}"
                });
            }

            reader.Close();
            conn.MtdCerrarConexion();

            return lista;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CapaDatos.CDenvios;

namespace CapaDatos
{
    public class CDpagoproveedores
    {

        CD_Conexion conn = new  CD_Conexion();

        public DataTable MtdConsultarCompras()
        {
            string Query = "Select * from tbl_pagosProveedores;";
            SqlDataAdapter da = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarPagosProveedores(int CodigoProveedor, DateTime Fecha, decimal Monto, string MetodoPago, string ReferenciaPago, string Estado)
        {
            string Query = @"Insert into tbl_pagosProveedores (CodigoProveedor, Fecha, Monto, MetodoPago, ReferenciaPago, Estado)
                             values (@CodigoProveedor, @Fecha, @Monto, @MetodoPago, @ReferenciaPago, @Estado)";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Monto", Monto);
            cmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            cmd.Parameters.AddWithValue("@ReferenciaPago", ReferenciaPago);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEditarPagosProveedores(int CodigoPagoP, int CodigoProveedor, DateTime Fecha, decimal Monto, string MetodoPago, string ReferenciaPago, string Estado)
        {
            string Query = @"Update tbl_pagosProveedores
                             set CodigoProveedor=@CodigoProveedor, Fecha=@Fecha, Monto=@Monto, MetodoPago=@MetodoPago, ReferenciaPago=@ReferenciaPago, Estado=@Estado
                            where CodigoPagoP=@CodigoPagoP";

            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoPagoP", CodigoPagoP);
            cmd.Parameters.AddWithValue("@CodigoProveedor", CodigoProveedor);
            cmd.Parameters.AddWithValue("@Fecha", Fecha);
            cmd.Parameters.AddWithValue("@Monto", Monto);
            cmd.Parameters.AddWithValue("@MetodoPago", MetodoPago);
            cmd.Parameters.AddWithValue("@ReferenciaPago", ReferenciaPago);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

        public void MtdEliminarPagoProveedores(int CodigoPagoP)
        {
            string Query = "Delete tbl_pagosProveedores where CodigoPagoP=@CodigoPagoP";
            SqlCommand cmd = new SqlCommand(Query, conn.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoPagoP", CodigoPagoP);
            cmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }
        public List<ItemComboBox> MtdConsultarProveedores()
        {
            List<ItemComboBox> lista = new List<ItemComboBox>();
            string query = "SELECT CodigoProveedor, Nombre FROM tbl_proveedores";
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

    }
}

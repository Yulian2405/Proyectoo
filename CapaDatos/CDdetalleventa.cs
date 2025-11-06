using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDdetalleventa
    {
        CD_Conexion cd_conexion =new CD_Conexion();

        public List<dynamic> MtdLista()
        {
            List<dynamic> Lista = new List<dynamic>();
            string QueryLista = "Select Codigoventa from tbl_Ventas";
            SqlCommand cmd = new SqlCommand(QueryLista, cd_conexion.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Lista.Add(new
                {
                    Value = reader["Codigoventa"],
                    Text = $"{reader["Codigoventa"]}"
                });
            }

            cd_conexion.MtdCerrarConexion();
            return Lista;
        }

        public DataTable MtdConsultar()
        {
            string QueryConsultar = "Select * from tbl_Detalleventa";
            SqlDataAdapter Adapter = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            Adapter.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }
        public void MtdAgregar(int Codigoventa, int Cantidad, double Descuentos, double Impuestos, double Total, string Estado, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryAgregar = "INSERT INTO tbl_Detalleventa( Codigoventa, Cantidad, Descuentos, Impuestos,Total, Estado,   UsuarioAuditoria, FechaAuditoria) VALUES (@Codigoventa, @Cantidad, @Descuentos, @Impuestos, @Total, @Estado,  @UsuarioAuditoria, @FechaAuditoria)";
            SqlCommand cmd = new SqlCommand(QueryAgregar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@Codigoventa", Codigoventa);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@Descuentos", Descuentos);
            cmd.Parameters.AddWithValue("@Impuestos", Impuestos);
            cmd.Parameters.AddWithValue("@Total", Total);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdActualizar(int Codigodetalleventa, int Codigoventa, int Cantidad, double Descuentos, double Impuestos, double Total, string Estado, string UsuarioAuditoria, DateTime FechaAuditoria)
        {
            string QueryActualizar = "Update tbl_Detalleventa set Codigoventa=@Codigoventa, Cantidad=@Cantidad, Descuentos=@Descuentos, Impuestos=@Impuestos, Total=@Total, Estado=@Estado,  UsuarioAuditoria=@UsuarioAuditoria, FechaAuditoria=@FechaAuditoria where Codigodetalleventa=@Codigodetalleventa ";
            SqlCommand cmd = new SqlCommand(QueryActualizar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@Codigodetalleventa", Codigodetalleventa);
            cmd.Parameters.AddWithValue("@Codigoventa", Codigoventa);
            cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
            cmd.Parameters.AddWithValue("@Descuentos", Descuentos);
            cmd.Parameters.AddWithValue("@Impuestos", Impuestos);
            cmd.Parameters.AddWithValue("@Total", Total);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.Parameters.AddWithValue("@FechaAuditoria", FechaAuditoria);
            cmd.Parameters.AddWithValue("@UsuarioAuditoria", UsuarioAuditoria);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdEliminar(int Codigodetalleventa)
        {
            string QueryEliminar = "Delete tbl_Detalleventa where Codigodetalleventa=@Codigodetalleventa";
            SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@Codigodetalleventa", Codigodetalleventa);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
    }
}

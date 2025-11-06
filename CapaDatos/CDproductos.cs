using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CDproductos     
    {
        CD_Conexion cd_conexion = new CD_Conexion();

        public List<dynamic> MtdListaDetalleVenta()
        {
            List<dynamic> Lista = new List<dynamic>();
            string QueryLista = "Select CodigoDetalle from tbl_Detalleventa";
            SqlCommand cmd = new SqlCommand(QueryLista, cd_conexion.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())   
            {
                Lista.Add(new
                {
                    Value = reader["CodigoDetalle"],
                    Text = $"{reader["CodigoDetalle"]}"
                });
            }

            cd_conexion.MtdCerrarConexion();
            return Lista;
        }

        public DataTable MtdConsultarProductos()
        {
            string QueryConsultar = "Select * from tbl_productos";
            SqlDataAdapter Adapter = new SqlDataAdapter(QueryConsultar, cd_conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            Adapter.Fill(Dt);
            cd_conexion.MtdCerrarConexion();
            return Dt;
        }

        public void MtdAgregarProducto( int CodigoDetalle, string CodigoBarra, string Nombre, double Precio, int Stock, string Categoria,  DateTime FechaVencimiento, string Estado)
        {
            string QueryAgregar = "INSERT INTO tbl_productos( CodigoDetalle, CodigoBarra, Nombre, Precio,Stock, Categoria,FechaVencimiento,Estado) VALUES (@CodigoDetalle, @CodigoBarra, @Nombre, @Precio, @Stock, @Categoria,@FechaVencimiento,@Estado)";
            SqlCommand cmd = new SqlCommand(QueryAgregar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoDetalle", CodigoDetalle);
            cmd.Parameters.AddWithValue("@CodigoBarra", CodigoBarra);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Precio", Precio);
            cmd.Parameters.AddWithValue("@Categoria", Categoria);
            cmd.Parameters.AddWithValue("@Stock", Stock);
            cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdActualizarProducto(int CodigoProducto, int CodigoDetalle, string CodigoBarra, string Nombre, double Precio, int Stock, string Categoria, DateTime FechaVencimiento, string Estado)
        {
            string QueryActualizar = @"Update tbl_productos set CodigoDetalle=@CodigoDetalle, CodigoBarra=@CodigoBarra, Nombre=@Nombre, Precio=@Precio, Stock=@Stock, Categoria=@Categoria, FechaVencimiento=@FechaVencimiento, Estado=@Estado where CodigoProducto=@CodigoProducto";
            SqlCommand cmd = new SqlCommand(QueryActualizar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.Parameters.AddWithValue("@CodigoDetalle", CodigoDetalle);
            cmd.Parameters.AddWithValue("@CodigoBarra", CodigoBarra);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Precio", Precio);
            cmd.Parameters.AddWithValue("@Stock", Stock);
            cmd.Parameters.AddWithValue("@Categoria", Categoria);
            cmd.Parameters.AddWithValue("@FechaVencimiento", FechaVencimiento);
            cmd.Parameters.AddWithValue("@Estado", Estado);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
        public void MtdEliminar(int CodigoProducto)
        {
            string QueryEliminar = "Delete tbl_Productos where CodigoProducto=@CodigoProducto";
            SqlCommand cmd = new SqlCommand(QueryEliminar, cd_conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoProducto", CodigoProducto);
            cmd.ExecuteNonQuery();
            cd_conexion.MtdCerrarConexion();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Empleados
    {
        CD_Conexion conn = new CD_Conexion();

        public DataTable MtdConsultarEmpleados()
        {
            string Query = "Select * from tbl_empleados";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(Query, conn.MtdAbrirConexion());
            DataTable dt = new DataTable();
            SqlAdap.Fill(dt);
            conn.MtdCerrarConexion();
            return dt;
        }

        public void MtdAgregarEmpleados(string Nombre, int Telefono, string Correo, string Cargo, double SalarioBase, DateTime FechaIngreso, string Estado)
        {
            string QueryAgregar = "Insert into tbl_empleados ( Nombre, Telefono, Correo, Cargo, SalarioBase,  FechaIngreso, Estado) values ( @Nombre, @Telefono, @Correo,  @Cargo,  @SalarioBase, @FechaIngreso, @Estado);";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@Nombre", Nombre);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@Cargo", Cargo);
            Sqlcmd.Parameters.AddWithValue("@SalarioBase", SalarioBase);
            Sqlcmd.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

 
        public void MtdEditarEmpleados(int CodigoEmpleado, string Nombre, int Telefono, string Correo, string Cargo, double SalarioBase, DateTime FechaIngreso, string Estado)
        {
            string QueryAgregar = "Update tbl_empleados set Nombre=@Nombre, Telefono=@Telefono, Correo=@Correo, Cargo=@Cargo, SalarioBase=@SalarioBase,  FechaIngreso=@FechaIngreso, Estado=@Estado Where CodigoEmpleado=@CodigoEmpleado";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            Sqlcmd.Parameters.AddWithValue("@Nombre", Nombre);
            Sqlcmd.Parameters.AddWithValue("@Telefono", Telefono);
            Sqlcmd.Parameters.AddWithValue("@Correo", Correo);
            Sqlcmd.Parameters.AddWithValue("@Cargo", Cargo);
            Sqlcmd.Parameters.AddWithValue("@SalarioBase", SalarioBase);
            Sqlcmd.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
            Sqlcmd.Parameters.AddWithValue("@Estado", Estado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }


        public void MtdEliminarEmpleados(int CodigoEmpleado)
        {
            string QueryAgregar = "Delete tbl_empleados where CodigoEmpleado=@CodigoEmpleado";
            SqlCommand Sqlcmd = new SqlCommand(QueryAgregar, conn.MtdAbrirConexion());
            Sqlcmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            Sqlcmd.ExecuteNonQuery();
            conn.MtdCerrarConexion();
        }

    }

}

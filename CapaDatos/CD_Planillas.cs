using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    
    public class CD_Planillas
    {
        CD_Conexion conexion = new CD_Conexion();
        public List<dynamic> MtdListaEmpleados()
        {
            List<dynamic> ListaEmpleados = new List<dynamic>();
            string QueryListaEmpleados = "Select CodigoEmpleado, Nombre from tbl_empleados; ";
            SqlCommand cmd = new SqlCommand(QueryListaEmpleados, conexion.MtdAbrirConexion());
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaEmpleados.Add(new
                {
                    Value = (int)reader["CodigoEmpleado"],
                    Text = $"{reader["CodigoEmpleado"]} - {reader["Nombre"]}"
                });
            }

            conexion.MtdCerrarConexion();
            return ListaEmpleados;
        }

    
        public double MtdConsultaSalarioBase(int CodigoEmpleado)
        {
            double SalarioBase = 0;

            string QueryConsultarSalarioBase = "Select SalarioBase from tbl_empleados where CodigoEmpleado=@CodigoEmpleado;";
            SqlCommand CommandSalarioBase = new SqlCommand(QueryConsultarSalarioBase, conexion.MtdAbrirConexion());
            CommandSalarioBase.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            SqlDataReader reader = CommandSalarioBase.ExecuteReader();

            if (reader.Read())
            {
                SalarioBase = double.Parse(reader["SalarioBase"].ToString());
            }
            else
            {
                SalarioBase = 0;
            }

            conexion.MtdCerrarConexion();
            return SalarioBase;
        }


     
        public DataTable MtdConsultarPlanillas()
        {
            string QueryConsultaPlanillas = "Select * from tbl_Planillas;";
            SqlDataAdapter SqlAdap = new SqlDataAdapter(QueryConsultaPlanillas, conexion.MtdAbrirConexion());
            DataTable Dt = new DataTable();
            SqlAdap.Fill(Dt);
            conexion.MtdCerrarConexion();
            return Dt;
        }

  
        public void MtdAgregarDatosPlanillas(int CodigoEmpleado, DateTime FechaPago, double SalarioBase, double Bonos, double Descuentos, double PagoFinal, string Estado)
        {
            string QueryAgregarDatos = "Insert into tbl_Planillas (CodigoEmpleado, FechaPago, SalarioBase, Bonos, Descuentos, PagoFinal, Estado) values (@CodigoEmpleado, @FechaPago, @SalarioBase, @Bonos, @Descuentos, @PagoFinal, @Estado);";
            SqlCommand comm = new SqlCommand(QueryAgregarDatos, conexion.MtdAbrirConexion());
            comm.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            comm.Parameters.AddWithValue("@FechaPago", FechaPago);
            comm.Parameters.AddWithValue("@SalarioBase", SalarioBase);
            comm.Parameters.AddWithValue("@Bonos", Bonos);
            comm.Parameters.AddWithValue("@Descuentos", Descuentos);
            comm.Parameters.AddWithValue("@PagoFinal", PagoFinal);
            comm.Parameters.AddWithValue("@Estado", Estado);
            comm.ExecuteNonQuery();
            conexion.MtdCerrarConexion();
        }


        public List<dynamic> MtdConsultaEmpleadoDgv(int CodigoEmpleado)
        {
            List<dynamic> ListaDgv = new List<dynamic>();
            string QueryListaDgv = "Select  CodigoEmpleado, Nombre from tbl_empleados where CodigoEmpleado=@CodigoEmpleado;";
            SqlCommand cmd = new SqlCommand(QueryListaDgv, conexion.MtdAbrirConexion());
            cmd.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListaDgv.Add(new
                {
                    Value = Convert.ToInt32(reader["CodigoEmpleado"]),
                    Text = $"{reader["CodigoEmpleado"]} - {reader["Nombre"]}"
                });
            }

            conexion.MtdCerrarConexion();
            return ListaDgv;
        }



 
        public void MtdEditarDatosPlanillas(int CodigoPlanilla, int CodigoEmpleado, DateTime FechaPago, double SalarioBase, double Bonos, double Descuentos, double PagoFinal, string Estado)
        {
            string QueryEditarDatos = "Update tbl_Planillas  set CodigoEmpleado=@CodigoEmpleado, FechaPago=@FechaPago, SalarioBase=@SalarioBase, Bonos=@Bonos, Descuentos=@Descuentos, PagoFinal=@PagoFinal, Estado=@Estado where CodigoPlanilla=@CodigoPlanilla;";
            SqlCommand comm = new SqlCommand(QueryEditarDatos, conexion.MtdAbrirConexion());
            comm.Parameters.AddWithValue("@CodigoPlanilla", CodigoPlanilla);
            comm.Parameters.AddWithValue("@CodigoEmpleado", CodigoEmpleado);
            comm.Parameters.AddWithValue("@FechaPago", FechaPago);
            comm.Parameters.AddWithValue("@SalarioBase", SalarioBase);
            comm.Parameters.AddWithValue("@Bonos", Bonos);
            comm.Parameters.AddWithValue("@Descuentos", Descuentos);
            comm.Parameters.AddWithValue("@PagoFinal", PagoFinal);
            comm.Parameters.AddWithValue("@Estado", Estado);
            comm.ExecuteNonQuery();
            conexion.MtdCerrarConexion();
        }

       
        public void MtdEliminarDatosPlanillas(int CodigoPlanilla)
        {
            string QueryEliminarDatos = "Delete tbl_Planillas where CodigoPlanilla=@CodigoPlanilla;";
            SqlCommand comm = new SqlCommand(QueryEliminarDatos, conexion.MtdAbrirConexion());
            comm.Parameters.AddWithValue("@CodigoPlanilla", CodigoPlanilla);
            comm.ExecuteNonQuery();
            conexion.MtdCerrarConexion();
        }


    }

}

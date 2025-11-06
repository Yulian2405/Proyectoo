using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Conexion
    {
        private SqlConnection db_conexion = new SqlConnection("Data Source=YULIAN\\SQLEXPRESS;Initial Catalog=db_ProyectoFinal;Integrated Security=True;Encrypt=False");

        // Metodo para abrir la conexión a la base de datos
        public SqlConnection MtdAbrirConexion()
        {
            if (db_conexion.State == System.Data.ConnectionState.Closed)
            {
                db_conexion.Open();
            }

            return db_conexion;
        }

        // Metodo para cerrar la conexión a la base de datos
        public SqlConnection MtdCerrarConexion()
        {
            if (db_conexion.State == System.Data.ConnectionState.Open)
            {
                db_conexion.Close();
            }

            return db_conexion;
        }
    }
}


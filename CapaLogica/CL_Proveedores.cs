using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class CL_Proveedores
    {
        public double MtdSalarioBase(string Cargo)
        {
            if (Cargo == "Gerente") return 45000;
            if (Cargo == "Supervisor") return 20000;
            if (Cargo == "Soporte") return 15000;
            if (Cargo == "Contador") return 9000;
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica
{
    public class CL_Planillas
    {
        public double MtdCalcularBono(double SalarioBase)
        {
            if (SalarioBase > 0 && SalarioBase <= 5000) return (250 + (SalarioBase * 0.05));
            else if (SalarioBase > 5000 && SalarioBase <= 7000) return (250 + (SalarioBase * 0.10));
            else if (SalarioBase > 7000 && SalarioBase <= 10000) return (250 + (SalarioBase * 0.15));
            else if (SalarioBase > 10000) return (250 + (SalarioBase * 0.20));
            else return 0;
        }

        public double MtdCalcularIggs(double SalarioBase)
        {
            if (SalarioBase > 0 && SalarioBase <= 5000) return (SalarioBase * 0.03);
            else if (SalarioBase > 5000 && SalarioBase <= 7000) return (SalarioBase * 0.04);
            else if (SalarioBase > 7000 && SalarioBase <= 10000) return (SalarioBase * 0.05);
            else if (SalarioBase > 10000) return (SalarioBase * 0.06);
            else return 0;
        }

        public double MtdCalcularIsr(double SalarioBase)
        {
            if (SalarioBase > 0 && SalarioBase <= 5000) return (SalarioBase * 0.05);
            else if (SalarioBase > 5000 && SalarioBase <= 7000) return (SalarioBase * 0.06);
            else if (SalarioBase > 7000 && SalarioBase <= 10000) return (SalarioBase * 0.07);
            else if (SalarioBase > 10000) return (SalarioBase * 0.08);
            else return 0;
        }

        public double MtdCalcularSalarioFinal(double SalarioBase, double Bono, double Iggs, double Isr)
        {
            return (SalarioBase + Bono) - (Iggs + Isr);
        }
    }
}

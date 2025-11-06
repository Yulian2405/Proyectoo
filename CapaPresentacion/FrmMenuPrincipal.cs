using ComponentFactory.Krypton.Toolkit;
using ProyectoMiguel;
using ProyectoUMGDarien;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmMenuPrincipal : KryptonForm
    {
        //Estilo Borde Form Menu Pricipal
        private int borderSize = 2;
        private Size formSize;
        public FrmMenuPrincipal()
        {
            InitializeComponent();
            //CollapseMenu();
            this.Padding = new Padding(borderSize);//Border size
            this.BackColor = Color.FromArgb(98, 102, 244);//Border color
        }

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            formSize = this.ClientSize;
        }

        //Logica para mover formulario
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmRoles>();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PanelSuperior_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0); // Mover el formulario
            }
        }

        private void PanelSuperior_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Cursor.Position.Y < 15)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
        }
        // Logica de Botones para Abrir Formularios Crud
        private void AbrirFormulario<MiForm>() where MiForm : Form, new()
        {
            Form formulario;
            formulario = PanelCentral.Controls.OfType<MiForm>().FirstOrDefault();
            if (formulario == null)
            {
                formulario = new MiForm();
                formulario.TopLevel = false;
                formulario.FormBorderStyle = FormBorderStyle.None;
                formulario.Dock = DockStyle.Fill;
                PanelCentral.Controls.Add(formulario);
                PanelCentral.Tag = formulario;
                formulario.Show();
                formulario.BringToFront();
            }
            //si el formulario/instancia existe
            else
            {
                formulario.BringToFront();
            }
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmVentas>();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmProveedores>();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmUsuario>();
        }

        private void btnEmpleados_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmEmpleados>();
        }

        private void btnDetalleVentas_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Frmdetalleventa>();
        }

        private void btnPagoPlanilla_Click(object sender, EventArgs e)
        {

        }

        private void btnPlanillas_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmPlanillas>();
        }

        private void btnClientes_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario<FrmClientes>();
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmCompras>();
        }

        private void btnEnvios_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmEnvios>();
        }

        private void btnPagosProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario <FrmPagosProveedores>();
        }

        private void btnPagosClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmPagoClientes>();
        }

        private void btnSucursales_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmSucursales>();
        }

        private void btnInventarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario<FrmInventarios>();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFormulario<Frmproductos>();
        }

        private void PanelSuperior_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
    
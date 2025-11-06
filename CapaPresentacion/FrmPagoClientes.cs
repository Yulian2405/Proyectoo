using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos;

namespace CapaPresentacion
{
    public partial class FrmPagoClientes : Form
    {
        CDpagosclientes cDpagosclientes = new CDpagosclientes();
        public FrmPagoClientes()
        {
            InitializeComponent();
        }

        private void FrmPagoClientes_Load(object sender, EventArgs e)
        {
            MtdCargarPagosClientes();
            MtdCargarEnvios();
        }

        private void MtdCargarPagosClientes()
        {
            DataTable dt = cDpagosclientes.MtdConsultarCompras();
            dgvPagosClientes.DataSource = dt;
        }

        private void MtdCargarEnvios()
        {
            cboxCodigoEnvio.DataSource = cDpagosclientes.MtdConsultarEnvios();
            cboxCodigoEnvio.DisplayMember = "Text";
            cboxCodigoEnvio.ValueMember = "Value";
            cboxCodigoEnvio.SelectedIndex = -1;
        }
        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            txtMonto.Clear();
        }

        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            txtReferenciaPago.Clear();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoEnvio.Text) || string.IsNullOrEmpty(txtMonto.Text) || string.IsNullOrEmpty(cboxMetodoPago.Text) || string.IsNullOrEmpty(txtReferenciaPago.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Empleado
                    var SelectedEnvio = (dynamic)cboxCodigoEnvio.SelectedItem;
                    int CodigoEnvio = (int)SelectedEnvio.GetType().GetProperty("Value").GetValue(SelectedEnvio, null);
                    
                    DateTime FechaPago = dtpFechaPago.Value;
                    double Monto = double.Parse(txtMonto.Text);
                    string MetodoPago = cboxMetodoPago.Text;
                    string ReferenciaPago = txtReferenciaPago.Text;
                    string Estado = cboxEstado.Text;



                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cDpagosclientes.MtdAgregarPagoClientes(CodigoEnvio, FechaPago, Monto, MetodoPago, ReferenciaPago, Estado);
                    MessageBox.Show("Pago agregado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdCargarPagosClientes();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPago.Text) || string.IsNullOrEmpty(cboxCodigoEnvio.Text) || string.IsNullOrEmpty(txtMonto.Text) || string.IsNullOrEmpty(cboxMetodoPago.Text) || string.IsNullOrEmpty(txtReferenciaPago.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    int CodigoPago = int.Parse(txtCodigoPago.Text);
                    //Codigo Empleado
                    var SelectedEnvio = (dynamic)cboxCodigoEnvio.SelectedItem;
                    int CodigoEnvio = (int)SelectedEnvio.GetType().GetProperty("Value").GetValue(SelectedEnvio, null);

                    DateTime FechaPago = dtpFechaPago.Value;
                    double Monto = double.Parse(txtMonto.Text);
                    string MetodoPago = cboxMetodoPago.Text;
                    string ReferenciaPago = txtReferenciaPago.Text;
                    string Estado = cboxEstado.Text;



                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cDpagosclientes.MtdEditarPagosClientes(CodigoPago, CodigoEnvio, FechaPago, Monto, MetodoPago, ReferenciaPago, Estado);
                    MessageBox.Show("Pago actualizado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdCargarPagosClientes();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPago.Text))
            {
                MessageBox.Show("Por favor, Seleccine el pago que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoPago = int.Parse(txtCodigoPago.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cDpagosclientes.MtdEliminarPagosClientes(CodigoPago);
                    MessageBox.Show("Pago eliminado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdCargarPagosClientes();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void dgvPagosClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvPagosClientes.Rows[e.RowIndex];

                txtCodigoPago.Text = fila.Cells["CodigoPago"].Value.ToString();
                cboxCodigoEnvio.SelectedValue = fila.Cells["CodigoEnvio"].Value;
                dtpFechaPago.Value = Convert.ToDateTime(fila.Cells["Fecha"].Value);
                txtMonto.Text = fila.Cells["Monto"].Value.ToString();
                cboxMetodoPago.Text = fila.Cells["MetodoPago"].Value.ToString();
                txtReferenciaPago.Text = fila.Cells["ReferenciaPago"].Value.ToString();
                cboxEstado.Text = fila.Cells["Estado"].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MtdLimpiarCampos()
        {
            txtCodigoPago.Clear();
            txtMonto.Clear();
            txtReferenciaPago.Clear();


            if (cboxCodigoEnvio.Items.Count > 0)
                cboxCodigoEnvio.SelectedIndex = -1;

            if (cboxMetodoPago.Items.Count > 0)
                cboxMetodoPago.SelectedIndex = -1;

            if (cboxEstado.Items.Count > 0)
                cboxEstado.SelectedIndex = -1;


            dtpFechaPago.Value = DateTime.Now;

        }
    }
}

using CapaDatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmPagosProveedores : Form
    {
        CDpagoproveedores CDpagos = new CDpagoproveedores();

        public FrmPagosProveedores()
        {
            InitializeComponent();
        }

        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            txtMonto.Clear();
        }

        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            txtReferenciaPago.Clear();
        }

        private void FrmPagosProveedores_Load(object sender, EventArgs e)
        {
            // Cargar proveedores en el ComboBox
            cboxCodigoProveedor.DataSource = CDpagos.MtdConsultarProveedores();
            cboxCodigoProveedor.DisplayMember = "Text";
            cboxCodigoProveedor.ValueMember = "Value";

            // Cargar estado y método de pago (ejemplo)
            cboxEstado.Items.AddRange(new string[] { "Pendiente", "Pagado", "Cancelado" });
            cboxMetodoPago.Items.AddRange(new string[] { "Efectivo", "Transferencia", "Cheque" });

            // Mostrar pagos en DataGridView
            MtdMostrarPagos();
        }

        private void MtdMostrarPagos()
        {
            dgvPagosProveedores.DataSource = CDpagos.MtdConsultarCompras(); // Tu método devuelve DataTable
        }

        private void LimpiarCampos()
        {
            txtCodigoPago.Clear();
            cboxCodigoProveedor.SelectedIndex = -1;
            dtpFechaPago.Value = DateTime.Now;
            txtMonto.Clear();
            cboxMetodoPago.SelectedIndex = -1;
            txtReferenciaPago.Clear();
            cboxEstado.SelectedIndex = -1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboxCodigoProveedor.SelectedIndex == -1 || string.IsNullOrEmpty(txtMonto.Text) || cboxEstado.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor complete todos los campos obligatorios.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int codigoProveedor = Convert.ToInt32(cboxCodigoProveedor.SelectedValue);
                DateTime fecha = dtpFechaPago.Value;
                decimal monto = Convert.ToDecimal(txtMonto.Text);
                string metodoPago = cboxMetodoPago.Text;
                string referencia = txtReferenciaPago.Text;
                string estado = cboxEstado.Text;

                CDpagos.MtdAgregarPagosProveedores(codigoProveedor, fecha, monto, metodoPago, referencia, estado);
                MessageBox.Show("Pago agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MtdMostrarPagos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPago.Text))
            {
                MessageBox.Show("Seleccione un pago para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int codigoPago = Convert.ToInt32(txtCodigoPago.Text);
                int codigoProveedor = Convert.ToInt32(cboxCodigoProveedor.SelectedValue);
                DateTime fecha = dtpFechaPago.Value;
                decimal monto = Convert.ToDecimal(txtMonto.Text);
                string metodoPago = cboxMetodoPago.Text;
                string referencia = txtReferenciaPago.Text;
                string estado = cboxEstado.Text;

                CDpagos.MtdEditarPagosProveedores(codigoPago, codigoProveedor, fecha, monto, metodoPago, referencia, estado);
                MessageBox.Show("Pago editado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MtdMostrarPagos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPago.Text))
            {
                MessageBox.Show("Por favor, Seleccine el PAGO que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoPago = int.Parse(txtCodigoPago.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    CDpagos.MtdEliminarPagoProveedores(CodigoPago);
                    MessageBox.Show("Pago eliminado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarPagos();
                    LimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvPagosProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPagosProveedores.Rows[e.RowIndex];
                txtCodigoPago.Text = row.Cells["CodigoPagoP"].Value.ToString();
                cboxCodigoProveedor.SelectedValue = Convert.ToInt32(row.Cells["CodigoProveedor"].Value);
                dtpFechaPago.Value = Convert.ToDateTime(row.Cells["Fecha"].Value);
                txtMonto.Text = row.Cells["Monto"].Value.ToString();
                cboxMetodoPago.Text = row.Cells["MetodoPago"].Value.ToString();
                txtReferenciaPago.Text = row.Cells["ReferenciaPago"].Value.ToString();
                cboxEstado.Text = row.Cells["Estado"].Value.ToString();
            }
        }
    }
}

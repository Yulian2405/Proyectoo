using CapaDatos;
using CapaLogica;
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
    public partial class FrmCompras : Form
    {
        CL_Compras Compras = new CL_Compras();
        CDcompras CDCompras = new CDcompras();
        public FrmCompras()
        {
            InitializeComponent();
            txtCantidad.TextChanged += txtTotalCompra_TextChanged;
            txtCosto.TextChanged += txtTotalCompra_TextChanged;
        }

        private void FrmCompras_Load(object sender, EventArgs e)
        {
            // Productos
            cboxCodigoProducto.DataSource = CDCompras.MtdConsultarProducto();
            cboxCodigoProducto.DisplayMember = "Text";
            cboxCodigoProducto.ValueMember = "Value";

            // Proveedores
            cboxCodigoProveedor.DataSource = CDCompras.MtdConsultarProveedor();
            cboxCodigoProveedor.DisplayMember = "Text";
            cboxCodigoProveedor.ValueMember = "Value";

            // Empleados
            cboxCodigoEmpleado.DataSource = CDCompras.MtdConsultarEmpleado();
            cboxCodigoEmpleado.DisplayMember = "Text";
            cboxCodigoEmpleado.ValueMember = "Value";

            MtdMostrarComprasDGV();

        }

        private void LimpiarCampos()
        {
            txtCodigoCompra.Text = " ";
            cboxCodigoProducto.SelectedIndex = -1;
            cboxCodigoProveedor.SelectedIndex = -1;
            cboxCodigoEmpleado.SelectedIndex = -1;
            dtpFechaCompra.Value = DateTime.Now;
            txtCantidad.Clear();
            txtCosto.Clear();
            txtTotalCompra.Clear();
            cboxEstado.Text = " ";
            cboxEstado.SelectedIndex = -1;
        }
        private void MtdMostrarComprasDGV()
        {
            dgvCompras.DataSource = CDCompras.MtdConsultarCompras();
        }

        private void txtTotalCompra_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(txtCantidad.Text, out double cantidad) &&
                double.TryParse(txtCosto.Text, out double costo))
            {
                CL_Compras compras = new CL_Compras();
                double total = compras.MtdCostoCompra(cantidad, costo);
                txtTotalCompra.Text = total.ToString("N2");
            }
            else
            {
                txtTotalCompra.Text = "0.00";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validar campos vacíos
            if (
               string.IsNullOrEmpty(cboxCodigoProducto.Text) ||
               string.IsNullOrEmpty(cboxCodigoProveedor.Text) ||
               string.IsNullOrEmpty(cboxCodigoEmpleado.Text) ||
               string.IsNullOrEmpty(txtCantidad.Text) ||
               string.IsNullOrEmpty(txtCosto.Text) ||
               string.IsNullOrEmpty(txtTotalCompra.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
               )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.",
                                "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Capturar datos del formulario
                int CodigoProducto = Convert.ToInt32(cboxCodigoProducto.SelectedValue);
                int CodigoProveedor = Convert.ToInt32(cboxCodigoProveedor.SelectedValue);
                int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                DateTime FechaCompra = dtpFechaCompra.Value;
                int Cantidad = Convert.ToInt32(txtCantidad.Text);
                decimal Costo = Convert.ToDecimal(txtCosto.Text);
                decimal TotalCompra = Convert.ToDecimal(txtTotalCompra.Text);
                string Estado = cboxEstado.Text;

                // Llamar al método de la capa de datos (instancia, no clase)
                CDcompras cd = new CDcompras();
                cd.MtdAgregarCompras(CodigoProveedor, CodigoProducto, CodigoEmpleado, FechaCompra, Cantidad, Costo, TotalCompra, Estado);

                MessageBox.Show("Compra agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                MtdMostrarComprasDGV();  // <-- Cambia esto al nombre real de tu método para refrescar el dgv
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            txtCantidad.Clear();
        }

        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            txtCosto.Clear();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvCompras_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoCompra.Text))
            {
                MessageBox.Show("Seleccione una compra para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Capturar datos
                int CodigoCompra = Convert.ToInt32(txtCodigoCompra.Text);
                int CodigoProducto = Convert.ToInt32(cboxCodigoProducto.SelectedValue);
                int CodigoProveedor = Convert.ToInt32(cboxCodigoProveedor.SelectedValue);
                int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.SelectedValue);
                DateTime FechaCompra = dtpFechaCompra.Value;
                int Cantidad = Convert.ToInt32(txtCantidad.Text);
                decimal Costo = Convert.ToDecimal(txtCosto.Text);
                decimal TotalCompra = Convert.ToDecimal(txtTotalCompra.Text);
                string Estado = cboxEstado.Text;

                CDCompras.MtdEditarCompras(CodigoCompra, CodigoProveedor, CodigoProducto, CodigoEmpleado, FechaCompra, Cantidad, Costo, TotalCompra, Estado);

                MessageBox.Show("Compra actualizada correctamente.", "Éxitoooo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                MtdMostrarComprasDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar la compra: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoCompra.Text))
            {
                MessageBox.Show("Por favor, Seleccine la compra que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoCompra = int.Parse(txtCodigoCompra.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    CDCompras.MtdEliminarCompras(CodigoCompra);
                    MessageBox.Show("Compra eliminada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarComprasDGV();
                    LimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }

        private void cboxCodigoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxCodigoProducto.SelectedValue != null && int.TryParse(cboxCodigoProducto.SelectedValue.ToString(), out int codigoProducto))
            {
                CDcompras cd = new CDcompras();
                decimal precio = cd.MtdObtenerPrecioProducto(codigoProducto);

                txtCosto.Text = precio.ToString("N2");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCompras.Rows[e.RowIndex];

                txtCodigoCompra.Text = row.Cells["CodigoCompra"].Value.ToString();
                cboxCodigoProducto.SelectedValue = Convert.ToInt32(row.Cells["CodigoProducto"].Value);
                cboxCodigoProveedor.SelectedValue = Convert.ToInt32(row.Cells["CodigoProveedor"].Value);
                cboxCodigoEmpleado.SelectedValue = Convert.ToInt32(row.Cells["CodigoEmpleado"].Value);
                dtpFechaCompra.Value = Convert.ToDateTime(row.Cells["FechaCompra"].Value);
                txtCantidad.Text = row.Cells["Cantidad"].Value.ToString();
                txtCosto.Text = row.Cells["Costo"].Value.ToString();
                txtTotalCompra.Text = row.Cells["TotalCompra"].Value.ToString();
                cboxEstado.Text = row.Cells["Estado"].Value.ToString();
            }
        }
    }
}

using CapaDatos;
using CapaLogica;
using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text.pdf.codec.wmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmInventarios : KryptonForm
    {
        CD_Inventarios cd_inventarios = new CD_Inventarios();
        public FrmInventarios()
        {
            InitializeComponent();
        }

        // Metodo que limpia datos de los campos del formulario planillas
        private void MtdLimpiarCampos()
        {
            txtCodigoInventario.Clear();
            cboxCodigoSucursal.Text = "";
            dtpFechaActualizacion.Value = DateTime.Now;
            cboxCodigoProducto.Text = " ";
            txtCantidad.Clear();
            txtStock.Clear();
            cboxEstado.Text = "";
        }

        private void kryptonLabel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kryptonLabel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboxEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpFechaVenta_ValueChanged(object sender, EventArgs e)
        {

        }

        private void kryptonLabel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboxMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void kryptonHeaderGroup2_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FrmInventarios_Load(object sender, EventArgs e)
        {
            MtdMostrarListaSucursales();
            MtdMostrarInventariosDGV();
            MtdMostrarListaProductos();

        }

        // Metodo que imprime la lista a mostrar el ComboBox
        private void MtdMostrarListaSucursales()
        {
            var ListaSucursales = cd_inventarios.MtdListaSucursales();
            cboxCodigoSucursal.Items.Clear();

            foreach (var Sucursales in ListaSucursales)
            {
                cboxCodigoSucursal.Items.Add(Sucursales);
            }

            cboxCodigoSucursal.DisplayMember = "Text";
            cboxCodigoSucursal.ValueMember = "Value";
        }

        // Metodo que imprime la lista a mostrar el ComboBox
        private void MtdMostrarListaProductos()
        {
            var ListaProductos = cd_inventarios.MtdListaProductos();
            cboxCodigoProducto.Items.Clear();

            foreach (var Productos in ListaProductos)
            {
                cboxCodigoProducto.Items.Add(Productos);
            }

            cboxCodigoProducto.DisplayMember = "Text";
            cboxCodigoProducto.ValueMember = "Value";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(cboxCodigoSucursal.Text) || string.IsNullOrEmpty(cboxCodigoProducto.Text) || string.IsNullOrEmpty(txtCantidad.Text) || string.IsNullOrEmpty(txtStock.Text) ||  string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Empleado
                    var SelectedSucursal = (dynamic)cboxCodigoSucursal.SelectedItem;
                    int CodigoSucursal = (int)SelectedSucursal.GetType().GetProperty("Value").GetValue(SelectedSucursal, null);

                    var SelectedProducto = (dynamic)cboxCodigoProducto.SelectedItem;
                    int CodigoProducto = (int)SelectedProducto.GetType().GetProperty("Value").GetValue(SelectedProducto, null);

                    int Cantidad = int.Parse(txtCantidad.Text);
                    int StockMinimo = int.Parse(txtStock.Text);
                    DateTime FechaActualizacion = dtpFechaActualizacion.Value;
                    string Estado = cboxEstado.Text;



                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_inventarios.MtdAgregarInventario(CodigoSucursal, CodigoProducto, Cantidad, StockMinimo, FechaActualizacion, Estado);
                    MessageBox.Show("Inventario agregado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarInventariosDGV();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void dgvInventarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvInventarios.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvInventarios.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtCodigoInventario.Text = dgvInventarios.SelectedCells[0].Value.ToString();

                // Codigo Empleado Foreing key
                int CodigoSucursalSeleccionado = Convert.ToInt32(dgvInventarios.CurrentRow.Cells[1].Value);
                dynamic Sucursal = cd_inventarios.MtdConsultaSucursalDgv(CodigoSucursalSeleccionado)[0];
                cboxCodigoSucursal.Text = Sucursal.GetType().GetProperty("Text").GetValue(Sucursal, null).ToString();

                int CodigoProductoSeleccionado = Convert.ToInt32(dgvInventarios.CurrentRow.Cells[2].Value);
                dynamic Producto = cd_inventarios.MtdConsultaProductoDgv(CodigoProductoSeleccionado)[0];
                cboxCodigoProducto.Text = Producto.GetType().GetProperty("Text").GetValue(Producto, null).ToString();

                dtpFechaActualizacion.Value = Convert.ToDateTime(dgvInventarios.CurrentRow.Cells[4].Value);
                txtCantidad.Text = Convert.ToDouble(dgvInventarios.CurrentRow.Cells[3].Value).ToString();
                txtStock.Text = Convert.ToDouble(dgvInventarios.CurrentRow.Cells[5].Value).ToString();
                cboxEstado.Text = dgvInventarios.CurrentRow.Cells[6].Value.ToString();
            }
        }

        private void cboxCodigoSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codigo Empleado
            var SucursalSeleccionado = (dynamic)cboxCodigoSucursal.SelectedItem;
            int CodigoSucursal = (int)SucursalSeleccionado.GetType().GetProperty("Value").GetValue(SucursalSeleccionado, null);
        }

        // Metodo que muestra las planillas en el DataGridView
        public void MtdMostrarInventariosDGV()
        {
            DataTable dtInventario = new DataTable();
            dgvInventarios.DataSource = cd_inventarios.MtdConsultarInventarios();
        }

        private void cboxCodigoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Codigo Empleado
            var ProductoSeleccionado = (dynamic)cboxCodigoProducto.SelectedItem;
            int CodigoProducto = (int)ProductoSeleccionado.GetType().GetProperty("Value").GetValue(ProductoSeleccionado, null);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoInventario.Text) || string.IsNullOrEmpty(cboxCodigoSucursal.Text) || string.IsNullOrEmpty(cboxCodigoProducto.Text) || string.IsNullOrEmpty(txtCantidad.Text) || string.IsNullOrEmpty(txtStock.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    int CodigoInventario = int.Parse(txtCodigoInventario.Text);
                    //Codigo Empleado
                    var SelectedSucursal = (dynamic)cboxCodigoSucursal.SelectedItem;
                    int CodigoSucursal = (int)SelectedSucursal.GetType().GetProperty("Value").GetValue(SelectedSucursal, null);

                    var SelectedProducto = (dynamic)cboxCodigoProducto.SelectedItem;
                    int CodigoProducto = (int)SelectedProducto.GetType().GetProperty("Value").GetValue(SelectedProducto, null);

                    int Cantidad = int.Parse(txtCantidad.Text);
                    int StockMinimo = int.Parse(txtStock.Text);
                    DateTime FechaActualizacion = dtpFechaActualizacion.Value;
                    string Estado = cboxEstado.Text;



                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_inventarios.MtdEditarInventario(CodigoInventario, CodigoSucursal, CodigoProducto, Cantidad, StockMinimo, FechaActualizacion, Estado);
                    MessageBox.Show("Inventario actualizado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarInventariosDGV();
                    MtdLimpiarCampos();

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
            MtdLimpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoInventario.Text))
            {
                MessageBox.Show("Por favor, Seleccine el inventario que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoInventario = int.Parse(txtCodigoInventario.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_inventarios.MtdEliminarInventario(CodigoInventario);
                    MessageBox.Show("Inventario eliminado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarInventariosDGV();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            }
        }
    }
}

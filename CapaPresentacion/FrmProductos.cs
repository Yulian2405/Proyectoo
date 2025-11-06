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
using CapaLogica;
using ComponentFactory.Krypton.Toolkit;
namespace CapaPresentacion
{
    public partial class Frmproductos : KryptonForm
    {
        CLproducto cl_productos=new CLproducto();
       CDproductos cd_producto =new CDproductos();
        public Frmproductos()
        {
            InitializeComponent();
        }
        private void MtdListaDetalleVenta()
        {
            var Lista = cd_producto.MtdListaDetalleVenta();

            foreach (var Codigodetalleventa in Lista)
            {
                cboxCodigoDetalleVenta.Items.Add(Codigodetalleventa);
            }

            cboxCodigoDetalleVenta.DisplayMember = "Text";
            cboxCodigoDetalleVenta.ValueMember = "Value";
        }
        private void MtdConsultarProductos()
        {
            DataTable Dt = cd_producto.MtdConsultarProductos();
            dgvProductos.DataSource = Dt;
        }
        private void Frmproductos_Load(object sender, EventArgs e)
        {
            MtdConsultarProductos();
            MtdListaDetalleVenta();
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoDetalleVenta.Text) || string.IsNullOrEmpty(txtCodigoBarra.Text) || string.IsNullOrEmpty(txtNombreProducto.Text) || string.IsNullOrEmpty(txtPrecio.Text) ||
             string.IsNullOrEmpty(cboxCategoria.Text) || string.IsNullOrEmpty(txtStock.Text) || string.IsNullOrEmpty(dtpfecha.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Favor ingresar todos los datos en pantalla", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    var SelectedDetalleVenta = (dynamic)cboxCodigoDetalleVenta.SelectedItem;
                    int CodigoDetalle = (int)SelectedDetalleVenta.GetType().GetProperty("Value").GetValue(SelectedDetalleVenta, null);

                    string CodigoBarra = txtCodigoBarra.Text;
                    string Nombre = txtNombreProducto.Text;
                    double Precio = double.Parse(txtPrecio.Text);
                    string Categoria = cboxCategoria.Text;
                    int Stock = int.Parse(txtStock.Text);
                    DateTime FechaVencimiento = dtpfecha.Value;
                    string Estado = cboxEstado.Text;

                    cd_producto.MtdAgregarProducto(CodigoDetalle, CodigoBarra, Nombre, Precio, Stock, Categoria, FechaVencimiento, Estado);
                    MessageBox.Show("Producto agregado correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarProductos();
                    MtdLimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoDetalleVenta.Text) || string.IsNullOrEmpty(txtCodigoBarra.Text) || string.IsNullOrEmpty(txtNombreProducto.Text) || string.IsNullOrEmpty(txtPrecio.Text) ||
              string.IsNullOrEmpty(cboxCategoria.Text) || string.IsNullOrEmpty(txtStock.Text) || string.IsNullOrEmpty(dtpfecha.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Favor ingresar todos los datos en pantalla", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    int CodigoProducto = int.Parse(txtCodigoProducto.Text);
                    var SelectedDetalleVenta = (dynamic)cboxCodigoDetalleVenta.SelectedItem;
                    int CodigoDetalle = (int)SelectedDetalleVenta.GetType().GetProperty("Value").GetValue(SelectedDetalleVenta, null);

                    string CodigoBarra = txtCodigoBarra.Text;
                    string Nombre = txtNombreProducto.Text;
                    double Precio = double.Parse(txtPrecio.Text);
                    string Categoria = cboxCategoria.Text;
                    int Stock = int.Parse(txtStock.Text);
                    DateTime FechaVencimiento = dtpfecha.Value;
                    string Estado = cboxEstado.Text;


                    cd_producto.MtdActualizarProducto(CodigoProducto, CodigoDetalle, CodigoBarra, Nombre, Precio, Stock, Categoria, FechaVencimiento, Estado);
                    MessageBox.Show("Producto editado correctamente", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarProductos();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoProducto.Text))
            {
                MessageBox.Show("Favor seleccione para eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    int Codigoproducto = (int.Parse(txtCodigoProducto.Text));

                    cd_producto.MtdEliminar(Codigoproducto);
                    MessageBox.Show("Producto eliminado", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultarProductos();
                    MtdLimpiarCampos();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            MtdLimpiarCampos();
        }
        private void MtdLimpiarCampos()
        {
            txtCodigoProducto.Text = "";
            cboxCodigoDetalleVenta.SelectedIndex = -1;
            txtCodigoBarra.Text = "";
            txtNombreProducto.Text = "";
            txtPrecio.Text = "";
            cboxCategoria.Text = "";
            txtStock.Text = "";
            dtpfecha.Text = "";
            cboxEstado.Text = "";

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dvgproductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvProductos.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvProductos.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtCodigoProducto.Text = dgvProductos.SelectedCells[0].Value.ToString();

                cboxCodigoDetalleVenta.Text = dgvProductos.CurrentRow.Cells[1].Value.ToString();
                txtCodigoBarra.Text = dgvProductos.CurrentRow.Cells[2].Value.ToString();
                txtNombreProducto.Text = dgvProductos.CurrentRow.Cells[3].Value.ToString();
                txtPrecio.Text = Convert.ToDouble(dgvProductos.CurrentRow.Cells[4].Value).ToString("c");
                txtStock.Text = Convert.ToInt32(dgvProductos.CurrentRow.Cells[5].Value).ToString("c");
                cboxCategoria.Text = dgvProductos.CurrentRow.Cells[6].Value.ToString();
                dtpfecha.Value = Convert.ToDateTime(dgvProductos.CurrentRow.Cells[7].Value);
                cboxEstado.Text = dgvProductos.CurrentRow.Cells[8].Value.ToString();

            }

        }

        private void buttonSpecAny9_Click(object sender, EventArgs e)
        {

        }
    }
}

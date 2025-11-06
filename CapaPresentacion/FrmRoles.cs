using CapaDatos;
using ComponentFactory.Krypton.Toolkit;
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
    public partial class FrmRoles : KryptonForm
    {
        CD_Roles cd_Roles = new CD_Roles();
        public FrmRoles()
        {
            InitializeComponent();
        }

        // Metod que muestra los datos de los empleados en el DataGridView

        private void MtdConsultar()
        {
            DataTable DtForm = cd_Roles.MtdConsultarRoles();
            dgvRoles.DataSource = DtForm;
        }

        private void FrmRoles_Load(object sender, EventArgs e)
        {
            MtdConsultar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (
               string.IsNullOrEmpty(txtNombre.Text) ||
               string.IsNullOrEmpty(cboxTipoPermiso.Text) ||
               string.IsNullOrEmpty(cboxPantalla.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
               )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                string NombreRol = txtNombre.Text;
                string TipoPermiso = cboxTipoPermiso.Text;
                string Pantalla = cboxPantalla.Text;
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_Roles.MtdAgregarRoles(NombreRol, TipoPermiso, Pantalla, Estado);
                    MessageBox.Show("Rol agregado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultar();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void LimpiarCampos()
        {
            txtCodigoRol.Text = " ";
            txtNombre.Text = " ";
            cboxTipoPermiso.Text = " ";
            cboxPantalla.Text = " ";
            cboxEstado.Text = " ";
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (
              string.IsNullOrEmpty(txtCodigoRol.Text) ||
              string.IsNullOrEmpty(txtNombre.Text) ||
              string.IsNullOrEmpty(cboxTipoPermiso.Text) ||
              string.IsNullOrEmpty(cboxPantalla.Text) ||
              string.IsNullOrEmpty(cboxEstado.Text)
              )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int CodigoRol = int.Parse(txtCodigoRol.Text);
                string NombreRol = txtNombre.Text;
                string TipoPermiso = cboxTipoPermiso.Text;
                string Pantalla = cboxPantalla.Text;
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_Roles.MtdActualizarRoles(CodigoRol, NombreRol, TipoPermiso, Pantalla, Estado);
                    MessageBox.Show("Rol actualizado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultar();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (
                 string.IsNullOrEmpty(txtCodigoRol.Text)
                 )
            {
                MessageBox.Show("Por favor, seleccione un Rol a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int Codigo = Convert.ToInt32(txtCodigoRol.Text);

                // Enviar datos a la capa de datos
                try
                {
                    cd_Roles.MtdEliminarRoles(Codigo);
                    MessageBox.Show("Rol eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdConsultar();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvRoles.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvRoles.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoRol.Text = FilaSeleccionada.Cells[0].Value.ToString();
                txtNombre.Text = FilaSeleccionada.Cells[1].Value.ToString();
                cboxTipoPermiso.Text = FilaSeleccionada.Cells[2].Value.ToString();
                cboxPantalla.Text = FilaSeleccionada.Cells[3].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells[4].Value.ToString();
            }
        }
    }


}

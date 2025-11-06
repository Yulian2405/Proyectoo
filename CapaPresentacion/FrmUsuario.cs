using CapaDatos;
using CapaLogica;
using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmUsuario : KryptonForm
    {
        CD_Usuarios cd_usuarios = new CD_Usuarios();
        CL_Usuarios cl_Usuarios = new CL_Usuarios();

        public void MtdMuestraDatosDGV()
        {
            DataTable dtUsuarios = cd_usuarios.MtdConsultarUsuarios();
            dgvUsuarios.DataSource = dtUsuarios;
        }


        public FrmUsuario()
        {
            InitializeComponent();
        }


        private void MtdMostrarListaEmpleados()
        {
            var ListaEmpleado = cd_usuarios.MtdListaEmpleados();
            cboxCodigoEmpleado.Items.Clear();

            foreach (var Empleados in ListaEmpleado)
            {
                cboxCodigoEmpleado.Items.Add(Empleados);
            }

            cboxCodigoEmpleado.DisplayMember = "Text";
            cboxCodigoEmpleado.ValueMember = "Value";
        }

        private void MtdMostrarListaRol()
        {
            var ListaRol = cd_usuarios.MtdListaRol();
            cboxCodigoRol.Items.Clear();

            foreach (var Rol in ListaRol)
            {
                cboxCodigoRol.Items.Add(Rol);
            }

            cboxCodigoRol.DisplayMember = "Text";
            cboxCodigoRol.ValueMember = "Value";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(cboxCodigoRol.Text) ||
                string.IsNullOrEmpty(cboxCodigoEmpleado.Text) ||
                string.IsNullOrEmpty(txtNombreUsuario.Text) ||
                string.IsNullOrEmpty(txtClave.Text) ||
                string.IsNullOrEmpty(cboxEstado.Text)
                )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                var SelectedRol = (dynamic)cboxCodigoEmpleado.SelectedItem;
                int CodigoRol = (int)SelectedRol.GetType().GetProperty("Value").GetValue(SelectedRol, null);

                var SelectedEmpleado = (dynamic)cboxCodigoEmpleado.SelectedItem;
                int CodigoEmpleado = (int)SelectedEmpleado.GetType().GetProperty("Value").GetValue(SelectedEmpleado, null);

                string NombreUsuario = txtNombreUsuario.Text;
                double Clave = Convert.ToInt32(txtClave.Text);
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_usuarios.MtdAgregarUsuarios(CodigoRol, CodigoEmpleado, NombreUsuario, Clave, Estado);
                    MessageBox.Show("Usuario agregado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            MtdMuestraDatosDGV();
            MtdMostrarListaEmpleados();
            MtdMostrarListaRol();

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(txtCodigoUsuario.Text) ||
                string.IsNullOrEmpty(cboxCodigoRol.Text) ||
                string.IsNullOrEmpty(cboxCodigoEmpleado.Text) ||
                string.IsNullOrEmpty(txtNombreUsuario.Text) ||
                string.IsNullOrEmpty(txtClave.Text) ||
                string.IsNullOrEmpty(cboxEstado.Text)
                )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text);
                int CodigoRol = Convert.ToInt32(cboxCodigoRol.Text);
                int CodigoEmpleado = Convert.ToInt32(cboxCodigoEmpleado.Text);
                string NombreUsuario = txtNombreUsuario.Text;
                double Clave = Convert.ToInt32(txtClave.Text);
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_usuarios.MtdEditarUsuarios(CodigoUsuario, CodigoRol, CodigoEmpleado, NombreUsuario, Clave, Estado);
                    MessageBox.Show("Usuario editado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
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
                 string.IsNullOrEmpty(txtCodigoUsuario.Text)
                 )
            {
                MessageBox.Show("Por favor, seleccione un Usuario a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int Codigo = Convert.ToInt32(txtCodigoUsuario.Text);

                // Enviar datos a la capa de datos
                try
                {
                    cd_usuarios.MtdEliminarUsuarios(Codigo);
                    MessageBox.Show("Usuario eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
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
            txtCodigoUsuario.Clear();
            cboxCodigoRol.SelectedIndex = -1;
            cboxCodigoEmpleado.SelectedIndex = -1;
            txtNombreUsuario.Clear();
            txtClave.Clear();
            cboxEstado.SelectedIndex = -1;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles de la Planilla", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }



        private void cboxCodigoRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var RolSeleccionado = (dynamic)cboxCodigoRol.SelectedItem;
            int codigoRol = (int)RolSeleccionado.GetType().GetProperty("Value").GetValue(RolSeleccionado, null);
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvUsuarios.SelectedRows[0];

                if (selectedRow.Cells["CodigoUsuario"].Value == null ||
                    selectedRow.Cells["CodigoRol"].Value == null ||
                    selectedRow.Cells["CodigoEmpleado"].Value == null ||
                    selectedRow.Cells["NombreUsuario"].Value == null ||
                    selectedRow.Cells["Clave"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var CodigoUsuario = selectedRow.Cells["CodigoUsuario"].Value.ToString();
                var CodigoRol = selectedRow.Cells["CodigoRol"].Value.ToString();
                var CodigoEmpleado = selectedRow.Cells["CodigoEmpleado"].Value.ToString();
                var NombreUsuario = selectedRow.Cells["NombreUsuario"].Value.ToString();
                var Clave = selectedRow.Cells["Clave"].Value.ToString();
                var estado = selectedRow.Cells["Estado"].Value.ToString();

                // Generar PDF con tamaño de PDF
                string pdfPath = @"C:\Users\julio\Downloads\DetallePlanilla.pdf"; // Ajusta la ruta
                Document doc = new Document(new iTextSharp.text.Rectangle(280, 300), 10, 10, 10, 10); // 280 mm de ancho, 500 mm de alto, márgenes
                PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
                doc.Open();

                // Agregar logo y contenido al PDF
                string logoPath = @"C:\Users\julio\Desktop\LOGOUMG.png"; // Ajusta la ruta
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 60f); // Ajusta el tamaño del logo para que quepa en el ticket
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);

                // Agregar encabezado y detalles
                doc.Add(new Paragraph("Detalles de la Planilla", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Codigo Usuario: {CodigoUsuario}"));
                doc.Add(new Paragraph($"CodigoRol: {CodigoRol}"));
                doc.Add(new Paragraph($"CodigoEmpleado: {CodigoEmpleado}"));
                doc.Add(new Paragraph($"NombreUsuario: {NombreUsuario}"));
                doc.Add(new Paragraph($"Clave: {Clave}"));
                doc.Add(new Paragraph($"Estado: {estado}"));
                doc.Close();

                // Previsualizar el PDF utilizando el lector de PDF predeterminado
                System.Diagnostics.Process.Start(pdfPath);

                // Configurar impresión
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                ConfigurarTamanioPapel(pd);

                // Vista previa de impresión
                PrintPreviewDialog vistaPrevia = new PrintPreviewDialog();
                vistaPrevia.Document = pd;
                vistaPrevia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsuarios.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos en el DataGridView para exportar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Crear una nueva instancia de Excel
                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false; // No mostrar Excel aun

                // Crear un nuevo libro de Excel
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                // Agregar encabezados de columna
                for (int i = 0; i < dgvUsuarios.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvUsuarios.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvUsuarios.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvUsuarios.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvUsuarios.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                    }
                }

                // Guardar el archivo temporalmente
                string excelFilePath = @"C:\julio\Downloads\DatosPlanilla.xlsx"; // Ajusta la ruta
                workbook.SaveAs(excelFilePath);
                workbook.Close();
                excelApp.Quit();

                // Liberar los recursos de COM
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                // Mostrar mensaje de éxito
                MessageBox.Show("Datos exportados a Excel con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Vista previa de impresión
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                ConfigurarTamanioPapel(pd);

                // Vista previa de impresión
                PrintPreviewDialog vistaPrevia = new PrintPreviewDialog();
                vistaPrevia.Document = pd;
                vistaPrevia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboxCodigoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {
            var EmpleadoSeleccionado = (dynamic)cboxCodigoEmpleado.SelectedItem;
            int codigoEmpleado = (int)EmpleadoSeleccionado.GetType().GetProperty("Value").GetValue(EmpleadoSeleccionado, null);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvUsuarios.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvUsuarios.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoUsuario.Text = FilaSeleccionada.Cells[0].Value.ToString();
                cboxCodigoRol.Text = FilaSeleccionada.Cells[1].Value.ToString();
                cboxCodigoEmpleado.Text = FilaSeleccionada.Cells[2].Value.ToString();
                txtNombreUsuario.Text = FilaSeleccionada.Cells[3].Value.ToString();
                txtClave.Text = FilaSeleccionada.Cells[4].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells[5].Value.ToString();
            }
        }
    }
}

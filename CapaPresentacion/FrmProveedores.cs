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
    public partial class FrmProveedores : KryptonForm
    {

        CD_Proveedores cd_proveedores= new CD_Proveedores();
        public void MtdMuestraDatosDGV()
        {
            DataTable dtProveedores = cd_proveedores.MtdConsultarProveedores();
            dgvProveedores.DataSource = dtProveedores;
        }
        public FrmProveedores()
        {
            InitializeComponent();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles de la Planilla", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (
              string.IsNullOrEmpty(txtNombre.Text) ||
              string.IsNullOrEmpty(txtTelefono.Text) ||
              string.IsNullOrEmpty(txtCorreo.Text) ||
              string.IsNullOrEmpty(txtDireccion.Text) ||
              string.IsNullOrEmpty(cboxEstado.Text)
              )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                string Nombre = txtNombre.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                string Direccion = txtDireccion.Text;
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_proveedores.MtdAgregarProveedores(Nombre, Telefono, Correo, Direccion, Estado);
                    MessageBox.Show("Proveedor agregado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void dgvProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            MtdMuestraDatosDGV();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (
               string.IsNullOrEmpty(txtNombre.Text) ||
               string.IsNullOrEmpty(txtTelefono.Text) ||
               string.IsNullOrEmpty(txtCorreo.Text) ||
               string.IsNullOrEmpty(txtDireccion.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
                )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int CodigoProveedor = Convert.ToInt32(txtCodigoProveedor.Text);
                string NombreProveedor = txtNombre.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                string Direccion = txtDireccion.Text;
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_proveedores.MtdEditarProveedores(CodigoProveedor, NombreProveedor, Telefono, Correo, Direccion, Estado);
                    MessageBox.Show("Proveedor editado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
                    LimpiarCampos();
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
            if (
                string.IsNullOrEmpty(txtCodigoProveedor.Text)
                )
            {
                MessageBox.Show("Por favor, seleccione un Proveedor a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int Codigo = Convert.ToInt32(txtCodigoProveedor.Text);

                // Enviar datos a la capa de datos
                try
                {
                    cd_proveedores.MtdEliminarProveedores(Codigo);
                    MessageBox.Show("Proveedor eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void LimpiarCampos()
        {
            txtCodigoProveedor.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            cboxEstado.SelectedIndex = -1;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProveedores.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvProveedores.SelectedRows[0];

                if (selectedRow.Cells["CodigoProveedor"].Value == null ||
                    selectedRow.Cells["Nombre"].Value == null ||
                    selectedRow.Cells["Telefono"].Value == null ||
                    selectedRow.Cells["Correo"].Value == null ||
                    selectedRow.Cells["Direccion"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var CodigoProveedor = selectedRow.Cells["CodigoProveedor"].Value.ToString();
                var Nombre = selectedRow.Cells["Nombre"].Value.ToString();
                var Telefono = selectedRow.Cells["Telefono"].Value.ToString();
                var Correo = selectedRow.Cells["Correo"].Value.ToString();
                var Direccion = selectedRow.Cells["Direccion"].Value.ToString();
                var estado = selectedRow.Cells["Estado"].Value.ToString();

                // Generar PDF con tamaño de PDF
                string pdfPath = @"C:\Users\darie\Downloads\DetallePlanilla.pdf"; // Ajusta la ruta
                Document doc = new Document(new iTextSharp.text.Rectangle(280, 300), 10, 10, 10, 10); // 280 mm de ancho, 500 mm de alto, márgenes
                PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
                doc.Open();

                // Agregar logo y contenido al PDF
                string logoPath = @"C:\Users\darie\Desktop\LOGOUMG.png"; // Ajusta la ruta
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 60f); // Ajusta el tamaño del logo para que quepa en el ticket
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);

                // Agregar encabezado y detalles
                doc.Add(new Paragraph("Detalles del Proveedor", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"CodigoProveedor: {CodigoProveedor}"));
                doc.Add(new Paragraph($"Nombre: {Nombre}"));
                doc.Add(new Paragraph($"Telefono: {Telefono}"));
                doc.Add(new Paragraph($"Correo: {Correo}"));
                doc.Add(new Paragraph($"Direccion: {Direccion}"));
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
                if (dgvProveedores.Rows.Count == 0)
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
                for (int i = 0; i < dgvProveedores.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvProveedores.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvProveedores.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvProveedores.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvProveedores.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                    }
                }

                // Guardar el archivo temporalmente
                string excelFilePath = @"C:\Users\darie\Downloads\DatosPlanilla.xlsx"; // Ajusta la ruta
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

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvProveedores.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvProveedores.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoProveedor.Text = FilaSeleccionada.Cells[0].Value.ToString();
                txtNombre.Text = FilaSeleccionada.Cells[1].Value.ToString();
                txtTelefono.Text = FilaSeleccionada.Cells[2].Value.ToString();
                txtCorreo.Text = FilaSeleccionada.Cells[3].Value.ToString();
                txtDireccion.Text = FilaSeleccionada.Cells[4].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells[5].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}

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
    public partial class FrmSucursales : KryptonForm
    {
        CD_Sucursales cd_sucursales = new CD_Sucursales();
        


        public void MtdMuestraDatosDGV()
        {
            DataTable dtSucursales = cd_sucursales.MtdConsultarSucursales();
            dgvSucursales.DataSource = dtSucursales;
        }
        public FrmSucursales()
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
               string.IsNullOrEmpty(txtNombreSucursal.Text) ||
               string.IsNullOrEmpty(txtDireccion.Text) ||
               string.IsNullOrEmpty(txtTelefono.Text) ||
               string.IsNullOrEmpty(txtCorreo.Text) ||
               string.IsNullOrEmpty(cboxCodigoPostal.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
               )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                string NombreSucursal = txtNombreSucursal.Text;
                string Direccion = txtDireccion.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                int CodigoPostal = Convert.ToInt32(cboxCodigoPostal.Text);
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_sucursales.MtdAgregarSucursales(NombreSucursal, Direccion, Telefono, Correo, CodigoPostal, Estado);
                    MessageBox.Show("Sucursal agregado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosDGV();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvSucursales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void FrmSucursales_Load(object sender, EventArgs e)
        {
            MtdMuestraDatosDGV();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (
               string.IsNullOrEmpty(txtNombreSucursal.Text) ||
               string.IsNullOrEmpty(txtDireccion.Text) ||
               string.IsNullOrEmpty(txtTelefono.Text) ||
               string.IsNullOrEmpty(txtCorreo.Text) ||
               string.IsNullOrEmpty(cboxCodigoPostal.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
                )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int CodigoSucursal = Convert.ToInt32(txtCodigoSucursal.Text);
                string NombreSucursal = txtNombreSucursal.Text;
                string Direccion = txtDireccion.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                int CodigoPostal = Convert.ToInt32(cboxCodigoPostal.Text);
                string Estado = cboxEstado.Text;

                // Enviar datos a la capa de datos
                try
                {
                    cd_sucursales.MtdEditarSucursales(CodigoSucursal, NombreSucursal, Direccion, Telefono, Correo, CodigoPostal, Estado);
                    MessageBox.Show("Sucursal editado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                 string.IsNullOrEmpty(txtCodigoSucursal.Text)
                 )
            {
                MessageBox.Show("Por favor, seleccione un sucursal a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //Capturar datos del formulario
                int Codigo = Convert.ToInt32(txtCodigoSucursal.Text);

                // Enviar datos a la capa de datos
                try
                {
                    cd_sucursales.MtdEliminarSucursales(Codigo);
                    MessageBox.Show("Sucursal eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtCodigoSucursal.Clear();
            txtNombreSucursal.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            cboxCodigoPostal.SelectedIndex = -1;
            cboxEstado.SelectedIndex = -1;
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvSucursales.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvSucursales.SelectedRows[0];

                if (selectedRow.Cells["CodigoSucursal"].Value == null ||
                    selectedRow.Cells["Nombre"].Value == null ||
                    selectedRow.Cells["Direccion"].Value == null ||
                    selectedRow.Cells["Telefono"].Value == null ||
                    selectedRow.Cells["Correo"].Value == null ||
                    selectedRow.Cells["CodigoPostal"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var CodigoSucursal = selectedRow.Cells["CodigoSucursal"].Value.ToString();
                var Nombre = selectedRow.Cells["Nombre"].Value.ToString();
                var Direccion = selectedRow.Cells["Direccion"].Value.ToString();
                var Telefono = selectedRow.Cells["Telefono"].Value.ToString();
                var Correo = selectedRow.Cells["Correo"].Value.ToString();
                var CodigoPostal = selectedRow.Cells["CodigoPostal"].Value.ToString();
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
                doc.Add(new Paragraph("Detalles de la Planilla", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"CodigoSucursal: {CodigoSucursal}"));
                doc.Add(new Paragraph($"Nombre: {Nombre}"));
                doc.Add(new Paragraph($"Direccion: {Direccion}"));
                doc.Add(new Paragraph($"Telefono: {Telefono}"));
                doc.Add(new Paragraph($"Correo: {Correo}"));
                doc.Add(new Paragraph($"CodigoPostal: {CodigoPostal}"));
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
                if (dgvSucursales.Rows.Count == 0)
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
                for (int i = 0; i < dgvSucursales.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvSucursales.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvSucursales.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvSucursales.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvSucursales.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvSucursales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvSucursales.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvSucursales.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoSucursal.Text = FilaSeleccionada.Cells[0].Value.ToString();
                txtNombreSucursal.Text = FilaSeleccionada.Cells[1].Value.ToString();
                txtDireccion.Text = FilaSeleccionada.Cells[2].Value.ToString();
                txtTelefono.Text = FilaSeleccionada.Cells[3].Value.ToString();
                txtCorreo.Text = FilaSeleccionada.Cells[4].Value.ToString();
                cboxCodigoPostal.Text = FilaSeleccionada.Cells[5].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells[6].Value.ToString();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}

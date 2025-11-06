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

namespace ProyectoUMGDarien
{
    public partial class FrmEmpleados : KryptonForm
    {
        CD_Empleados cd_empleados  = new CD_Empleados();
        CL_Empleados cl_empleados = new CL_Empleados();

        public FrmEmpleados()
        {
            InitializeComponent();
        }

        public void MtdMuestraDatosEmpleados()
        {
            DataTable dtEmpleados = cd_empleados.MtdConsultarEmpleados();
            dgvEmpleadoss.DataSource = dtEmpleados;
        }


        private void FrmEmpleados_Load(object sender, EventArgs e)
        {
            MtdMuestraDatosEmpleados();
        }

        private void kryptonHeaderGroup2_Panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (
               string.IsNullOrEmpty(txtNombre.Text) ||
               string.IsNullOrEmpty(txtTelefono.Text) ||
               string.IsNullOrEmpty(txtCorreo.Text) ||
               string.IsNullOrEmpty(dtpFechaingreso.Text) ||
               string.IsNullOrEmpty(txtSalarioBase.Text) ||
               string.IsNullOrEmpty(cboxCargo.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text)
               )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                
                string Nombre = txtNombre.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                DateTime FechaIngreso = dtpFechaingreso.Value;
                double SalarioBase = Convert.ToDouble(txtSalarioBase.Text);
                string Cargo = cboxCargo.Text;
                string Estado = cboxEstado.Text;

            
                try
                {
                    cd_empleados.MtdAgregarEmpleados(Nombre, Telefono, Correo, Cargo, SalarioBase, FechaIngreso, Estado);
                    MessageBox.Show("Empleado agregado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosEmpleados();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void kryptonLabel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboxCargo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSalarioBase.Text = cl_empleados.MtdSalarioBase(cboxCargo.Text).ToString();
        }

        private void dgvEmpleadoss_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvEmpleadoss.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvEmpleadoss.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoEmpleado.Text = FilaSeleccionada.Cells[0].Value.ToString();
                txtNombre.Text = FilaSeleccionada.Cells[1].Value.ToString();
                txtTelefono.Text = FilaSeleccionada.Cells[2].Value.ToString();
                txtCorreo.Text = FilaSeleccionada.Cells[3].Value.ToString();
                cboxCargo.Text = FilaSeleccionada.Cells[4].Value.ToString();
                txtSalarioBase.Text = FilaSeleccionada.Cells[5].Value.ToString();
                dtpFechaingreso.Value = Convert.ToDateTime(FilaSeleccionada.Cells[6].Value);
                cboxEstado.Text = FilaSeleccionada.Cells[7].Value.ToString();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(txtCodigoEmpleado.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtTelefono.Text) ||
                string.IsNullOrEmpty(txtCorreo.Text) ||
                string.IsNullOrEmpty(txtSalarioBase.Text) ||
                string.IsNullOrEmpty(cboxCargo.Text) ||
                string.IsNullOrEmpty(dtpFechaingreso.Text) ||
                string.IsNullOrEmpty(cboxEstado.Text)
                )
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int Codigo = Convert.ToInt32(txtCodigoEmpleado.Text);
                string Nombre = txtNombre.Text;
                int Telefono = Convert.ToInt32(txtTelefono.Text);
                string Correo = txtCorreo.Text;
                DateTime FechaIngreso = dtpFechaingreso.Value;
                double SalarioBase = Convert.ToDouble(txtSalarioBase.Text);
                string Cargo = cboxCargo.Text;
                string Estado = cboxEstado.Text;

             
                try
                {
                    cd_empleados.MtdEditarEmpleados(Codigo, Nombre, Telefono, Correo, Cargo, SalarioBase, FechaIngreso, Estado);
                    MessageBox.Show("Empleado actualizado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosEmpleados();
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
                string.IsNullOrEmpty(txtCodigoEmpleado.Text)
                )
            {
                MessageBox.Show("Por favor, seleccione un Empleado a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
           
                int Codigo = Convert.ToInt32(txtCodigoEmpleado.Text);

     
                try
                {
                    cd_empleados.MtdEliminarEmpleados(Codigo);
                    MessageBox.Show("Empleado eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMuestraDatosEmpleados();
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
            txtCodigoEmpleado.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            dtpFechaingreso.Value = DateTime.Now;
            txtSalarioBase.Clear();
            cboxCargo.SelectedIndex = -1;
            cboxEstado.SelectedIndex = -1;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSpecAny6_Click(object sender, EventArgs e)
        {
            txtNombre.Clear();
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            txtTelefono.Clear();
        }

        private void buttonSpecAny8_Click(object sender, EventArgs e)
        {
            txtCorreo.Clear();
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles de la Planilla", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleadoss.Rows.Count == 0)
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
                for (int i = 0; i < dgvEmpleadoss.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvEmpleadoss.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvEmpleadoss.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvEmpleadoss.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvEmpleadoss.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
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

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmpleadoss.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvEmpleadoss.SelectedRows[0];


                if (selectedRow.Cells["CodigoEmpleado"].Value == null ||
                    selectedRow.Cells["Nombre"].Value == null ||
                    selectedRow.Cells["Telefono"].Value == null ||
                    selectedRow.Cells["Correo"].Value == null ||
                    selectedRow.Cells["Cargo"].Value == null ||
                    selectedRow.Cells["FechaIngreso"].Value == null ||
                    selectedRow.Cells["SalarioBase"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                 
                var CodigoEmpleado = selectedRow.Cells["CodigoEmpleado"].Value.ToString();
                var Nombre = selectedRow.Cells["Nombre"].Value.ToString();
                var Telefono = selectedRow.Cells["Telefono"].Value.ToString();
                var Correo = selectedRow.Cells["Correo"].Value.ToString();
                var Cargo = selectedRow.Cells["Cargo"].Value.ToString();
                var FechaIngreso = selectedRow.Cells["FechaIngreso"].Value.ToString();
                var SalarioBase = selectedRow.Cells["SalarioBase"].Value.ToString();
                var Estado = selectedRow.Cells["Estado"].Value.ToString();

                // Generar PDF con tamaño de PDF
                string pdfPath = @"C:\Users\darie\Downloads\DetalleEmpleado.pdf"; // Ajusta la ruta
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
                doc.Add(new Paragraph("Detalles del Empleado", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Código Empleado: {CodigoEmpleado}"));
                doc.Add(new Paragraph($"Nombre: {Nombre}"));
                doc.Add(new Paragraph($"Telefono: {Telefono}"));
                doc.Add(new Paragraph($"Correo: {Correo}"));
                doc.Add(new Paragraph($"Cargo: {Cargo}"));
                doc.Add(new Paragraph($"FechaIngreso: {FechaIngreso}"));
                doc.Add(new Paragraph($"SalarioBase: {SalarioBase}"));
                doc.Add(new Paragraph($"Estado: {Estado}"));
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

        private void dtpFechaingreso_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}

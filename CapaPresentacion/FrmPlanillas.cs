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
    public partial class FrmPlanillas : KryptonForm
    {
        CD_Planillas cd_planillas = new CD_Planillas();
        CL_Planillas cl_planillas = new CL_Planillas();

        public FrmPlanillas()
        {
            InitializeComponent();
        }

        private void FrmPlanillas_Load(object sender, EventArgs e)
        {
            MtdMostrarListaEmpleados();
            MtdMostrarPlanillas();
        }

        private void MtdMostrarListaEmpleados()
        {
            var ListaEmpleado = cd_planillas.MtdListaEmpleados();
            cboxCodigoEmpleado.Items.Clear();

            foreach (var Empleados in ListaEmpleado)
            {
                cboxCodigoEmpleado.Items.Add(Empleados);
            }

            cboxCodigoEmpleado.DisplayMember = "Text";
            cboxCodigoEmpleado.ValueMember = "Value";
        }

        public void MtdMostrarPlanillas()
        {
            DataTable dtPlanillas = new DataTable();
            dgvPlanillas.DataSource = cd_planillas.MtdConsultarPlanillas();
        }

        private void cboxCodigoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
        {        
            var EmpleadoSeleccionado = (dynamic)cboxCodigoEmpleado.SelectedItem;
            int codigoEmpleado = (int)EmpleadoSeleccionado.GetType().GetProperty("Value").GetValue(EmpleadoSeleccionado, null);

            double SalarioBase = cd_planillas.MtdConsultaSalarioBase(codigoEmpleado);
            txtSalarioBase.Text = SalarioBase.ToString("c");

            double Bono = cl_planillas.MtdCalcularBono(SalarioBase);
            txtBonos.Text = Bono.ToString("c");

            double Igss = cl_planillas.MtdCalcularIggs(SalarioBase);
            double Isr = cl_planillas.MtdCalcularIsr(SalarioBase);
            double Descuentos = Igss + Isr;
            txtDescuentos.Text = Descuentos.ToString("c");

            double SalarioFinal = cl_planillas.MtdCalcularSalarioFinal(SalarioBase, Bono, Igss, Isr);
            txtPagoFinal.Text = SalarioFinal.ToString("c");
        }
        private void MtdLimpiarCampos()
        {
            txtCodigoPlanilla.Clear();
            cboxCodigoEmpleado.Text = "";
            dtpFFechaPago.Value = DateTime.Now;
            txtSalarioBase.Clear();
            txtBonos.Clear();
            txtDescuentos.Clear();
            txtPagoFinal.Clear();
            cboxEstado.Text = "";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoEmpleado.Text) || string.IsNullOrEmpty(dtpFFechaPago.Text) || string.IsNullOrEmpty(txtSalarioBase.Text) || string.IsNullOrEmpty(txtBonos.Text) || string.IsNullOrEmpty(txtDescuentos.Text) || string.IsNullOrEmpty(txtPagoFinal.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    var SelectedEmpleado = (dynamic)cboxCodigoEmpleado.SelectedItem;
                    int codigoEmpleado = (int)SelectedEmpleado.GetType().GetProperty("Value").GetValue(SelectedEmpleado, null);

                    double SalarioBase = cd_planillas.MtdConsultaSalarioBase(codigoEmpleado);

                    double Bono = cl_planillas.MtdCalcularBono(SalarioBase);

                    double Igss = cl_planillas.MtdCalcularIggs(SalarioBase);
                    double Isr = cl_planillas.MtdCalcularIsr(SalarioBase);
                    double Descuentos = Igss + Isr;

                    //Salario Final Empleado
                    double SalarioFinal = cl_planillas.MtdCalcularSalarioFinal(SalarioBase, Bono, Igss, Isr);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_planillas.MtdAgregarDatosPlanillas(codigoEmpleado, dtpFFechaPago.Value, SalarioBase, Bono, Descuentos, SalarioFinal, cboxEstado.Text);
                    MessageBox.Show("Planilla agregada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarPlanillas();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles de la Planilla", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPlanillas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvPlanillas.SelectedRows[0];

                if (selectedRow.Cells["CodigoPlanilla"].Value == null ||
                    selectedRow.Cells["CodigoEmpleado"].Value == null ||
                    selectedRow.Cells["FechaPago"].Value == null ||
                    selectedRow.Cells["SalarioBase"].Value == null ||
                    selectedRow.Cells["Bonos"].Value == null ||
                    selectedRow.Cells["Descuentos"].Value == null ||
                    selectedRow.Cells["PagoFinal"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var codigoPlanilla = selectedRow.Cells["CodigoPlanilla"].Value.ToString();
                var codigoEmpleado = selectedRow.Cells["CodigoEmpleado"].Value.ToString();
                var fechaPago = selectedRow.Cells["FechaPago"].Value.ToString();
                var salarioBase = selectedRow.Cells["SalarioBase"].Value.ToString();
                var bonos = selectedRow.Cells["Bonos"].Value.ToString();
                var descuentos = selectedRow.Cells["Descuentos"].Value.ToString();
                var pagoFinal = selectedRow.Cells["PagoFinal"].Value.ToString();
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
                doc.Add(new Paragraph($"Código Planilla: {codigoPlanilla}"));
                doc.Add(new Paragraph($"Código Empleado: {codigoEmpleado}"));
                doc.Add(new Paragraph($"Fecha de Pago: {fechaPago}"));
                doc.Add(new Paragraph($"Salario Base: {salarioBase}"));
                doc.Add(new Paragraph($"Bonos: {bonos}"));
                doc.Add(new Paragraph($"Descuentos: {descuentos}"));
                doc.Add(new Paragraph($"Pago Final: {pagoFinal}"));
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
                if (dgvPlanillas.Rows.Count == 0)
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
                for (int i = 0; i < dgvPlanillas.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvPlanillas.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvPlanillas.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvPlanillas.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvPlanillas.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
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

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoEmpleado.Text) || string.IsNullOrEmpty(dtpFFechaPago.Text) || string.IsNullOrEmpty(txtSalarioBase.Text) || string.IsNullOrEmpty(txtBonos.Text) || string.IsNullOrEmpty(txtDescuentos.Text) || string.IsNullOrEmpty(txtPagoFinal.Text) || string.IsNullOrEmpty(cboxEstado.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoPlanilla = int.Parse(txtCodigoPlanilla.Text);

                    //Codigo Empleado
                    var SelectedEmpleado = (dynamic)cboxCodigoEmpleado.SelectedItem;
                    int codigoEmpleado = (int)SelectedEmpleado.GetType().GetProperty("Value").GetValue(SelectedEmpleado, null);

                    //Salario Base Empleado
                    double SalarioBase = cd_planillas.MtdConsultaSalarioBase(codigoEmpleado);

                    //Bono Empleado
                    double Bono = cl_planillas.MtdCalcularBono(SalarioBase);

                    //Descuentos Empleado
                    double Igss = cl_planillas.MtdCalcularIggs(SalarioBase);
                    double Isr = cl_planillas.MtdCalcularIsr(SalarioBase);
                    double Descuentos = Igss + Isr;

                    //Salario Final Empleado
                    double SalarioFinal = cl_planillas.MtdCalcularSalarioFinal(SalarioBase, Bono, Igss, Isr);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_planillas.MtdEditarDatosPlanillas(CodigoPlanilla, codigoEmpleado, dtpFFechaPago.Value, SalarioBase, Bono, Descuentos, SalarioFinal, cboxEstado.Text);
                    MessageBox.Show("Planilla actualizada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarPlanillas();
                    MtdLimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
        }   }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoPlanilla.Text))
            {
                MessageBox.Show("Por favor, Seleccine la planilla que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoPlanilla = int.Parse(txtCodigoPlanilla.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_planillas.MtdEliminarDatosPlanillas(CodigoPlanilla);
                    MessageBox.Show("Planilla eliminada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarPlanillas();
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

        private void dgvPlanillas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvPlanillas.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvPlanillas.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila con datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                txtCodigoPlanilla.Text = dgvPlanillas.SelectedCells[0].Value.ToString();

                // Codigo Empleado Foreing key
                int CodigoEmpleadoSeleccionado = Convert.ToInt32(dgvPlanillas.CurrentRow.Cells[1].Value);
                dynamic Empleado = cd_planillas.MtdConsultaEmpleadoDgv(CodigoEmpleadoSeleccionado)[0];
                cboxCodigoEmpleado.Text = Empleado.GetType().GetProperty("Text").GetValue(Empleado, null).ToString();

                dtpFFechaPago.Value = Convert.ToDateTime(dgvPlanillas.CurrentRow.Cells[2].Value);
                txtSalarioBase.Text = Convert.ToDouble(dgvPlanillas.CurrentRow.Cells[3].Value).ToString("c");
                txtBonos.Text = Convert.ToDouble(dgvPlanillas.CurrentRow.Cells[4].Value).ToString("c");
                txtDescuentos.Text = Convert.ToDouble(dgvPlanillas.CurrentRow.Cells[5].Value).ToString("c");
                txtPagoFinal.Text = Convert.ToDouble(dgvPlanillas.CurrentRow.Cells[6].Value).ToString("c");
                cboxEstado.Text = dgvPlanillas.CurrentRow.Cells[7].Value.ToString();
            }
        }
    }
}

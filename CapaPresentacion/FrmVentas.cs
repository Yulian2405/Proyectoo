using Capa_Datos;
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


namespace ProyectoMiguel
{

    public partial class FrmVentas : KryptonForm
    {
        CD_Ventas CD_Ventas = new CD_Ventas();


        public FrmVentas()
        {
            InitializeComponent();

        }
       
        private void MtdMostrarListaSucursal()
        {
            var ListaSucursal = CD_Ventas.MtdListaSucursal();
            cboxCodigoSucursal.Items.Clear();

            foreach (var Sucursal in ListaSucursal)
            {
                cboxCodigoSucursal.Items.Add(Sucursal);
            }

            cboxCodigoSucursal.DisplayMember = "Text";
            cboxCodigoSucursal.ValueMember = "Value";
        }

        public void MtdMostrarVentas()
        {
            DataTable dtVentas = new DataTable();
            dgvVentas.DataSource = CD_Ventas.MtdConsultarVentas();
        }
        private void kryptonLabel1_Paint(object sender, PaintEventArgs e)
        {

        }
   

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboxCodigoCliente.Text) ||
               string.IsNullOrEmpty(cboxCodigoSucursal.Text) ||
               string.IsNullOrEmpty(dtpFechaVenta.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text) ||
               string.IsNullOrEmpty(cboxMetodoPago.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    var SelectedCliente = (dynamic)cboxCodigoCliente.SelectedItem;
                    int codigoCliente = (int)SelectedCliente.GetType().GetProperty("Value").GetValue(SelectedCliente, null);

                    var SelectedSucursal = (dynamic)cboxCodigoSucursal.SelectedItem;
                    int codigoSucursal = (int)SelectedSucursal.GetType().GetProperty("Value").GetValue(SelectedSucursal, null);

                    DateTime FechaVenta = dtpFechaVenta.Value;
                    string MetodoPago = cboxMetodoPago.Text;
                    string  Estado = cboxEstado.Text;


                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    CD_Ventas.MtdAgregarVentas(codigoCliente, codigoSucursal, FechaVenta, MetodoPago, Estado);
                    MessageBox.Show("Venta agregada exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarVentas();
                    LimpiarCampos();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR CRITICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void FrmVentas_Load(object sender, EventArgs e)
        {
            MtdMostrarListaClientes();
            MtdMostrarListaSucursal();
            MtdMostrarVentas();
        }

        private void MtdMostrarListaClientes()
        {
            var ListaClientes = CD_Ventas.MtdListaClientes();
            cboxCodigoCliente.Items.Clear();

            foreach (var Clientes in ListaClientes)
            {
                cboxCodigoCliente.Items.Add(Clientes);
            }

            cboxCodigoCliente.DisplayMember = "Text";
            cboxCodigoCliente.ValueMember = "Value";
        }

        private void cboxCodigoCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
           

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoVenta.Text) ||
                string.IsNullOrEmpty(cboxCodigoCliente.Text) ||
               string.IsNullOrEmpty(cboxCodigoSucursal.Text) ||
               string.IsNullOrEmpty(dtpFechaVenta.Text) ||
               string.IsNullOrEmpty(cboxEstado.Text) ||
               string.IsNullOrEmpty(cboxMetodoPago.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                int CodigoVenta = Convert.ToInt32(txtCodigoVenta.Text);
                int CodigoCliente = Convert.ToInt32(cboxCodigoCliente.Text);
                int CodigoSucursal = Convert.ToInt32(cboxCodigoSucursal.Text);
                DateTime FechaVenta = dtpFechaVenta.Value;
                string MetodoPago = cboxMetodoPago.Text;
                string Estado = cboxEstado.Text;


                try
                {
                    CD_Ventas.MtdEditarVentas(CodigoVenta, CodigoCliente, CodigoSucursal, FechaVenta, MetodoPago, Estado);
                    MessageBox.Show("Venta actualizado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarVentas();
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
            txtCodigoVenta.Clear();
            cboxCodigoCliente.SelectedIndex = -1;
            cboxCodigoSucursal.SelectedIndex = -1;
            dtpFechaVenta.Value = DateTime.Now;
            cboxMetodoPago.SelectedIndex = -1;
            cboxEstado.SelectedIndex = -1;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrEmpty(txtCodigoVenta.Text)
                )
            {
                MessageBox.Show("Por favor, seleccione un Empleado a eliminar", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {

                int Codigo = Convert.ToInt32(txtCodigoVenta.Text);


                try
                {
                    CD_Ventas.MtdEliminarVentas(Codigo);
                    MessageBox.Show("Venta eliminado correctamente.", "Éxitooooo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarVentas();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles de la Venta", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVentas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvVentas.SelectedRows[0];


                if (selectedRow.Cells["CodigoVenta"].Value == null ||
                    selectedRow.Cells["CodigoCliente"].Value == null ||
                    selectedRow.Cells["CodigoSucursal"].Value == null ||
                    selectedRow.Cells["FechaVenta"].Value == null ||
                    selectedRow.Cells["MetodoPago"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Por favor, selecciona una fila válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var CodigoVenta = selectedRow.Cells["CodigoVenta"].Value.ToString();
                var CodigoCliente = selectedRow.Cells["CodigoCliente"].Value.ToString();
                var CodigoSucursal = selectedRow.Cells["CodigoSucursal"].Value.ToString();
                var FechaVenta = selectedRow.Cells["FechaVenta"].Value.ToString();
                var MetodoPago = selectedRow.Cells["MetodoPago"].Value.ToString();
                var Estado = selectedRow.Cells["Estado"].Value.ToString();

                // Generar PDF con tamaño de PDF
                string pdfPath = @"C:\Users\elfid\Desktop\PROYECTOFINAL2.pdf"; // Ajusta la ruta
                Document doc = new Document(new iTextSharp.text.Rectangle(280, 300), 10, 10, 10, 10); // 280 mm de ancho, 500 mm de alto, márgenes
                PdfWriter.GetInstance(doc, new FileStream(pdfPath, FileMode.Create));
                doc.Open();

                // Agregar logo y contenido al PDF
                string logoPath = @"C:\Users\elfid\Desktop\PROYECTOFINAL2.pdf"; // Ajusta la ruta
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 60f); // Ajusta el tamaño del logo para que quepa en el ticket
                logo.Alignment = Element.ALIGN_CENTER;
                doc.Add(logo);

                // Agregar encabezado y detalles
                doc.Add(new Paragraph("Detalles de la Venta", new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                });
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph($"Código Venta: {CodigoVenta}"));
                doc.Add(new Paragraph($"Codigo Cliente: {CodigoCliente}"));
                doc.Add(new Paragraph($"Codigo Sucursal: {CodigoSucursal}"));
                doc.Add(new Paragraph($"Fecha Venta: {FechaVenta}"));
                doc.Add(new Paragraph($"Metodo Pago: {MetodoPago}"));
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

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvVentas.Rows.Count == 0)
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
                for (int i = 0; i < dgvVentas.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dgvVentas.Columns[i].HeaderText;
                }

                // Agregar los datos del DataGridView
                for (int i = 0; i < dgvVentas.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvVentas.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvVentas.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
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

        private void dgvVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var FilaSeleccionada = dgvVentas.SelectedRows[0];

            if (FilaSeleccionada.Index == dgvVentas.Rows.Count - 1)
            {
                MessageBox.Show("Seleccione una fila valida, por favor,", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                txtCodigoVenta.Text = FilaSeleccionada.Cells[0].Value.ToString();
                cboxCodigoCliente.Text = FilaSeleccionada.Cells[1].Value.ToString();
                cboxCodigoSucursal.Text = FilaSeleccionada.Cells[2].Value.ToString();
                dtpFechaVenta.Text = FilaSeleccionada.Cells[3].Value.ToString();
                cboxMetodoPago.Text = FilaSeleccionada.Cells[4].Value.ToString();
                cboxEstado.Text = FilaSeleccionada.Cells[5].Value.ToString();
            }
        }
    }
}

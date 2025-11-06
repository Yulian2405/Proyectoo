using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using CapaDatos;
using static CapaDatos.CDenvios;
using System.Drawing.Printing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;

namespace CapaPresentacion
{
    public partial class FrmEnvios : KryptonForm
    {
        CDenvios cd_envios = new CDenvios();


        public FrmEnvios()
        {
            InitializeComponent();

        }

        private void FrmEnvios_Load(object sender, EventArgs e)
        {
            MtdCargarVentas(); 
            MtdCargarEnvios();
            MtdMostrarEnviosDGV();
        }

        // Metodo que muestra las planillas en el DataGridView
        public void MtdMostrarEnviosDGV()
        {
            DataTable dtEnvios = new DataTable();
            dgvEnvios.DataSource = cd_envios.MtdConsultarEnvios();
        }


        private void LimpiarCampos()
        {
            txtCodigoEnvio.Clear();

            if (cboxCodigoVenta.Items.Count > 0)
                cboxCodigoVenta.SelectedIndex = 0;

            dtpFechaEnvio.Value = DateTime.Now;
            txtDireccion.Clear();

            if (cboxVehiculo.Items.Count > 0)
                cboxVehiculo.SelectedIndex = 0;

            txtCostoEnvio.Clear();

            if (cboxEstado.Items.Count > 0)
                cboxEstado.SelectedIndex = 0;

            cboxVehiculo.Text = " ";
            cboxEstado.Text = " ";
        }


        private void MtdCargarVentas()
        {
            CD_Conexion conn = new CD_Conexion();

            string query = "SELECT CodigoVenta, CONCAT('Venta ', CodigoVenta, ' - ', FORMAT(FechaVenta, 'dd/MM/yyyy')) AS Descripcion FROM tbl_ventas";
            SqlCommand cmd = new SqlCommand(query, conn.MtdAbrirConexion());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.MtdCerrarConexion();

            cboxCodigoVenta.DataSource = dt;
            cboxCodigoVenta.DisplayMember = "Descripcion";
            cboxCodigoVenta.ValueMember = "CodigoVenta";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoVenta = Convert.ToInt32(cboxCodigoVenta.SelectedValue);
                DateTime fechaEnvio = dtpFechaEnvio.Value;
                string direccion = txtDireccion.Text;
                string vehiculo = cboxVehiculo.Text;
                decimal costoEnvio = Convert.ToDecimal(txtCostoEnvio.Text);
                string estado = cboxEstado.Text;

                cd_envios.MtdAgregarEnvios(codigoVenta, fechaEnvio, direccion, vehiculo, costoEnvio, estado);

                MessageBox.Show("Envío agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MtdMostrarEnviosDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el envío: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoEnvio = Convert.ToInt32(txtCodigoEnvio.Text);
                int codigoVenta = Convert.ToInt32(cboxCodigoVenta.SelectedValue);
                DateTime fechaEnvio = dtpFechaEnvio.Value;
                string direccion = txtDireccion.Text;
                string vehiculo = cboxVehiculo.Text;
                decimal costoEnvio = Convert.ToDecimal(txtCostoEnvio.Text);
                string estado = cboxEstado.Text;

                cd_envios.MtdEditarEnvios(codigoEnvio, codigoVenta, fechaEnvio, direccion, vehiculo, costoEnvio, estado);

                MessageBox.Show("Envío editado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MtdMostrarEnviosDGV();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el envío: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoEnvio.Text))
            {
                MessageBox.Show("Por favor, Seleccine el envio que desea eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    //Codigo Planilla
                    int CodigoEnvio = int.Parse(txtCodigoEnvio.Text);

                    //Enviar datos al metodo de la capa datos para agregar a la base de datos
                    cd_envios.MtdEliminarEnvios(CodigoEnvio);
                    MessageBox.Show("Envio eliminado exitosamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MtdMostrarEnviosDGV();
                    LimpiarCampos();

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
            LimpiarCampos();
        }

  
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MtdCargarEnvios()
        {
            try
            {
                DataTable dt = cd_envios.MtdConsultarEnvios(); // Método que ya tienes en CDenvios
                dgvEnvios.DataSource = dt;

                dgvEnvios.Columns["CodigoEnvio"].HeaderText = "Código Envío";
                dgvEnvios.Columns["CodigoVenta"].HeaderText = "Código Venta";
                dgvEnvios.Columns["FechaEnvio"].HeaderText = "Fecha Envío";
                dgvEnvios.Columns["Direccion"].HeaderText = "Dirección";
                dgvEnvios.Columns["Vehiculo"].HeaderText = "Vehículo";
                dgvEnvios.Columns["CostoEnvio"].HeaderText = "Costo";
                dgvEnvios.Columns["Estado"].HeaderText = "Estado";

                dgvEnvios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar envíos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEnvios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

                
        }

        private void btnImprimirPdf_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvEnvios.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Por favor, seleccione una línea del DataGridView de envíos.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var selectedRow = dgvEnvios.SelectedRows[0];

                // Validación de celdas requeridas (ajusta nombres si tus columnas difieren)
                if (selectedRow.Cells["CodigoEnvio"].Value == null ||
                    selectedRow.Cells["CodigoVenta"].Value == null ||
                    selectedRow.Cells["FechaEnvio"].Value == null ||
                    selectedRow.Cells["Direccion"].Value == null ||
                    selectedRow.Cells["Vehiculo"].Value == null ||
                    selectedRow.Cells["CostoEnvio"].Value == null ||
                    selectedRow.Cells["Estado"].Value == null)
                {
                    MessageBox.Show("La fila seleccionada contiene valores vacíos. Selecciona una fila válida.",
                        "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Lectura de valores
                var codigoEnvio = selectedRow.Cells["CodigoEnvio"].Value.ToString();
                var codigoVenta = selectedRow.Cells["CodigoVenta"].Value.ToString();
                var fechaEnvio = selectedRow.Cells["FechaEnvio"].Value.ToString();
                var direccion = selectedRow.Cells["Direccion"].Value.ToString();
                var vehiculo = selectedRow.Cells["Vehiculo"].Value.ToString();
                var costoEnvio = selectedRow.Cells["CostoEnvio"].Value.ToString();
                var estado = selectedRow.Cells["Estado"].Value.ToString();

                // ====== Generar PDF ======
                // (Cambia esta ruta por la tuya)
                string pdfPath = @"C:\Users\ciber\Documents" + codigoEnvio + ".pdf";

                // A6 es buen tamaño de ticket. (Los tamaños en iTextSharp van en puntos, no en mm)
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A6, 10, 10, 10, 10);
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new System.IO.FileStream(pdfPath, System.IO.FileMode.Create));
                doc.Open();

                // Logo (Cambia por la ruta de tu imagen; NO pongas carpeta)
                // Ejemplo: @"C:\TU\RUTA\logo.png"
                string logoPath = @"C:\Users\ciber\Documents\Call of Duty\ProyectoFinal\ProyectoFinal\LOgo";
                if (System.IO.File.Exists(logoPath))
                {
                    var logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(120f, 60f);
                    logo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    doc.Add(logo);
                }

                // Título
                var fTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 14f, iTextSharp.text.Font.BOLD);
                var fNormal = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10f, iTextSharp.text.Font.NORMAL);

                var titulo = new iTextSharp.text.Paragraph("Detalles del Envío", fTitulo)
                {
                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new iTextSharp.text.Paragraph(" "));

                // Detalle simple en líneas (como el inge)
                doc.Add(new iTextSharp.text.Paragraph($"Código Envío: {codigoEnvio}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Código Venta: {codigoVenta}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Fecha de Envío: {fechaEnvio}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Dirección: {direccion}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Vehículo: {vehiculo}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Costo Envío: {costoEnvio}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Estado: {estado}", fNormal));

                doc.Close();

                // Abrir PDF con el lector predeterminado (igual que el inge)
                System.Diagnostics.Process.Start(pdfPath);

                // ====== Vista previa/impresión opcional (si ya tienes estos métodos en tu form) ======
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                ConfigurarTamanioPapel(pd); // tu método existente

                PrintPreviewDialog vistaPrevia = new PrintPreviewDialog();
                vistaPrevia.Document = pd;
                vistaPrevia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Detalles del Envios", new System.Drawing.Font("Arial", 20, FontStyle.Bold), Brushes.Black, new PointF(100, 100));
        }

        private void ConfigurarTamanioPapel(PrintDocument pd)
        {
            pd.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 en 100 DPI
        }

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (dgvEnvios.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos en el DataGridView de envíos para exportar.", "Advertencia",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;

                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                // Encabezados
                for (int i = 0; i < dgvEnvios.Columns.Count; i++)
                    worksheet.Cells[1, i + 1] = dgvEnvios.Columns[i].HeaderText;

                // Datos
                for (int i = 0; i < dgvEnvios.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvEnvios.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dgvEnvios.Rows[i].Cells[j].Value?.ToString() ?? string.Empty;
                    }
                }

                // Ruta (cámbiala)
                string excelFilePath = @"C:\Users\ciber\Documents\Envios.xlsx";
                workbook.SaveAs(excelFilePath);
                workbook.Close();
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                MessageBox.Show("Datos de envíos exportados a Excel con éxito.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Vista previa impresión (reutilizando tus handlers existentes)
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                ConfigurarTamanioPapel(pd);

                PrintPreviewDialog vistaPrevia = new PrintPreviewDialog();
                vistaPrevia.Document = pd;
                vistaPrevia.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvEnvios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvEnvios.Rows[e.RowIndex];
                txtCodigoEnvio.Text = row.Cells["CodigoEnvio"].Value.ToString();
                cboxCodigoVenta.SelectedValue = row.Cells["CodigoVenta"].Value;
                dtpFechaEnvio.Value = Convert.ToDateTime(row.Cells["FechaEnvio"].Value);
                txtDireccion.Text = row.Cells["Direccion"].Value.ToString();
                cboxVehiculo.Text = row.Cells["Vehiculo"].Value.ToString();
                txtCostoEnvio.Text = row.Cells["CostoEnvio"].Value.ToString();
                cboxEstado.Text = row.Cells["Estado"].Value.ToString();
            }
        }
    }
}




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalAgenda.WinForm
{
    public partial class FrmBuscarContacto : Form
    {
        private ServiceReferenceWCF.AgendaContactosServiceClient wcfService = new ServiceReferenceWCF.AgendaContactosServiceClient();

        public FrmBuscarContacto()
        {
            InitializeComponent();
            btnBuscar.Click += btnBuscar_Click;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombreBusqueda = txtNombreBusqueda.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombreBusqueda))
            {
                MessageBox.Show("Por favor, ingresa un nombre para buscar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Llamada al método que devuelve una lista de contactos por coincidencia parcial
                var contactos = wcfService.BuscarContactosPorNombre(nombreBusqueda);

                // Si usas arrays en el WCF (ServiceReference genera arrays por defecto), usa esto:
                dgvContactos.DataSource = null;
                if (contactos != null && contactos.Length > 0)
                {
                    dgvContactos.DataSource = contactos;
                }
                else
                {
                    MessageBox.Show("No se encontraron contactos con ese nombre.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvContactos.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al consultar el servicio:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

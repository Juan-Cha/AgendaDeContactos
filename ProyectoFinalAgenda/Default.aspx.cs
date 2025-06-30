using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProyectoFinalAgenda.ServiceReferenceAsmx;

namespace ProyectoFinalAgenda
{
    public partial class Default : System.Web.UI.Page
    {

        private ServiceReferenceAsmx.AgendaContactosService1SoapClient service = new ServiceReferenceAsmx.AgendaContactosService1SoapClient();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarContactos();
        }

        void CargarContactos()
        {
            try
            {
                gvContactos.DataSource = service.GetContactos();
                gvContactos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarSweetAlert("error", "Error al cargar", ex.Message);
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MostrarSweetAlert("warning", "Campos requeridos", "Por favor, llena los campos obligatorios.");
                return;
            }

            try
            {
                var contacto = new Contacto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim()
                };

                if (!string.IsNullOrWhiteSpace(hfId.Value))
                {
                    // Editar
                    contacto.Id = int.Parse(hfId.Value);
                    service.EditarContacto(contacto);
                    MostrarSweetAlert("success", "Editado", "Contacto actualizado correctamente.");
                }
                else
                {
                    // Agregar
                    service.AgregarContacto(contacto);
                    MostrarSweetAlert("success", "Agregado", "Contacto registrado exitosamente.");
                }

                LimpiarFormulario();
                CargarContactos();
            }
            catch (Exception ex)
            {
                MostrarSweetAlert("error", "Error", "No se pudo guardar: " + ex.Message);
            }
        }

        protected void gvContactos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var c = service.GetContactoPorId(id);
                if (c != null)
                {
                    hfId.Value = c.Id.ToString();
                    txtNombre.Text = c.Nombre;
                    txtTelefono.Text = c.Telefono;
                    txtEmail.Text = c.Email;
                    txtDireccion.Text = c.Direccion;
                    btnGuardar.Text = "<i class='fa fa-save'></i> Actualizar";
                    btnCancelar.Visible = true;
                }
            }
            else if (e.CommandName == "Eliminar")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // Depuración temporal
                MostrarSweetAlert("info", "Depuración", $"ID recibido: {id}");

                try
                {
                    bool eliminado = service.EliminarContacto(id);
                    if (eliminado)
                    {
                        MostrarSweetAlert("success", "Eliminado", "Contacto eliminado correctamente.");
                    }
                    else
                    {
                        MostrarSweetAlert("warning", "No encontrado", "El contacto no existe.");
                    }
                    CargarContactos();
                }
                catch (Exception ex)
                {
                    MostrarSweetAlert("error", "Error al eliminar", ex.Message);
                }
            }
        }


        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        void LimpiarFormulario()
        {
            hfId.Value = "";
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtDireccion.Text = "";
            btnGuardar.Text = "<i class='fa fa-save'></i> Guardar";
            btnCancelar.Visible = false;
        }

        public void MostrarSweetAlert(string icon, string title, string text)
        {
            string script = $@"
        Swal.fire({{
            icon: '{icon}',
            title: '{title}',
            text: '{text}',
            confirmButtonColor: '#0099ff'
        }});";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }

    }
}
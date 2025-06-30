<%@ Page Title="Agenda" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProyectoFinalAgenda.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Formulario de alta/edición -->
    <asp:Panel ID="pnlFormulario" runat="server" CssClass="mb-3">
        <asp:HiddenField ID="hfId" runat="server" /> <!-- Usado para edición -->
        <asp:TextBox ID="txtNombre" runat="server" CssClass="input-txt" placeholder="Nombre*" MaxLength="100"
            onkeypress="return validarNombre(event);" />

        <asp:TextBox ID="txtTelefono" runat="server" CssClass="input-txt" placeholder="Teléfono*" MaxLength="10"
            onkeypress="return validarTelefono(event);" />

        <asp:TextBox ID="txtEmail" runat="server" CssClass="input-txt" placeholder="Email*" MaxLength="100" TextMode="Email"
            onkeypress="return validarEmail(event);" onblur="validarFormatoEmail(this);" />

        <asp:TextBox ID="txtDireccion" runat="server" CssClass="input-txt" placeholder="Dirección" MaxLength="200"
            onkeypress="return validarDireccion(event);" />
        <div>
            <asp:LinkButton ID="btnGuardar" runat="server" CssClass="btn-icon" OnClick="btnGuardar_Click" CausesValidation="true">
                <i class="fa fa-save"></i> Guardar
            </asp:LinkButton>
            <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn-icon" OnClick="btnCancelar_Click" Visible="false" CausesValidation="false">
                <i class="fa fa-ban"></i> Cancelar
            </asp:LinkButton>
        </div>
    </asp:Panel>

            <!-- Tabla de contactos -->
<div style="overflow-x:auto;">
    <asp:GridView ID="gvContactos" runat="server" AutoGenerateColumns="False" CssClass="table"
        DataKeyNames="Id" OnRowCommand="gvContactos_RowCommand" ShowHeaderWhenEmpty="true">
        <Columns>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:TemplateField HeaderText="Dirección">
                <ItemTemplate>
                    <%# string.IsNullOrWhiteSpace(Eval("Direccion") as string)
                        ? "<span style='display:flex;align-items:center;justify-content:center;height:100%;'><i class=\"fa fa-map-marker-alt\" style=\"font-size:1.3em;color:#bbb;line-height:1.5;\" title=\"Sin dirección\"></i></span>"
                        : Server.HtmlEncode(Eval("Direccion").ToString()) %>
                </ItemTemplate>
            </asp:TemplateField>
           <asp:TemplateField HeaderText="Acciones">
    <ItemTemplate>
        <div style="display:flex; gap:8px; justify-content:center; align-items:center;">
            <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id") %>'
                CssClass="action-btn" ToolTip="Editar">
                <i class="fa fa-pen"></i>
            </asp:LinkButton>
           <asp:LinkButton 
                ID="btnEliminar" 
                runat="server" 
                CommandName="Eliminar" 
                CommandArgument='<%# Eval("Id") %>'
                CssClass="action-btn delete" 
                
                ToolTip="Eliminar">
                <i class="fa fa-trash"></i>
            </asp:LinkButton>

        </div>
    </ItemTemplate>
    <ItemStyle HorizontalAlign="Center" Width="80px" />
</asp:TemplateField>

        </Columns>
        <EmptyDataTemplate>
            <div style="padding:20px; color:#888;">
                <i class="fa fa-address-book fa-2x"></i> <br />
                No hay contactos registrados.
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
</div>


    <!-- Script para confirmación de eliminación con SweetAlert2 -->
    <script type="text/javascript">
        function confirmarEliminacion(btn) {
            Swal.fire({
                title: '¿Eliminar contacto?',
                text: 'Esta acción no se puede deshacer.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',
                cancelButtonColor: '#0099ff',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack(btn.name, '');
                }
            });
            return false;
        }

    </script>
    <script>
        function validarNombre(e) {
            const key = e.key;
            // Letras, espacio, acentos y Ñ/ñ
            return /^[a-zA-ZáéíóúÁÉÍÓÚüÜñÑ\s]$/.test(key);
        }

        function validarTelefono(e) {
            const key = e.key;
            const value = e.target.value;
            // Permitir solo un "+" al inicio
            if (key === "+") {
                return value.length === 0 && !value.includes("+");
            }
            // Permitir números, espacios, paréntesis y guion
            return /^[0-9\s\-\(\)]$/.test(key);
        }

        function validarFormatoEmail(input) {
            const email = input.value.trim();
            if (email.length === 0) return;
            // Regex simple para email válido
            const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!regex.test(email)) {
                Swal.fire({
                    icon: 'error',
                    title: 'Formato de email inválido',
                    text: 'Ingresa un email válido (ej: nombre@dominio.com)',
                    confirmButtonColor: '#0099ff'
                });
                input.value = "";
                input.focus();
            }
        }

        function validarDireccion(e) {
            const key = e.key;
            // Letras, números, espacios, punto, coma, #, -, /
            return /^[a-zA-Z0-9\s\.\,\#\-\u00F1\u00D1\/]$/.test(key);
        }
    </script>

</asp:Content>

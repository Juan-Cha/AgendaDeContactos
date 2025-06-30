using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using ProyectoFinalAgenda.Models;
using System.Data.SqlClient;


namespace ProyectoFinalAgenda.Servicios
{
    /// <summary>
    /// Descripción breve de AgendaContactosService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class AgendaContactosService1 : System.Web.Services.WebService
    {

        private readonly string connStr;

        public AgendaContactosService1()
        {
            connStr = ConfigurationManager.ConnectionStrings["MiConexionLocal"].ConnectionString;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        [WebMethod]
        public List<Contacto> GetContactos()
        {
            var lista = new List<Contacto>();
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Contactos", conn);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Contacto
                        {
                            Id = (int)dr["Id"],
                            Nombre = dr["Nombre"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Email = dr["Email"].ToString(),
                            Direccion = dr["Direccion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        [WebMethod]
        public Contacto GetContactoPorId(int id)
        {
            Contacto contacto = null;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Contactos WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        contacto = new Contacto
                        {
                            Id = (int)dr["Id"],
                            Nombre = dr["Nombre"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Email = dr["Email"].ToString(),
                            Direccion = dr["Direccion"].ToString()
                        };
                    }
                }
            }
            return contacto;
        }

        [WebMethod]
        public void AgregarContacto(Contacto c)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Contactos (Nombre, Telefono, Email, Direccion) VALUES (@Nombre, @Telefono, @Email, @Direccion)", conn);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.Parameters.AddWithValue("@Direccion", c.Direccion ?? "");
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public void EditarContacto(Contacto c)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Contactos SET Nombre=@Nombre, Telefono=@Telefono, Email=@Email, Direccion=@Direccion WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                cmd.Parameters.AddWithValue("@Email", c.Email);
                cmd.Parameters.AddWithValue("@Direccion", c.Direccion ?? "");
                cmd.Parameters.AddWithValue("@Id", c.Id);
                cmd.ExecuteNonQuery();
            }
        }

        [WebMethod]
        public bool EliminarContacto(int id)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Contactos WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }

    }
}

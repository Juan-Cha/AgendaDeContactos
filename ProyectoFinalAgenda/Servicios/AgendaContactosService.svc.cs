using ProyectoFinalAgenda.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProyectoFinalAgenda.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "AgendaContactosService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione AgendaContactosService.svc o AgendaContactosService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class AgendaContactosService : IAgendaContactosService
    {
        public void DoWork()
        {
        }

        private readonly string connStr;

        public AgendaContactosService()
        {
            connStr = ConfigurationManager.ConnectionStrings["MiConexionLocal"].ConnectionString;
        }

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

        public void EliminarContacto(int id)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Contactos WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Contacto> BuscarContactosPorNombre(string nombre)
        {
            var lista = new List<Contacto>();
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Contactos WHERE Nombre LIKE @Nombre", conn);
                cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
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


    }
}

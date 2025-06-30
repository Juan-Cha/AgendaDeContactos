using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProyectoFinalAgenda.Models
{
    [DataContract]
    public class Contacto
    {
        [DataMember] public int Id { get; set; }
        [DataMember] public string Nombre { get; set; }
        [DataMember] public string Telefono { get; set; }
        [DataMember] public string Email { get; set; }
        [DataMember] public string Direccion { get; set; }
    }
}
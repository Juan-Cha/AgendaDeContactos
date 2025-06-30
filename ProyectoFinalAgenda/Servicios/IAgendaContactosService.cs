using ProyectoFinalAgenda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ProyectoFinalAgenda.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IAgendaContactosService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IAgendaContactosService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        List<Contacto> GetContactos();

        [OperationContract]
        Contacto GetContactoPorId(int id);

        [OperationContract]
        void AgregarContacto(Contacto c);

        [OperationContract]
        void EditarContacto(Contacto c);

        [OperationContract]
        void EliminarContacto(int id);

        [OperationContract]
        List<Contacto> BuscarContactosPorNombre(string nombre);


    }
}

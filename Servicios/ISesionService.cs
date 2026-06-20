using ServidorAhorcado.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    [ServiceContract]
    public interface ISesionService
    {
        [OperationContract]
        KeyValuePair<int, JugadorDTO> IniciaSesion(string usuario, string contrasena);

        [OperationContract]
        void CerrarSesion(string usuario);
    }

    
}

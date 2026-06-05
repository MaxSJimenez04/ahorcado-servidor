using ServidorAhorcado.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "ISesionService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface ISesionService
    {
        [OperationContract]
        KeyValuePair<int, JugadorDTO> IniciaSesion(string usuario, string contrasena);
    }

    
}

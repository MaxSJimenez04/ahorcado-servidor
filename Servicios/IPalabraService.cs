using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServidorAhorcado.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IPalabraService" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IPalabraService
    {
        [OperationContract]
        List<PalabraDTO> obtenerPalabrasPorCategoria(int categoriaId);

        [OperationContract]
        List<CategoriaDTO> obtenerCategorias();

        [OperationContract]
        PalabraDTO obtenerDatosPalabra(int palabraID);
    }
}

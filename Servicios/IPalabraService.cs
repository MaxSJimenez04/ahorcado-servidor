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
    [ServiceContract]
    public interface IPalabraService
    {
        [OperationContract]
        List<PalabraDTO> ObtenerPalabrasPorCategoria(int categoriaId);

        [OperationContract]
        List<CategoriaDTO> ObtenerCategorias();

        [OperationContract]
        PalabraDTO ObtenerDatosPalabra(int palabraID);
    }
}

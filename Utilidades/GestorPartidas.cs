using ServidorAhorcado.DTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServidorAhorcado.Utilidades
{
    public static class GestorPartidas
    {
        // Guarda todas las partidas activas en memoria por su IdPartida
        private static readonly ConcurrentDictionary<int, PartidaDTO> _partidasActivas = new ConcurrentDictionary<int, PartidaDTO>();

        public static void AgregarPartida(int idPartida, PartidaDTO partida)
        {
            _partidasActivas[idPartida] = partida;
        }

        public static bool TryObtenerPartida(int idPartida, out PartidaDTO partida)
        {
            return _partidasActivas.TryGetValue(idPartida, out partida);
        }

        public static void EliminarPartida(int idPartida)
        {
            _partidasActivas.TryRemove(idPartida, out _);
        }
    }
}
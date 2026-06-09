using ServidorAhorcado.Servicios;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServidorAhorcado.Utilidades
{
    public static class CallbackManager
    {
        // Guarda el canal de comunicación de cada jugador por su IdJugador
        private static readonly ConcurrentDictionary<int, IPartidaCallback> _callbacks = new ConcurrentDictionary<int, IPartidaCallback>();

        public static void RegistrarCallback(int idJugador, IPartidaCallback callback)
        {
            _callbacks[idJugador] = callback;
        }

        public static bool TryObtenerCallback(int idJugador, out IPartidaCallback callback)
        {
            return _callbacks.TryGetValue(idJugador, out callback);
        }

        public static void EliminarCallback(int idJugador)
        {
            _callbacks.TryRemove(idJugador, out _);
        }
    }
}
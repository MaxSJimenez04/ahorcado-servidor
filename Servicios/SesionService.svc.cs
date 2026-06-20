using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ServidorAhorcado.Servicios
{
    public class SesionService : ISesionService
    {
        static ConcurrentDictionary<string, JugadorDTO> _SesionesActivas = new ConcurrentDictionary<string, JugadorDTO>();
        public KeyValuePair<int,JugadorDTO> IniciaSesion(string usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {

                return new KeyValuePair<int, JugadorDTO>(1, null);
            }

            try
            {
                var db = new AhorcadoEntities();
                var jugadorDB = db.Jugador.FirstOrDefault(j => j.Usuario == usuario);

                if (jugadorDB == null)
                {

                    return new KeyValuePair<int, JugadorDTO>(2, null);
                }

                bool esContrasenaCorrecta = BC.Verify(contrasena, jugadorDB.Contrasena);

                if (!esContrasenaCorrecta)
                {
                    return new KeyValuePair<int, JugadorDTO>(2, null);
                }

                if (_SesionesActivas.ContainsKey(usuario))
                {
                    return new KeyValuePair<int, JugadorDTO>(3, null);
                }

                JugadorDTO jugador = new JugadorDTO{
                    idJugador = jugadorDB.IdJugador,
                    usuario = jugadorDB.Usuario,
                    nombre = jugadorDB.Nombre,
                    primerApellido = jugadorDB.PrimerApellido,
                    segundoApellido = jugadorDB.SegundoApellido,
                    correo = jugadorDB.Correo,
                    telefono = jugadorDB.Telefono,
                    fechaNacimiento = jugadorDB.FechaNacimiento,
                    contrasena = jugadorDB.Contrasena,
                    puntos = jugadorDB.Puntos
                };

                _SesionesActivas.TryAdd(usuario, jugador);
                return new KeyValuePair<int, JugadorDTO>(0, jugador);

            }
            catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return new KeyValuePair<int, JugadorDTO>(4, null);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return new KeyValuePair<int, JugadorDTO>(5, null);
            }
        }

        void ISesionService.CerrarSesion(string usuario)
        {
            _SesionesActivas.TryRemove(usuario, out _);
        }
    }
}

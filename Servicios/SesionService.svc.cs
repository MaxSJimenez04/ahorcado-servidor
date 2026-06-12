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
        static Dictionary<string, JugadorDTO> _SesionesActivas = new Dictionary<string, JugadorDTO>();
        public KeyValuePair<int,JugadorDTO> IniciaSesion(string usuario, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
            {

                //Estado 1: No se recibió usuario o contraseña
                return new KeyValuePair<int, JugadorDTO>(1, null);
            }

            try
            {
                var db = new AhorcadoEntities();
                var jugadorDB = db.Jugador.FirstOrDefault(j => j.Usuario == usuario);

                if (jugadorDB == null)
                {
                    //Estado 2: No se encontró el usuario
                    return new KeyValuePair<int, JugadorDTO>(2, null);
                }

                bool esContrasenaCorrecta = BC.Verify(contrasena, jugadorDB.Contrasena);

                if (!esContrasenaCorrecta)
                {
                    //Estado 2 de nuevo por que no se encontró la contraseña
                    return new KeyValuePair<int, JugadorDTO>(2, null);
                }

                if (_SesionesActivas.ContainsKey(usuario))
                {
                    //Estado 3: Ya hay una sesión activa para el usuario
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
                    contrasena = jugadorDB.Contrasena
                };

                _SesionesActivas.Add(usuario, jugador);
                //Estado 0: si se pudo iniciar sesión
                return new KeyValuePair<int, JugadorDTO>(0, jugador);

            }
            catch(EntityException ee)
            {
                //Error en la BD
                Console.WriteLine(ee.Message);
                return new KeyValuePair<int, JugadorDTO>(4, null);
            }catch(Exception e)
            {
                //Cualquier otra Excepción
                Console.WriteLine(e.Message + e.StackTrace);
                return new KeyValuePair<int, JugadorDTO>(5, null);
            }
        }
    }
}

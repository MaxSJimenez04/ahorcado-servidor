using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace ServidorAhorcado.Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "UsuarioService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione UsuarioService.svc o UsuarioService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class UsuarioService : IUsuarioService
    {
        public int ActualizarJugador(JugadorDTO datosActualizados)
        {
            throw new NotImplementedException();
        }

        public int RegistrarJugador(JugadorDTO nuevoJugador)
        {
            try
            {
                var db = new AhorcadoEntities();
                var jugadorExistente = db.Jugador.Any(j => j.Usuario == nuevoJugador.usuario);

                if (jugadorExistente)
                {
                    //Estado 1, ya hay una persona registrada con ese username
                    return 1;
                }

                bool correoEnUso = db.Jugador.Any(j => j.Correo == nuevoJugador.correo);
                if (correoEnUso)
                {
                    //Estado 2: Correo en uso
                    return 2;
                }

                string contrasenaHasheada = BC.HashPassword(nuevoJugador.contrasena);

                Jugador datosJugador = new Jugador
                {
                    Usuario = nuevoJugador.usuario,
                    Nombre = nuevoJugador.nombre,
                    PrimerApellido = nuevoJugador.primerApellido,
                    SegundoApellido = nuevoJugador.segundoApellido,
                    Contrasena = contrasenaHasheada,
                    Correo = nuevoJugador.correo,
                    Telefono = nuevoJugador.telefono,
                    FechaNacimiento = nuevoJugador.fechaNacimiento,
                    Puntos = 0
                };

                db.Jugador.Add(datosJugador);
                db.SaveChanges();
                return 0;
            }
            catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return 3;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 3;
            }
        }

        JugadorDTO IUsuarioService.ObtenerDatosJugador(string usuario)
        {
            try
            {
                var db = new AhorcadoEntities();
                var jugador = db.Jugador.First(j => j.Usuario == usuario);

                if (jugador == null)
                {
                    return null;
                }

                JugadorDTO datosJugador = new JugadorDTO
                {
                    idJugador = jugador.IdJugador,
                    usuario = jugador.Usuario,
                    nombre = jugador.Nombre,
                    primerApellido = jugador.PrimerApellido,
                    segundoApellido = jugador.SegundoApellido,
                    contrasena = jugador.Contrasena,
                    correo = jugador.Correo,
                    telefono = jugador.Telefono,
                    fechaNacimiento = jugador.FechaNacimiento
                };
                return datosJugador;
            }
            catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

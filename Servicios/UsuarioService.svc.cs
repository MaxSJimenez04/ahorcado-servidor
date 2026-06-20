using ServidorAhorcado.DTO;
using ServidorAhorcado.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace ServidorAhorcado.Servicios
{
    public class UsuarioService : IUsuarioService
    {
        public int ActualizarJugador(JugadorDTO datosActualizados)
        {
            if (!validarDatosActualizados(datosActualizados))
            {
                return 1;
            }

            try
            {
                var db = new AhorcadoEntities();
                var jugador = db.Jugador.Find(datosActualizados.idJugador);

                if (jugador == null)
                {
                    return 2;
                }

                if (jugador.Usuario != datosActualizados.usuario)
                {
                    bool usuarioOcupado = db.Jugador.Any(j => j.Usuario == datosActualizados.usuario);
                    if (usuarioOcupado)
                    {
                        return 3;
                    }
                }
                string nuevaContrasenaHasheada = BC.HashPassword(datosActualizados.contrasena);

                jugador.Usuario = datosActualizados.usuario;
                jugador.Nombre = datosActualizados.nombre;
                jugador.PrimerApellido = datosActualizados.primerApellido;
                jugador.SegundoApellido = datosActualizados.segundoApellido;
                jugador.Contrasena = nuevaContrasenaHasheada;
                jugador.Telefono = datosActualizados.telefono;
                jugador.FechaNacimiento = datosActualizados.fechaNacimiento;

                db.SaveChanges();

                return 0;
                
            }catch(EntityException ee)
            {
                Console.WriteLine(ee.Message);
                return 4;
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 4;
            }
        }

        public int RegistrarJugador(JugadorDTO nuevoJugador)
        {
            try
            {
                var db = new AhorcadoEntities();
                var jugadorExistente = db.Jugador.Any(j => j.Usuario == nuevoJugador.usuario);

                if (!validarDatosActualizados(nuevoJugador))
                {
                    return 1;
                }

                if (jugadorExistente)
                {
                    return 2;
                }

                bool correoEnUso = db.Jugador.Any(j => j.Correo == nuevoJugador.correo);
                if (correoEnUso)
                {
                    return 3;
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
                return 4;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 4;
            }
        }

        JugadorDTO IUsuarioService.ObtenerDatosJugador(string usuario)
        {
            try
            {
                var db = new AhorcadoEntities();
                var jugador = db.Jugador.FirstOrDefault(j => j.Usuario == usuario);

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
                    correo = jugador.Correo,
                    telefono = jugador.Telefono,
                    fechaNacimiento = jugador.FechaNacimiento,
                    puntos = jugador.Puntos,
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
        private bool validarDatosActualizados(JugadorDTO datos)
        {
            bool datosCorrectos = true;
            string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrWhiteSpace(datos.nombre) || datos.nombre.Length > 255)
            {
                datosCorrectos = false;
            }

            if (string.IsNullOrEmpty(datos.primerApellido) || datos.primerApellido.Length > 255)
            {
                datosCorrectos = false;
            }

            if (string.IsNullOrEmpty(datos.segundoApellido) || datos.segundoApellido.Length > 255)
            {
                datosCorrectos = false;
            }

            if (string.IsNullOrEmpty(datos.telefono) || datos.telefono.Length > 15)
            {
                datosCorrectos = false;
            }

            if (string.IsNullOrEmpty(datos.usuario) || datos.usuario.Length > 255) 
            {
                datosCorrectos = false;
            }

            if (datos.fechaNacimiento > DateTime.Now)
            {
                datosCorrectos = false;
            }

            if (string.IsNullOrEmpty(datos.correo) || !Regex.IsMatch(datos.correo, patronEmail) || datos.correo.Length > 255)
            {
                datosCorrectos = false;
            }
            return datosCorrectos;
        }
    }
    
}

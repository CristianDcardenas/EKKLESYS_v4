using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using ENTITY;
using static System.Collections.Specialized.BitVector32;

namespace BLL
{
    public class UsuarioService
    {
        private readonly UsuarioRepository usuarioRepository;
        private readonly string connectionString;

        public UsuarioService()
        {
            connectionString = "Data Source=localhost;Initial Catalog=IglesiaDB;Integrated Security=True";
            var connectionManager = new ConnectionManager();
            usuarioRepository = new UsuarioRepository(connectionManager);
        }

        public string Guardar(Usuario usuario)
        {
            try
            {
                // Validar que el email no exista
                var usuarioExistente = usuarioRepository.BuscarPorEmail(usuario.email);
                if (usuarioExistente != null)
                {
                    return $"Error al guardar: El email {usuario.email} ya está registrado";
                }

                usuarioRepository.Guardar(usuario);
                return $"Usuario {usuario.nombre} {usuario.apellido_paterno} guardado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al guardar: {ex.Message}";
            }
        }

        public string Modificar(Usuario usuario)
        {
            try
            {
                // Validar que el usuario exista
                var usuarioExistente = usuarioRepository.BuscarPorId(usuario.id_usuario);
                if (usuarioExistente == null)
                {
                    return $"Error al modificar: El usuario con ID {usuario.id_usuario} no existe";
                }

                // Validar que el email no esté en uso por otro usuario
                var usuarioPorEmail = usuarioRepository.BuscarPorEmail(usuario.email);
                if (usuarioPorEmail != null && usuarioPorEmail.id_usuario != usuario.id_usuario)
                {
                    return $"Error al modificar: El email {usuario.email} ya está en uso por otro usuario";
                }

                usuarioRepository.Modificar(usuario);
                return $"Usuario {usuario.nombre} {usuario.apellido_paterno} modificado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al modificar: {ex.Message}";
            }
        }

        public string CambiarPassword(int idUsuario, string passwordActual, string nuevoPassword)
        {
            try
            {
                // Verificar que la contraseña actual sea correcta
                var usuario = usuarioRepository.BuscarPorId(idUsuario);
                if (usuario == null)
                {
                    return "Error: Usuario no encontrado";
                }

                if (usuario.clave != passwordActual)
                {
                    return "Error: La contraseña actual es incorrecta";
                }

                // Cambiar la contraseña
                usuarioRepository.CambiarPassword(idUsuario, passwordActual, nuevoPassword);

                // Devolver el usuario actualizado para que el llamador pueda actualizar la sesión si es necesario
                return "Contraseña actualizada correctamente";
            }
            catch (Exception ex)
            {
                return $"Error al cambiar contraseña: {ex.Message}";
            }
        }

        public string Eliminar(int idUsuario)
        {
            try
            {
                // Validar que el usuario exista
                var usuario = usuarioRepository.BuscarPorId(idUsuario);
                if (usuario == null)
                {
                    return $"Error al eliminar: El usuario con ID {idUsuario} no existe";
                }

                usuarioRepository.Eliminar(idUsuario);
                return $"Usuario {usuario.nombre} {usuario.apellido_paterno} eliminado exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar: {ex.Message}";
            }
        }

        public Usuario BuscarPorId(int idUsuario)
        {
            return usuarioRepository.BuscarPorId(idUsuario);
        }

        public Usuario BuscarPorEmail(string email)
        {
            return usuarioRepository.BuscarPorEmail(email);
        }

        public Usuario Login(string email, string clave)
        {
            return usuarioRepository.Login(email, clave);
        }

        public List<Usuario> Consultar()
        {
            return usuarioRepository.ConsultarTodos();
        }

        public List<UsuarioDTO> ConsultarDTO()
        {
            var usuarios = usuarioRepository.ConsultarTodos();
            var usuariosDTO = new List<UsuarioDTO>();

            foreach (var usuario in usuarios)
            {
                usuariosDTO.Add(new UsuarioDTO
                {
                    id_usuario = usuario.id_usuario,
                    NombreCompleto = usuario.NombreCompleto,
                    email = usuario.email,
                    es_miembro = usuario.es_miembro == "S" ? "Sí" : "No",
                    es_administrador = usuario.es_administrador == "S" ? "Sí" : "No"
                });
            }

            return usuariosDTO;
        }

        public string CambiarRolAdministrador(int usuarioId, string nuevoEstado)
        {
            try
            {
                // Validar que el nuevo estado sea válido
                if (nuevoEstado != "S" && nuevoEstado != "N")
                {
                    return "Error: Estado no válido. Debe ser 'S' o 'N'";
                }

                // Validar que no sea el último administrador
                if (nuevoEstado == "N")
                {
                    // Verificar si es el último administrador
                    var usuarios = usuarioRepository.ConsultarTodos();
                    int contadorAdmins = 0;

                    foreach (var usuario in usuarios)
                    {
                        if (usuario.es_administrador == "S")
                        {
                            contadorAdmins++;
                        }
                    }

                    // Si solo hay un administrador y estamos intentando quitarle el rol
                    if (contadorAdmins == 1)
                    {
                        var usuarioActual = usuarioRepository.BuscarPorId(usuarioId);
                        if (usuarioActual != null && usuarioActual.es_administrador == "S")
                        {
                            return "Error: No se puede quitar el rol de administrador al último administrador del sistema";
                        }
                    }
                }

                // Actualizar el estado de administrador
                usuarioRepository.ActualizarEstadoAdministrador(usuarioId, nuevoEstado);

                return "Rol de administrador actualizado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error al cambiar rol de administrador: {ex.Message}";
            }
        }

        public string CambiarEstadoMiembro(int usuarioId, string nuevoEstado)
        {
            try
            {
                // Validar que el nuevo estado sea válido
                if (nuevoEstado != "S" && nuevoEstado != "N")
                {
                    return "Error: Estado no válido. Debe ser 'S' o 'N'";
                }

                // Actualizar el estado de miembro
                usuarioRepository.ActualizarEstadoMiembro(usuarioId, nuevoEstado);

                return "Estado de miembro actualizado correctamente";
            }
            catch (Exception ex)
            {
                return $"Error al cambiar estado de miembro: {ex.Message}";
            }
        }
    }
}

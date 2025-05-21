using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class UsuarioDTO
    {
        public int id_usuario { get; set; }
        public string NombreCompleto { get; set; }
        public string email { get; set; }
        public string es_miembro { get; set; }
        public string es_administrador { get; set; }
    }
}

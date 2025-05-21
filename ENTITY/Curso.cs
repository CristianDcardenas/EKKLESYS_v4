using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    public class Curso
    {
        public int id_curso { get; set; }
        public string nombre_curso { get; set; }
        public string descripcion_curso { get; set; }
        public DateTime fecha_inicio_curso { get; set; }
        public DateTime fecha_fin_curso { get; set; }
        public int capacidad_max_curso { get; set; }
        public List<Usuario> Estudiantes { get; set; }
        public string ruta_imagen_curso { get; set; }

        public Curso()
        {
            Estudiantes = new List<Usuario>();
        }

        public int NumeroInscritos
        {
            get { return Estudiantes?.Count ?? 0; }
        }

       
    }
}

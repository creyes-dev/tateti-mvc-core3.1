using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tateti.Extensiones
{
    public static class ColeccionesExtension
    {
        public static bool EstaNulaOVacia<T>(this IEnumerable<T> enumerableGenerico)
        {
            return (enumerableGenerico == null) || (!enumerableGenerico.Any());
        }

        public static bool EstaNulaOVacia<T>(this ICollection<T> coleccionGenerica)
        {
            if(coleccionGenerica == null)
            {
                return true;
            }
            return coleccionGenerica.Count < 1;
        }


    }
}

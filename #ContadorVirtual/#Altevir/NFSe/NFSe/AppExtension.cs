using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSe
{
    static class AppExtension
    {
        /// <summary>
        /// Pega parte do texto informado. (Substitui o uso do Substring)
        /// </summary>
        /// <param name="texto">Informe o Texto.</param>
        /// <param name="posInicial">Posição inicial (Indíce inicial pra pegar o texto).</param>
        /// <param name="numCaracteres">Número de caracteres à retornar.</param>
        /// <returns></returns>
        public static String GetSubstring(this String texto, int posInicial, int numCaracteres)
        {
            string resultado = string.Empty;
            char[] arr_caracteres = texto.ToCharArray();

            if (arr_caracteres.Length > 0)
            {
                for (int a = posInicial; a < arr_caracteres.Length; a++)
                {
                    numCaracteres--;
                    if (numCaracteres == -1) { break; }

                    resultado += arr_caracteres.GetValue(a).ToString();
                }
            }

            return resultado;
        }
    }
}

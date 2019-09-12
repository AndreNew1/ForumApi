using Model;
using System;
using Newtonsoft.Json;
using System.IO;

namespace Core.Util
{
    public class Arquivo
    {
        private static string NomeArquivo = $"{AppDomain.CurrentDomain.BaseDirectory}/Sistema.json";
        public static Sistema LerArquivo()
        {
            try
            {
                return JsonConvert.DeserializeObject<Sistema>(File.ReadAllText(NomeArquivo));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Sistema Escrita(Sistema sistema)
        {
            File.WriteAllText(NomeArquivo, JsonConvert.SerializeObject(sistema, Formatting.Indented));

            return sistema;
        }
    }
}

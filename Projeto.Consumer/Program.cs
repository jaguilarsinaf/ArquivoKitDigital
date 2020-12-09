using Projeto.Domain;
using Projeto.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Projeto.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Console.SetWindowSize(50, 100);
                Console.WriteLine();
                Console.WriteLine("############################################################");
                Console.WriteLine("##                Iniciando Processo do KIT               ##");
                Console.WriteLine("############################################################");
                Console.WriteLine();
                //args = new string[4];
                //args[0] = "Ampsoft";
                //args[1] = "41";
                //args[1] = "47";
                //args[1] = "47";                
                //args[4] = "49";                
                //args[5] = "51";
                //args[2] = "100";
                //args[7] = "123";
                //args[3] = "124";


                //var diretorio = Environment.CurrentDirectory + @"\ArquivoKitDigital";
                var diretorioAtual = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).ToString());
                var diretorio = diretorioAtual + @"\ArquivoKitDigital";

                if (!Directory.Exists(diretorio))
                {
                    Directory.CreateDirectory(diretorio);
                }
                string[] arquivos = Directory.GetFiles(diretorio);
                if (arquivos.Count() > 0)
                {
                    var subDiretorio = Path.Combine(diretorio, "Old");
                    if (!Directory.Exists(subDiretorio))
                    {
                        Directory.CreateDirectory(subDiretorio);
                    }
                    foreach(var item in arquivos)
                    {
                        File.Copy(item, Path.Combine(subDiretorio, Path.GetFileName(item)), true);
                        File.Delete(item);
                    }                    
                }
                var lista = new ConcurrentBag<Kitdigital>();
                string nomeArquivo = "KitDigital" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                if (args.Length > 0)
                {
                    string usuario = args[0];
                    var lstKit = new List<int>();
                    for (int i = 1; i < args.Length; i++)
                    {
                        lstKit.Add(Int32.Parse(args[i]));                       
                    }
                    var listaRetorno = new ProdutoDao().BuscarCertificado(lstKit, usuario, nomeArquivo);
                    if (listaRetorno.Count() > 0)
                    {
                        Parallel.ForEach(listaRetorno, item =>
                        {
                            lista.Add(item);
                        });
                        listaRetorno = null;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Criando Arquivo Json...");

                var path = Path.Combine(diretorio, nomeArquivo + ".json");

                //REMOVE BOM
                Encoding utf8WithoutBom = new UTF8Encoding(false);
                //FIM REMOVE BOM

                using (StreamWriter file = new StreamWriter(File.Open(path, FileMode.CreateNew), utf8WithoutBom))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, lista);
                }

                Console.WriteLine();
                Console.WriteLine("############################################################");
                Console.WriteLine("##                   FIM Processo do KIT                  ##");
                Console.WriteLine("############################################################");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

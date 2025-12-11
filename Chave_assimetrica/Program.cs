using System;
using System.Security.Cryptography;
using System.Text;

namespace Chave_assimetrica
{
    internal class Program
    {
        // Gerar chave privada (completa)
        public static RSAParameters GerarChavePrivada()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                return rsa.ExportParameters(true);
            }
        }

        public static RSAParameters GerarChavePublica(RSAParameters chavePrivada)
        {
            return new RSAParameters
            {
                Modulus = chavePrivada.Modulus,
                Exponent = chavePrivada.Exponent
            };
        }

        public static string CriptografarComChavePublica(string texto, RSAParameters chavePublica)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(chavePublica);

                byte[] dados = Encoding.UTF8.GetBytes(texto);
                byte[] cript = rsa.Encrypt(dados, false);

                return Convert.ToBase64String(cript);
            }
        }

        public static string DescriptografarComChavePrivada(string base64, RSAParameters chavePrivada)
        {
            byte[] dadosCript = Convert.FromBase64String(base64);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(chavePrivada);

                byte[] dados = rsa.Decrypt(dadosCript, false);
                return Encoding.UTF8.GetString(dados);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== Criptografia Assimétrica RSA ===");

            // Gera as chaves
            RSAParameters chavePrivada = GerarChavePrivada();
            RSAParameters chavePublica = GerarChavePublica(chavePrivada);

            Console.WriteLine("\nChave pública gerada e disponível para criptografia.");
            Console.WriteLine("Chave privada gerada e usada INTERNAMENTE (não exibida por segurança).");

            Console.Write("\nDigite o texto para criptografar: ");
            string texto = Console.ReadLine();

            string textoCriptografado = CriptografarComChavePublica(texto, chavePublica);

            Console.WriteLine("\n--- TEXTO CRIPTOGRAFADO ---");
            Console.WriteLine(textoCriptografado);

            Console.WriteLine("\nCopie o texto criptografado e pressione ENTER...");
            Console.ReadLine();

            Console.Write("\nCole o texto criptografado para descriptografar: ");
            string colado = Console.ReadLine();

            try
            {
                string textoDescriptografado = DescriptografarComChavePrivada(colado, chavePrivada);

                Console.WriteLine("\n--- TEXTO DESCRIPTOGRAFADO ---");
                Console.WriteLine(textoDescriptografado);
            }
            catch
            {
                Console.WriteLine("\nErro: o texto criptografado está inválido.");
            }

            Console.WriteLine("\nPressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}


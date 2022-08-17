using System;
using System.IO;
using System.Windows.Forms;

namespace ShellCodeConverter
{
    class Program
    {
        static byte[] Xor(byte[] inputByteArray, string keyString)
        {
            byte[] key = System.Text.Encoding.UTF8.GetBytes(keyString);
            byte[] data = new byte[inputByteArray.Length];

            for (int i = 0; i < inputByteArray.Length; i++)
            {
                data[i] = (byte)(inputByteArray[i] ^ key[i % key.Length]);
            }
            return data;
        }

        public static string ByteArrayToString(byte[] byteArray)
        {
            string shellCode = BitConverter.ToString(byteArray).Replace("-", ", 0x");

            string formattedShellCode = "0x" + shellCode;

            return formattedShellCode;
        }
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                string path = args[0].ToString();
                string key = args[1].ToString();

                byte[] fileBytes = File.ReadAllBytes(path);

                Console.WriteLine($"[*] Xor'ing shellcode from {path} with key {key}...");
                Console.WriteLine("[*] Hit enter to continue...");
                Console.ReadKey();

                byte[] xoredShellCode = Xor(fileBytes, key);

                Console.WriteLine("[*] Xor'ed shellcode:\n");
                Console.WriteLine(ByteArrayToString(xoredShellCode));

                Console.WriteLine("\n[*] Copyied shellcode to clipboard!");
                Clipboard.SetText(ByteArrayToString(xoredShellCode));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[!] File was not found. Supply a valid path arg.");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("[!] Supply at least two args. One for the .bin file and one for the xor key.");
                Console.WriteLine("[!] Example: ShellCodePrepper.exe evil.bin testing123");
            }
        }
    }
}

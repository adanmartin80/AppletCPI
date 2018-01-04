using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterCPI
{
    class Program
    {
        /// <summary>
        /// Instala/des-instala el Applet en el panel de control.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var title = args.ToList().Skip(1).Take(1).SingleOrDefault() ?? "Prueba";
            var guid = Guid.Parse(System.Configuration.ConfigurationManager.AppSettings["Guid"]);
            var reg = Registration.CreateInstanceRegForm<App.Form1>(guid, title);

            if (args.First() == "/i")
            {
                Install(reg);
            }
            else if (args.First() == "/u")
            {
                Uninstall(reg);
            }
            else
            {
                Uninstall(reg);
                Install(reg);
            }

            Console.WriteLine("\n\nPresione una tecla para cerrar la aplicación...");
            Console.ReadKey();
        }

        private static void Install(Registration reg)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n ************ Instalando... ************ \n");
                reg.Register_App();
                Console.WriteLine("\n ************ COMPLETADO!!! ************ \n");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error a la hora de instalar el Applet:");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\t{ex.Message.Replace("\r\n", "\n\t")}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\tTraza: ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{ex.StackTrace.Replace("\r\n   ", "\n\t\t  ")}");
                Console.ResetColor();
            }
        }

        private static void Uninstall(Registration reg)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" ************ Des-instalando... ************ \n");
                reg.Unregister_App();
                Console.WriteLine("\n ************ COMPLETADO!!! ************ \n");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error a la hora de des-instalar el Applet:");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"\t{ex.Message.Replace("\r\n", "\n\t")}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"\tTraza: ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{ex.StackTrace.Replace("\r\n   ", "\n\t\t  ")}");
                Console.ResetColor();
            }
        }
    }
}

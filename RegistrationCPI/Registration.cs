using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegisterCPI
{
    public class Registration
    {
        readonly string _localPath = string.Empty;
        readonly string _title = string.Empty;
        readonly Guid _id = Guid.Empty;
        readonly Type _type = default(Type);

        public static Registration CreateInstanceRegForm<App>(Guid id, string title)
            where App : Form
        {
            return new Registration(id, title, typeof(App));
        }

        public static Registration CreateInstanceRegWindow<App>(Guid id, string title)
            where App : System.Windows.Window
        {
            return new Registration(id, title, typeof(App));
        }

        private Registration(Guid id, string title, Type type)
           
        {
            _localPath = type.Assembly.Location;
            _title = title;
            _id = id;
            _type = type;
        }

        public void Register_App()
        {
            try
            {
                SetNameSpace();
                SetClassRoot();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Unregister_App()
        {
            var key = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ControlPanel\NameSpace\{{{_id}}}";
            try
            {
                Registry.LocalMachine.DeleteSubKeyTree(key);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\r\nLa clave usada es: {key}", ex);
            }
        }

        private void SetNameSpace()
        {
            var key = $@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ControlPanel\NameSpace\{{{_id}}}";
            try
            {
                var subKey = Registry.LocalMachine.CreateSubKey(key);
                subKey.SetValue("", _title);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\r\nLa clave usada es: {key}", ex);
            }
        }

        private void SetClassRoot()
        {
            var key = $@"CLSID\{{{_id}}}";
            try
            {
                var subKey = Registry.ClassesRoot.CreateSubKey(key);
                subKey.SetValue("", _title);
                subKey.SetValue("LocalizedString", _title);
                subKey.SetValue("InfoTip", $"Gestión del servicio Windows {System.IO.Path.GetFileNameWithoutExtension(_localPath)}.");
                subKey.SetValue("System.ApplicationName", _type.AssemblyQualifiedName);
                subKey.SetValue("System.ControlPanel.Category", "1,8");
                //key.SetValue("System.Software.TasksFileUrl", _title);
                SetIcon(subKey);
                SetCommand(subKey);

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\r\nLa clave usada es: {key}", ex);
            }
        }

        private void SetCommand(RegistryKey key)
        {
            var subKey = key.CreateSubKey("DefaultIcon");
            subKey.SetValue("", $"{_localPath}");
        }

        private void SetIcon(RegistryKey key)
        {
            var subKey = key.CreateSubKey(@"Shell\Open\Command");
            subKey.SetValue("", $"{_localPath}");
        }
    }
}

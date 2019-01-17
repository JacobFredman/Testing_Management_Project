using System;
using System.Windows;
using System.Windows.Media;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     Load data and set theme
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var bl = FactoryBl.GetObject;
            SetTheme(Configuration.Color, Configuration.Theme);

            base.OnStartup(e);
        }

        /// <summary>
        ///     Set theme for app
        /// </summary>
        /// <param name="newColor">New color</param>
        /// <param name="newLight">New Theme</param>
        /// <param name="oldColor">Old color</param>
        /// <param name="oldLight">Old theme</param>
        /// <param name="startup">if it is startup</param>
        public static void SetTheme(string newColor, string newLight, string oldColor = "Blue",
            string oldLight = "Light", bool startup = true)
        {
            //set background color
            if (newLight == "Light")
                switch (newColor)
                {
                    case "Red":
                        Current.Resources["Background"] = Brushes.LightCoral;
                        break;
                    case "Green":
                        Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Blue":
                        Current.Resources["Background"] = Brushes.LightSkyBlue;
                        break;
                    case "Purple":
                        Current.Resources["Background"] = Brushes.MediumPurple;
                        break;
                    case "Orange":
                        Current.Resources["Background"] = Brushes.Orange;
                        break;
                    case "Lime":
                        Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Emerald":
                        Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Teal":
                        Current.Resources["Background"] = Brushes.PaleTurquoise;
                        break;
                    case "Cyan":
                        Current.Resources["Background"] = Brushes.LightCyan;
                        break;
                    case "Cobalt":
                        Current.Resources["Background"] = Brushes.CornflowerBlue;
                        break;
                    case "Indigo":
                        Current.Resources["Background"] = Brushes.IndianRed;
                        break;
                    case "Violet":
                        Current.Resources["Background"] = Brushes.PaleVioletRed;
                        break;
                    case "Pink":
                        Current.Resources["Background"] = Brushes.Pink;
                        break;
                    case "Magenta":
                        Current.Resources["Background"] = Brushes.MediumOrchid;
                        break;
                    case "Crimson":
                        Current.Resources["Background"] = Brushes.LightCoral;
                        break;
                    case "Sienna":
                        Current.Resources["Background"] = Brushes.SandyBrown;
                        break;
                    case "Taupe":
                        Current.Resources["Background"] = Brushes.Tan;
                        break;
                    case "Mauve":
                        Current.Resources["Background"] = Brushes.AntiqueWhite;
                        break;
                    case "Steel":
                        Current.Resources["Background"] = Brushes.LightSteelBlue;
                        break;
                    case "Olive":
                        Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Brown":
                        Current.Resources["Background"] = Brushes.SandyBrown;
                        break;
                    case "Yellow":
                        Current.Resources["Background"] = Brushes.LightGoldenrodYellow;
                        break;
                    case "Amber":
                        Current.Resources["Background"] = Brushes.LightGoldenrodYellow;
                        break;
                }
            else
                Current.Resources["Background"] = Brushes.Black;

            //change the directory
            var dictUri =
                new Uri(
                    @"pack://application:,,,/MahApps.Metro;component/Styles/Themes/" + newLight + "." + newColor +
                    ".xaml", UriKind.RelativeOrAbsolute);
            var removeDictUri =
                new Uri(
                    @"pack://application:,,,/MahApps.Metro;component/Styles/Themes/" + oldLight + "." + oldColor +
                    ".xaml", UriKind.RelativeOrAbsolute);
            var removeResourceDict = new ResourceDictionary {Source = removeDictUri};
            var resourceDict = new ResourceDictionary {Source = dictUri};
            if (!startup)
                Current.Resources.MergedDictionaries.Remove(removeResourceDict);
            Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
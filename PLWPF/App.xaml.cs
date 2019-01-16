using System;
using System.Windows;
using System.Windows.Media;
using BE;

namespace PLWPF
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Load data and set theme
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var bl=BL.FactoryBl.GetObject;
            SetTheme(Configuration.Color,Configuration.Theme);
          
            base.OnStartup(e);
        }

        /// <summary>
        /// Set theme for app
        /// </summary>
        /// <param name="newColor">New color</param>
        /// <param name="newLight">New Theme</param>
        /// <param name="oldColor">Old color</param>
        /// <param name="oldLight">Old theme</param>
        /// <param name="startup">if it is startup</param>
        public static void SetTheme(string newColor, string newLight ,string oldColor="Blue",string oldLight="Light",bool startup=true)
        {
            //set background color
            if (newLight == "Light")
            {
                //on light theme
                switch (newColor)
                {
                    case "Red":
                        Application.Current.Resources["Background"] = Brushes.LightCoral;
                        break;
                    case "Green":
                        Application.Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Blue":
                        Application.Current.Resources["Background"] = Brushes.LightSkyBlue;
                        break;
                    case "Purple":
                        Application.Current.Resources["Background"] = Brushes.MediumPurple;
                        break;
                    case "Orange":
                        Application.Current.Resources["Background"] = Brushes.Orange;
                        break;
                    case "Lime":
                        Application.Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Emerald":
                        Application.Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Teal":
                        Application.Current.Resources["Background"] = Brushes.PaleTurquoise;
                        break;
                    case "Cyan":
                        Application.Current.Resources["Background"] = Brushes.LightCyan;
                        break;
                    case "Cobalt":
                        Application.Current.Resources["Background"] = Brushes.CornflowerBlue;
                        break;
                    case "Indigo":
                        Application.Current.Resources["Background"] = Brushes.IndianRed;
                        break;
                    case "Violet":
                        Application.Current.Resources["Background"] = Brushes.PaleVioletRed;
                        break;
                    case "Pink":
                        Application.Current.Resources["Background"] = Brushes.Pink;
                        break;
                    case "Magenta":
                        Application.Current.Resources["Background"] = Brushes.MediumOrchid;
                        break;
                    case "Crimson":
                        Application.Current.Resources["Background"] = Brushes.LightCoral;
                        break;
                    case "Sienna":
                        Application.Current.Resources["Background"] = Brushes.SandyBrown;
                        break;
                    case "Taupe":
                        Application.Current.Resources["Background"] = Brushes.Tan;
                        break;
                    case "Mauve":
                        Application.Current.Resources["Background"] = Brushes.AntiqueWhite;
                        break;
                    case "Steel":
                        Application.Current.Resources["Background"] = Brushes.LightSteelBlue;
                        break;
                    case "Olive":
                        Application.Current.Resources["Background"] = Brushes.LightGreen;
                        break;
                    case "Brown":
                        Application.Current.Resources["Background"] = Brushes.SandyBrown;
                        break;
                    case "Yellow":
                        Application.Current.Resources["Background"] = Brushes.LightGoldenrodYellow;
                        break;
                    case "Amber":
                        Application.Current.Resources["Background"] = Brushes.LightGoldenrodYellow;
                        break;
                }
            }
            else
            {
                //on black theme
                Application.Current.Resources["Background"] = Brushes.Black;
            }

            //change the directory
            var dictUri = new Uri(@"pack://application:,,,/MahApps.Metro;component/Styles/Themes/"+newLight+"."+newColor+".xaml", UriKind.RelativeOrAbsolute);
            var removeDictUri = new Uri(@"pack://application:,,,/MahApps.Metro;component/Styles/Themes/"+oldLight+"."+oldColor+".xaml", UriKind.RelativeOrAbsolute);
            var removeResourceDict = new ResourceDictionary() { Source = removeDictUri };
            var resourceDict = new ResourceDictionary() { Source = dictUri };
            if(!startup)
                 Application.Current.Resources.MergedDictionaries.Remove(removeResourceDict);
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
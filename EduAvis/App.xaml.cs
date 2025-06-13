using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace EduAvis
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            // Set application-wide culture to English (United States)
            CultureInfo englishCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = englishCulture;
            Thread.CurrentThread.CurrentUICulture = englishCulture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(englishCulture.IetfLanguageTag)));

            base.OnStartup(e);
        }
    }

}

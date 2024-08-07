using Xamarin.Forms;
using SQLite;
using AppNotas.Dependencies;
using AppNotas.Views;
using Telerik.XamarinForms.RichTextEditor;
using System.Threading.Tasks;

namespace AppNotas
{
    public partial class App : Application
    {

        public App ()
        {
            // Pop the display alert at the start
            new RadRichTextEditor();

            InitializeComponent();

            DependencyService.Register<SQLiteDefaultConnection>();

            MemoryManager mm = new MemoryManager();
            DependencyService.RegisterSingleton(mm);

            MainPage = new AppShell();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }

        public void note()
        {
        }
    }
}


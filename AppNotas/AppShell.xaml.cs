using AppNotas.Views;
using Xamarin.Forms;
using AppNotas.Dependencies;
using System.Threading.Tasks;
using Telerik.XamarinForms.RichTextEditor;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace AppNotas
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public SQLiteDefaultConnection database;

        public AppShell()
        {
            Routing.RegisterRoute(nameof(NoteDetailPage), typeof(NoteDetailPage));
            Routing.RegisterRoute(nameof(SectionsPage), typeof(SectionsPage));
            Routing.RegisterRoute(nameof(MusicPlayer), typeof(MusicPlayer));
            Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));


            InitializeComponent();

            Database.Database.runMigration();
        }
    }
}


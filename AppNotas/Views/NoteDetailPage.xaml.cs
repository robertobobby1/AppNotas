using System.ComponentModel;
using Xamarin.Forms;
using Telerik.XamarinForms.RichTextEditor;
using AppNotas.ViewModels;
using AppNotas.Database;
using AppNotas.Behaviours;

namespace AppNotas.Views
{
    public partial class NoteDetailPage : ContentPage
    {
        NoteDetailViewModel _viewModel;

        public NoteDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NoteDetailViewModel();

            _viewModel.editor = this.richTextEditor;
            this.richTextEditor.Behaviors.Add(new PickImageBehavior());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            save();
        }

        private async void save()
        {
            _viewModel.saveNoteContent(
                await this.richTextEditor.GetHtmlAsync()
                );
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}

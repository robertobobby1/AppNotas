using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppNotas.Models;
using SQLiteNetExtensions.Extensions;
using Telerik.XamarinForms.RichTextEditor;
using Xamarin.Forms;

namespace AppNotas.ViewModels
{
    [QueryProperty(nameof(NoteId), nameof(NoteId))]
    public class NoteDetailViewModel : BaseViewModel
    {
        private string text;
        public string Text {get => text; set => SetProperty(ref text, value); }

        private string noteId;
        public string NoteId { get { return noteId; } set { noteId = value; LoadNoteId(value); } }

        public Note note;
        public string getNoteContent() { return note == null ? "" : note.text; }
        public RadRichTextEditor editor { get; set; }

        public void LoadNoteId(string noteId)
        {
            try
            {
                if (!Int32.TryParse(noteId, out int j))
                    return;

                note = Database.GetWithChildren<Note>(j);
                if (note == null)
                    return;

                Title = note.name;
                editor.Source = RichTextSource.FromString(note.text);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Note");
            }
        }

        public void saveNoteContent(string htmlContent)
        {
            note.text = htmlContent;
            Database.Update(note);
        }
    }
}


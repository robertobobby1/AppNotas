using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

using AppNotas.Models;
using AppNotas.Views;
using AppNotas.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;

using SQLiteNetExtensions.Extensions;
using System.Threading.Tasks;
using AppNotas.Utils;
using System.Linq;

namespace AppNotas.ViewModels
{
    public class SectionsViewModel : BaseViewModel
    {
        // SECTION BINDINGS
        private List<Section> navigation;
        public List<Section> Navigation { set => SetProperty(ref navigation, value); get => navigation; }
        private ImageButton editImageButton;
        public ImageButton EditImageButton { set => SetProperty(ref editImageButton, value); get => editImageButton; }
        private string entryTextColor;
        public string EntryTextColor { get => entryTextColor; set => SetProperty(ref entryTextColor, value); }


        // COMMANDS
        public ICommand NavigateBack => new Command(PerformNavigateBack);
        public ICommand AddSection => new Command(AddSectionCommand);
        public ICommand DeleteAction => new Command<INamableAndOrderable>(_deleteAction);
        public ICommand EditAction => new Command<object>(_editAction);
        public ICommand ThreeLineAction => new Command(AddSectionCommand);
        public ICommand NavigateHome => new Command(_navigateHome);
        public ICommand LoadSectionsCommand => new Command(ExecuteLoadItemsCommand);
        public ICommand Tapped => new Command<INamableAndOrderable>(OnItemSelected); 
        public ICommand Dropped => new Command<INamableAndOrderable>(dropAction);
        public ICommand Dragged => new Command<INamableAndOrderable>(dragAction);
        public ICommand EndDragged => new Command<INamableAndOrderable>(endDragAction);
        public ICommand NavTapped => new Command<Section>(navigateToSection);

        // POP UP BINDINGS
        private string popupTitle;
        public string PopupTitle { get => popupTitle; set => SetProperty(ref popupTitle, value); }
        private string inputText;
        public string InputText { get => inputText; set => SetProperty(ref inputText, value); }
        private string popUpPickerSelection;
        public string PopUpPickerSelection { get => popUpPickerSelection; set => SetProperty(ref popUpPickerSelection, value); }
        private List<string> pickerList;
        public List<string> PickerList { get => pickerList; set => SetProperty(ref pickerList, value); }
        private string errorMessage;
        public string ErrorMessage { get => errorMessage; set => SetProperty(ref errorMessage, value); }

        private Section _selectedSection;
        public ObservableCollection<INamableAndOrderable> Sections { get; }
        private bool reloadingImage = false;
        private INamableAndOrderable activeDragItem;
        public Edit activeEdit;

        public Section SelectedSection
        {
            get => _selectedSection;
            set
            {
                SetProperty(ref _selectedSection, value);
                OnItemSelected(value);
            }
        }

        /*************************************************************************
         * 
         *                      INITIALIZERS
         * 
         *************************************************************************/

        public struct Edit
        {
            public Edit(ImageButton ib, INamableAndOrderable no)
            {
                imageButton = ib;
                namable = no;
                isNull = false;
            }

            public ImageButton imageButton;
            public INamableAndOrderable namable;
            public bool isNull;
        }

        public SectionsViewModel()
        {
            Title = "Browse";
            Sections = new ObservableCollection<INamableAndOrderable>();
            PickerList = new List<string> { "Section" };
            activeDragItem = null;

            activeEdit = new Edit();
            activeEdit.isNull = true;

            // Initialize memory synchronously to show correctly
            Memory.loadSectionsSync();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            _selectedSection = null;
        }

        public void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                if (Memory.FatherSection == null)
                {
                    setCollection(Memory.Sections);
                    setNavigation(null, true);
                }
                else
                    OnItemSelected(Memory.FatherSection); 
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            finally { IsBusy = false; }
        }

        /*************************************************************************
         * 
         *                      SELECTED ELEMENT FUNCTION
         * 
         *************************************************************************/

        async void OnItemSelected<T>(T NameAndOrderable) where T : INamableAndOrderable
        {
            if (reloadingImage)
            {
                reloadingImage = false;
                toggleEdit();
                return;
            }

            if (NameAndOrderable == null)
                return;

            if (NameAndOrderable.GetType() == typeof(Note))
            {
                Note note = Casts.getNote(NameAndOrderable);
                await Shell.Current.GoToAsync($"{nameof(NoteDetailPage)}?{nameof(NoteDetailViewModel.NoteId)}={note.Id}");
            }
            else
            {
                Section section = Casts.getSection(NameAndOrderable);
                if (section.isFinal)
                    setCollection(section.Notes);
                else
                    setCollection(section.Sections);

                setNavigation(section.Id);
            }
        }

        /*************************************************************************
         * 
         *                      COLLECTION SETTERS 
         * 
         *************************************************************************/

        private void setCollection<T>(List<T> collection) where T : INamableAndOrderable
        {
            if (collection.Exists((section) => { return section.order == 0; }))
                collection = defaultOrderValues(collection);

            // sort numerically ascendant 
            collection.Sort((x, y) => { return x.order.CompareTo(y.order); });
            Sections.Clear();

            collection.ForEach((section) => {
                if (section.GetType() == typeof(Section))
                {
                    Section aux = Casts.getSection(section);
                    Sections.Add(aux);
                }
                else
                {
                    Note aux = Casts.getNote(section);
                    Sections.Add(aux);
                }
            });
        }
         
        private List<T> defaultOrderValues<T>(List<T> collection) where T : INamableAndOrderable
        {
            var max = 0;

            // find max
            collection.ForEach((item) => {
                if (item.order <= max)
                    return;
                max = item.order;
            });

            // if values with order 0 => should default to max value(the end)
            collection.ForEach((item) => {
                if (item.order != 0)
                    return;

                max += 1;
                item.order = max;
                Database.Update(item);
            });

            return collection;
        }

        /*************************************************************************
         * 
         *                           NAVIGATION 
         * 
         *************************************************************************/

        private void _navigateHome()
        {
            setNavigation(null, true);
            setCollection(Memory.Sections);
        }

        public void setNavigation(int? sectionId, bool isHome = false)
        {
            if (isHome)
                Memory.navigateHome();
            else
                Memory.navigateToSection(sectionId);

            Array array = Memory.Navigation.ToArray();
            Array.Reverse(array);

            Navigation = new List<Section>((IEnumerable<Section>)array);
        }

        public void PerformNavigateBack()
        {
            if (Memory.FatherSection == null)
                return;
            setNavigation(null);
            if (Memory.FatherSection == null)
                setCollection(Memory.Sections);
            else
                setCollection(Memory.FatherSection.Sections);
        }

        private void navigateToSection(Section section)
        {
            while (Memory.FatherSection != null
                && Memory.FatherSection.Id != section.Id)
            {
                PerformNavigateBack();
            }
        }

        /*************************************************************************
         * 
         *                           CRUD OPERATIONS 
         * 
         *************************************************************************/

        private void _editAction(object parameters)
        {
            ImageButton imageButton = (ImageButton)((object[])parameters)[0];
            INamableAndOrderable namable = (INamableAndOrderable)((object[])parameters)[1];

            if (!activeEdit.isNull && activeEdit.namable != namable)
                toggleEdit();

            activeEdit = new Edit(imageButton, namable);
            toggleEdit();
        }

        private void toggleEdit()
        {
            if (activeEdit.isNull)
                return;

            Entry entry = getEntry(activeEdit.imageButton);
            if (!entry.IsEnabled)
            {
                reloadingImage = true;
                activeEdit.imageButton.Source = "correct.png";
                entry.IsEnabled = true;
                entry.TextColor = Color.Black;
                entry.Focus();
            }
            else
            {
                reloadingImage = false;
                activeEdit.imageButton.Source = "pencil.png";
                activeEdit.isNull = true;
                entry.IsEnabled = false;
                entry.TextColor = Color.LightGray;

                Database.Update(activeEdit.namable);
            }
        }

        private Entry getEntry(ImageButton imageButton)
        {
            Grid grid = (Grid)imageButton.Parent;
            foreach (Element element in grid.Children)
            {
                if (element.GetType() == typeof(Entry))
                    return (Entry)element;
            }
            return null;
        }

        private async void _deleteAction<T>(T NameAndOrderable) where T : INamableAndOrderable
        {
            bool res = await App.Current.MainPage.DisplayAlert(
                "Are you sure you want to delete this section", "All the sections and notes inside will be deleted", "Yes", "No"
            );
            if (!res)
                return;

            Database.Delete(NameAndOrderable, true);

            if (Memory.FatherSection != null)
            {
                if (NameAndOrderable.GetType() == typeof(Section))
                    Memory.FatherSection.Sections.Remove(Casts.getSection(NameAndOrderable));
                else
                {
                    Memory.FatherSection.Notes.Remove(Casts.getNote(NameAndOrderable));
                    // if notes is empty this section is no longer final
                    if (Memory.FatherSection.Notes.Count == 0)
                    {
                        Memory.FatherSection.isFinal = false;
                        Database.Update(Memory.FatherSection);
                    }
                }
            }
            Memory.loadSectionsSync();
            ExecuteLoadItemsCommand();
        }

        private void dragAction(INamableAndOrderable namable) { activeDragItem = namable; }
        private void endDragAction(INamableAndOrderable namable) { activeDragItem = null; } 
        private void dropAction(INamableAndOrderable NameAndOrderable)
        {
            Sections.Remove(NameAndOrderable);
            Sections.Remove(activeDragItem);

            (NameAndOrderable.order, activeDragItem.order) = (activeDragItem.order, NameAndOrderable.order);

            Database.Update(NameAndOrderable);
            Database.Update(activeDragItem);

            Sections.Add(NameAndOrderable);
            Sections.Add(activeDragItem);

            setCollection(Sections.ToList());
        }

        /*************************************************************************
         * 
         *                           ADD POP UP SECTION 
         * 
         *************************************************************************/

        private async void AddSectionCommand()
        {
            pickerList.Clear();
            pickerList.Add("Section");

            if (Memory.FatherSection == null)
                PopupTitle = "Add a section";
            else if (Memory.FatherSection.isFinal)
            {
                pickerList.Add("Note");
                PopupTitle = "Add a section or a note";
            }
            else
                PopupTitle = "Add a section";

            AddNotesPopUp popUp = new AddNotesPopUp(this);
            object res = await App.Current.MainPage.Navigation.ShowPopupAsync(popUp);

        }

        public async Task<bool> checkInput()
        {
            if (PopUpPickerSelection == null)
            {
                ErrorMessage = "Select note or section is mandatory";
                return false;
            }
            if (InputText == null)
            {
                ErrorMessage = "The input text cannot be empty";
                return false;
            }
            // Trying to create a section on a final section, must wrap up existing notes
            var res = false;
            if (PopUpPickerSelection == "Section")
                res = await checkAndAddSectionPicked();
            else if (PopUpPickerSelection == "Note")
                res = checkAndAddNotePicked();

            if (res)
                resetTexts();

            return res;
        }

        private bool checkAndAddNotePicked()
        {
            Note note = new Note(InputText);

            Database.Insert(note);

            Memory.FatherSection.Notes.Add(note);
            Database.UpdateWithChildren(Memory.FatherSection);
            setCollection(Memory.FatherSection.Notes);

            return true;
        }

        private async Task<bool> checkAndAddSectionPicked()
        {
            Section section = new Section();
            // Initial section only more sections can be added
            if (Memory.FatherSection == null)
            {
                section.name = InputText;
                Database.Insert(section);

                Memory.Sections.Add(section);

                Memory.reloadSectionsAsync();
                setCollection(Memory.Sections);

                return true;
            }
            // Final sections
            else if (Memory.FatherSection != null && Memory.FatherSection.isFinal)
            {
                // Final section already has notes 
                if (Memory.FatherSection.Notes.Count > 0)
                {
                    // Should we wrap up the notes or delete them
                    bool answer = await App.Current.MainPage.DisplayAlert("This section has notes", "Would you like to wrap up the notes in a new section?", "Yes", "No");
                    if (answer)
                    {
                        // WRAP UP IN THE NEW SECTION
                        section.name = InputText;
                        Database.Insert(section);

                        foreach (Note note in Memory.FatherSection.Notes)
                        {
                            note.SectionId = section.Id;
                            section.Notes.Add(note);
                        }

                        Database.UpdateWithChildren(section);
                        
                    }
                    // Delete existing notes
                    else
                    {
                        answer = await App.Current.MainPage.DisplayAlert("This action will delete this notes", "Are you sure?", "Yes", "No");
                        // Last warning, we delete the notes
                        if (!answer)
                            return false;

                        foreach (Note note in Memory.FatherSection.Notes)
                            Database.Delete(note);

                        section.name = InputText;
                        Database.Insert(section);
                    }
                }
                // Empty notes and sections
                else
                {
                    section.name = InputText;
                    Database.Insert(section);
                }
                // On the three cases(wrap up notes, delete notes and non-existing notes)
                //          => update memory father section, memory seections and persist in database relationship
                Memory.FatherSection.Sections.Add(section);
                Memory.FatherSection.Notes.Clear();
                Memory.FatherSection.isFinal = false;
                Database.UpdateWithChildren(Memory.FatherSection);

                Memory.reloadSectionsAsync();
                setCollection(Memory.FatherSection.Sections);

                return true;
            }
            // middle sections only sections can be added
            else
            {
                section.name = InputText;
                Database.Insert(section);

                Memory.FatherSection.Sections.Add(section);
                Database.UpdateWithChildren(Memory.FatherSection);
                setCollection(Memory.FatherSection.Sections);

                Memory.reloadSectionsAsync();

                return true;
            }
        } 

        public void resetTexts()
        {
            InputText = "";
            errorMessage = "";
        }
    }
}

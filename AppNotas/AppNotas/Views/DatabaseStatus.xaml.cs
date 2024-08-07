using System;
using System.Collections.Generic;
using AppNotas.ViewModels;
using Xamarin.Forms;
using AppNotas.Database;
using AppNotas.Dependencies;

namespace AppNotas.Views
{	
	public partial class DatabaseStatus : ContentPage
	{
        public DatabaseStatusViewModel _viewModel;

        public DatabaseStatus ()
		{
			InitializeComponent ();

			BindingContext = _viewModel = new DatabaseStatusViewModel();
		}

        void RunMigration(System.Object sender, System.EventArgs e)
        {
            switch (_viewModel.PopUpPickerSelection)
            {
                case "restart":
                    Database.Database.CurrentMigrationStatus = Database.Database.MigrationStatus.RESTART;
                    break;
                case "restart and seed":
                    Database.Database.CurrentMigrationStatus = Database.Database.MigrationStatus.RESTARTANDSEED;
                    break;
                case "restore":
                    Database.Database.CurrentMigrationStatus = Database.Database.MigrationStatus.RESTORE;
                    break;
                case "do nothing":
                    return;
            }
            Database.Database.runMigration();
            _viewModel.setStatus();

            MemoryManager Memory = DependencyService.Get<MemoryManager>();

            Memory.reloadSectionsAsync();
            Memory.navigateHome();
        }
    }
}


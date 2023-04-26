using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppNotas.Database;

namespace AppNotas.ViewModels
{
	public class DatabaseStatusViewModel : BaseViewModel
	{
		public DatabaseStatusViewModel()
        {
            setStatus();
            PickerList = new List<string>
            {
                "do nothing",
                "restart",
                "restart and seed",
                "restore"
            };
            PopUpPickerSelection = "";
        }

        private string status;
        public string Status { get => status; set => SetProperty(ref status, value); }

        private string popUpPickerSelection; 
        public string PopUpPickerSelection { get => popUpPickerSelection; set => SetProperty(ref popUpPickerSelection, value); }

        private List<string> pickerList; 
        public List<string> PickerList { get => pickerList; set => SetProperty(ref pickerList, value); }

        public void setStatus()
        {
            switch (AppNotas.Database.Database.CurrentMigrationStatus)
            {
                case (AppNotas.Database.Database.MigrationStatus.NONE):
                    Status = "none";
                    break;
                case (AppNotas.Database.Database.MigrationStatus.MIGRATED):
                    Status = "migrated";
                    break;
                case (AppNotas.Database.Database.MigrationStatus.MIGRATEDANDSEEDED):
                    Status = "migrated and seeded";
                    break;
                case (AppNotas.Database.Database.MigrationStatus.RESTORED):
                    Status = "restored";
                    break;
            }

        }
    }
}
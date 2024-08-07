using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AppNotas.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;

using Xamarin.Forms;

namespace AppNotas.Views.Popups
{	
	public partial class AddNotesPopUp : Popup
    {
        SectionsViewModel _viewModel;

        public AddNotesPopUp(SectionsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        void CancelClicked(object sender, System.EventArgs e)
        {
            _viewModel.resetTexts();
            Dismiss("Close button tapped");
        }

        async void AddClicked(object sender, System.EventArgs e)
        {
            if (await _viewModel.checkInput())
            {
                Dismiss("");
                _viewModel.InputText = "";
            }
        }
        protected override object GetLightDismissResult()
        {
            _viewModel.resetTexts();
            return base.GetLightDismissResult();
        }
    }
}


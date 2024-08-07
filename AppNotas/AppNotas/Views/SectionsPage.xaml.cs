using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppNotas.Utils;
using AppNotas.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppNotas.Views
{
    public partial class SectionsPage : ContentPage
    {
        SectionsViewModel _viewModel;

        public SectionsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SectionsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            _viewModel.PerformNavigateBack();
            return true;
        }
    }
}


using System;
using System.Collections.Generic;
using AppNotas.ViewModels;
using Xamarin.Forms;

namespace AppNotas.Views
{	
	public partial class MusicPlayer : ContentPage
	{
		MusicPlayerViewModel _viewModel;

		public MusicPlayer ()
		{
			InitializeComponent ();

			BindingContext = _viewModel = new MusicPlayerViewModel();
        }
	}
}


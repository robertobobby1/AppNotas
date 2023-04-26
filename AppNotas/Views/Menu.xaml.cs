using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace AppNotas.Views
{	
	public partial class Menu : Grid
	{
        /*************************************************************************
         * 
         *                      BINDABLE PROPERTIES SECTION
         * 
         *************************************************************************/

        // BINDABLE VALUES
        public bool IsArrow { get { return (bool)GetValue(IsArrowProperty); } set { SetValue(IsArrowProperty, value); } }
        public Command LeftCommand { get { return (Command)GetValue(LeftCommandProperty); } set { SetValue(LeftCommandProperty, value); } }
        public Command RightCommand { get { return (Command)GetValue(RightCommandProperty); } set { SetValue(RightCommandProperty, value); } }

        // BINDABLE PROPERTIES
        public static readonly BindableProperty IsArrowProperty = BindableProperty.Create(
            nameof(IsArrow), typeof(bool), typeof(Menu), false, propertyChanged: IsArrowPropertyChanged
        );
        public static readonly BindableProperty LeftCommandProperty = BindableProperty.Create(
            nameof(LeftCommand), typeof(ICommand), typeof(Menu), null, propertyChanged: LeftCommandPropertyChanged
        );
        public static readonly BindableProperty RightCommandProperty = BindableProperty.Create(
            nameof(RightCommand), typeof(ICommand), typeof(Menu), null, propertyChanged: RightCommandPropertyChanged
        );

        // BINDABLE PROPERTY CHANGED
        private static void IsArrowPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((Menu)bindable).setImage();
        }

        private static void LeftCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((Menu)bindable).leftIcon.Command = (Command)newValue;
        }

        private static void RightCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((Menu)bindable).rightIcon.Command = (Command)newValue;
        }

        /*************************************************************************
         * 
         *                      MENU CONSTRUCTOR SECTION
         * 
         *************************************************************************/

        public ICommand DefaultLeftIconCommand => new Command(leftIconClicked);
        public ICommand DefaultRightIconCommand => new Command(rightIconClicked);

        public Menu ()
		{
			InitializeComponent ();

            this.leftIcon.Command = DefaultLeftIconCommand;
            this.rightIcon.Command = DefaultLeftIconCommand;

            setImage();
		}

        public void setImage()
        {
            if (IsArrow)
                image.Source = "back.png";
            else
                image.Source = "menu.png";
        }

        public void leftIconClicked()
        {
            if (IsArrow)
                Shell.Current.SendBackButtonPressed();
            else
                Shell.Current.FlyoutIsPresented = !Shell.Current.FlyoutIsPresented;
        }

        public static void rightIconClicked()
        {

        }
    }
}


using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Windows.Shell;

namespace CustomChromeLibrary
{
	/// <summary>
	/// Interaction logic for CaptionButtons.xaml
	/// </summary>
	public partial class CaptionButtons : UserControl
	{
		public CaptionButtons()
		{
			InitializeComponent();
		}

		private void SpecialMinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			SystemCommands.MinimizeWindow(Window.GetWindow(this));
		}

		private void SpecialCloseButton_Click(object sender, RoutedEventArgs e)
		{
			SystemCommands.CloseWindow(Window.GetWindow(this));
		}
	}
}

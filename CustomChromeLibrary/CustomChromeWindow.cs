using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace CustomChromeLibrary
{
	public class CustomChromeWindow: Window, INotifyPropertyChanged
	{

		protected override void OnStateChanged(EventArgs e)
		{
			base.OnStateChanged(e);
			OnPropertyChanged("CaptionButtonMargin");
		}

		public Thickness CaptionButtonMargin
		{
			get
			{
				if (WindowState == System.Windows.WindowState.Maximized)
					return new Thickness(6, 6, 0, 0); //Margin="0,0,12,0"
				else
					return new Thickness(0, 0, 0, 0);
			}
		}

		#region INotifyPropertyChanged
		private void OnPropertyChanged(String info)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(info));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
}

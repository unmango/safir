using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Windows.Shell;

namespace CustomChromeLibrary
{
	public class MaximizeButton : CaptionButton, INotifyPropertyChanged
	{
		static MaximizeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MaximizeButton), new FrameworkPropertyMetadata(typeof(MaximizeButton)));
		}

		public MaximizeButton()
		{
			DataContext = this;
		}

		protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			Window w = Window.GetWindow(this);
			if (w != null)
			{
				w.StateChanged += new EventHandler(w_StateChanged);
			}
		}

		void w_StateChanged(object sender, EventArgs e)
		{
			OnPropertyChanged("MaximizeIconVisibility");
			OnPropertyChanged("RestoreIconVisibility");
		}

		protected override void OnClick()
		{
			base.OnClick();
			Window w = Window.GetWindow(this);
			if (w.WindowState == System.Windows.WindowState.Maximized)
				SystemCommands.RestoreWindow(w);
			else
				SystemCommands.MaximizeWindow(w);
		}

		public Visibility MaximizeIconVisibility
		{
			get 
			{
				Window w = Window.GetWindow(this);
				return w.WindowState == System.Windows.WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible; 
			}
		}

		public Visibility RestoreIconVisibility
		{
			get 
			{
				Window w = Window.GetWindow(this);
				return w.WindowState != System.Windows.WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible; 
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

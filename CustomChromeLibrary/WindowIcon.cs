using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Shell;

namespace CustomChromeLibrary
{
	public class WindowIcon : ContentControl
	{
		static WindowIcon()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowIcon), new FrameworkPropertyMetadata(typeof(WindowIcon)));
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			Window w = Window.GetWindow(this);
			if (e.ClickCount == 1)
			{
				Point p;
				if (e.ChangedButton == MouseButton.Left)
				{
					p = this.PointToScreen(e.GetPosition(this));
					p.X += 1;
					p.Y += 1;
				}
				else
				{
					p = this.PointToScreen(e.GetPosition(this));
					p.X += 1;
					p.Y += 1;
				}
				SystemCommands.ShowSystemMenu(w, p);
			}
			if (e.ClickCount == 2 && e.ChangedButton == MouseButton.Left)
				w.Close();
		}
	}
}

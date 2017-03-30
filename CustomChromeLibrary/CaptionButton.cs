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
using System.ComponentModel;

namespace CustomChromeLibrary
{
	public class CaptionButton : Button
	{
		static CaptionButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CaptionButton), new FrameworkPropertyMetadata(typeof(CaptionButton)));
		}

		public CornerRadius CornerRadius
		{
			get { return (CornerRadius)GetValue(CornerRadiusProperty); }
			set { SetValue(CornerRadiusProperty, value); }
		}

		public static readonly DependencyProperty CornerRadiusProperty =
			DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CaptionButton), new UIPropertyMetadata(new CornerRadius()));



		public Brush HoverBackground
		{
			get { return (Brush)GetValue(HoverBackgroundProperty); }
			set { SetValue(HoverBackgroundProperty, value); }
		}

		public static readonly DependencyProperty HoverBackgroundProperty =
			DependencyProperty.Register("HoverBackground", typeof(Brush), typeof(CaptionButton), new UIPropertyMetadata(new SolidColorBrush(Colors.LightBlue)));



		public Brush PressedBackground
		{
			get { return (Brush)GetValue(PressedBackgroundProperty); }
			set { SetValue(PressedBackgroundProperty, value); }
		}

		public static readonly DependencyProperty PressedBackgroundProperty =
			DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(CaptionButton), new UIPropertyMetadata(new SolidColorBrush(Colors.LightBlue)));

	}
}

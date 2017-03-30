using System;
using System.Windows;
using Microsoft.Windows.Shell;

namespace CustomChromeLibrary
{
	public class HelpButton : CaptionButton
    {
        static HelpButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HelpButton), new FrameworkPropertyMetadata(typeof(HelpButton)));
        }
    }
}

using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EAPS
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static void ChangeDarkLight(bool isLightTheme)
		{
			if (isLightTheme)
				ThemeManager.Current.ChangeTheme(Current, "Light.Blue");
			else
				ThemeManager.Current.ChangeTheme(Current, "Dark.Blue");
		}
	}
}

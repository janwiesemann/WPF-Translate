# Welcome to WPF-Translate
WPF-Translate is a samal and simple Application to help you developing multi lingual applications.
This project was created as a part of my job quallification as an 'Computer Science Expert Subject Area: Software Development'.
WPF has multiple ways of creating a localised application. One of the easiest ways is it to use ResourceDicionarys. Just define all your string in a Dictionary and you are pretty much done. This is easy for one language but if you want to use multiple languages things get a bitt messy. This is where WPF-Translate helps you. Simply load all your files and start edeting.

# Features

 - Easy editing of multiple ResourceDictionarys
 - Easy to use Interface
 - Connection to Google Translate (Experimental)
 - Opem source

# Getting started
Just download it, unpack it and run it.
Open some files or create some. Thats is!

# Screenshots
![enter image description here](https://raw.githubusercontent.com/Jan18101997/WPF-Translate/master/screenshots/MainWindow.png)

# Help bringing this application forward
Some plans for the future

 - Add more supported files
   If you want to help just create another FileReader and FileWriter. Add a new class in IO and implement the interface `IResourceFileReader`. You also need to create a second class implementing `IResourceFileWriter`. Thats it! You don't need to add it somewhere else. WPF-Translate is able to find those files automatically.
 - Adding more UI Langauges
   Use WPF-Translate to add a new Resource file in `Resources/Langs/Strings_xx.xaml` WPF-Translate will load it automatically on startup.

# Implementation Example
So you've just created some Resource files. Don't worry. This is easy.
You can fine all this code used in WPF-Translate ([App.xaml.cs in LoadCultureStrings](https://raw.githubusercontent.com/Jan18101997/WPF-Translate/master/src/WPF-Translate/App.xaml.cs)

1. Add make source your Resources are included as Page (VS Project explorer -> Right click on your file -> Properties -> Propertypanel -> Build Action -> Set it to `Page`)
2. Add your main ResourceFile (Default language) to your `App.xaml` Resources
    Example: 
	```<Application
		x:Class="de.LandauSoftware.WPFTranslate.App"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:Controls="clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro"
		xmlns:local="clr-namespace:de.LandauSoftware.WPFTranslate"
		ShutdownMode="OnMainWindowClose"
		StartupUri="GUI/MainWindow.xaml">
			<Application.Resources>
				<ResourceDictionary>
					<ResourceDictionary.MergedDictionaries>
						<ResourceDictionary Source="pack://application:,,,/WPF-Translate;component/Resources/Langs/Strings_en.xaml" />
					</ResourceDictionary.MergedDictionaries>
				</ResourceDictionary>
			</Application.Resources>
		</Application>

3. Use your defined strings by using `DynamicResource` or `StaticResource`
    Example: 
    ```<MenuItem Header="{DynamicResource open}" InputGestureText="{DynamicResource keysCtrlO}" />```
4. Add some logic to detect the current language and load the right dictionary. You can add this in your App.xaml.cs by overriding `OnStartup()`. You can add this wherever you want to have it.
    Example:
    ```
    protected override void OnStartup(StartupEventArgs e)
	{
		CultureInfo current = CultureInfo.CurrentCulture;
		string name = current.Name;
		
		//this block is used to remove the second part of a LangKey i.e. ‘en-us’ will become ‘en’ this depends on your naming style. WPF-Translate itself does not care if it is US or GB English. You can change this however you want.
		int i = name.IndexOf('-');
		if (i > 0)
			name = name.Substring(0, i);
			
		if (name == "en") //default and its already added in App.xaml
			return;
			
		try
		{
			//Loading resources
			ResourceDictionary rd = new ResourceDictionary()
			{
				Source = new Uri($"pack://application:,,,/WPF-Translate;component/Resources/Langs/Strings_{name}.xaml", UriKind.RelativeOrAbsolute)
			};
			Resources.MergedDictionaries.Add(rd);
		}
		catch (Exception) //not found it’s the easiest way to test, if it exists.
		{ }
	
		base.OnStartup(e);
	}
5. Done!

# License
it's simple. I don't care what you are doing with this. Just don't tell people its yours and give me at least some credit.
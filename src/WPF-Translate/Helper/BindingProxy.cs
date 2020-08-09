using System.Windows;

namespace de.LandauSoftware.WPFTranslate
{
    /// <summary>
    /// Binind Proxy
    ///
    /// Example at https://code.4noobz.net/wpf-mvvm-proxy-binding/
    /// </summary>
    internal class BindingProxy : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
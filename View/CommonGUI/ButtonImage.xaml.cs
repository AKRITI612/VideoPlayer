using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VideoPlayerApplication.View.CommonGUI
{
    /// <summary>
    /// Interaction logic for ButtonImage.xaml
    /// </summary>
    public partial class ButtonImage : UserControl
    {
        public ButtonImage()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Image Source Property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(string), typeof(ButtonImage),new PropertyMetadata(null));

        /// <summary>
        /// Content property
        /// </summary>
        public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register("ContentText", typeof(string), typeof(ButtonImage),new PropertyMetadata(null));

        /// <summary>
        /// Button Command property
        /// </summary>
        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register("ButtonCommand", typeof(ICommand), typeof(ButtonImage), new PropertyMetadata(null));

        /// <summary>
        ///  Button Command Property
        /// </summary>
        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        /// <summary>
        /// Source property
        /// </summary>
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        /// <summary>
        /// ContentText property
        /// </summary>
        public string ContentText
        {
            get => (string)GetValue(ContentTextProperty);
            set => SetValue(ContentTextProperty, value);
        }

    }
}

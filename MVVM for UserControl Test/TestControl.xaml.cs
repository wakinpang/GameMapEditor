using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_for_UserControl_Test
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class TestControl : UserControl
    {
        public String MyText
        {
            get { return (String)GetValue(MyTextProperty); }
            set {
                if(value == (String)GetValue(MyTextProperty))
                {
                    return;
                }
                SetValue(MyTextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyTextProperty =
            DependencyProperty.Register("MyText", typeof(String), typeof(TestControl), new PropertyMetadata("", OnMyTextChanged));

        private static void OnMyTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TestControl;
            //Messenger.Default.Send<String>(control.MyText, MessageTokens.MyTextChangedFromView);
        }

        public TestControl()
        {
            InitializeComponent();

            Messenger.Default.Register<String>(this, MessageTokens.MyTextChangedFromViewModel, (msg) =>
            {
                MyText = msg;
            });
        }
    }
}

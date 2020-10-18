using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Admandev.Switch
{
    public partial class SwitchBox : UserControl
    {
        //Delegate
        public delegate void CheckedChangedDelegate(bool isChecked);

        //Events
        public event CheckedChangedDelegate CheckedChanged;

        //Variables
        private bool allowMoving = false;
        private bool moved = false;

        //Properties
        #region Properties
        public static DependencyProperty CheckedBackgroundProperty = DependencyProperty.Register(nameof(CheckedBackground), typeof(Brush), typeof(SwitchBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 120, 215))));
        public Brush CheckedBackground
        {
            get => (Brush)GetValue(CheckedBackgroundProperty);
            set => SetValue(CheckedBackgroundProperty, value);
        }

        public static DependencyProperty UncheckedBackgroundProperty = DependencyProperty.Register(nameof(UncheckedBackground), typeof(Brush), typeof(SwitchBox), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));
        public Brush UncheckedBackground
        {
            get => (Brush)GetValue(UncheckedBackgroundProperty);
            set => SetValue(UncheckedBackgroundProperty, value);
        }

        public static DependencyProperty SwitchBackgroundProperty = DependencyProperty.Register(nameof(SwitchBackground), typeof(Brush), typeof(SwitchBox), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public Brush SwitchBackground
        {
            get => (Brush)GetValue(SwitchBackgroundProperty);
            set => SetValue(SwitchBackgroundProperty, value);
        }

        public static DependencyProperty RectBorderProperty = DependencyProperty.Register(nameof(RectBorder), typeof(Brush), typeof(SwitchBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public Brush RectBorder
        {
            get => (Brush)GetValue(RectBorderProperty);
            set => SetValue(RectBorderProperty, value);
        }

        public static DependencyProperty SwitchBorderProperty = DependencyProperty.Register(nameof(SwitchBorder), typeof(Brush), typeof(SwitchBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        public Brush SwitchBorder
        {
            get => (Brush)GetValue(SwitchBorderProperty);
            set => SetValue(SwitchBorderProperty, value);
        }

        public static DependencyProperty CheckedProperty = DependencyProperty.Register(nameof(Checked), typeof(bool), typeof(SwitchBox), new PropertyMetadata(false, CheckedPropUpdate));
        public bool Checked
        {
            get => (bool)GetValue(CheckedProperty);
            set
            {
                SetValue(CheckedProperty, value);
                CheckedChanged?.Invoke(value);
            }
        }

        private bool AllowMoving { get => allowMoving; set => allowMoving = value; }

        #endregion

        //Constructor
        public SwitchBox()
        {
            InitializeComponent();
        }

        private void SwitchBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateSwitchButtonPos(this.Checked);
        }

        private void SwitchValue()
        {
            Checked = !Checked;
        }

        private static void CheckedPropUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SwitchBox sw = (SwitchBox)d;
            sw.UpdateSwitchButtonPos((bool)e.NewValue);
        }

        #region Switch button moving
        private void SetCheckedWithButtonPosition()
        {
            if (this.SwitchButton.RenderTransform.Value.OffsetX < (this.Width - this.SwitchButton.Width) / 2)
            {
                Checked = false;
            }
            else
            {
                Checked = true;
            }

            UpdateSwitchButtonPos(this.Checked);
        }

        private void UpdateSwitchButtonPos(bool isChecked)
        {
            if (isChecked)
            {
                this.SwitchButton.RenderTransform = new TranslateTransform() { X = this.Width - this.SwitchButton.Width };
            }
            else
            {
                this.SwitchButton.RenderTransform = new TranslateTransform() { X = 0 };
            }
        }

        private void SwitchButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AllowMoving = false;

            if (!moved)
            {
                SwitchValue();
            }
            else
            {
                SetCheckedWithButtonPosition();
            }
            moved = false;
        }

        private void Rect_Background_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SwitchValue();
            moved = false;
        }

        private void SwitchButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            allowMoving = true;
        }

        private void SwitchButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (AllowMoving)
            {
                AllowMoving = false;
                SetCheckedWithButtonPosition();
            }
            moved = false;
        }

        private void SwitchButton_MouseMove(object sender, MouseEventArgs e)
        {
            if (!AllowMoving) return;

            moved = true;

            TranslateTransform transform = new TranslateTransform
            {
                X = ClampSwitchButtonPosition(Mouse.GetPosition(this).X - this.SwitchButton.Width / 2)
            };
            
            this.SwitchButton.RenderTransform = transform;

        }

        private double ClampSwitchButtonPosition(double xPos)
        {
            if (xPos < 0)
                return 0;
            if (xPos > this.Width - this.SwitchButton.Width)
                return this.Width- this.SwitchButton.Width;
            return xPos;
        }

        #endregion

    }
}

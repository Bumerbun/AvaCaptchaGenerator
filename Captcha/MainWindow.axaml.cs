using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace Captcha
{
    public partial class MainWindow : Window
    {
        private CaptchaGenerator captchaGenerator = new CaptchaGenerator();
        public MainWindow()
        {
            InitializeComponent();

            UpdateCaptchaButton.Click += UpdateCaptchaButton_Click;
            UpdateCaptcha();
        }

        private void UpdateCaptcha()
        {
            captchaGenerator.GenerateCaptcha();
            CaptchaImage.Source = captchaGenerator.CaptchaImage;
        }

        private void UpdateCaptchaButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            UpdateCaptcha();
        }
    }

}
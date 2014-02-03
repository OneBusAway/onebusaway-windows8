using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace OneBusAway.Controls
{
    public partial class FlyoutContainerControl : UserControl
    {
        public static readonly DependencyProperty IsShowingFlyoutProperty = DependencyProperty.Register("IsShowingFlyout", 
            typeof(bool), 
            typeof(FlyoutContainerControl), 
            new PropertyMetadata(false));

        /// <summary>
        /// This animates the flyin animation.
        /// </summary>
        private Storyboard flyInAnimation;

        /// <summary>
        /// This animates the flyout animation.
        /// </summary>
        private Storyboard flyOutAnimation;

        public FlyoutContainerControl()
        {
            InitializeComponent();
            this.flyInAnimation = (Storyboard)this.Resources["flyInAnimation"];
            this.flyOutAnimation = (Storyboard)this.Resources["flyOutAnimation"];
        }
        
        public bool IsShowingFlyout
        {
            get { return (bool)GetValue(IsShowingFlyoutProperty); }
            set { SetValue(IsShowingFlyoutProperty, value); }
        }

        /// <summary>
        /// Animates the fly in animation.
        /// </summary>
        public async Task AnimateFlyInAsync()
        {
            await this.flyInAnimation.WaitForStoryboardToFinishAsync();
            this.IsShowingFlyout = true;
        }

        public async Task AnimateFlyOutAsync()
        {
            await this.flyOutAnimation.WaitForStoryboardToFinishAsync();
            this.IsShowingFlyout = false;
        }

        /// <summary>
        /// Sets the size of the flyout control.
        /// </summary>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = e.NewSize.Width;
            double h = e.NewSize.Height;

            this.flyInAnimationKeyFrame.Value = w - 380;
            this.flyOutAnimationKeyFrame.Value = w;
            this.contentControl.SetValue(Canvas.LeftProperty, w);
            this.contentControl.Width = w;
            this.contentControl.Height = h;

            this.canvas.Width = w;
            this.canvas.Height = h;
            this.dismissGrid.Width = w;
            this.dismissGrid.Height = h;
        }

        /// <summary>
        /// When the back button is tapped, close the flyout.
        /// </summary>
        private async void OnBackButtonTapped(object sender, GestureEventArgs e)
        {
            await this.AnimateFlyOutAsync();
        }
    }
}

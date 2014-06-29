
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SkinSafeApp.ViewControllers
{
    public partial class EnvironmentViewController : UIViewController
    {
        public EnvironmentViewController() : base("EnvironmentViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
			
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            IndexImage.Image = UIImage.FromFile("Images/big_sun.png");
            IndexCollection.BackgroundColor = UIColor.Clear;
        }
    }
}


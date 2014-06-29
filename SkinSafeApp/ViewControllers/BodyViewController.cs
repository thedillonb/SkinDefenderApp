using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Dialog;

namespace SkinSafeApp.ViewControllers
{
    public class BodyViewController : DialogViewController
    {
        private UIImageView _bodyImage;
        private UITapGestureRecognizer _tapGesture;

        public event Action<string> AreaSelected;

        protected virtual void OnAreaSelected(string area)
        {
            var handler = AreaSelected;
            if (handler != null)
                handler(area);
        }

        public BodyViewController()
            : base(UITableViewStyle.Plain, new RootElement("Where?"))
        {
        }

        public override void ViewDidLoad()
        {
            Title = "Where?";

            base.ViewDidLoad();

//            View.BackgroundColor = UIColor.White;
//
//            _bodyImage = new UIImageView(new RectangleF(0, 64f, View.Bounds.Width, View.Bounds.Height - 64f));
//            _bodyImage.UserInteractionEnabled = true;
//            _bodyImage.Image = UIImage.FromFile("Images/body_outline.jpg");
//            _bodyImage.AutoresizingMask = UIViewAutoresizing.All;
//            View.Add(_bodyImage);
//
//            _tapGesture = new UITapGestureRecognizer();
//            _tapGesture.NumberOfTapsRequired = 1;
//            _tapGesture.AddTarget(ImageTapped);
//            _bodyImage.AddGestureRecognizer(_tapGesture);

            var areas = new [] { "Face", "Neck", "Chest", "Back", "Left Arm", "Right Arm", "Left Leg", "Right Leg", "Abdomen" };

            var sec = new Section();
            foreach (var a in areas)
            {
                var scopedA = a;
                var el = new StyledStringElement(a, () => OnAreaSelected(scopedA));
                el.Accessory = UITableViewCellAccessory.DisclosureIndicator;
                sec.Add(el);
            }

            Root.Add(sec);
        }
    }
}


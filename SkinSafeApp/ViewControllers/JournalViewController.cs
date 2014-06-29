using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using SkinSafeApp.Data;
using MonoTouch.Foundation;
using BigTed;
using System.Linq;
using MonoTouch.AddressBook;
using System.Drawing;

namespace SkinSafeApp.ViewControllers
{
    public class JournalViewController : AreasViewController
    {
        private AreasViewController _areaViewController;

        public JournalViewController()
        {
            Title = "Journal";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

//            View.BackgroundColor = UIColor.White;
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s, e) => AddNew());
//
//            _areaViewController = new AreasViewController();
//            AddChildViewController(_areaViewController);
//            _areaViewController.LoadView();
//            _areaViewController.CollectionView.Frame = View.Bounds;
//            _areaViewController.CollectionView.AutoresizingMask = UIViewAutoresizing.All;
//            View.Add(_areaViewController.CollectionView);
        }

        private void AddNew()
        {
            var bodyController = new BodyViewController();
            bodyController.NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (s, e) => DismissViewController(true, null));
            bodyController.AreaSelected += (obj) => 
            {
                var picker = new UIImagePickerController();
                picker.SourceType = UIImagePickerControllerSourceType.Camera;
                //picker.MediaTypes = UIImagePickerController.AvailableMediaTypes (UIImagePickerControllerSourceType.Camera);
                picker.ShowsCameraControls = true;
                picker.AllowsEditing = false;
                picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                picker.Canceled += (sender, e) => bodyController.DismissViewController(true, null);
                picker.FinishedPickingMedia += (sender, e) =>
                {
                    CreateNewAreaWithImage(obj, e.OriginalImage);
                    DismissViewController(true, null);
                };

                bodyController.PresentViewController (picker, true, null);
            };

            PresentViewController(new UINavigationController(bodyController), true, null);
        }

        private void CreateNewAreaWithImage(string areaName, UIImage img)
        {
            NSError error;
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine (documentsDirectory, Guid.NewGuid().ToString("N") + ".jpg");

            if (!img.AsJPEG(0.9f).Save(path, false, out error))
            {
                Console.WriteLine("Unable to save: " + error);
                return;
            }

            var scaledImage = img.Scale(new SizeF(200, 200));
            var thumbPath = System.IO.Path.Combine (documentsDirectory, Guid.NewGuid().ToString("N") + ".jpg");
            if (!scaledImage.AsJPEG(0.8f).Save(thumbPath, false, out error))
            {
                Console.WriteLine("Unable to save thumbnail: " + error);
            }

            var area = new Area { Location = areaName };
            Database.Instance.Insert(area);

            var sample = new Sample { AreaId = area.Id, Date = DateTime.Now, ImagePath = path, ThumbnailPath = thumbPath };
            Database.Instance.Insert(sample);

            BTProgressHUD.ShowSuccessWithStatus("Saved!");
        }
    }
}


using System;
using MonoTouch.UIKit;
using System.Drawing;
using SkinSafeApp.Data;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.Foundation;
using BigTed;

namespace SkinSafeApp.ViewControllers
{
    public class AreaViewController : UIViewController
    {
        private UIScrollView _scrollView;
        private List<Sample> _samples;
        private Area _area;

        public AreaViewController(Area area)
        {
            _area = area;
            _samples = Database.Instance.GetSamples().Where(x => x.AreaId == area.Id).OrderByDescending(x => x.Date).ToList();

            NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s_, e_) =>
            {
                var picker = new UIImagePickerController();
                picker.SourceType = UIImagePickerControllerSourceType.Camera;
                picker.ShowsCameraControls = true;
                picker.AllowsEditing = false;
                picker.CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo;
                picker.Canceled += (sender, e) => DismissViewController(true, null);
                picker.FinishedPickingMedia += (sender, e) =>
                {
                    CreateNewAreaWithImage(e.OriginalImage);
                    _samples = Database.Instance.GetSamples().Where(x => x.AreaId == area.Id).OrderByDescending(x => x.Date).ToList();
                    RefreshImages();
                    DismissViewController(true, null);
                };

                PresentViewController(picker, true, null);
            });
        }

        private void CreateNewAreaWithImage(UIImage img)
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

            var sample = new Sample { AreaId = _area.Id, Date = DateTime.Now, ImagePath = path, ThumbnailPath = thumbPath };
            Database.Instance.Insert(sample);

            BTProgressHUD.ShowSuccessWithStatus("Saved!");
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            EdgesForExtendedLayout = UIRectEdge.None;

            View.BackgroundColor = UIColor.White;

            RefreshImages();
        }

        private void RefreshImages()
        {
            if (_scrollView != null)
            {
                _scrollView.RemoveFromSuperview();
                _scrollView.Dispose();
            }

            _scrollView = new UIScrollView(new RectangleF(0, 0, View.Bounds.Width, View.Bounds.Height));
            _scrollView.BackgroundColor = UIColor.Clear;
            _scrollView.PagingEnabled = true;
            Add(_scrollView);

            _scrollView.ContentSize = new SizeF(_samples.Count * _scrollView.Bounds.Width, _scrollView.Bounds.Height);
            for (var i = 0; i < _samples.Count; i++)
            {
                var imgView = new UIImageView(new RectangleF(i * _scrollView.Bounds.Width, 0, _scrollView.Bounds.Width, _scrollView.Bounds.Height));
                imgView.Image = UIImage.FromFile(_samples[i].ImagePath);
                _scrollView.Add(imgView);
            }
        }
    }
}


using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;
using System.Drawing;
using SkinSafeApp.Data;
using System.Linq;

namespace SkinSafeApp.ViewControllers
{
    public class AreasViewController : UICollectionViewController
    {
        private readonly AreaSource _areaSource = new AreaSource();

        private static UICollectionViewFlowLayout _flow = new UICollectionViewFlowLayout
        {
            ItemSize = new SizeF(145, 180),
            ScrollDirection = UICollectionViewScrollDirection.Vertical,
            MinimumInteritemSpacing = 10f,
            MinimumLineSpacing = 10f,
            SectionInset = new UIEdgeInsets(10, 10, 10, 10)

        };

        public AreasViewController()
            : base(_flow)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
            CollectionView.BackgroundColor = UIColor.White;

            var areaSource = _areaSource;
            areaSource.FontSize = 11f;

            CollectionView.RegisterClassForCell(typeof(UserCell), UserCell.CellID);
            CollectionView.ShowsHorizontalScrollIndicator = false;
            CollectionView.Source = areaSource;

            CollectionView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _areaSource.Rows.Clear();
            foreach (var area in Database.Instance.GetAreas().ToList())
            {
                var scopedArea = area;
                var first = Database.Instance.GetSamples().Where(x => x.AreaId == scopedArea.Id).OrderByDescending(x => x.Date).FirstOrDefault();
                string image = null;

                if (first != null)
                {
                    if (!string.IsNullOrEmpty(first.ThumbnailPath))
                        image = first.ThumbnailPath;
                    else
                        image = first.ImagePath;
                }

                _areaSource.Rows.Add(new AreaElement(area.Location, image, () => 
                {
                    NavigationController.PushViewController(new AreaViewController(scopedArea), true);
                }));
            }

            CollectionView.ReloadData();
        }


        public class UserCell : UICollectionViewCell
        {
            public static NSString CellID = new NSString("UserSource");

            [Export("initWithFrame:")]
            public UserCell(RectangleF frame)
                : base(frame)
            {
                BackgroundView = new UIView { BackgroundColor = UIColor.FromWhiteAlpha(0.9f, 1.0f) };


                ImageView = new UIImageView();
                ImageView.Layer.CornerRadius = 3f;
                ImageView.Layer.MasksToBounds = true;
                ImageView.ContentMode = UIViewContentMode.ScaleAspectFill;

                ContentView.AddSubview(ImageView);

                LabelView = new UILabel();
                LabelView.BackgroundColor = UIColor.Clear;
                LabelView.TextColor = UIColor.DarkGray;
                LabelView.TextAlignment = UITextAlignment.Center;

                ContentView.AddSubview(LabelView);
            }

            public UIImageView ImageView { get; private set; }

            public UILabel LabelView { get; private set; }

            public void UpdateRow(AreaElement element, Single fontSize)
            {
                LabelView.Text = element.Caption;

                if (!string.IsNullOrEmpty(element.Image))
                    ImageView.Image = UIImage.FromFile(element.Image);
                else
                    ImageView.Image = null;

                LabelView.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

                ImageView.Frame = new RectangleF(4, 4, ContentView.Bounds.Width - 8f, ContentView.Bounds.Width - 8f);
                LabelView.Frame = new RectangleF(4, ImageView.Frame.Bottom, ContentView.Bounds.Width - 8f,
                    ContentView.Frame.Height - ImageView.Frame.Bottom);
            }
        }


        public class AreaElement
        {
            public AreaElement(String caption, string image, NSAction tapped)
            {
                Caption = caption;
                Image = image;
                Tapped = tapped;
            }

            public String Caption { get; set; }

            public string Image { get; set; }

            public NSAction Tapped { get; set; }
        }

        public class AreaSource : UICollectionViewSource
        {
            public AreaSource()
            {
                Rows = new List<AreaElement>();
            }

            public List<AreaElement> Rows { get; private set; }

            public Single FontSize { get; set; }

            public override Int32 NumberOfSections(UICollectionView collectionView)
            {
                return 1;
            }

            public override Int32 GetItemsCount(UICollectionView collectionView, Int32 section)
            {
                return Rows.Count;
            }

            public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
            {
                return true;
            }



            public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var cell = (UserCell) collectionView.CellForItem(indexPath);
                cell.ImageView.Alpha = 0.5f;
            }

            public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var cell = (UserCell) collectionView.CellForItem(indexPath);
                cell.ImageView.Alpha = 1;

                AreaElement row = Rows[indexPath.Row];
                row.Tapped.Invoke();
            }

            public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
            {
                var cell = (UserCell) collectionView.DequeueReusableCell(UserCell.CellID, indexPath);

                AreaElement row = Rows[indexPath.Row];

                cell.UpdateRow(row, FontSize);

                return cell;
            }
        }
    }
}



using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SkinSafeApp.TableViewCells
{
    public partial class ArticleTableViewCell : UITableViewCell
    {
        public static readonly UINib Nib = UINib.FromName("ArticleTableViewCell", NSBundle.MainBundle);
        public static readonly NSString Key = new NSString("ArticleTableViewCell");

        public ArticleTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public UIImage Image
        {
            get { return ImageView.Image; }
            set { ImageView.Image = value; }
        }

        public string Title
        {
            get { return TitleLabel.Text; }
            set { TitleLabel.Text = value; }
        }

        public string Description
        {
            get { return DescriptionLabel.Text; }
            set { DescriptionLabel.Text = value; }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            this.TitleLabel.PreferredMaxLayoutWidth = this.TitleLabel.Frame.Width;
            this.DescriptionLabel.PreferredMaxLayoutWidth = this.DescriptionLabel.Frame.Width;

            this.ContentView.SetNeedsLayout();
            this.ContentView.LayoutIfNeeded();
        }

        public static ArticleTableViewCell Create()
        {
            var cell = (ArticleTableViewCell)Nib.Instantiate(null, null)[0];
            cell.ImageView.Layer.MasksToBounds = true;
            cell.ImageView.Layer.CornerRadius = 6f;
            cell.SeparatorInset = new UIEdgeInsets(0, 88f, 0, 0);
            return cell;
        }
    }
}


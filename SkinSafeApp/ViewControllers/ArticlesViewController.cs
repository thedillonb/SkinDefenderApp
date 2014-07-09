using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using SkinSafeApp.TableViewCells;
using MonoTouch.Dialog.Utilities;

namespace SkinSafeApp.ViewControllers
{
    public class ArticlesViewController : DialogViewController
    {
        private readonly Article[] Articles = {
            new Article 
            { 
                Title = "Sun Saftey for Beachgoers",
                Description = "Summer may be fading fast, but the sun isn't going anywhere. People heading to the beach this August should take precautions.",
                ImageUrl = "http://www.skincancer.org/Media/Default/Page/prevention/sun-protection/guidelines/beachfamily-160w.jpg",
                Url = "http://www.skincancer.org/prevention/sun-protection/prevention-guidelines/sun-safety-tips-for-beachgoers"
            },
            new Article 
            { 
                Title = "Year-Round Sun Protection",
                Description = "The summer is not the only time you are at risk for damage from the sun. Find out how to protect yourself no matter what the season. ",
                ImageUrl = "http://www.skincancer.org/Media/Default/Page/news/greenleaves.jpg",
                Url = "http://www.skincancer.org/prevention/sun-protection/prevention-guidelines/year-round-sun-protection"
            },
            new Article 
            { 
                Title = "Understanding UVA and UVB",
                Description = "For a six billion-year-old star, the sun is certainly in the news a lot lately, mainly because it is still a source of uncertainty and confusion to many of us.",
                ImageUrl = "http://www.skincancer.org/media/legacy/stories/UVA_UVB/handandsun3.jpg",
                Url = "http://www.skincancer.org/prevention/uva-and-uvb/understanding-uva-and-uvb"
            },
            new Article 
            { 
                Title = "Shining Light on Ultraviolet Radition",
                Description = "Ultraviolet radiation is composed of three wavelengths: UVA, UVB and UVC.  While UVC isn't a concern for skin cancer, UVA and UVB play different roles when it comes to tanning, burning, and photoaging.",
                ImageUrl = "http://www.skincancer.org/media/legacy/stories/HealthySkin/sunsmart_living/sun.jpg",
                Url = "http://www.skincancer.org/prevention/uva-and-uvb/shining-light-on-ultraviolet-radiation"
            },
            new Article 
            { 
                Title = "Sun Safety In Cars",
                Description = "For most people, car safety means seatbelts and airbags. But there's another important way to stay out of harm's way on the road, and that's by protecting your skin from the sun.",
                ImageUrl = "http://www.skincancer.org/Media/Default/Page/prevention/sun-protection/guidelines/Family-Car.jpg",
                Url = "http://www.skincancer.org/prevention/sun-protection/shade/sun-safety-cars"
            },
            new Article 
            { 
                Title = "Golfers: You've Got Skin in the Game",
                Description = "Sunburn is a major cause of skin cancer, and every hour, golfers can receive up to five times the amount of sun exposure needed to cause sunburn.",
                ImageUrl = "http://www.skincancer.org/Media/Default/Page/men-golfing.jpg",
                Url = "http://www.skincancer.org/healthy-lifestyle/outdoor-activities/golf"
            }
        };

        public ArticlesViewController()
            : base(UITableViewStyle.Plain, null)
        {
            Title = "Protection";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var sec = new Section();
            foreach (var a in Articles)
                sec.Add(new ArticleElement(a));

            var root = new RootElement(Title) { UnevenRows = true };
            root.Add(sec);
            Root = root;
        }

        private class Article
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
            public string Url { get; set; }
        }

        private class ArticleElement : Element, IElementSizing, IImageUpdated
        {
            private readonly Article _article;
            private UIImage _articleImage;

            public ArticleElement(Article article)
                : base(article.Title)
            {
                _article = article;
                _articleImage = ImageLoader.DefaultRequestImage(new Uri(article.ImageUrl), this);
            }

            public override UITableViewCell GetCell(UITableView tv)
            {
                var cell = tv.DequeueReusableCell(ArticleTableViewCell.Key) as ArticleTableViewCell ?? ArticleTableViewCell.Create();
                cell.Title = _article.Title;
                cell.Description = _article.Description;
                cell.Image = _articleImage;
                return cell;
            }

            public void UpdatedImage(Uri uri)
            {
                var img = ImageLoader.DefaultRequestImage(uri, this);
                if (img != null)
                {
                    _articleImage = img;
                    var root = GetImmediateRootElement();
                    root.Reload(this, UITableViewRowAnimation.None);
                }
            }

            public float GetHeight(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(ArticleTableViewCell.Key) as ArticleTableViewCell ?? ArticleTableViewCell.Create();
                cell.Title = _article.Title;
                cell.Description = _article.Description;
                cell.Image = _articleImage;
                cell.SetNeedsUpdateConstraints();
                cell.UpdateConstraintsIfNeeded();
                cell.Bounds = new System.Drawing.RectangleF(0, 0, tableView.Bounds.Width, tableView.Bounds.Height);
                cell.SetNeedsLayout();
                cell.LayoutIfNeeded();



                return cell.ContentView.SystemLayoutSizeFittingSize(UIView.UILayoutFittingCompressedSize).Height + 1.0f;
            }
        }
    }
}


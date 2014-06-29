using System;
using MonoTouch.UIKit;

namespace SkinSafeApp.ViewControllers
{
    public class MainViewController : UITabBarController
    {
        public MainViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var articles = new ArticlesViewController();
            articles.TabBarItem = new UITabBarItem("Protection", UIImage.FromBundle("/Images/shield"), UIImage.FromBundle("/Images/shield"));

            var environment = new EnvironmentViewController();
            environment.TabBarItem = new UITabBarItem("Environment", UIImage.FromBundle("/Images/sun"), UIImage.FromBundle("/Images/sun"));

            var journal = new JournalViewController();
            journal.TabBarItem = new UITabBarItem("Skin", UIImage.FromBundle("/Images/camera"), UIImage.FromBundle("/Images/camera"));

            ViewControllers = new UIViewController[]
            {
                new UINavigationController(environment),
                new UINavigationController(journal),
                new UINavigationController(articles),
            };
        }
    }
}


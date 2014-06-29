// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace SkinSafeApp.ViewControllers
{
	[Register ("EnvironmentViewController")]
	partial class EnvironmentViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel AreaLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel DateLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UICollectionView IndexCollection { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView IndexImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel IndexLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DateLabel != null) {
				DateLabel.Dispose ();
				DateLabel = null;
			}

			if (AreaLabel != null) {
				AreaLabel.Dispose ();
				AreaLabel = null;
			}

			if (IndexImage != null) {
				IndexImage.Dispose ();
				IndexImage = null;
			}

			if (IndexLabel != null) {
				IndexLabel.Dispose ();
				IndexLabel = null;
			}

			if (IndexCollection != null) {
				IndexCollection.Dispose ();
				IndexCollection = null;
			}
		}
	}
}

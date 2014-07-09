using MonoTouch.UIKit;
using Xamarin.Geolocation;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SkinSafeApp.ViewControllers
{
    public partial class EnvironmentViewController : UIViewController
    {
        public EnvironmentViewController() 
            : base("EnvironmentViewController", null)
        {
            Title = "Environment";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			
            IndexImage.Image = UIImage.FromFile("Images/big_sun.png");
            IndexCollection.BackgroundColor = UIColor.Clear;

            GetGeolocation();
        }

        private async void GetGeolocation()
        {
            try
            {
                var locator = new Geolocator { DesiredAccuracy = 50 };
                var pos = await locator.GetPositionAsync (timeout: 10000);
                var coder = new CLGeocoder();
                var place = await coder.ReverseGeocodeLocationAsync(new CLLocation(pos.Latitude, pos.Longitude));

                var centerCoord = new CLLocationCoordinate2D(pos.Latitude, pos.Longitude);
                var region = MKCoordinateRegion.FromDistance(centerCoord, 10000, 10000);

                if (place.Length > 0)
                {
                    #if DEBUG
                    var zipCode = "02648";
                    #else
                    var zipCode = place[0].PostalCode;
                    #endif


                    var client = new RestSharp.RestClient();
                    var request = new RestSharp.RestRequest("http://iaspub.epa.gov/enviro/efservice/getEnvirofactsUVHOURLY/ZIP/" + zipCode + "/JSON");
                    var r = await Task.Run(() => client.Execute<List<UIModel>>(request));

                    var now = DateTime.Now;
                    var indexValue = 0;
                    foreach (var d in r.Data)
                    {
                        if (now > d.DATE_TIME)
                            indexValue = d.UV_VALUE;
                        else
                            break;
                    }
                    IndexLabel.Text = indexValue.ToString();

//                    //                _uvLabel.Text = zipCode;
//                    var plot = new CPTScatterPlot { DataSource = new MyDataSource(r.Data.Select(x => (float)x.UV_VALUE).ToArray()) };
//                    _graph.AddPlot (plot);
//                    _graph.DefaultPlotSpace.ScaleToFitPlots(new [] { plot });

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get UV iNdex: " + e.Message);
            }

        }

        private class UIModel
        {
            public string ORDER { get; set; }
            public string ZIP { get; set; }
            public DateTime DATE_TIME { get; set; }
            public int UV_VALUE { get; set; }
        }
    }
}


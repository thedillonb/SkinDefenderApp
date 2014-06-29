using System;
using MonoTouch.UIKit;
using Xamarin.Geolocation;
using MonoTouch.CoreLocation;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using CorePlot;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
using System.Linq;
using MonoTouch.MapKit;
using System.Net.Mail;

namespace SkinSafeApp.ViewControllers
{
    public class Environment2ViewController : UIViewController
    {
        private UILabel _uvLabel;
        private CPTXYGraph _graph;
        private MKMapView _map;

        public Environment2ViewController()
        {
            Title = "Environment";
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            _map = new MKMapView(new RectangleF(0, 64f, View.Bounds.Width, View.Bounds.Height - 64f));
            _map.MapType = MKMapType.Standard;
            _map.ZoomEnabled = false;
            _map.ScrollEnabled = false;
            _map.ShowsBuildings = false;
            _map.ShowsPointsOfInterest = false;
            _map.Alpha = 0.8f;
            View.Add(_map);

            var circle = new UIView(new RectangleF(0, 0, 100, 100));
            circle.Layer.CornerRadius = 50f;
            circle.Layer.MasksToBounds = true;
            circle.BackgroundColor = UIColor.FromWhiteAlpha(0.9f, 0.5f);
            circle.Center = View.Center;
            View.Add(circle);

            _uvLabel = new UILabel(new RectangleF(0, 0, circle.Bounds.Width, 20));
            _uvLabel.Center = new PointF(circle.Bounds.Width / 2, circle.Bounds.Height / 2);
            _uvLabel.TextAlignment = UITextAlignment.Center;
            circle.Add(_uvLabel);

            var linear = CPTScaleType.Linear;
            var graph = _graph = new CPTXYGraph (View.Frame, linear, linear) {
                Title = "UV Index",
                BackgroundColor = new CGColor (0.982f, 0.988f, 0.890f),
                PaddingTop = 20f,
                PaddingBottom = 20f,
                PaddingLeft = 20f,
                PaddingRight = 20f,
            };

//            _map.AddSubview (new CPTGraphHostingView (new RectangleF(0, _map.Bounds.Height - 80f, View.Bounds.Width, 80f)) {
//                HostedGraph = graph
//            });
//
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
                region = _map.RegionThatFits(region);
                _map.SetRegion(region, true);

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

    //                _uvLabel.Text = zipCode;
                    var plot = new CPTScatterPlot { DataSource = new MyDataSource(r.Data.Select(x => (float)x.UV_VALUE).ToArray()) };
                    _graph.AddPlot (plot);
                    _graph.DefaultPlotSpace.ScaleToFitPlots(new [] { plot });

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

        // A simple data source for the plot
        class MyDataSource : CPTScatterPlotDataSource
        {
            float [] _values;

            public MyDataSource(float[] values)
            {
                _values = values;
            }

            public override int NumberOfRecordsForPlot (CPTPlot plot)
            {
                return _values.Length;
            }

            public override NSNumber NumberForPlot (CPTPlot plot, CPTPlotField field, uint index)
            {
                return field == CPTPlotField.ScatterPlotFieldX ? index : _values [(int)index];
            }
        }

    }
}


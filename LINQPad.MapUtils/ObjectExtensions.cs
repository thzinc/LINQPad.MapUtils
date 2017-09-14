using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Polylines;

namespace LINQPad.MapUtils
{
    public static class ObjectExtensions
    {
        private static readonly object PanelSync = new object();
        public static IEnumerable<T> DumpMarkers<T>(this IEnumerable<T> items, Func<T, (double latitude, double longitude)> coordinateSelector, Func<T, string> tooltipSelector = null, string panel = "Map", string overlay = "Markers")
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            var mapControl = GetMapControl<T>(panel);

            var mapOverlay = GetMapOverlay<T>(overlay, mapControl);

            var markers = items.Select(i => new GoogleMarkerWithData(coordinateSelector(i), GMarkerGoogleType.red, i)
            {
                ToolTipText = tooltipSelector?.Invoke(i) ?? i.ToString()
            });
            foreach (var marker in markers)
            {
                mapOverlay.Markers.Add(marker);
            }

            mapControl.ZoomAndCenterMarkers(null);
            mapControl.OnMarkerClick += (marker, sender) =>
            {
                switch (marker)
                {
                    case GoogleMarkerWithData markerWithData:
                        markerWithData.Item.Dump();
                        break;
                }
            };

            return items;
        }

        public static IEnumerable<T> DumpRoute<T>(this IEnumerable<T> items, Func<T, (double latitude, double longitude)> coordinateSelector, string routeName = "Route", string panel = "Map", string overlay = "Routes")
        {
            if (panel == null) throw new ArgumentNullException(nameof(panel));

            var mapControl = GetMapControl<T>(panel);

            var mapOverlay = GetMapOverlay<T>(overlay, mapControl);

            var route = new GMapRoute(items
                .Select(i =>
                {
                    var coords = coordinateSelector(i);
                    return new PointLatLng(coords.latitude, coords.longitude);
                }),
                routeName);

            mapOverlay.Routes.Add(route);
            mapControl.ZoomAndCenterRoute(route);

            return items;
        }

        public static IEnumerable<T> DumpRoute<T>(this IEnumerable<T> items, Func<T, string> encodedPolylineSelector, Func<T, string> routeNameSelector = null, string panel = "Map", string overlay = "Routes")
        {
            foreach (var i in items)
            {
                Polyline.DecodePolyline(encodedPolylineSelector(i))
                    .DumpRoute(x => (x.Latitude, x.Longitude), routeNameSelector?.Invoke(i), panel, overlay);
            }

            return items;
        }

        private static GMapOverlay GetMapOverlay<T>(string overlay, GMapControl mapControl)
        {
            GMapOverlay mapOverlay;
            lock (mapControl)
            {
                mapOverlay = mapControl.Overlays.FirstOrDefault(o => o.Id == overlay);
                if (mapOverlay == null)
                {
                    mapOverlay = new GMapOverlay(overlay);
                    mapControl.Overlays.Add(mapOverlay);
                }
            }
            return mapOverlay;
        }

        private static GMapControl GetMapControl<T>(string panel)
        {
            GMapControl mapControl;
            lock (PanelSync)
            {
                var outputPanel = PanelManager.GetOutputPanel(panel);
                if (outputPanel == null)
                {
                    mapControl = new GMapControl
                    {
                        MapProvider = GMapProviders.GoogleMap,
                        MinZoom = 0,
                        MaxZoom = 18,
                        CanDragMap = true,
                        DragButton = MouseButtons.Left,
                        IgnoreMarkerOnMouseWheel = true,
                    };
                    PanelManager.DisplayControl(mapControl, panel);
                }
                else
                {
                    mapControl = (GMapControl) outputPanel.GetControl();
                }
            }
            return mapControl;
        }

        public class GoogleMarkerWithData : GMarkerGoogle
        {
            public object Item { get; }

            public GoogleMarkerWithData((double latitude, double longitude) coords, GMarkerGoogleType markerType, object item)
                : base(new PointLatLng(coords.latitude, coords.longitude), markerType)
            {
                Item = item;
            }
        }
    }
}

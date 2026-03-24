using Microsoft.Maui.Devices.Sensors;

namespace Culinary_Guide.Services
{
    public interface ILocationService
    {
        Task<(double Latitude, double Longitude)> GetUserLocationAsync();
        double CalculateDistance(double lat1, double lon1, double lat2, double lon2);
    }

    public class SimulatedLocationService : ILocationService
    {
        private const double UserLatitude = 39.9087;
        private const double UserLongitude = 116.3975;

        public Task<(double, double)> GetUserLocationAsync() =>
            Task.FromResult((UserLatitude, UserLongitude));

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadius * c;
        }

        private static double ToRadians(double deg) => deg * Math.PI / 180;
    }

    public class RealLocationService : ILocationService
    {
        private const double DefaultLatitude = 39.9087;
        private const double DefaultLongitude = 116.3975;

        public async Task<(double Latitude, double Longitude)> GetUserLocationAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                {
                    Console.WriteLine("位置权限被拒绝，使用默认位置");
                    return (DefaultLatitude, DefaultLongitude);
                }

                var request = new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                };

                var location = await Geolocation.Default.GetLocationAsync(request);

                if (location != null)
                {
                    Console.WriteLine($"获取位置成功: {location.Latitude}, {location.Longitude}");
                    return (location.Latitude, location.Longitude);
                }
                
                Console.WriteLine("获取位置为空，使用默认位置");
                return (DefaultLatitude, DefaultLongitude);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取位置出错: {ex.Message}");
                return (DefaultLatitude, DefaultLongitude);
            }
        }

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371;
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadius * c;
        }

        private static double ToRadians(double deg) => deg * Math.PI / 180;
    }
}
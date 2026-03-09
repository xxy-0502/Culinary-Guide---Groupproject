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
}

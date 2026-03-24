using Culinary_Guide.Services;
using Xunit;

namespace Culinary_Guide.Tests
{
    public class LocationServiceTests
    {
        [Fact]
        public void CalculateDistance_BeijingToShanghai_ReturnsCorrectDistance()
        {
            var service = new SimulatedLocationService();
            
            double beijingLat = 39.9087;
            double beijingLon = 116.3975;
            double shanghaiLat = 31.2304;
            double shanghaiLon = 121.4737;
            
            var distance = service.CalculateDistance(beijingLat, beijingLon, shanghaiLat, shanghaiLon);
            
            Assert.InRange(distance, 1057, 1077);
        }

        [Fact]
        public void CalculateDistance_SameLocation_ReturnsZero()
        {
            var service = new SimulatedLocationService();
            
            double lat = 39.9087;
            double lon = 116.3975;
            
            var distance = service.CalculateDistance(lat, lon, lat, lon);
            
            Assert.Equal(0, distance, 0);
        }

        [Fact]
        public async Task SimulatedLocationService_GetUserLocation_ReturnsBeijing()
        {
            var service = new SimulatedLocationService();
            
            var location = await service.GetUserLocationAsync();
            
            Assert.Equal(39.9087, location.Latitude);
            Assert.Equal(116.3975, location.Longitude);
        }
    }
}
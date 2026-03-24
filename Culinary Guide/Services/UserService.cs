using Culinary_Guide.Models;

namespace Culinary_Guide.Services
{
    public interface IUserService
    {
        Task<UserProfile> GetUserProfileAsync();
        Task SaveUserProfileAsync(UserProfile profile);
        Task<string?> PickAvatarAsync();
        Task<string?> TakePhotoAsync();
        event EventHandler? ProfileChanged;
    }

    public class UserService : IUserService
    {
        private readonly DatabaseService _databaseService;
        private UserProfile? _cachedProfile;

        public event EventHandler? ProfileChanged;

        public UserService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            if (_cachedProfile != null)
                return _cachedProfile;

            var item = await _databaseService.GetUserProfileAsync();
            
            if (item == null)
            {
                _cachedProfile = new UserProfile
                {
                    Nickname = "",
                    Bio = "",
                    AvatarPath = null,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
            else
            {
                _cachedProfile = new UserProfile
                {
                    Nickname = item.Nickname,
                    Bio = item.Bio,
                    AvatarPath = item.AvatarPath,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                };
            }

            return _cachedProfile;
        }

        public async Task SaveUserProfileAsync(UserProfile profile)
        {
            var item = new UserProfileItem
            {
                Nickname = profile.Nickname,
                Bio = profile.Bio,
                AvatarPath = profile.AvatarPath
            };

            await _databaseService.SaveUserProfileAsync(item);
            
            _cachedProfile = profile;
            ProfileChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task<string?> PickAvatarAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "选择头像"
                });

                if (photo == null)
                    return null;

                return await SaveAvatarAsync(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"选择图片失败: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> TakePhotoAsync()
        {
            try
            {
                if (!MediaPicker.IsCaptureSupported)
                    return null;

                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "拍摄头像"
                });

                if (photo == null)
                    return null;

                return await SaveAvatarAsync(photo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"拍照失败: {ex.Message}");
                return null;
            }
        }

        private async Task<string?> SaveAvatarAsync(FileResult photo)
        {
            try
            {
                var avatarDir = Path.Combine(FileSystem.Current.AppDataDirectory, "avatars");
                if (!Directory.Exists(avatarDir))
                {
                    Directory.CreateDirectory(avatarDir);
                }

                var fileName = $"avatar_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                var filePath = Path.Combine(avatarDir, fileName);

                using var sourceStream = await photo.OpenReadAsync();
                using var destStream = File.Create(filePath);
                await sourceStream.CopyToAsync(destStream);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存头像失败: {ex.Message}");
                return null;
            }
        }
    }
}
using System;
using System.IO;

namespace VRCX
{
    public partial class AppApi
    {
        public string AvatarImagePath(string avatarId)
        {
            if (string.IsNullOrEmpty(avatarId))
                return string.Empty;

            var folder = Path.Join(Program.AppDataDirectory, "AvatarImages");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = Path.Join(folder, $"{avatarId}.png");
            if (File.Exists(filePath))
                return filePath;

            return string.Empty;
        }

        public bool SaveLocalAvatarImage(string avatarId, string sourcePath)
        {
            if (string.IsNullOrEmpty(avatarId) || string.IsNullOrEmpty(sourcePath))
                return false;

            try
            {
                var folder = Path.Join(Program.AppDataDirectory, "AvatarImages");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var filePath = Path.Join(folder, $"{avatarId}.png");
                File.Copy(sourcePath, filePath, true);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to save local avatar image");
                return false;
            }
        }
    }
}

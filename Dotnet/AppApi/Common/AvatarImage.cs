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

        public string SaveAvatarPreviewImage(string sourcePath, string avatarId)
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(avatarId))
                return string.Empty;

            var folder = Path.Join(Program.AppDataDirectory, "AvatarImages");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = Path.Join(folder, $"{avatarId}.png");

            try
            {
                File.Copy(sourcePath, filePath, true);
                return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

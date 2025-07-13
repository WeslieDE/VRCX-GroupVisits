using System;
using System.IO;
using NLog;

namespace VRCX;

public partial class AppApi
{
    public string GetLocalAvatarImage(string avatarId)
    {
        try
        {
            var dir = Path.Join(Program.AppDataDirectory, "AvatarImages");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var file = Path.Join(dir, $"{avatarId}.png");
            if (File.Exists(file))
                return file;
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Failed to get local avatar image");
        }
        return string.Empty;
    }
}

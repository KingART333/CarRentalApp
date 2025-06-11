using System;
using System.Collections.Generic;
using System.IO;

namespace CarRentalApp.Helpers
{
    public static class ImageHelper
    {
        public static List<string> ConvertCarImagesToBase64(string folderPath)
        {
            List<string> base64Images = new();
            for (int i = 1; i <= 6; i++)
            {
                var fullPath = Path.Combine(folderPath, $"{i}.jpg");
                if (File.Exists(fullPath))
                {
                    byte[] imageBytes = File.ReadAllBytes(fullPath);
                    string base64String = Convert.ToBase64String(imageBytes);
                    base64Images.Add(base64String);
                }
            }
            return base64Images;
        }
    }
}

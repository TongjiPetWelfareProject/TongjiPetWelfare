namespace Glue.Controllers
{
    public class FileHelper
    {
        private static readonly int maxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB大小限制
        public static async Task<string> SaveFileAsync(IFormFile file, string uploadFolder= "wwwroot/uploads")
        {
            try
            {
                if(file.Length > 0)
                {
                    if (!file.ContentType.StartsWith("image/"))
                    {
                        throw new Exception("只允许上传图片文件");
                    }
                    if (file.Length > maxFileSizeInBytes)
                    {
                        throw new Exception("文件大小超过允许的限制");
                    }
                    DateTime currentDate = DateTime.Now;
                    // Create a folder path based on the current date
                    string dateFolder = Path.Combine(uploadFolder, currentDate.ToString("yyyy/MM/dd"));
                    // Create the date folder if it doesn't exist
                    if (!Directory.Exists(dateFolder))
                    {
                        Directory.CreateDirectory(dateFolder);
                    }
                    string uniqueFileName = Guid.NewGuid().ToString();

                    // Calculate the file path within the specified uploadFolder
                    string filePath = Path.Combine(dateFolder, uniqueFileName);

                    // Save the file to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Return the file path (or URL) for the client
                    return filePath;
                }
                else
                {
                    throw new Exception("No file uploaded");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<List<string>> SaveImagesAsync(List<IFormFile> files, string uploadFolder = "wwwroot/uploads")
        {
            var tasks = new List<Task<string>>();

            foreach (var file in files)
            {
                tasks.Add(SaveFileAsync(file, uploadFolder));
            }

            await Task.WhenAll(tasks);

            return tasks.Select(t => t.Result).ToList();
        }
    }
}

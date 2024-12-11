using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opencv
{
    class DownloadImage
    {
        public async Task DownloadImageIfNotExists(string url, string savePath)
        {
            // Создание папки для скачивания изображения
            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
            }
            // Если нету файла скачивается в папку
            if (!File.Exists(savePath))
            {
                using HttpClient client = new();
                Console.WriteLine("Скачивание изображения...");
                byte[] imageBytes = await client.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(savePath, imageBytes);
                Console.WriteLine("Изображение успешно сохранено!");
            }
            else
            {
                Console.WriteLine("Изображение уже существует.");
            }
        }
    }
}

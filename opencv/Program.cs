using System;
using System.Drawing;
using System.IO;
using OpenCvSharp;
using SixLabors.ImageSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using opencv;

class ImageAugmentation
{
    static async Task Main(string[] args)
    {
        // С ПЕРВОГО РАЗА МОЖЕТ НЕ ВСЕ ФОТОГРАФИИ СДЕЛАТЬ


        // Папка для оригинальных изображений
        string inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
        // Путь до оригинальных изображений
        string inputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images\\oscar.png");

        string imageUrl = "https://i.postimg.cc/DZB6FMmG/oscar.png"; // Ссылка на изображение
        string savePath = Path.Combine(inputFolder, "oscar.png"); // Путь сохранения

        // Папки для сохранения аугментированных изображений
        string outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "opencv_augmented_images");
        string outputFolder_ = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "opencv_augmented_images_");
        string outputFolder__ = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "opencv_augmented_images__");

        // Создание папок для сохранения изображений
        if (!Directory.Exists(outputFolder) || !Directory.Exists(outputFolder_) || !Directory.Exists(outputFolder__))
        {
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(outputFolder_);
            Directory.CreateDirectory(outputFolder__);
        }

        // Скачивание + проверка изображения
        DownloadImage download = new();
        await download.DownloadImageIfNotExists(imageUrl, savePath);

        // Загрузка изображения
        Mat image = Cv2.ImRead(inputPath);

        if (image.Empty())
        {
            Console.WriteLine("Ошибка: изображение пустое или не может быть прочитано.");
        }
        else
        {
            Console.WriteLine($"Изображение загружено: {image.Width}x{image.Height}");
        }

        Random random = new();

        // ПЕРВАЯ Генерация 10 изображений
        for (int i = 0; i < 10; i++)
        {
            Mat augmentedImage = image.Clone();

            /*if (random.NextDouble() > 0.5)
                augmentedImage = ApplyShift(augmentedImage, random.Next(-50, 50), random.Next(-50, 50)); // Сдвиг НЕ РАБОТАЕТ ПАДЛА */

            if (random.NextDouble() > 0.5)
                augmentedImage = Methods.ApplyRotation(augmentedImage, random.Next(-15, 15)); // Поворот

            if (random.NextDouble() > 0.5)
                Methods.AddRandomLines(augmentedImage, random, 5); // Линии

            if (random.NextDouble() > 0.5)
                Methods.AddLensFlare(augmentedImage, random); // Блики

            if (random.NextDouble() > 0.5)
                Cv2.GaussianBlur(augmentedImage, augmentedImage, new Size(9, 9), random.Next(1, 5)); // Размытие

            if (random.NextDouble() > 0.5)
                augmentedImage = Methods.ApplyScaling(augmentedImage, random.NextFloat(0.8f, 1.2f), random.NextFloat(0.8f, 1.2f)); // Сжатие/растяжение


            // Сохранение аугментированного изображения
            string outputFileName = Path.Combine(outputFolder, $"augmented_{i}.png");
            Cv2.ImWrite(outputFileName, augmentedImage);

            Console.WriteLine($"Сохранено: {outputFileName}");

            augmentedImage.Dispose();
        }

        // ВТОРАЯ Генерация 10 изображений
        using (Image<Rgba32> image_ = SixLabors.ImageSharp.Image.Load<Rgba32>(inputPath))
        {
            Random random_ = new();
            for (int i = 0; i < 10; i++)
            {
                using Image<Rgba32> augmentedImage_ = image_.Clone(context =>
                {
                    if (random.NextDouble() > 0.5)
                        context.Rotate(random.Next(-15, 15)); // Случайный поворот
                    if (random.NextDouble() > 0.5)
                        context.Flip(random.Next(0, 2) == 0 ? SixLabors.ImageSharp.Processing.FlipMode.Horizontal : SixLabors.ImageSharp.Processing.FlipMode.Vertical); // Отражение
                    if (random.NextDouble() > 0.5)
                        context.Brightness(random.NextFloat(0.5f, 1.5f)); // Изменение яркости
                    if (random.NextDouble() > 0.5)
                        context.Contrast(random.NextFloat(0.5f, 1.5f)); // Изменение контраста
                });
                string outputFileName = Path.Combine(outputFolder_, $"augmented_{i}.png");
                augmentedImage_.SaveAsPng(outputFileName);
                Console.WriteLine($"Сохранено: {outputFileName}");
            }
        }

        // ТРЕТЬЯ Генерация 10 изображений
        for (int i = 0; i < 10; i++)
        {
            Mat augmentedImage = image.Clone();

            // Применение случайных эффектов
            if (random.NextDouble() > 0.5)
                Methods.AddGaussianNoise(augmentedImage); // Добавляем шум

            if (random.NextDouble() > 0.5)
                Methods.AddSaltAndPepperNoise(augmentedImage, 0.02); // Шум

            if (random.NextDouble() > 0.5)
                Cv2.GaussianBlur(augmentedImage, augmentedImage, new Size(5, 5), 1); // Гауссовское размытие

            if (random.NextDouble() > 0.5)
                Methods.ChangeBrightnessAndContrast(augmentedImage, random.NextDouble() * 2, random.Next(-50, 50)); // Яркость и контраст

            // Сохранение аугментированного изображения
            string outputFileName = Path.Combine(outputFolder__, $"augmented_{i}.png");
            Cv2.ImWrite(outputFileName, augmentedImage);

            Console.WriteLine($"Сохранено: {outputFileName}");

            augmentedImage.Dispose();
        }
        Console.WriteLine("Аугментация завершена :D");
    }
}
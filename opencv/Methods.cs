using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opencv
{
    static class Methods
    {
        // Случайные линии
        public static void AddRandomLines(Mat image, Random random, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Point start = new Point(random.Next(image.Cols), random.Next(image.Rows));
                Point end = new Point(random.Next(image.Cols), random.Next(image.Rows));
                Scalar color = new Scalar(random.Next(256), random.Next(256), random.Next(256));
                int thickness = random.Next(1, 5);
                Cv2.Line(image, start, end, color, thickness);
            }
        }

        // Поворот изображения
        public static Mat ApplyRotation(Mat image, double angle)
        {
            Mat result = new Mat();
            Point2f center = new Point2f(image.Width / 2f, image.Height / 2f);
            Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Cv2.WarpAffine(image, result, rotationMatrix, image.Size());
            return result;
        }

        // Блики //Эти Блики ослепляют
        public static void AddLensFlare(Mat image, Random random)
        {
            int radius = random.Next(30, 100);
            Point center = new Point(random.Next(image.Cols), random.Next(image.Rows));
            Scalar color = new Scalar(255, 255, 255, 100); // Белый цвет
            Cv2.Circle(image, center, radius, color, -1, LineTypes.AntiAlias);
        }

        // Меняем скейлинг - сжатие растяжение изображения
        public static Mat ApplyScaling(Mat image, float scaleX, float scaleY)
        {
            Mat result = new Mat();
            Cv2.Resize(image, result, new Size(image.Width * scaleX, image.Height * scaleY));
            return result;
        }
         
        // Гаусс мужик + шумы
        public static void AddGaussianNoise(Mat image)
        {
            Mat noise = new(image.Size(), image.Type());
            Cv2.Randn(noise, 0, 25); // Генерируем шум с нормальным распределением
            Cv2.Add(image, noise, image);
            noise.Dispose();
        }

        // Поперчить + солененькое 
        public static void AddSaltAndPepperNoise(Mat image, double amount)
        {
            int numSalt = (int)(amount * image.Rows * image.Cols);
            Random random = new();

            for (int i = 0; i < numSalt; i++)
            {
                int x = random.Next(image.Cols);
                int y = random.Next(image.Rows);

                if (random.NextDouble() < 0.5)
                {
                    image.Set(y, x, Scalar.All(0)); // Черные пиксели (перец)
                }
                else
                {
                    image.Set(y, x, Scalar.All(255)); // Белые пиксели (соль)
                }
            }
        }

        //Смена яркости
        public static void ChangeBrightnessAndContrast(Mat image, double alpha, int beta)
        {
            // alpha - коэффициент контраста
            // beta - смещение яркости
            image.ConvertTo(image, -1, alpha, beta);
        }

        /*//Сдвиг изображений НЕ РАБОТАЕТ ЧЕРНАЯ КАРТИНКА 3/10 СЛУЧАЕВ
    static Mat ApplyShift(Mat image, int shiftX, int shiftY)
    {
        Mat result = new Mat();
        Mat transformationMatrix = Mat.Eye(2, 3, MatType.CV_32F);
        transformationMatrix.Set(0, 2, shiftX);
        transformationMatrix.Set(1, 2, shiftY);
        Cv2.WarpAffine(image, result, transformationMatrix, image.Size());
        return result;
    }*/

    }
}

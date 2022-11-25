using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Drawing;
using System.Drawing.Imaging;

namespace TelegramBot
{
    class Program
    {

        static void Main(string[] args)
        {
            var client = new TelegramBotClient("5685383171:AAEZeajf7LYXTc8Xc_JnYzG14tedgaUhDO4");
            client.StartReceiving(Update, Error);
            Console.ReadLine();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        //Проверка сообщений пользователя
        async static Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message.Text != null)
            {
                Console.WriteLine($"{message.Chat.Id}     |    {message.Text}");
                if (message.Text.ToLower().Contains("привет"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Добрый день!");
                    return;
                }

            }
            if (message.Photo != null)
            {
                //Проверка на наличие папки photo
                if (System.IO.File.Exists("photo"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято");
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    string destinationFileDath = $@"photo\1{message.Photo.Last().FileSize}.png";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                    await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                    Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}");
                    fileStream.Close();

                    Image image = Image.FromFile($@"photo\1{message.Photo.Last().FileSize}.png");

                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(image, 1200, 900);
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                    // Create an Encoder object based on the GUID
                    // for the Quality parameter category.
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter
                    // objects. In this case, there is only one
                    // EncoderParameter object in the array.
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    // Save the bitmap as a JPG file with zero quality level compression.
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save($@"photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;

                }
                else
                {
                    Directory.CreateDirectory("photo");
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято");
                    var fileId = update.Message.Photo.Last().FileId;
                    var fileInfo = await botClient.GetFileAsync(fileId);
                    var filePath = fileInfo.FilePath;
                    string destinationFileDath = $@"photo\1{message.Photo.Last().FileSize}.png";
                    await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                    await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                    Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}");
                    fileStream.Close();

                    Image image = Image.FromFile($@"photo\1{message.Photo.Last().FileSize}.png");

                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(image, 1200, 900);
                    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                    // Create an Encoder object based on the GUID
                    // for the Quality parameter category.
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    // Create an EncoderParameters object.
                    // An EncoderParameters object has an array of EncoderParameter
                    // objects. In this case, there is only one
                    // EncoderParameter object in the array.
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);

                    // Save the bitmap as a JPG file with zero quality level compression.
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp1.Save($@"photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
            }
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
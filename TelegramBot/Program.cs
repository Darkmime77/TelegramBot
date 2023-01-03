using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.Drawing;
using System.Drawing.Imaging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("5685383171:AAEZeajf7LYXTc8Xc_JnYzG14tedgaUhDO4");

using var cts = new CancellationTokenSource();

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};

botClient.StartReceiving(
    HandleUpdatesAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начал прослушку @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update?.Message?.Text != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }
    if (update.Type == UpdateType.Message && update?.Message?.Photo != null)
    {
        await HandleMessage(botClient, update.Message);
        return;
    }
    if (update.Message.ReplyToMessage != null && update.Message.ReplyToMessage.Text.Contains("Отправьте новый список прокси"))
    {

    }
}

async Task HandleMessage(ITelegramBotClient botClient, Message message)
{
    if (message.Text == "/start")
    {
        ReplyKeyboardMarkup keyboard = new(new[]
        {
            new KeyboardButton[] {"Добавить", "Заменить"},
        })
        {
            ResizeKeyboard = true
        };
        await botClient.SendTextMessageAsync(message.Chat.Id, "Выбирите один из вариантов", replyMarkup: keyboard);
        return;
    }

    if (message.ReplyToMessage != null && message.ReplyToMessage.Text.Contains("Отправьте фотографии для добавления"))
    {
        if (message.Photo != null)
        {
            Directory.CreateDirectory("Photo");
            if (System.IO.File.Exists($@"Photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg"))
            {
                string Number = "1";
                ReplyKeyboardMarkup keyboard = new(new[]
                {
                    new KeyboardButton[] {"Добавить", "Заменить"},
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято", replyMarkup: keyboard);
                var fileId = message.Photo.Last().FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                string destinationFileDath = $@"Photo\1{message.Photo.Last().FileSize}.png";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number})");
                fileStream.Close();

                Image image = Image.FromFile($@"Photo\1{message.Photo.Last().FileSize}.png");

                Bitmap src = Image.FromFile(destinationFileDath) as Bitmap;
                if (src.Width < src.Height)
                {
                    Rectangle cropRect = new Rectangle(0, src.Width, src.Width, src.Width);
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }

                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(src, 1200, 900);
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
                    bmp1.Save($@"Photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number}).jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
                else
                {
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
                    bmp1.Save($@"Photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number}).jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }

            }
            else
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                {
                    new KeyboardButton[] {"Добавить", "Заменить"},
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято", replyMarkup: keyboard);
                var fileId = message.Photo.Last().FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                string destinationFileDath = $@"Photo\1{message.Photo.Last().FileSize}.png";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}");
                fileStream.Close();

                Image image = Image.FromFile($@"Photo\1{message.Photo.Last().FileSize}.png");

                Bitmap src = Image.FromFile(destinationFileDath) as Bitmap;
                if (src.Width < src.Height)
                {
                    Rectangle cropRect = new Rectangle(0, src.Width/4, src.Width, src.Width);
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }
                    target.Save($@"Photo\1{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg");
                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(src, 1200, 900);
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
                    bmp1.Save($@"Photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
                else
                {
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
                    bmp1.Save($@"Photo\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
            }
        }
    }
    if (message.Text == "Заменить")
    {
        //В разработке
        await botClient.SendTextMessageAsync(message.Chat.Id, "Отправьте фотографии для замены", ParseMode.Html, replyMarkup: new ForceReplyMarkup { Selective = true });
        return;
    }
    if (message.Text == "Добавить")
    {
        //В разработке
        await botClient.SendTextMessageAsync(message.Chat.Id, "Отправьте фотографии для добавления", ParseMode.Html, replyMarkup: new ForceReplyMarkup { Selective = true });
        return;
    }
    if (message.ReplyToMessage != null && message.ReplyToMessage.Text.Contains("Отправьте фотографии для замены"))
    {
        if (message.Photo != null)
        {
            Directory.CreateDirectory("Zamena");
            if (System.IO.File.Exists($@"Zamena\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg"))
            {
                string Number = "1";
                ReplyKeyboardMarkup keyboard = new(new[]
                {
                    new KeyboardButton[] {"Добавить", "Заменить"},
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято", replyMarkup: keyboard);
                var fileId = message.Photo.Last().FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                string destinationFileDath = $@"Zamena\1{message.Photo.Last().FileSize}.png";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number})");
                fileStream.Close();

                Image image = Image.FromFile($@"Zamena\1{message.Photo.Last().FileSize}.png");

                Bitmap src = Image.FromFile(destinationFileDath) as Bitmap;
                if (src.Width < src.Height)
                {
                    Rectangle cropRect = new Rectangle(0, src.Width, src.Width, src.Width);
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }

                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(src, 1200, 900);
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
                    bmp1.Save($@"Zamena\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number}).jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
                else
                {
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
                    bmp1.Save($@"Zamena\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}({Number}).jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }

            }
            else
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                {
                    new KeyboardButton[] {"Добавить", "Заменить"},
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Фото принято", replyMarkup: keyboard);
                var fileId = message.Photo.Last().FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;
                string destinationFileDath = $@"Zamena\1{message.Photo.Last().FileSize}.png";
                await using FileStream fileStream = System.IO.File.OpenWrite(destinationFileDath);
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
                Console.WriteLine($"Пользователь {message.Chat.Id} загрузил фотографию - {DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}");
                fileStream.Close();

                Image image = Image.FromFile($@"Zamena\1{message.Photo.Last().FileSize}.png");

                Bitmap src = Image.FromFile(destinationFileDath) as Bitmap;
                if (src.Width < src.Height)
                {
                    Rectangle cropRect = new Rectangle(0, src.Width, src.Width, src.Width);
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }

                    // Get a bitmap.
                    Bitmap bmp1 = new Bitmap(src, 1200, 900);
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
                    bmp1.Save($@"Zamena\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
                else
                {
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
                    bmp1.Save($@"Zamena\{DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss")}.jpeg", jgpEncoder, myEncoderParameters);

                    src.Dispose();
                    image.Dispose();
                    bmp1.Dispose();
                    System.IO.File.Delete(destinationFileDath);
                    return;
                }
            }
        }
    }
    Console.WriteLine($"Пользователь {message.Chat.Id} написал - {message.Text}");
    await botClient.SendTextMessageAsync(message.Chat.Id, $"Вы сказали:\n{message.Text}");
}

Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Ошибка телеграм АПИ:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
        _ => exception.ToString()
    };
    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

static ImageCodecInfo GetEncoder(ImageFormat format)
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
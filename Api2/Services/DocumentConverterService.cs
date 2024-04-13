using Api2.Services.Interfaces;
using Aspose.Words;
using Core;
using System.Diagnostics;

namespace Api2.Services
{
    public class DocumentConverterService(ICustomSettings customSettings) : IDocumentConverterService
    {
        private readonly ICustomSettings _customSettings = customSettings ?? throw new ArgumentNullException(nameof(customSettings));
        private const string PDF_FILENAME = "convertedPdfDocument";
        private readonly static SemaphoreSlim _semaphore = new(1, 1);

        public async Task<(byte[], string)> ConvertAsync(IFormFile file)
        {
            try
            {
                await _semaphore.WaitAsync();

                var generatedName = GenerateName(8);
                var docName = $"{generatedName}{file.FileName}";

                var rootPath = _customSettings.DocConvertDir!;
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                var docPath = Path.Combine(rootPath, docName);
                using (Stream fileStream = new FileStream(docPath, FileMode.Create))
                    await file.CopyToAsync(fileStream);

                var pdfName = Path.GetFileNameWithoutExtension(docPath);
                var pdfPath = Path.Combine(rootPath, $"{pdfName}.pdf");

                await ExecuteCommand(_customSettings.LibreOfficeModule!, docPath, rootPath);

                var bytes = File.ReadAllBytes(pdfPath);

                File.Delete(docPath);
                File.Delete(pdfPath);

                return (bytes, pdfName.Replace(generatedName, ""));
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<(byte[], string)> ConvertAsync(byte[] fileBytes)
        {
            try
            {
                await _semaphore.WaitAsync();

                var generatedName = GenerateName(8);
                var docName = $"{generatedName}{PDF_FILENAME}";

                var rootPath = _customSettings.DocConvertDir!;
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                var docPath = Path.Combine(rootPath, docName);
                using (Stream fileStream = new FileStream(docPath, FileMode.Create))
                    await fileStream.WriteAsync(fileBytes);

                var pdfName = Path.GetFileNameWithoutExtension(docPath);
                var pdfPath = Path.Combine(rootPath, $"{pdfName}.pdf");

                await ExecuteCommand(_customSettings.LibreOfficeModule!, docPath, rootPath);

                var bytes = File.ReadAllBytes(pdfPath);

                File.Delete(docPath);
                File.Delete(pdfPath);

                return (bytes, pdfName.Replace(generatedName, ""));
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<(MemoryStream, string)> ConvertAsync(MemoryStream fileStream)
        {
            try
            {
                await _semaphore.WaitAsync();

                var generatedName = GenerateName(8);
                var docName = $"{generatedName}{PDF_FILENAME}";

                var rootPath = _customSettings.DocConvertDir!;
                if (!Directory.Exists(rootPath))
                    Directory.CreateDirectory(rootPath);

                var docPath = Path.Combine(rootPath, docName);

                using (FileStream file = new FileStream(docPath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.WriteTo(file);
                }

                var pdfName = Path.GetFileNameWithoutExtension(docPath);
                var pdfPath = Path.Combine(rootPath, $"{pdfName}.pdf");

                await ExecuteCommand(_customSettings.LibreOfficeModule!, docPath, rootPath);

                MemoryStream pdfStream = new();
                using (FileStream file = new(pdfPath, FileMode.Open, FileAccess.Read))
                {
                    await file.CopyToAsync(pdfStream);
                }

                pdfStream.Position = 0;

                File.Delete(docPath);
                File.Delete(pdfPath);

                return (pdfStream, pdfName.Replace(generatedName, ""));
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
        public async Task<MemoryStream> ConvertToPdfAsync(MemoryStream docxFileStream)
        {
            try
            {
                // Incarcă documentul DOCX folosind Aspose.Words
                var doc = new Document(docxFileStream);

                // Creează un stream pentru fișierul PDF rezultat
                var pdfStream = new MemoryStream();

                // Salvează documentul ca PDF în stream
                doc.Save(pdfStream, SaveFormat.Pdf);

                // Resetează poziția stream-ului la început
                pdfStream.Position = 0;

                return pdfStream;
            }
            catch (Exception ex)
            {
                // Tratează excepțiile sau raportează eroarea
                throw new Exception("Conversia în PDF a eșuat.", ex);
            }
        }

        private static async Task ExecuteCommand(string librePath, string inputPath, string outputPath)
        {
            var command = $"{librePath} --convert-to pdf:writer_pdf_Export --norestore --writer --headless --outdir {outputPath} {inputPath}";

            ProcessStartInfo processInfo = new()
            {
                FileName = "cmd",
                Arguments = $"/c {command}",
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo);

            ArgumentNullException.ThrowIfNull(process);

            await process.WaitForExitAsync();
            process.Close();
        }

        public static string GenerateName(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

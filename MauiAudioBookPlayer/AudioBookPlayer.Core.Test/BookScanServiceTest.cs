using AudioBookPlayer.Core.Model.Entities;
using AudioBookPlayer.Core.Services.BookScanService;

namespace AudioBookPlayer.Core.Test
{
    public class BookScanServiceTest
    {
        private const string TestDataFolder = "TestData";

		/// <summary>
		/// Testing of scaning folders for books via multiple folder
		/// structure templates.
		/// </summary>
        [Fact]
        public void TestScan()
        {
            if (Directory.Exists(TestDataFolder))
            {
                // Clean old test data
                Directory.Delete(TestDataFolder, true);
            }

			// Template 1 book folders
            MakeTemplate1("BookTemplate1");

			// Template 2 book folders
            MakeTemplate2("BookTemplate2");

			// Multiple single file book variations
            MakeTemplate3(TestDataFolder, "SingleFileBook01");
            MakeTemplate3(Path.Combine(TestDataFolder, "SingleFileBookTemplate1"), "SingleFileBook02");
            MakeTemplate3(Path.Combine(TestDataFolder, "Author", "SingleFileBookTemplate2"), "SingleFileBook03");
           
			// Additional garbage folder
			Directory.CreateDirectory(Path.Combine(TestDataFolder, "Book03", "Books"));
          
			var scanFolder = new ScanFolder(TestDataFolder);
            var service = new BookScanService();
            var books = service.SearchBooks(new List<ScanFolder> { scanFolder });

            Assert.Equal(5, books.Count);

            var bookTemplate1 = books
            .Where(a => a.Caption == "BookTemplate1")
            .Where(a => a.FolderPath == Path.Combine(TestDataFolder, "BookTemplate1"))
            .Where(a => a.Files.Count == 2)
            .Where(a => a.Files[0].Name == "01 audio")
            .Where(a => a.Files[1].Name == "02 audio")
            .SingleOrDefault();

            Assert.NotNull(bookTemplate1);

            var bookTemplate2 = books
            .Where(a => a.Caption == "BookTemplate2")
            .Where(a => a.FolderPath == Path.Combine(TestDataFolder, "BookTemplate2"))
            .Where(a => a.Files.Count == 3)
            .Where(a => a.Files[0].Name == "01 audio")
            .Where(a => a.Files[1].Name == "02 audio")
            .Where(a => a.Files[2].Name == "03 audio")
            .SingleOrDefault();

            Assert.NotNull(bookTemplate2);

            var singleFileBook1 = books
            .Where(a => a.Caption == "SingleFileBook01")
            .Where(a => a.FolderPath == TestDataFolder)
            .Where(a => a.Files.Count == 1)
            .Where(a => a.Files[0].Name == "SingleFileBook01")
            .SingleOrDefault();
            Assert.NotNull(singleFileBook1);

            var singleFileBook2 = books
            .Where(a => a.Caption == "SingleFileBook02")
            .Where(a => a.FolderPath == Path.Combine(TestDataFolder, "SingleFileBookTemplate1"))
            .Where(a => a.Files.Count == 1)
            .Where(a => a.Files[0].Name == "SingleFileBook02")
            .SingleOrDefault();
            Assert.NotNull(singleFileBook2);

            var singleFileBook3 = books
            .Where(a => a.Caption == "SingleFileBook03")
            .Where(a => a.FolderPath == Path.Combine(TestDataFolder, "Author", "SingleFileBookTemplate2"))
            .Where(a => a.Files.Count == 1)
            .Where(a => a.Files[0].Name == "SingleFileBook03")
            .SingleOrDefault();
            Assert.NotNull(singleFileBook2);
        }

        private void MakeTemplate1(string bookName)
        {
            var folderPath = Path.Combine(TestDataFolder, bookName);
            Directory.CreateDirectory(folderPath);
            var file1Path = Path.Combine(folderPath, "01 audio.mp3");
            var file2Path = Path.Combine(folderPath, "02 audio.mp3");
            var coverPath = Path.Combine(folderPath, "cover.jpeg");
            File.CreateText(file1Path);
            File.CreateText(file2Path);
            File.CreateText(coverPath);
        }

        private void MakeTemplate2(string bookName)
        {
            var folderPath = Path.Combine(TestDataFolder, bookName, "01");
            Directory.CreateDirectory(folderPath);
            var file1Path = Path.Combine(folderPath, "01 audio.mp3");
            var file2Path = Path.Combine(folderPath, "02 audio.mp3");
            folderPath = Path.Combine(TestDataFolder, bookName, "02");
            Directory.CreateDirectory(folderPath);
            var file3Path = Path.Combine(folderPath, "03 audio.mp3");
            var coverPath = Path.Combine(TestDataFolder, bookName, "cover.jpeg");
            File.CreateText(file1Path);
            File.CreateText(file2Path);
            File.CreateText(file3Path);
            File.CreateText(coverPath);
        }

        private void MakeTemplate3(string basePath, string fileName)
        {
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            var file1Path = Path.Combine(basePath, $"{fileName}.mp3");
            var coverPath = Path.Combine(basePath, $"{fileName}.png");
            File.CreateText(file1Path);
            File.CreateText(coverPath);
        }
    }
}
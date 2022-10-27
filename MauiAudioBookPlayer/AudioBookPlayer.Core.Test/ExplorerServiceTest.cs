using AudioBookPlayer.Core.Services.ExplorerService;

namespace AudioBookPlayer.Core.Test
{
	public class ExplorerServiceTest
	{
		private const string TestDataFolder = "TestData";

		/// <summary>
		/// Testing of scaning folders for books via multiple folder
		/// structure templates.
		/// </summary>
		[Fact]
		public void LoadFolderItem()
		{
			if (Directory.Exists(TestDataFolder))
			{
				// Clean old test data
				Directory.Delete(TestDataFolder, true);
			}
			var service = new ExplorerService();
			var folderItem = service.LoadFolder("C:\\Dist");
			var folderChildren = folderItem.GetChildren();
		}
	}
}
# MAUI AudioBookPlayer
<p>Hello! Do you like audiobooks? I really love listening to them. I have listened to more books than I have read. :)</p>
<p>I created this project for personal use to experiment with the .NET <a href="https://learn.microsoft.com/ru-ru/dotnet/maui/what-is-maui?view=net-maui-7.0">MAUI framework</a>. <br/>The project was developed using the MVVM pattern.</p>
<p>
An audiobook is a collection of audio files arranged in folders. Very often, the creators of audiobooks do not adhere to a single file structure, sometimes an audiobook may consist of a single file. In my project, the BookScanService class is responsible for searching for audiobooks. The user specifies the root folder relative to which the scan will be performed. The BookScanService uses heuristics to search for audio files and book cover image files located in three basic file structure templates:
<ol>
<li>/ScanFolder/../BookName(non numeric)/file.mp3</li>
<li>/ScanFolder/../BookName(non numeric)/01(numeric)/file.mp3</li>
<li>/ScanFolder/file.mp3 -- Single file book.</li>
</ol>
</p>

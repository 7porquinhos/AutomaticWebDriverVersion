![Logo](https://raw.githubusercontent.com/7porquinhos/AutomaticWebDriverVersion/main/DownloadWebDriver/AutomaticWebDriver/DownloadChromeDrive2.png)

# AutomaticChromeDriveDownload
AutomaticChromeDriveDownload is a Csharp library chromedrive extension auto-download.

## Installation

Use the package manager [Package Manager](https://www.nuget.org/packages/AutomaticChromeDriveDownload) to install AutomaticChromeDriveDownload.

```bash
PM > Install-Package AutomaticChromeDriveDownload -Version 1.0.6
```

## Usage

```csharp
using AutomaticWebDriver;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Return string: with browser google chrome version.
            Console.WriteLine(ChromeDriver.GetVersion());
            //Return bool: true to download chromedrive success ou false to error.
            Console.WriteLine(ChromeDriver.Download(Directory.GetCurrentDirectory()));
        }
    }
}

```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.
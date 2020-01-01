# Photo Organizer

This Windows Form application is used to sort photos and videos by date or type into a new folder.  It recursively searches the source directory for photo or video files and either moves or copies them to a new folder structure by either date or file type.  It can also recursively compare two folders to identify photo & video files that may be missing from the target folder.

## Getting Started

After building the project, open PhotoOrganizer.exe and select your source and destination directories. Use the buttons to perform different actions with the source files.

- Make Copy?: when checked, will copy the files to the destination.  When unchecked, original files will be moved.
- Sort By Type: Destination folder will contain subdirectories by filetype.
- Sort By Date: Destination folder will contain directory structure based on identified file date.  Structure is as follows:
  - Year
    - Month (mm)
      - Day (yyyymmdd)
        - file
  - Videos
    - video files
- Rename Vids: Copy video files to directory "Renamed", adding a prefix of the video date (yyyymmdd) to the filename
- Remove Short Vids: Move video files less than 3 seconds to directory "Short"
- Compare Pics: Identify photos from source directory not found in destination directory (compare by name), write results to logfile in destination directory
- Compare Vids: Identify possible duplicate videos based on filesize or duration, write results to CSV in destination folder / "Duplicates"
- Merge HEIC:
- Handle HEVC:
 

### Prerequisites

MetaDataExtractor.dll must exist in same directory as PhotoOrganizer.exe


### Installing

After building, PhotoOrganizer.exe may be run from bin/Debug or bin/Release, or if copied to another folder must be accompanied by MetaDataExtractor.dll


## Built With

* [.Net Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653)
* [MetaDataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) - Extracts metadata (EXIF) from files

## Contributing

Please read [CONTRIBUTING.md]() for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning



## Authors

* **Whit Matthews** - *Initial work* 

See also the list of [contributors](https://github.com/whitmell/PhotoOrganizer/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Thanks to Drew Noakes for creating MetaDataExtractor

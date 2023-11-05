# Project Name

The wastewatch web application functions as a software for people to select items on images with selectionboxes. These boxes are then labled and can be dowloaded for use, for example: Training an AI for object recognition

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Contributors](#contributors)
- [License](#license)

## Installation

#### Prerequisites

- Make sure you have [.NET Core](https://dotnet.microsoft.com/en-us/download) installed.
- You'll need a database connection string for configuration.

#### Database Configuration

1. Edit the `appsettings.json` file and replace `your_connection_string_here` with your actual database connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string_here"
  }
}
```
2. Make sure that in the `Program.cs` the string entered into the connection matches your desired connectionstring in the `appsettings.json`:
3. Run update-database in the nuget console.


## Usage
Using the Application
1. Launch the application by running WasteWatch.
2. Open a web browser and navigate to http://localhost:your_port to access the application.
3. Use the application's user interface to upload images or load images from the database to select items in those images with selection boxes.
4. Label the selected items as needed.
5. Download the labeled items for your use.
6. Explore the different functionalities of loading in unprocessed and processediamges, uploading images from your device and loading images from the database uploaded via the API.

Using the API
1. The Category part is straightforward.
2. To upload an image to the database navigate to the Image part and select the POST functionality, click try it out. Configure the request body as follows( fill `your_base64_string` with you base64 that represents your image):

```json
{
  "id": 0,
  "apiBase64Data": "your_base64_string"
}
```

## Contributors
- [Oscar Theelen](https://github.com/Ozziehman)
- [Menno Rompelberg](https://github.com/MasterDisaster7)
- [Axel Frederiks](https://github.com/ProgrammerGhostPrK)
- [Jonah Siemers](https://github.com/Doomayy)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

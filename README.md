# Project Name

The wastewatch web application functions as a software for people to select items on images with selectionboxes. These boxes are then labled and can be dowloaded for use, for example: Training an AI for object recognition

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Application_Features](#application_features)
- [API_Features](#api_features)
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
3. Run update-database in the package manager console to build the database.





## Usage

#### Using the Application
1. Launch the application by running WasteWatch.
2. Open a web browser and navigate to http://localhost:your_port to access the application.
3. Log into an account or register if you do not have an account.
4. Use the application's user interface to upload images or load images from the database to select items in those images with selection boxes.
5. Label the selected items as needed.
6. Press `Save Box` for each box.
7. Press `Save Image` to upload/update the image in the database.
8. On the page `Image` you can download all images and boxes in a zip file.
9. Explore the different functionalities of loading in unprocessed and processediamges, uploading images from your device and loading images from the database uploaded via the API.

#### Using the API
1. The Category part is straightforward.
2. To upload an image to the database navigate to the Image part and select the POST functionality, click try it out. Configure the request body as follows( fill `your_base64_string` with you base64 that represents your image):

```json
{
  "id": 0,
  "apiBase64Data": "your_base64_string"
}
```




# Application_Features
#### Login/Register
This will keep track of who edited what in the database.
#### Upload Images from device
#### Load 10 Images
Loads the first 10 unprocessed images from the database that were added via API.
#### Load 50 Images
Loads the first 50 unprocessed images from the database that were added via API.
#### Load All Images
Loads all unprocessed images from the database that were added via API.
#### Download Image Data
Donwloads a .zip file with the folders: `images` and `labels` with the images and the labels added to their respective folder with matching id's.
#### Processed Gallery View
Displays a page with all the processed images with all the boxes drawn onto them, you can click the view/edit button under each image to update it.
#### Unrpocessed Gallery View
Displays a page with all the unprocessed images that are loaded from the database, these images were added via the API.

####Category CRUD
This page has all the CRUD functionalities for `Category`




# API_Features
#### Category
1. GET
2. POST
3. GET{id}
4. PUT
5. DELETE
#### Image
1. GET
2. POST
3. GET{id}
4. DELETE




## Contributors
- [Oscar Theelen](https://github.com/Ozziehman)
- [Menno Rompelberg](https://github.com/MasterDisaster7)
- [Axel Frederiks](https://github.com/ProgrammerGhostPrK)
- [Jonah Siemers](https://github.com/Doomayy)




## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

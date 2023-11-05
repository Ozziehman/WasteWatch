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
- 
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



## Contributors
- [Oscar Theelen](https://github.com/Ozziehman)
- [Menno Rompelberg](https://github.com/MasterDisaster7)
- [Axel Frederiks](https://github.com/ProgrammerGhostPrK)
- [Jonah Siemers](https://github.com/Doomayy)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

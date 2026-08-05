# Unity 2D Game

A 2D game project developed with **Unity** and **C#**.

## Requirements

Before opening the project, install the following software:

* Git
* Unity Hub
* Unity Editor `2019.4.16f1`
* Visual Studio with Unity development tools, or another C# editor supported by Unity

Using the same Unity version is recommended to avoid package, asset, and scene compatibility issues.

## Clone the Repository

Open a terminal and run:

```bash
git clone https://github.com/quangduy377/Unity.git
cd Unity
```

Alternatively, download the repository as a ZIP file from GitHub and extract it.

## Install the Required Unity Version

1. Open **Unity Hub**.
2. Select **Installs**.
3. Click **Install Editor**.
4. Find and install:

```text
Unity 2019.4.16f1
```

If the version is not shown in Unity Hub, locate it through the Unity Download Archive and choose **Install with Unity Hub**.

For Windows builds, include the following module during installation:

```text
Windows Build Support (Mono)
```

Visual Studio can also be selected during installation if a C# editor is not already installed.

## Open the Project

1. Open **Unity Hub**.
2. Select **Projects**.
3. Click **Open** or **Add project from disk**.
4. Select the cloned `Unity` folder.

The selected folder must directly contain these directories:

```text
Assets/
Packages/
ProjectSettings/
```

Do not select the `Assets` folder by itself.

Unity will import the assets and restore the required packages when the project is opened for the first time.

## Run the Game

The project contains the following main scenes:

```text
Assets/Level/Intro.unity
Assets/Level/Level.unity
```

To run the game:

1. In the Unity Project window, open:

```text
Assets/Level/Intro.unity
```

2. Wait until Unity finishes compiling the C# scripts.
3. Check the **Console** window for errors.
4. Click the **Play** button at the top of the Unity Editor.

The game should begin from the intro scene.

## Build the Game for Windows

1. Open the intro scene:

```text
Assets/Level/Intro.unity
```

2. Go to:

```text
File > Build Settings
```

3. Confirm that the following scenes are included and enabled in this order:

```text
0 - Assets/Level/Intro.unity
1 - Assets/Level/Level.unity
```

4. Select **PC, Mac & Linux Standalone**.
5. Set the target platform to **Windows**.
6. Click **Switch Platform** if required.
7. Click **Build and Run**.
8. Select or create a build output folder, for example:

```text
Build/
```

Unity will generate the Windows executable and its required data files.

Keep the generated `.exe` file and its accompanying `_Data` folder together.

## Troubleshooting

### Incorrect Unity Version

If Unity displays a version mismatch warning, close the project and reopen it using:

```text
Unity 2019.4.16f1
```

Opening and saving the project with a newer Unity version may upgrade project files and create compatibility issues.

### Scripts Are Still Compiling

Wait until the loading indicator in the lower-right corner of the Unity Editor disappears before pressing Play.

### Missing Packages

Make sure the computer has an internet connection when opening the project for the first time. Unity should restore the packages defined in:

```text
Packages/manifest.json
```

If packages are not restored:

1. Open Unity.
2. Go to **Window > Package Manager**.
3. Allow Unity to refresh the package list.
4. Close and reopen the project if necessary.

### Scene Is Not Included in the Build

Open:

```text
File > Build Settings
```

Then add and enable these scenes:

```text
Assets/Level/Intro.unity
Assets/Level/Level.unity
```

The intro scene should be the first scene in the list.

### Visual Studio Does Not Open C# Scripts

In Unity, go to:

```text
Edit > Preferences > External Tools
```

Set **External Script Editor** to Visual Studio or another installed C# editor.

### Project Has Import or Cache Errors

Close Unity and delete these generated folders if they exist:

```text
Library/
Logs/
obj/
.vs/
```

Do not delete:

```text
Assets/
Packages/
ProjectSettings/
```

Afterward, reopen the project in Unity Hub. Unity will regenerate the deleted folders.

## Recommended Git Ignore Configuration

Unity automatically generates several large local folders that normally should not be committed to Git.

A Unity `.gitignore` file should exclude folders such as:

```text
Library/
Logs/
Temp/
Obj/
Build/
Builds/
.vs/
```

The following project folders should remain tracked:

```text
Assets/
Packages/
ProjectSettings/
```

## Technologies

* Unity `2019.4.16f1`
* C#
* Unity 2D
* Visual Studio

## Project Structure

```text
Unity/
├── Assets/
│   ├── Config/
│   ├── Level/
│   ├── Media/
│   ├── MickeyAnimation/
│   ├── Prefab/
│   ├── RalphAnimation/
│   ├── Scenes/
│   └── Scripts/
├── Packages/
├── ProjectSettings/
└── README.md
```

## Author

Developed by [quangduy377](https://github.com/quangduy377).

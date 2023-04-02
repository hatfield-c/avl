## UT Dallas Autonomous Vehicle Lab [AVL]

Welcome to the repository for UT Dallas' Autonomous Vehicle Lab. This is an early prototype for the lab environment, and as such everything is subject to change.

The current version of the AVL is 0.0.6.2.

## Introduction

The Autonomous Vehicle Lab (AVL) is an open-source project at The University of Texas at Dallas. The AVL serves as a testing environment for student-developed autonomous vehicle control systems that run on a simplified simulation of a real-time operating system. 

Students will find multiple assignment scenarios in which they will utilize these vehicles to maneuver about various environments and obstacles. Further documentation is provided to explain the techinal details of these assignment environments, task creation and implimentation, vehicle properties, and more. 
Both the assignments and documentation can be found in the UTD-RTOS folder.


## Requirements

- OS: 
    - The AVL was developed using Windows 10. There are Unity versions available for Mac OS and Linux, however their stability is not guaranteed.
    - Other versions of Windows might also work, but their stability is not guaranteed.

- GPU:
    - A GPU should not be required, but it will be helpful.

- Memory:
    - Your computer should have at least 2GB of RAM.

- Software
    - Unity 2021.3.7f1
    - *Note: This project is not compatible with older versions of Unity. Ensure that the correct Unity version is installed before proceeding. Newer versions may throw error messages, but the project should still work.*

Unity is a very light-weight game engine, and should thus be able to run on most computers. If your computer cannot meet the requirements, then please contact the course TA to make arrangements. There are courses taught at UT Dallas that involve the Unity engine, so at the very least the TA might be able to schedule you some time on the classroom computers used by these courses.

## Unity Installation

These steps are for the installation of Unity 2021.3.7f1. Check system requirements to verify the program will run properly before installation.

1. Visit https://unity.com/download.
2. Download Unity Hub for the correct OS.
3. Open UnityHubSetup.exe.
4. Follow instructions for the Unity installation prompt window.
5. Run Unity Hub.
6. Create a Unity account if you do not already have one.
7. Once your account is created, Unity Hub will prompt to download the latest version of Unity. Make sure this version is 2021.3.7f1 to avoid compatibility issues.
    - If the suggested version is not correct, you will need to download from the Unity archive. Visit https://unity3d.com/get-unity/download/archive.
    - The necessary version of Unity will be found under Unity 2021.x. Version 2021.3.7f1 was released July 28, 2022.
8. Follow prompts to install the Unity Editor. Installation may take anywhere from 10 to 45 minutes depending on your PC. Install times of over an hour have been reported, but are rare.
9. Once installation is complete, click finish.
10. The downloaded version of Unity should be on your desktop, and can now be used to access this project.

## Project Installation

Follow these instructions to download the project directly from GitHub.

1. Visit the GitHub page for the project, https://github.com/hatfield-c/avl.
2. Click on the green Code button near the upper right.
3. Select to download the repository as a ZIP file.
4. Extract the ZIP file into your desired location, such as your "Documents" folder. These files are now accessible to locally run the project on Unity.
5. To set up the project to be used in Unity, first open Unity Hub.
6. Unity will be opened to a list of projects automatically. This will be empty if this is the first time being used. 
7. Click Open, and navigate to the directory you extracted from the ZIP file, then select the Unity-AVL folder and click "Open". This will add the project to Unity and it can now be accessed.
8. Unity will open an empty scene. You can now proceed to the Quick Start Guide.

## Quick Start Guide

If all instructions were properly followed above, you should be able to easily access the EgoPlayground scene in Unity as follows:
1. Open Unity and select Unity-AVL from your listed projects.
2. Click "File" then "Open" in the Unity Editor, and select the "EgoPlayground.scene" file located at Unity-AVL/Assets/Scenes/.
3. Press the "Play" button at the top of the editor.
4. The simulation will begin, and the game window will render the Ego vehicle's 16x8 pixel camera.
5. Click the "Stop" button at the top of the editor to stop the simulation.
6. Practice opening the other scenes in the same folder as the EgoPlayground, and see how their simulations behave.

## Contributors

The development team of the AVL is as follows:
- Cody Hatfield - Lead Developer - cxh124730@utdallas.edu
- Hannah Ostoja - Junior Developer - hjostoja@gmail.com

*Please direct any inquiries about the project to the Lead Developer.*

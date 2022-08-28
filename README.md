## UT Dallas Autonomous Vehicle Lab [AVL]

Welcome to the repository for UT Dallas' Autonomous Vehicle Lab. This is an early prototype for the lab environment, and as such everything is subject to change.

## Introduction

The Autonomous Vehicle Lab (AVL) is an open-source project at The University of Texas at Dallas. The AVL serves as a testing environment for student-developed autonomous vehicle control systems that run on a simplified simulation of a real-time operating system. 
Students will find multiple assignment scenarios in which they will utilize these vehicles to maneuver about various environments and obstacles. Further documentation is provided to explain the techinal details of these assignment environments, task creation and implimentation, vehicle properties, and more. 
Both the assignments and documentation can be found in the UTD-RTOS folder.


## Requirements

**System Requirements:**

-OS: 

  Windows 7 SP1+, 8, 10, 64-bit versions only; Mac OS X 10.12+; Ubuntu 16.04, 18.04, and CentOS 7.

-GPU:
 
  Graphics card with DX10 (shader model 4.0) capabilities.
  
**Programs:**

-Unity 2021.3.7f1

*Note: This project is not compatible with older versions of Unity. Ensure that the correct Unity version is installed before proceeding.*

## Unity Installation

These steps are for installation of Unity 2021.3.7f1. Check system requirements to verify the program will run properly before installation.

1. Visit https://unity.com/download.
2. Download Unity Hub for the correct OS.
3. Open UnityHubSetup.exe.
4. Follow instructions in Unity installation prompt window.
5. Install and Run Unity Hub.
6. Create an Unity account if you do not already have one.
7. Once account is created, Unity Hub will prompt to download the latest version of Unity. Make sure this version is 2021.3.7f1 to avoid compatibility issues.
8. If the suggested version is not correct, you will need to download from the Unity archive. Visit https://unity3d.com/get-unity/download/archive.
9. The necessary version of Unity will be found under Unity 2021.x. Version 2021.3.7f1 was released July 28, 2022 to confirm it is correct.
10. Download Windows Editor 64 bit. Open the setup file once downloaded.
11. Follow prompts to install. Installation may take several minutes.
12. Once installation is complete, click finish.
13. The downloaded version of Unity should be on your desktop, and can now be used to access this project.

## Project Installation

Follow these instructions to download the project directly from GitHub.

1. Visit the GitHub page for the project, https://github.com/hatfield-c/avl.
2. Click on the green Code button near the upper right.
3. Select to download the repository as a ZIP file.
4. Go to downloads and select the correct .zip file. File name should include avl-dev. (It is possible for the filename to differ than what is pictured, confirm you are selecting the correct file.)
5. Unzip the file (right click + Extract.) This creates a local directory (folder) named after the Github Repository.
6. Move the directory somewhere that is easy to locate, such as your Documents folder. These files are now accessible to locally run the project on Unity.
7. To set up the project to be used in Unity, first open Unity. DO NOT UPDATE VERSIONS IF PROMPTED!
8. Unity will be opened to Projects automatically. This will be empty if this is the first time being used. 
9. Click Open, and navigate to the directory you downloaded avl-dev. Open the next avl-dev folder, then select Unity-AVL and open in Unity. This will add the project to Unity and it can now be accessed.
10. The EgoPlayground scene for this project will automatically open at this point in Unity.

## Quick Start Guide

If all instructions were properly followed above, you should be able to easily access the EgoPlayground scene in Unity. 
Open Unity and select Unity-AVL from your listed projects. The EgoPlayground scene will run at this point by default.
This scene can also be accessed by directly opening the Ego Playground scene file located at avl-dev\avl-dev\Unity-AVL\Assets\Scenes.
To control the Ego vehicle, click the 'Game' window in Unity while the scene is running (press play if the scene is not running).
The vehicle will automatically start moving in a forward direction unless other input is received. Use the WASD keys to move around in the environment.
Be sure to pause the scene when finished.

## Contributors

Cody Hatfield - Lead Developer - cxh124730@utdallas.edu

Hannah Ostoja - Junior Developer - hjostoja@gmail.com

*Please direct any inquiries about the project to the Lead Developer.*
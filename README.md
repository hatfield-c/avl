## UT Dallas Autonomous Vehicle Lab [AVL]

Welcome to the repository for UT Dallas' Autonomous Vehicle Lab. This is an early prototype for the lab environment, and as such everything is subject to change.

##Introduction

Briefly explain in a paragraph what the AVL is, and what it is meant to be used for

## Requirements

System Requirements:
-OS: 
  Windows 7 SP1+, 8, 10, 64-bit versions only; Mac OS X 10.12+; Ubuntu 16.04, 18.04, and CentOS 7.
-GPU: 
  Graphics card with DX10 (shader model 4.0) capabilities.
Programs:
- Unity 2021.3.7f1


Note: This code is not compatible with older versions of Unity. Ensure that the correct Unity version is installed before proceeding.

##Unity Installation

These steps are for installation of Unity 2021.3.7f1. Check system requirements to ensure the program will run properly before proceeding.

1. Visit unity.com/download
2. Download the Unity Hub for the correct OS
3. Open UnityHubSetup.exe
4. Allow app to make changes to your device
5. Agree to TOS
6. Select folder to save program (usually best to keep as is)
7. Install and Run Unity Hub
8. Create an Unity account if you do not already have one
9. Once account is created, Unity Hub will prompt to download the latest version of Unity. Make sure this version is 2021.3.7f1 to avoid compatibility issues.
10. If the suggested version is not correct, you will need to download from the Unity archive. Visit unity3d.com/get-unity/download/archive.
11. The correct version of Unity will be found under Unity 2021.x. Version 2021.3.7f1 was released July 28, 2022 to confirm it is correct.
12. Download Windows Editor 64 bit. Open the setup file once downloaded.
13. Allow to make changes.
14. Follow prompts to install
15. 

##Project Installation

List the steps necessary to download the project from github (without using git, as students might not know how to use git). Also describe how to set up the project to be used in Unity

##Quick Start Guide

Explain how to open the Playground scene file, and explain how to control the Ego vehicle with the keyboard

## Instructions

1. Download this repository
2. Open Unity, and open the existing scene:
    - Unity-AVL/Assets/Scenes/ClearScene.Unity
4. Open a command line with access to python, and navigate to the folder Python-AVL
5. Open the file Python-AVL/Config.py in vim (or your preferred text exitor), and set the file path configurations. Specifically, set the following variables:
    - ROOT_PATH  :  The path to the cloned repository on your local machine
    - SUMO_BINARY_PATH : The path to the sumo-gui.exe file in your SUMO installation directory (remove the '.exe' ending from the path)
6. On the command line, use python to run the file Python-AVL/AvlApplication.py from within the Python-AVL folder. The SUMO application should open
7. In Unity, hit the "play" button
8. The Unity scene should instantiate the topology of the SUMO road network, as well as the vehicles in the sumo scene.
9. As of Alpha Release 0.0.2, ego vehicles are now supported, and an ego vehicle is added to the scene at runtime. To control the ego vehicle, click the 'Game Window' in Unity while the scene is running, and use the WASD keys.

## Credits

This application suite was based on the work done by the BME Automated Drive team, and the original code as well as their research paper can be found at the following links:
- https://github.com/BMEAutomatedDrive/SUMO-Unity3D-connection
- https://ieeexplore.ieee.org/document/8519486

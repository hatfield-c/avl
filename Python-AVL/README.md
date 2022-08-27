## AVL - Traffic Simulator

In a previous project, the AVL was set up to work with the SUMO traffic simulator as a means to further test FSD algorithms. The AVL has since changed from a testing environment to an educational one, and the traffic simulator was not deemed necessary for the educational requirements at this present time.

Most of the code should still be functional even after the mission change, so this folder, which controls the traffic simulator, has been left as a part of the file hierarchy in the event that the traffic simulator functionality is deemed necessary once again.

## Requirements

- Unity 2019.3.0f6
- Python 3.7.9
- SUMO sumo-gui 1.9.2

Note: This code should be compatible with higher versions of Unity, Python, and SUMO, but it has not been guaranteed yet. Lower versions are possibly compatible, but considered *highly* unstable.

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

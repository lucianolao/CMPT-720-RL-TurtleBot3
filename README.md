# CMPT 720 - Final Project

## Group Members:
* Cong Yuan 301409283 (cong_yuan)
* Luciano Oliveira 301393900 (loliveir)  
* Oleksandr Volkanov 301308828 (avolkano)  
* Yu Guo 301265589 (yu_guo_4)

## Setup
The project was tested on Ubuntu 18.04 and 20.04, using Unity 2020.2.x.
The Unity ML-Agents package is required for model training and inference.
Please follow the ML-Agents installation instructions found here (https://github.com/Unity-Technologies/ml-agents#releases--documentation), and make sure to install Unity 1.9.0 and Python 0.25.0 package versions.

## Inference
1. Open the "MainScene.unity" found in the "Assets/Custom/Scenes" folder.
2. For model inference, click on the "TurtleBot/base_footprint" game object in the hierarchy viewer.
3. Then, scroll down the object inspector pane until you see the "Behaviour Parameters" component.
4. There, set the "Model" file that you want to use for inference, and the "Behaviour Type" to "Inference Only".
5. A) If using a simple model, make sure that in the "Ray Perception Sensor 3D" component, the "Rays Per Direction" and the "Stacked Raycasts" are set to 10 and 1 respectively.
5. B) If using an enhanced model, make sure that in the "Ray Perception Sensor 3D" component, the "Rays Per Direction" and the "Stacked Raycasts" are set to 20 and 10 respectively.
6. Both the simple and the enhanced model files can be found in the "Assets/ML-Agents/Models" folder.

## Supplementary Materials
The copy of the project report and the demo videos can be found in the "Supplementary Materials" root folder.

# Procedural Terrain & Ocean

A Unity project demonstrating procedural terrain and water generation, created for the **Computer Graphics and Vision Seminar**.

The project focuses on generating and updating a 3D environment dynamically based on the player's position. Both the terrain and ocean are created procedurally using custom C# scripts.

## Features

- Procedural 3D terrain mesh generation
- Dynamic terrain regeneration based on player position
- Height-based terrain coloring using gradients
- Procedurally generated ocean mesh
- Animated ocean waves
- Configurable wave amplitude, frequency and speed
- Custom terrain and water shaders
- Free-flying camera controller
- Unity Input System integration
- Runtime mesh generation and manipulation

## Procedural Terrain

The terrain is generated as a mesh composed of vertices and triangles. Vertex heights are calculated procedurally and the resulting mesh is colored according to elevation using a configurable gradient.

The terrain follows the player through the world and regenerates when the player moves into a new area.

## Procedural Ocean

The ocean is generated as a separate dynamic mesh. Wave animation is created by modifying vertex heights over time using a combination of sine and cosine waves.

Several properties can be configured directly in the Unity Inspector:

- Wave amplitude
- Wave frequency
- Wave speed
- Water level
- Mesh resolution

## Fly Camera

A custom camera controller allows the scene to be explored freely using keyboard and mouse controls.

The camera uses Unity's new **Input System** and supports normal and accelerated movement as well as mouse-controlled rotation.

## Technologies

- Unity
- C#
- Universal Render Pipeline (URP)
- Shader Graph
- Unity Input System
- Procedural mesh generation

## University Project

Developed as part of the **Computer Graphics and Vision Seminar** at university.

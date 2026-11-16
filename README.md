# Draw Texture Object

https://github.com/user-attachments/assets/b67bf68f-5e29-48cb-a288-2db5d0598ac4

A simple Unity script that lets you paint directly on a texture mapped to an object in the scene.

## Overview

This project demonstrates how to draw on a texture using the mouse pointer and a MeshCollider.  
The script detects where the user clicks on a 3D object, converts that hit point into UV coordinates, and paints on the target texture in real time.

It is useful for:
- whiteboard or sketching effects
- painting on objects
- prototyping texture interaction
- quick editor experiments

## Features

- Draw with the left mouse button
- Continuous strokes between cursor positions
- Brush with configurable size
- Texture generated at runtime
- Clear button support via script
- Suitable for MeshRenderer + MeshCollider objects

## How it works

The script performs these steps:

1. Creates a Texture2D at runtime
2. Assigns it to the target Renderer material
3. Uses a raycast from the mouse position
4. Converts the hit point into UV coordinates
5. Maps UVs to texture pixels
6. Draws a brush at the clicked point
7. Connects nearby points with a Bresenham line algorithm to create smooth strokes

## Setup

1. Create a new Unity project
2. Add a 3D object (cube, plane, etc.)
3. Add a MeshCollider to the object
4. Add a MeshRenderer
5. Add this script to the object
6. Assign:
   - Camera
   - Renderer
   - Texture size
   - Brush size

Example inspector values:
- Brush Size: 10
- Texture Size: 1024
- Camera: Main Camera
- Target Renderer: the object renderer

## Script usage

The script automatically draws while holding the left mouse button.

```csharp
if (Mouse.current.leftButton.isPressed)
{
    // drawing happens here
}

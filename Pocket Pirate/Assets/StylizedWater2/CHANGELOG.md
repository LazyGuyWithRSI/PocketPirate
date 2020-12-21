1.0.5

Added:
- Particle effects, composed out of flipbooks with normal maps:
    * Big splash
    * Stationary ripples
    * Trail
    * Collision effect (eg. boat bows)
    * Splash ring (eg. footsteps)
    * Waterfall mist
    * Splash upwards
- Water Grid component, can create a grid of water objects and follow a specific transform.

Fixed:
- Material turning black if normal map was enabled, yet no normal map was assigned.
- Intersection texture was using mesh UV, even when UV source was set to "World XZ Projected".

Changed:
- Waves no longer displace along the mesh's normal when "World XZ projected" UV is used, which was incorrect behaviour
- Sparkles are no longer based on sun direction, this way they stay a consistent size in dynamically lit scenes. Instead they fade out when the sun approaches the horizon.
- When using Flat Shading, refraction and caustics still use the normal map, instead of the flat face normals.
- Planar Reflections render scale now takes render scale configured in URP settings into account
- Improved Rough Waves and Sea Foam textures

1.0.4

Added:
- CanTouchWater function to Buoyancy class: Checks if a position is below the maximum possible wave height. Can be used as a fast broad-phase check, before actually using the more expensive SampleWaves function
- Context menu option to Transform component to add Floating Transform component

Fixed:
- Error in material UI when no pipeline asset was assigned in Graphics Settings
- Floating Transform, sample points not being saved if object was a prefab

Changed:
- Revised demo content
- Floating Transform no longer animates when editing a prefab using the component
- Minor UI improvements

1.0.3

Added:
- Water Object script, as a general means of identifying and finding water meshes between systems, this is now attached to all prefabs
- Planar reflections renderer, enables mirror-like reflections from specific layers

Changed:
- Buoyancy sample positions of Floating Transform can now be manipulated in the scene view

Fixed:
- Error when assigning material to Floating Transform component, or not being able to

1.0.2
Verified compatibility with Wavemaker

Added:
- Support for Boxophobic's Atmospheric Height Fog (can be enabled through the Help window)

Changed:
- Shader now correctly takes the mesh's vertex normal into account, making it suitable for use with spheres and rivers

1.0.1
Verified compatibility with Oculus Quest (see compatibility section of documentation, some caveats apply due to an OpenGLES bug)

Fixed:
- Hard visible transition in translucency effect when exponent was set to 1
- Back faces not visible when culling was set to "Double-sided" and depth texture was enabled

1.0.0
Initial release (Nov 3 2020)
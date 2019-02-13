# Animation_Proj_3

Create a program by Unity3D that loads a character skin from a .skin file (described below) and
attach it to a skeleton (loaded from a .skel file).
The skin surface should be rendered using at least two different colored lights.
The program should allow some way to adjust DOF values in the skeleton. At a minimum, it
could have keys for next/last DOF and increase/decrease value. Optionally, it could use widgets
in a GUI to adjust the values or allow the user to interactively pick a joint and adjust it with the
mouse.
The program should be able to load any .skin and .skel file given to it.
Skin File Description
The .skin file contains arrays of vertex data, an array of triangle data, and an array of binding
matrices. For a sample .skin file, see tube4unity.skin and its corresponding .skel file
tube4unity.skel.

positions [numverts] {

[x] [y] [z]

…

}

normals [numverts] {

[x] [y] [z]

….

}

skinweights [numverts]

[numattachments] [joint0] [weight0] … [jointN] [weightN]

…

}

triangles [numtriangles] {

[vertex0] [vertex1] [vertex2]

…

}

bindings [numjoints]

matrix {

[m00] [m10] [m20]

[m01] [m11] [m21]

[m02] [m12] [m22]

[m03] [m13] [m23]

}

…

}

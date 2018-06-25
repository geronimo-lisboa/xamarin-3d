using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin3d.model
{
    interface IRotatable
    {
        void Rotate(Vector3 axis, float angleInDegree);
    }
}

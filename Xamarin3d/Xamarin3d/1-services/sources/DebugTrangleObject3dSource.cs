using System;
using System.Collections.Generic;
using System.Text;
using Xamarin3d.model;

namespace Xamarin3d.services.sources
{
    class DebugTrangleObject3dSource : Object3dSource
    {
        public Object3d Fabricate()
        {
            var vertices = new float[] {
                    0.0f, 0.5f, 0.0f,
                    -0.5f, -0.5f, 0.0f,
                    0.5f, -0.5f, 0.0f
            };
            var colors = new float[]
{
                1.0f, 0.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f, 1.0f,
            };
            Object3d obj = new Object3d(vertices, colors, null);
            return obj;
        }
    }
}

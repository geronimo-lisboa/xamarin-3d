using System;
using System.Collections.Generic;
using System.Text;
using Xamarin3d.model;

namespace Xamarin3d.services.sources
{
    public interface Object3dSource
    {
        Object3d Fabricate();
    }
}

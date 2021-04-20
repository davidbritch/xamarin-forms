using System;
using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class PlaneNode : SCNNode
    {
        public PlaneNode(nfloat width, nfloat length, SCNVector3 position, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(width, length, color),
                Position = position,
                Opacity = 0.5f
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(nfloat width, nfloat length, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;
            material.DoubleSided = false;

            SCNPlane geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class CylinderNode : SCNNode
    {
        public CylinderNode(float radius, float height, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(radius, height, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float radius, float height, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNCylinder geometry = SCNCylinder.Create(radius, height);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

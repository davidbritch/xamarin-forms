using UIKit;
using SceneKit;

namespace ARKitFun.Nodes
{
    public class SphereNode : SCNNode
    {
        public SphereNode(float size, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(size, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float size, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNSphere geometry = SCNSphere.Create(size);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

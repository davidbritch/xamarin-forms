using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class PlaneNode : SCNNode
    {
        public PlaneNode(float width, float length, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(width, length, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float width, float length, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;
            material.DoubleSided = true;

            SCNPlane geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class PyramidNode : SCNNode
    {
        public PyramidNode(float width, float height, float length, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(width, height, length, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float width, float height, float length, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNPyramid geometry = SCNPyramid.Create(width, height, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

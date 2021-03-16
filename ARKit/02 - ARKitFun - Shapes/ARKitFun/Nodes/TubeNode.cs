using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class TubeNode : SCNNode
    {
        public TubeNode(float innerRadius, float outerRadius, float height, UIColor color)
        {
            SCNNode node = new SCNNode
            {
                Geometry = CreateGeometry(innerRadius, outerRadius, height, color)
            };

            AddChildNode(node);
        }

        SCNGeometry CreateGeometry(float innerRadius, float outerRadius, float height, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNTube geometry = SCNTube.Create(innerRadius, outerRadius, height);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}

using SceneKit;
using UIKit;

namespace ARKitFun.Nodes
{
    public class CubeNode :SCNNode
    {
        public CubeNode(float size, UIColor color)
        {
            SCNMaterial material = new SCNMaterial();
            material.Diffuse.Contents = color;

            SCNBox geometry = SCNBox.Create(size, size, size, 0);
            geometry.Materials = new[] { material };

            SCNNode node = new SCNNode
            {
                Geometry = geometry
                //Opacity = 0.90f
            };

            AddChildNode(node);            
        }
    }
}

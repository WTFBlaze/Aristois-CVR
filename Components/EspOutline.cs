using Aristois.Configs;
using Aristois.Utils;
using UnityEngine;

namespace Aristois.Components
{
    internal class EspOutline : MonoBehaviour
    {   
        // Credits to Edward7s for the base of this component. We have used it and modified it for our shader and usage instead <3
        private GameObject _espGameObject { get; set; }
        public Material material { get; private set; }

        private void Awake()
        {
            float Y = gameObject.GetComponentInChildren<ABI.CCK.Components.CVRAvatar>().viewPosition.y;
            _espGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _espGameObject.SetActive(Config.Main.CapsuleEsp);
            _espGameObject.layer = 5;
            _espGameObject.name = "Esp";
            _espGameObject.transform.parent = this.transform;
            _espGameObject.transform.localEulerAngles = Vector3.zero;
            _espGameObject.transform.localPosition = new Vector3(0, Y / 1.6f, 0);
            _espGameObject.transform.localScale = new Vector3(Y / 1.5f, Y / 1.4f, Y / 1.5f);
            _espGameObject.GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
            Destroy(_espGameObject.GetComponent<CapsuleCollider>());
            material = new(BundleManager.EspShader);

            // Set Rendering Values
            material.SetFloat("_Cull", 0);
            material.SetFloat("_ZWrite", 0);
            material.SetFloat("_ZTest", 0);
            material.SetFloat("_AlphaToMask", 0);

            if (Config.Colors.RainbowEsp)
                SetRainbowEsp();
            else if (Config.Colors.GradientEsp)
                SetGradientEsp();
            else if (Config.Colors.ScanLinesEsp)
                SetScanLinesEsp();
            else
                SetSingleColorEsp();

            // Set Rim Alpha Values
            material.SetFloat("_Smoothstepmin", 0.2f);
            material.SetFloat("_Smoothstepmax", 1);
            material.SetFloat("_RimmeshAlpha", 1);
            material.SetFloat("_Rimpower", 1);

            _espGameObject.GetComponent<Renderer>().material = material;
        }

        private void ResetAllEspValues()
        {
            material.SetFloat("_HueAlpha", 0);
            material.SetFloat("_ScanAlpha", 0);
            material.SetFloat("_GradiantAlhpa", 0);
        }

        public void SetRainbowEsp()
        {
            ResetAllEspValues();
            material.SetFloat("_HueTile", 1);
            material.SetFloat("_HueSpeed", 0.2f);
            material.SetFloat("_HueAlpha", 1);
        }

        public void SetGradientEsp()
        {
            ResetAllEspValues();
            material.SetFloat("_GradiantAlhpa", 1);
            material.SetColor("_G1", Config.Colors.EspGradient.Start.GetColor());
            material.SetFloat("_G1Min", 0.2f);
            material.SetFloat("_G1Max", 1);
            material.SetFloat("_MoveG1", 2);
            material.SetColor("_G2", Config.Colors.EspGradient.End.GetColor());
            material.SetFloat("_G2Min", 0.2f);
            material.SetFloat("_G2Max", 1);
            material.SetFloat("_MoveG2", 0);
        }

        public void SetScanLinesEsp()
        {
            ResetAllEspValues();
            material.SetFloat("_ScanAlpha", 1);
            material.SetFloat("_ScanLinesMin", 0);
            material.SetFloat("_ScanLinesMax", 0.5f);
            material.SetFloat("_ScanPower", 1);
            material.SetColor("_Scancolor", Config.Colors.EspColor.GetColor());
        }

        public void SetSingleColorEsp()
        {
            ResetAllEspValues();
            material.SetColor("_Color", Config.Colors.EspColor.GetColor());
        }
    }
}

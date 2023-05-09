using Aristois.Utils;
using UnityEngine;

namespace Aristois.Components
{
    internal class EspOutline : MonoBehaviour
    {
        private GameObject _espGameObject { get; set; }

        private void Awake()
        {
            float Y = gameObject.GetComponentInChildren<ABI.CCK.Components.CVRAvatar>().viewPosition.y;
            _espGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _espGameObject.SetActive(false);
            _espGameObject.layer = 5;
            _espGameObject.name = "Esp";
            _espGameObject.transform.parent = this.transform;
            _espGameObject.transform.localEulerAngles = Vector3.zero;
            _espGameObject.transform.localPosition = new Vector3(0, Y / 1.6f, 0);
            _espGameObject.transform.localScale = new Vector3(Y / 1.5f, Y / 1.4f, Y / 1.5f);
            _espGameObject.GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
            Destroy(_espGameObject.GetComponent<CapsuleCollider>());

            Material material = new(BundleManager.EspShader);
            // TODO: Make esp material values be set based on esp config settings! Defaulting to Rainbow ESP.

            // Set Rendering Values
            material.SetFloat("_Cull", 0);
            material.SetFloat("_ZWrite", 0);
            material.SetFloat("_ZTest", 0);
            material.SetFloat("_AlphaToMask", 0);

            // Set Hue Values
            material.SetFloat("_HueTile", 1);
            material.SetFloat("_HueSpeed", 0.2f);
            material.SetFloat("_HueAlpha", 1);

            // Set Rim Alpha Values
            material.SetFloat("_Smoothstepmin", 0.25f);
            material.SetFloat("_Smoothstepmax", 1);
            material.SetFloat("_RimmeshAlpha", 1);
            material.SetFloat("_Rimpower", 1);

            _espGameObject.GetComponent<Renderer>().material = material;
        }
    }
}

using UnityEngine;


[System.Serializable]
public class Item : MonoBehaviour
{
    public string id;
    public string type;

    void Update()
    {
        print(this.transform.position.y);
    }
}

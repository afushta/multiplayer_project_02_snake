using Colyseus.Schema;
using System.Collections.Generic;
using UnityEngine;


public class Apple : MonoBehaviour
{
    private string _appleId;
    private AppleNO _appleNO;

    public void Init(string appleId, AppleNO appleNO)
    {
        _appleId = appleId;
        _appleNO = appleNO;
        _appleNO.OnChange += OnChange;
    }

    private void OnChange(List<DataChange> changes)
    {
        Vector3 position = transform.position;

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    position.x = (float)change.Value;
                    break;
                case "z":
                    position.z = (float)change.Value;
                    break;
                default:
                    Debug.LogWarning($"Change of field {change.Field} is not supported in AppleNO");
                    break;
            }
        }

        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Collect()
    {
        gameObject.SetActive(false);

        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            { "id", _appleId },
        };
        MultiplayerManager.Instance.SendMessage("collect", data);
    }

    public void Destroy()
    {
        if (_appleNO != null) _appleNO.OnChange -= OnChange;
        Destroy(gameObject);
    }
}

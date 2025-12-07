using UnityEngine;

[CreateAssetMenu(fileName = "AbilityInfo", menuName = "Scriptable Objects/AbilityInfo")]
public class AbilityInfo : ScriptableObject
{
    [SerializeField] private GameObject _abilityPrefab;

    public void SpawnAbility(GameObject go)
    {
        GameObject ability = Instantiate(_abilityPrefab, go.transform.position, Quaternion.identity);

        ability.transform.SetParent(go.transform);
    }
}

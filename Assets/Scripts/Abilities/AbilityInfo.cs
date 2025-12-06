using UnityEngine;

[CreateAssetMenu(fileName = "AbilityInfo", menuName = "Scriptable Objects/AbilityInfo")]
public class AbilityInfo : ScriptableObject
{
    [SerializeField] private GameObject _abilityPrefab;

    public void SpawnAbility(Vector3 position) => Instantiate(_abilityPrefab, position, Quaternion.identity);
}

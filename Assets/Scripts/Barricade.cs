using Unity.Mathematics;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    [SerializeField] private GameObject _barricadePrefab;
    [SerializeField] private Vector2 _distanceFromPlayer;
    public void BuildWall(Vector2 directionAimed)
    {
        GameObject instantiatedWall = Instantiate(_barricadePrefab, directionAimed + _distanceFromPlayer, quaternion.identity);

    }
    private void BreakWall()
    {
        
    }
}

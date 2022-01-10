using Random = UnityEngine.Random;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int Min;
        public int Max;

        public Count(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    public Count BackgroundItemPrefabCount = new Count(5, 15);
    public Count FoliagePrefabCount = new Count(5, 15);

    public GameObject ExitPrefab;
    public float ExitsYPos = 26.75f;

    public GameObject[] GoatPrefabs;
    private bool _placingGoats;

    public GameObject[] BackgroundItemPrefabs;
    public GameObject[] FoliagePrefabs;

    public int XPosLimit = 30;
    public int YPosLimit = 18;
    private List<Vector2> _posList = new List<Vector2>();

    public void GenerateFor(int week)
    {
        Instantiate(ExitPrefab, new Vector3(0f, ExitsYPos, 0f), Quaternion.identity);

        InitializePosList();

        _placingGoats = true;
        int goatCount = week % 10 + 1;
        PlacePrefabsAtRandom(GoatPrefabs, goatCount, goatCount);
        _placingGoats = false;

        PlacePrefabsAtRandom(BackgroundItemPrefabs, BackgroundItemPrefabCount.Min, BackgroundItemPrefabCount.Max);
        PlacePrefabsAtRandom(FoliagePrefabs, FoliagePrefabCount.Min, FoliagePrefabCount.Max);
    }

    private void InitializePosList()
    {
        _posList.Clear();

        for (int x = 3; x <= XPosLimit; x += 3)
        {
            for (int y = 6; y <= YPosLimit; y += 6)
            {
                _posList.Add(new Vector3(x, y));
            }
        }
    }

    private void PlacePrefabsAtRandom(GameObject[] PrefabArray, int min, int max)
    {
        int prefabCount = Random.Range(min, max + 1);

        for (int i = 0; i < prefabCount; i++)
        {
            Vector2 randomPos = RandomPos();
            GameObject locChoice = PrefabArray[Random.Range(0, PrefabArray.Length)];
            Instantiate(locChoice, randomPos, Quaternion.identity);
        }
    }

    private Vector2 RandomPos()
    {
        int randomIndex = Random.Range(0, _posList.Count);
        Vector2 randomPos = _posList[randomIndex];

        if (!_placingGoats)
        {
            _posList.RemoveAt(randomIndex);
        }

        return randomPos;
    }
}

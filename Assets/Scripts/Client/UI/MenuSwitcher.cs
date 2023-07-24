using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;

    private void Start()
    {
        OpenPage(0);
    }

    public void OpenPage(int index) 
    {
        if(index < 0 || index >= _pages.Length)
            throw new System.ArgumentOutOfRangeException("index");

        for (int i = 0; i < _pages.Length; i++)
            _pages[i].SetActive(i == index);
    }
}

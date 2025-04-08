using UnityEngine;

public class DeckListUI : MonoBehaviour
{
    public Transform contentHolder;
    public GameObject cardListItemPrefab;

    private DeckManager deckManager;

    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        deckManager.OnDeckChanged += RefreshUI;
        RefreshUI();
    }

    void OnDestroy()
    {
        if (deckManager != null)
            deckManager.OnDeckChanged -= RefreshUI;
    }

    void RefreshUI()
    {
        foreach (Transform child in contentHolder)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in deckManager.DeckCards)
        {
            GameObject item = Instantiate(cardListItemPrefab, contentHolder);
            item.GetComponent<CardListItemUI>().Setup(card);
        }
    }
}

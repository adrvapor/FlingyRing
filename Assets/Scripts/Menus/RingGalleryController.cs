using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RingList;
using TMPro;

public class RingGalleryController : MonoBehaviour
{
    public RingList RingList;
    List<RingType> OwnedRingList;
    List<string> UnseenRingList;
    public GameObject RingHolder;

    public NumberNotificationController RingsNotification;
    public TextMeshProUGUI OwnedRingsText;
    public TextMeshProUGUI RingNameText;
    public GameObject SelectedPanel;
    public GameObject PrevButton;
    public GameObject NextButton;

    private int currentRingIndex;

    public const float ringMargin = 1.5f;
    
    void Start()
    {
        UpdateRingGallery();
    }

    public void UpdateRingGallery()
    {
        StartCoroutine(GetUnlockedRings());
    }

    private IEnumerator GetUnlockedRings()
    {
        int unseenRings = UserDataManager.GetUnseenRings();
        RingsNotification.SetNotification(unseenRings);

        yield return new WaitUntil(() => UserDataManager.DataLoaded);
        var unlockedRingKeys = UserDataManager.GetUnlockedRings();

        OwnedRingList = RingList.Rings
            .Where(ring => unlockedRingKeys.Contains(ring.key))
            .ToList();

        UnseenRingList = unlockedRingKeys
            .Skip(unlockedRingKeys.Count - unseenRings)
            .ToList();

        currentRingIndex = OwnedRingList.FindIndex(ring => ring.key == UserDataManager.GetSelectedRing());
        RenderRings();

        OwnedRingsText.text = $"{OwnedRingList.Count}/{RingList.Rings.Count}";
    }

    public void RenderRings()
    {
        foreach (Transform child in RingHolder.transform)
            Destroy(child.gameObject);

        float xPos = ringMargin;
        foreach(RingType ring in OwnedRingList)
        {
            var r = Instantiate(ring.ring);
            r.transform.SetParent(RingHolder.transform);
            r.transform.localPosition = new Vector3(xPos, 0);
            xPos += ringMargin;
        }
    }

    #region Button methods
    public void SelectRing()
    {
        UserDataManager.UpdateSelectedRing(OwnedRingList[currentRingIndex].key);
    }
    public void NextRing()
    {
        StartCoroutine(SmoothMove(new Vector3(-ringMargin * (1 + currentRingIndex), 0),
                                  new Vector3(-ringMargin * (2 + currentRingIndex), 0),
                                  0.5f));
        currentRingIndex++;
        SetUIElements();
    }
    public void PrevRing()
    {
        StartCoroutine(SmoothMove(new Vector3(-ringMargin * (1 + currentRingIndex), 0),
                                  new Vector3(-ringMargin * (currentRingIndex), 0),
                                  0.5f));
        currentRingIndex--;
        SetUIElements();
    }
    public void EnterGallery()
    {
        currentRingIndex = OwnedRingList.FindIndex(ring => ring.key == UserDataManager.GetSelectedRing());
        StartCoroutine(SmoothMove(new Vector3(0, 0),
                                  new Vector3(-ringMargin * (1 + currentRingIndex), 0),
                                  0.5f));
        SetUIElements();
    }
    public void ExitGallery()
    {
        RingsNotification.SetNotification(UserDataManager.GetUnseenRings());
        StartCoroutine(SmoothMove(new Vector3(-ringMargin * (1 + currentRingIndex), 0),
                                  new Vector3(0, 0),
                                  0.5f));
    }

    private void SetUIElements()
    {
        var ringKey = OwnedRingList[currentRingIndex].key;

        SelectedPanel.SetActive(ringKey == UserDataManager.GetSelectedRing());
        PrevButton.SetActive(currentRingIndex > 0);
        NextButton.SetActive(currentRingIndex < OwnedRingList.Count -1);
        RingNameText.text = RingNameLocalizer.GetNameByKey(ringKey);

        if (UnseenRingList.Contains(ringKey))
        {
            UnseenRingList.Remove(ringKey);
            UserDataManager.UpdateUnseenRings(-1);
            RingsNotification.SetNotification(UnseenRingList.Count);
        }
    }

    #endregion

    IEnumerator SmoothMove(Vector3 startPos, Vector3 finalPos, float time)
    {
        var yz = new Vector3(0, RingHolder.transform.position.y, RingHolder.transform.position.z);
        startPos += yz;
        finalPos += yz;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            RingHolder.transform.position = Vector3.Lerp(startPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}

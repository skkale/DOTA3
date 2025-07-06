using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviourPun
{
    public static NotificationManager Instance;

    [SerializeField] private Transform notificationPanel;
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private float notificationDuration = 3f;
    private HashSet<string> shownMessages = new HashSet<string>();

    private void Awake()
    {
        if (photonView.IsMine)
        {
            Instance = this;
        }
    }

    public void ShowNotification(string message)
    {
        if (!photonView.IsMine || shownMessages.Contains(message)) return;
        shownMessages.Add(message);

        GameObject notif = Instantiate(notificationPrefab, notificationPanel);
        notif.GetComponent<TMP_Text>().text = message;
        StartCoroutine(DestroyAfterDelay(notif, notificationDuration));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
// DialogueManager.cs
// �Դ��駷�� GameObject 㴡���㹩ҡ (�й�������ҧ GameObject ���� "Managers")

using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // ��ҧ�ԧ��ѧʤ�Ի��Ǻ��������蹢ͧ�س
    public MonoBehaviour playerControlScript;

    private void OnEnable()
    {
        // ŧ����¹�Ѻ���ǹ��ҡ NPCInteraction
        NPCInteraction.OnDialogueStateChanged += HandleDialogueStateChanged;
    }

    private void OnDisable()
    {
        // ¡��ԡ���ŧ����¹
        NPCInteraction.OnDialogueStateChanged -= HandleDialogueStateChanged;
    }

    // �Ѵ��áѺ���ǹ�������ʶҹк�ʹ�������¹
    private void HandleDialogueStateChanged(bool isDialogueActive)
    {
        if (playerControlScript != null)
        {
            // �Դ/�Դ��÷ӧҹ�ͧʤ�Ի��Ǻ���������
            playerControlScript.enabled = !isDialogueActive;
        }
    }
}
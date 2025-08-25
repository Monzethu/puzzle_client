using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataMono : MonoBehaviour
{
    [System.Serializable]
    public class WallInfo
    {
        public Vector3Int position;
        public string color; // "R","G","B"
    }

    [System.Serializable]
    public class StageData
    {
        public Vector3Int playerStartPos;   // �v���C���[�J�n�ʒu
        public Vector3Int goalPos;          // �S�[���ʒu
        public StageDataMono.WallInfo[] walls; // �Ǐ��
        public Vector3Int[] lightFragments; // ���̌��Ђ̈ʒu

        //�C���X�y�N�^�[�ŐF��I��
        public StartColor startColor = StartColor.White;
    }


}

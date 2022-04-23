using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] ExplosionEffect _explosionParticle;
    [SerializeField] Debris _debris;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple Game Manager is running");
        }
        instance = this;
    }

    public void GenerateExplosionParticle(Vector3 pos, bool isDebri)
    {
        ExplosionEffect ps = Instantiate(_explosionParticle, pos, Quaternion.identity) as ExplosionEffect;
        ps.PlayParticle(isDebri);
        Destroy( ps.gameObject, 2f); //2초후 삭제
    }

    public void MakeDebris(float ratio, Sprite sprite, Vector3 pos, Vector3 forceDirection, float power)
    {
        Texture2D tex = sprite.texture;
        
        List<Texture2D> pieceList = new List<Texture2D> ();
        for(int i = 0; i < 4; i++)
        {
            pieceList.Add(new Texture2D(tex.width / 2, tex.height / 2));
        }

        for (int y = 0; y < tex.height; y++)
        {
            for (int x = 0; x < tex.width; x++)
            {
                SetPixel(x, y, pieceList, tex);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            pieceList[i].Apply(); //그려진 텍스쳐를 반영
            Sprite sp = Sprite.Create(pieceList[i], new Rect(0, 0, pieceList[i].width, pieceList[i].height), Vector2.one * 0.5f, ratio);
            
            Debris debri = Instantiate(_debris, pos, Quaternion.identity);
            debri.SetSprite(sp);

            float angle = Random.Range(-30f, 30f);
            Vector3 rotateVector = Quaternion.Euler(0, 0, angle) * forceDirection;
            debri.AddForceToDebri(rotateVector * power);
        }

        /* list의 순서 
         * 
         *    2    3
         *  
         *    0    1
         */

    }

    private void SetPixel(int x, int y, List<Texture2D> list, Texture2D tex)
    {
        Color color = tex.GetPixel(x, y);

        int idx = -1;
        int nx = 0, ny = 0;
        if (x < tex.width / 2 && y < tex.height / 2)
        {
            idx = 0;
            nx = x;
            ny = y;
        }
        else if (x >= tex.width / 2 && y < tex.height / 2)
        {
            idx = 1;
            nx = x - tex.width / 2;
            ny = y;
        }
        else if (x < tex.width / 2 && y >= tex.height / 2)
        {
            idx = 2;
            nx = x;
            ny = y - tex.height / 2;
        }
        else if (x >= tex.width / 2 && y >= tex.height / 2)
        {
            idx = 3; // (x > width / 2 && y > height / 2)}
            nx = x - tex.width / 2;
            ny = y - tex.height / 2;
        }

        list[idx].SetPixel(nx, ny, color);
    }
}

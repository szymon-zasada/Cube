using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public enum EnemyType { normal, ranged }
public class EnemyGenerator : MonoBehaviour
{

    //v________________________________________________________________________________________________________________________________________________________
    [SerializeField] GameObject enemyBody;
    [SerializeField] Renderer bodyRenderer;
    [SerializeField] BoxCollider bodyCollider;
    [SerializeField] Transform bodyTransform;
    [SerializeField] GameObject enemyDeadBody;
    [SerializeField] List<Renderer> deadBodyRenderers;
    [SerializeField] Transform deadBodyTransform;
    [SerializeField] List<Transform> deadBodyTransformList;
    [SerializeField] CharacterController controller;
    [SerializeField] bool isDead = false;
    [SerializeField] RectTransform canvas;

    //V________________________________________________________________________________________________________________________________________________________

    public bool IsDead { get => isDead; set => isDead = value; }


    //F________________________________________________________________________________________________________________________________________________________

    public Enemy GenerateRandomStats()
    {
        float difficultyFactor = Random.Range(GameManager.Instance.Difficulty * 2f, GameManager.Instance.Difficulty*5.5f);
        controller.enabled = true;
        if(GameManager.Instance.Difficulty > 1.5f && GameManager.Instance.Turrets.Count < 2)
            difficultyFactor*=1.5f;

        BodyTransform(difficultyFactor);
            
        MaterialChange();

        return Enemy.CreateInstance(
            (30 * (difficultyFactor * difficultyFactor)) - 10f, //maxhealth
            Mathf.Clamp(1 / bodyTransform.localScale.x, 0.05f, 0.85f), //speed
            4.5f * difficultyFactor, //damage
            (int)(235f * difficultyFactor * bodyTransform.localScale.x), //cashvalue
            0.35f + (GameManager.Instance.Difficulty * 0.3f) //crystalchance
        );
    }

    public Enemy GenerateBossStats()
    {
        float difficultyFactor = Random.Range(GameManager.Instance.Difficulty * 5f, GameManager.Instance.Difficulty*8f);
        controller.enabled = true;
        if(GameManager.Instance.Difficulty > 1.5f && GameManager.Instance.Turrets.Count < 2)
            difficultyFactor*=1.5f;

        BodyTransform(difficultyFactor);
            
        MaterialChange();

        return Enemy.CreateInstance(
            (500 * difficultyFactor) - 18f, //maxhealth
            Mathf.Clamp(1 / bodyTransform.localScale.x, 0.05f, 0.2f), //speed
            15.5f * difficultyFactor, //damage
            (int)(320f * difficultyFactor * bodyTransform.localScale.x), //cashvalue
            1 //crystalchance
        );
    }

 
    void BodyTransform(float difficultyFactor)
    {
        transform.localScale *= Mathf.Clamp(difficultyFactor, 0.4f, 4f);
        deadBodyTransform.localScale *= Mathf.Clamp(difficultyFactor, 0.2f, 2f);
        transform.position = new Vector3(bodyTransform.position.x, bodyTransform.localScale.y * 0.25f - 0.05f, bodyTransform.position.z);
        //deadBodyTransform.position = new Vector3(bodyTransform.position.x, deadBodyTransform.localScale.y * 0.25f, bodyTransform.position.z);
        //bodyCollider.size *= difficultyFactor * 1.2f;
        canvas.localPosition = new Vector3(0f,0.3f+bodyTransform.localScale.y, -0.3f);
    }


    void MaterialChange()
    {
        var red = (byte)(Mathf.Clamp(bodyTransform.localScale.x * 200f, 0, 255));
        var yellow = (byte)(255 - Mathf.Clamp(bodyTransform.localScale.x * 128f, 0, 255));
        bodyRenderer.material.color = new Color32(red, yellow, 10, 255);

        foreach (Renderer r in deadBodyRenderers)
            r.material.color = bodyRenderer.material.color;
    }

    public async void DeathTransition()
    {
        enemyBody.SetActive(false);
        controller.enabled = false;
        enemyDeadBody.SetActive(true);
        

        float timer = 4f;
        while (timer > 0)
        {
            foreach (Transform t in deadBodyTransformList)
                t.localScale *= 0.995f;


            timer -= Time.deltaTime;
            await Task.Yield();
        }

       
        enemyBody.SetActive(true);
        enemyDeadBody.SetActive(false);
        gameObject.SetActive(false);
    }


}

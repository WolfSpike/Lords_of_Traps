using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HP_BAR : MonoBehaviour {

    public Image Mask;
    public Image Hp_bar;

    Life_Object _Player;

	void Start () {
        if (gameObject.GetComponent<Life_Object>())
            _Player = gameObject.GetComponent<Life_Object>();         
	}
	
	void Update () {

        HP_BAR_CONTROLLER();

	}

    void HP_BAR_CONTROLLER()
    {
        float Persent_HP = _Player._Heal_Point / _Player._Max_Heal_Point * 100;
        float X_POS_BAR = Mask.rectTransform.rect.width * (Persent_HP / 100);

        Hp_bar.rectTransform.localPosition = new Vector2(-(Mask.rectTransform.rect.width - X_POS_BAR), 0);
    }
}

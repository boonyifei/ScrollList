using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum SpriteDigitAlignment
{
    LEFT = 0,
    RIGHT = 1,
    CENTER = 2
}

public class SpriteDigit : MonoBehaviour {

    public SpriteDigitAlignment Alignment = SpriteDigitAlignment.LEFT;
    public Sprite[] DigitSprites;
    public Image[] DigitImage;
    public bool HideIfZero = true;

    private int[] digits;
    private Transform cachedTransform;
    private Vector3 basePosition;
    private float digitSpriteWidth;
    private Vector3 rightVector = Vector3.right;

    void Awake()
    {
        digits = new int[DigitImage.Length];
        cachedTransform = transform;
        basePosition = cachedTransform.localPosition;
        digitSpriteWidth = DigitImage[0].rectTransform.sizeDelta.x;
    }

    public void DisplayNumber(int number)
    {
        //split number to digits
        int num = number;
        int lastDigit = 0;
        int numDigits = 0;
        while (num > 0 && numDigits < digits.Length)
        {
            lastDigit = num % 10;
            num /= 10;

            digits[numDigits] = lastDigit;
            numDigits++;
        }
        if (!HideIfZero && numDigits <= 0)
        {
            numDigits = 1;
            digits[0] = 0;
        }

        int startIndex = numDigits - 1;
        switch (Alignment)
        {
            case SpriteDigitAlignment.LEFT:
            case SpriteDigitAlignment.CENTER:
                {
                    for (int i = 0; i < numDigits; i++)
                    {
                        DigitImage[i].sprite = DigitSprites[digits[startIndex - i]];
                        DigitImage[i].gameObject.SetActive(true);
                    }
                    for (int i = numDigits; i < DigitImage.Length; i++)
                    {
                        DigitImage[i].gameObject.SetActive(false);
                    }

                    if (Alignment == SpriteDigitAlignment.CENTER)
                    {
                        cachedTransform.localPosition = basePosition + (rightVector * digitSpriteWidth * DigitImage.Length * 0.5f) + (-rightVector * digitSpriteWidth * numDigits * 0.5f);
                    }
                }
                break;
            case SpriteDigitAlignment.RIGHT:
                {
                    int imageStartIndex = DigitImage.Length - numDigits - 1;
                    for (int i=0; i <= imageStartIndex; i++)
                    {
                        DigitImage[i].gameObject.SetActive(false);
                    }
                    int count = 0;
                    for (int i=imageStartIndex+1; i< DigitImage.Length; i++)
                    {
                        DigitImage[i].sprite = DigitSprites[digits[startIndex - count]];
                        DigitImage[i].gameObject.SetActive(true);
                        count++;
                    }
                }
                break;
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
	private const string excludedChars = ",.?!-/ $%";

	[SerializeField]
	private TextMeshProUGUI text;

	[SerializeField]
	private Image background;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private float loadSpeed;

	private int state; // 0 là xong, 1 đang chạy, 2 dừng tạm thời

	private Coroutine cor;

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && Time.timeScale > 0f)
		{
			state = 0;
		}
	}

	public void StartDialogue(int from, int to, UnityAction callback = null)
	{
		if (cor != null)
		{
			StopCoroutine(cor);
		}
		cor = StartCoroutine(LoadDialogue(from, to, callback));
	}

	private IEnumerator LoadDialogue(int from, int to, UnityAction callback)
	{
		if (background) background.enabled = true;
		text.text = "";
		int lineIndex = from;
		while (lineIndex <= to)
		{
			string[] array = Lines.getLine(lineIndex).Split("|");
			string dialogue = array[^1];
			string role = (array.Length > 1) ? GetRole(array[0]) : "SON";
			state = 1;
			if (PlayerPrefs.GetInt("language", 0) == 5)
			{
				string arabicRole = GetRoleArabic(role);
				for (int i = 0; i < dialogue.Length; i++)
				{
					text.text = arabicRole + "\n" + dialogue.Substring(0, i + 1) + "<color=#00000000>" + dialogue.Substring(i + 1) + "<color=#00000000>";
					char value = dialogue[i];
					if (",.?!-/ $%".IndexOf(value) == -1)
					{
						source.clip = Resources.Load<AudioClip>("Sounds/Voice/" + role + Random.Range(1, 4));
						source.Play();
					}
					yield return new WaitForSeconds(loadSpeed);
					if (state == 0)
					{
						text.text = arabicRole + "\n" + dialogue;
						break;
					}
				}
			}
			else if (PlayerPrefs.GetInt("language", 0) == 21)
			{
				string arabicRole = GetRolePersian(role);
				for (int i = 0; i < dialogue.Length; i++)
				{
					text.text = arabicRole + "\n" + dialogue.Substring(0, i + 1) + "<color=#00000000>" + dialogue.Substring(i + 1) + "<color=#00000000>";
					char value2 = dialogue[i];
					if (",.?!-/ $%".IndexOf(value2) == -1)
					{
						source.clip = Resources.Load<AudioClip>("Sounds/Voice/" + role + Random.Range(1, 4));
						source.Play();
					}
					yield return new WaitForSeconds(loadSpeed);
					if (state == 0)
					{
						text.text = arabicRole + "\n" + dialogue;
						break;
					}
				}
			}
			else
			{
				for (int i = 0; i < dialogue.Length; i++)
				{
					text.text = role + "\n";
					text.text += dialogue.Substring(0, i + 1);
					TextMeshProUGUI rTLTextMeshPro = text;
					rTLTextMeshPro.text = rTLTextMeshPro.text + "<color=#00000000>" + dialogue.Substring(i + 1) + "<color=#00000000>";
					char value3 = dialogue[i];
					if (",.?!-/ $%".IndexOf(value3) == -1)
					{
						if (source)
						{
							source.clip = Resources.Load<AudioClip>("Sounds/Voice/" + role + Random.Range(1, 4));
							source.Play();
						}
					}
					yield return new WaitForSeconds(loadSpeed);
					if (state == 0)
					{
						text.text = role + "\n";
						text.text += dialogue;
						break;
					}
				}
			}
			state = 2;
			lineIndex++;
			yield return new WaitUntil(() => state == 0);
		}

		
        if(background)
		{
			background.enabled = false;
		}
		text.text = "";
		callback?.Invoke();
	}

	private string GetRole(string index)
	{
		return index switch
		{
			"1" => "MOM",
			"2" => "DAD",
			"3" => "DOLL",
			"4" => "CAT",
			"5" => "KID",
			"6" => "COP",
			"7" => "RED FACE",
			_ => "",
		};
	}

	private string GetRoleArabic(string role)
	{
		return role switch
		{
			"SON" => "الابن",
			"MOM" => "الأم",
			"DAD" => "الأب",
			"DOLL" => "دمية",
			"CAT" => "قطة",
			"KID" => "فتى",
			"COP" => "شرطي",
			"RED FACE" => "الوجه الأحمر",
			_ => "",
		};
	}

	private string GetRolePersian(string role)
	{
		return role switch
		{
			"SON" => "پسر",
			"MOM" => "مامان",
			"DAD" => "پدر",
			"DOLL" => "عروسک",
			"CAT" => "گربه",
			"KID" => "بچه",
			"COP" => "پلیس",
			"RED FACE" => "صورت قرمز",
			_ => "",
		};
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterDatabase : MonoBehaviour
{
	public static CharacterDatabase m_instance = null;
	public Char[] chars;

	void Awake ()
	{
		m_instance = this;
		Initialize ();
	}

	void Initialize ()
	{
		chars = new Char[7];
		chars [0] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [1] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [2] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [3] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [4] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [5] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
		chars [6] = new Char (0, 1, "char_mailman", "Mail Man", "Mail Man", "delivers mail", 6, "town", false, Char.CharCurrencyType.Coins, 1000, 1);
	}
}

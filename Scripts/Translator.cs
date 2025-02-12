using Godot;
using System;
using System.Linq;

public partial class Translator : Control
{

	// --- Reference variables ---

	readonly string[] validPairs = new string[] { "ge","da","di","go","be","ka","ke","li","la","ko"};
	readonly string[] validPunctuation = new string[] {" ", ",", ".", ":", ";", "\n", "?", "!", "'", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "&", "#", "/", "\\", "(", ")" };

	readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";

	// --- Exposed parameters ---

	[Export]
	TextEdit _input, _output;

	[Export]
	Label _errorLabel;

    // ---

	// --- Checks ---

	#region Checks

	/// <summary>
	/// Checks whether if the input contains any valid letters.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool IsEmpty(string input)
	{
		if (RemovePunctuationOf(input).Length != 0) return false;

		SetErrorLabel("There's nothing to decode!");
		return true;
	}

	/// <summary>
	/// Checks whether if the given input contains an even amount of valid letters.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool IsEven(string input)
	{
		if (RemovePunctuationOf(input).Length % 4 == 0)
			return true;

		SetErrorLabel("Input is not divisible by 4! It can't possibly be Gedagi language! (One character in gedagi language must consist of 4 letters)");
		return false;
	}

	/// <summary>
	/// Checkes whether if the given input consists of the valid Gedagi language "letters": GE, DA, DI, GO, BE, KA, KE, LI, LA and KO.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool AreCharactersValid(string input)
	{
		input = RemovePunctuationOf(input);

		foreach (var pair in validPairs)
		{
			input = input.ReplaceN(pair,"");
		}

		if(input.Length == 0) return true;

		SetErrorLabel("The input contains invalid Gedagi language! Gedagi language can ONLY consist of GE, DA, DI, GO, BE, KA, KE, LI, LA, and KO.");
		return false;
	}

	/// <summary>
	/// Checks whether if the Gedagi language "letters" are properly grouped into groups of two (two gedagi letters form a normal letter).
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool AreGroupsValid(string input)
	{
		var groups = input.Split(" ");

		Godot.Collections.Array<string> groupsNoPunctiation = new();
		foreach (var group in groups)
		{
			groupsNoPunctiation.Add(RemovePunctuationOf(group));
		}

		foreach (var group in groupsNoPunctiation)
		{
			if(group.Length % 4 != 0) 
			{
				SetErrorLabel("There is a space that splits a character in half! (One character in gedagi language consists of four letters)");
				return false;
			}
		}

		return true;
	}

	/// <summary>
	/// Checks whether if a given string is punctuation. If so, returns what punctuation it is with the out parameter.
	/// </summary>
	/// <param name="input">A letter</param>
	/// <param name="punctuation">If succesful, gives what punctuation it is. If it isnt, gives an empty string. </param>
	/// <returns></returns>
	bool IsPunctuation(string input, out string punctuation)
	{
		var punctuationTypeIndex = Array.IndexOf(validPunctuation, input);
		
		if (punctuationTypeIndex != -1)
		{
			punctuation = validPunctuation[punctuationTypeIndex];
			return true;
		} else 
		{
			punctuation = "";
			return false;
		}
	}

	/// <summary>
	/// Checks whether if the given text contains characters we haven't accounted for.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool IsTextValid(string input)
	{
		foreach (var validLetter in alphabet)
		{
			input = input.ReplaceN(validLetter.ToString(), "");
		}

		foreach (var punctuation in validPunctuation)
		{
			input = input.Replace(punctuation, "");
		}

		if(input.Length == 0) return true;

		SetErrorLabel("Your text contains invalid characters!");

		return false;
	}

	/// <summary>
	/// Given a string, gives back that same string but with all punctuation removed.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	string RemovePunctuationOf(string input)
	{
		foreach (var punctuation in validPunctuation)
		{
			input = input.Replace(punctuation, "");
		}

		return input;
	}

	#endregion

	/// <summary>
	/// Updates the error text on the GUI to say the input.
	/// </summary>
	/// <param name="input"></param>
	void SetErrorLabel(string input)
	{
		_errorLabel.Text = input;
	}
}

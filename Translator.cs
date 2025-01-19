using Godot;
using System;
using System.Linq;

public partial class Translator : Control
{

	readonly string[] validPairs = new string[] { "ge","da","di","go","be","ka","ke","li","la","ko"};
	readonly string[] validPunctuation = new string[] {" ", ",", ".", ":", ";", "\n", "?", "!", "'"};

	readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";
	

	// ---

	[Export]
	TextEdit _input, _output;

	[Export]
	Label _errorLabel;

    // ---

    #region Decode
    public void Decode()
	{
		string input = _input.Text;

		// Is the string empty?
		if(IsEmpty(input)) return;
		// Is the string uneven?
		if(!IsEven(input)) return;
		// Does the string contain invalid characters?
		if(!AreCharactersValid(input)) return;
		// Are characters divided into groups of four?
		if(!AreGroupsValid(input)) return;

		// Divide into letter groups
		Godot.Collections.Array<string> gedaGroups = new();
		int lastGroupIndex = 0;
		int spacelessIndex = 0;
		for (int i = 0; i < input.Length; i++)
		{		
			// If this character is a space...
			if (IsPunctuation(input[i].ToString(), out string punctuation))
			{
				// Add it to the list
				gedaGroups.Add(punctuation);

				// Update our last group index
				lastGroupIndex++;

				// Continue without counting up the index
				continue;
			}

			// If this character isn't a space, add one to our index
			spacelessIndex++;

			// If our current index is not divisible by 4, we keep going
			if(spacelessIndex % 4 != 0) continue;


			// If it IS divisible by 4, it means we have a letter! Add it to our list.
			gedaGroups.Add(input.Substr(lastGroupIndex, i - lastGroupIndex + 1));

			lastGroupIndex = i + 1;
		}

		// Convert geda groups into numbers
		Godot.Collections.Array<string> alphabetNumbers = new();
		foreach (var group in gedaGroups)
		{
			// If space, add space and continue
			if(IsPunctuation(group, out string punctuation))
			{
				alphabetNumbers.Add(punctuation);
				continue;
			}

			// If not a space, divide 4-letter group into pairs, convert each pair into a number.
			Godot.Collections.Array<string> characterNumbers = new();
			int lastPairIndex = 0;
			for (int i = 1; i < group.Length + 1; i++)
			{
				if(i % 2 != 0) continue;

				string pair = group.Substr(lastPairIndex, i - lastPairIndex);
				string number = Array.IndexOf(validPairs, pair.ToLower()).ToString();
				characterNumbers.Add(number);

				lastPairIndex = i;
			}
			
			alphabetNumbers.Add(String.Join("", characterNumbers));
		}

		// Convert numbers into letters
		Godot.Collections.Array<string> lettersList = new();
		foreach (var number in alphabetNumbers)
		{
			if(IsPunctuation(number, out string punctuation))
			{
				lettersList.Add(punctuation);
				continue;
			}

			int numberInt = number.ToInt();
			lettersList.Add(alphabet[numberInt].ToString().ToUpper());
		}

		// Join letters together, done!
		_output.Text = String.Join("", lettersList);

		SetErrorLabel(" ");
	}

	#endregion

	#region Encode

	public void Encode()
	{
		string input = _input.Text;

		// Is the string empty?
		if (IsEmpty(input)) return;
		// Does the text contain characters we didnt consider?
		if (!IsTextValid(input)) return;

		// letters -> numbers
		Godot.Collections.Array<string> lettersToNumbers = new();
		foreach (var letter in input)
		{
			if (IsPunctuation(letter.ToString(), out string punctuation))
			{
				lettersToNumbers.Add(punctuation);
				continue;
			}

			lettersToNumbers.Add(Array.IndexOf(alphabet.ToArray(), letter.ToString().ToLower()[0]).ToString().PadZeros(2));
		}

		Godot.Collections.Array<string> numbersToGedas = new();
		// pairs of numbers -> ge's and da's
		foreach (var number in lettersToNumbers)
		{
			if (IsPunctuation(number.ToString(), out string punctuation))
			{
				numbersToGedas.Add(punctuation);
				continue;
			}

			foreach (var character in number)
			{
				numbersToGedas.Add(validPairs[character.ToString().ToInt()]);
			}
		}

		_output.Text = String.Join("", numbersToGedas);

		SetErrorLabel(" ");
	}

	#endregion

	#region Checks

	bool IsEmpty(string input)
	{
		if (RemovePunctuationOf(input).Length != 0) return false;

		SetErrorLabel("There's nothing to decode!");
		return true;
	}

	bool IsEven(string input)
	{
		if (RemovePunctuationOf(input).Length % 4 == 0)
			return true;

		SetErrorLabel("Input is not divisible by 4! It can't possibly be Gedagi language! (One character in gedagi language must consist of 4 letters)");
		return false;
	}

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

	#endregion

	string RemovePunctuationOf(string input)
	{
		foreach (var punctuation in validPunctuation)
		{
			input = input.Replace(punctuation, "");
		}

		return input;
	}

	void SetErrorLabel(string input)
	{
		_errorLabel.Text = input;
	}
}

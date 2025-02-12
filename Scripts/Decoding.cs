using Godot;
using System;
using System.Linq;

public partial class Translator : Control
{

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
}

using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		List<string> result = new();

		List<string> groups = new();
		string currentLetters = "";

		// Format the string into a list of strings
		// The items in the string will either consist of groups of 4 letters, or punctuation/numbers.
		foreach (var character in input)
		{
			// If this character is a letter...
			if(char.IsLetter(character))
			{
				// Add it to our current letters string
				currentLetters += character;

				// If we have 4 characters in that string, it means we have a pair!
				if(currentLetters.Length == 4)
				{
					// Add it to the group.
					groups.Add(currentLetters);
					currentLetters = "";
				}
			} else // If this character is NOT a letter...
			{
				// Lets save whatever progress into current letters we may have
				if(currentLetters.Length > 0)
				{
					groups.Add(currentLetters);
					currentLetters = "";
				}

				// Add the character into the list.
				groups.Add(character.ToString());
			}
		}

		// In case we have any unfinish letters leftover, lets just save em onto the list as well.
		if (currentLetters.Length > 0)
		{
			groups.Add(currentLetters);
		}
		
		foreach (var pair in groups)
		{
			
			// If our 'pair' is not 4 characters, it means its not a pair at all!
			// We have to handle these cases separately then.
			if(pair.Length != 4)
			{
				// If it's only one character, and it isnt a letter, it means its punctuation!
				// Let's just pass it on as is.
				if(pair.Length == 1)
				{
					result.Add(pair);
					continue;
				} else // If not, then it means something's wrong in the input! Let's exit.
				{
					SetErrorLabel("There is a letter split in half! Each letter consists of a group of 4 pairs. Make sure there isnt a space between them, like 'ge da'.");
					return;
				}
			}

			// Now that we know we have a valid pair, lets convert it into a number
			// Get the numbers of each half of the pair.
			// We do this by getting the index of the string in the word replacement array.
			var firstNumber = Array.IndexOf(replacementArray, pair.Substring(0, 2).ToLower());
			var secondNumber = Array.IndexOf(replacementArray, pair.Substring(2, 2).ToLower());

			// If we cant find one or both of the words, error out.
			if(firstNumber == -1 || secondNumber == -1)
			{
				GD.Print(pair);
				SetErrorLabel("Input text is not valid! The text must consist of groups of 4 of the following: ge, da, di, go, be, ka, ke, li, la and ko");
				return;
			}

			// Get the combined number.
			int combinedNumber = int.Parse(string.Concat(firstNumber, secondNumber));

			// Convert that number into a letter.
			var letter = NumberToLetter(combinedNumber).ToString();
			letter = pair.All(c => char.IsUpper(c)) ? letter.ToUpper() : letter.ToLower();

			// Add it to the result.
			result.Add(letter);
		}

		_output.Text = String.Join("", result);
		SetErrorLabel(" ");
	}

	#endregion
}

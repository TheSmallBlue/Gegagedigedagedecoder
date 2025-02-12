using Godot;
using System;
using System.Linq;

public partial class Translator : Control
{

	#region Encode

	public void Encode()
	{
		string input = _input.Text;

		// Is the string empty?
		if (IsEmpty(input)) return;
		// Does the text contain characters we didnt consider?
		if (!IsTextValid(input)) return;

		// Convert letters to numbers
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

		// Convert pairs of numbers to gedagi letters
		Godot.Collections.Array<string> numbersToGedas = new();
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
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Translator : Control
{
	#region Encode

	public void Encode()
	{
		string input = _input.Text;

		// Is the string empty?
		if (IsEmpty(input)) return;

		List<string> result = new();

		foreach (var letter in input)
		{
			// Convert to number
			int num = LetterToNumber(letter);

			// If the letter couldn't be converted to number, pass it on as-is.
			if(num == -1)
			{
				result.Add(letter.ToString());
				continue;
			}
			// If the letter's number is bigger than 25, pass it on as-is.
			// This is mainly to avoid having numbers that are 3 digits or more.
			if(num > 25)
			{
				result.Add(letter.ToString());
				continue;
			}

			// Pad the number with two zeroes
			string paddedNum = num.ToString().PadZeros(2);

			// Use each digit of the padded number as an index for our array of words to replace
			foreach (var digit in paddedNum)
			{
				var replacement = replacementArray[int.Parse(digit.ToString())];
				replacement = char.IsUpper(letter) ? replacement.ToUpper() : replacement.ToLower();
				
				result.Add(replacement);
			}
		}

		// Join the string, shrimple!
		_output.Text = String.Join("", result);
		SetErrorLabel(" ");
	}

	#endregion
}

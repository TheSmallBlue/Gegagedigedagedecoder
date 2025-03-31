using Godot;
using System;
using System.Linq;

public partial class Translator : Control
{

	// --- Reference variables ---

	readonly string[] replacementArray = new string[] { "ge","da","di","go","be","ka","ke","li","la","ko"};

	// --- Exposed parameters ---

	[Export]
	TextEdit _input, _output;

	[Export]
	Label _errorLabel;

	// --- Checks ---

	#region Methods

	/// <summary>
	/// Converts any letter into a number.
	/// </summary>
	/// <param name="chara"> The input character. </param>
	/// <returns> The character as a number, or -1 if the input is not a letter. </returns>
	int LetterToNumber(Char chara)
	{
		if(Char.IsLetter(chara))
			return (int)Char.ToLower(chara) - (int)'a';
		
		return -1;
	}

	/// <summary>
	/// Converts a number between 0 and 25 into a letter
	/// </summary>
	/// <param name="number"> The input number. Must be between 0 and 25. </param>
	/// <returns> The number as a letter. </returns>
	Char NumberToLetter(int number)
	{
		if(number >= 0 && number < 26)
			return (char)('a' + number);

		return '-';
	}

	/// <summary>
	/// Checks whether if the input contains any valid letters.
	/// </summary>
	/// <param name="input">The full, unedited text.</param>
	/// <returns></returns>
	bool IsEmpty(string input)
	{
		if (RemovePunctuation(input).Length != 0) 
			return false;

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
		if (RemovePunctuation(input).Length % 4 == 0)
			return true;

		SetErrorLabel("Input is not divisible by 4! It can't possibly be Gedagi language! (One character in gedagi language must consist of 4 letters)");
		return false;
	}

	string RemovePunctuation(string input)
	{
		return new string(input.Where(x => Char.IsLetter(x)).ToArray());
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

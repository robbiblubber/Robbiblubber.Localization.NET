namespace Robbiblubber.Localization.Controls;

using System.Runtime.Versioning;



public partial class LocaleProvider
{
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [enum] _Kind                                                                                                     //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This enumeration defines the localized values of a component.</summary>
	private enum _Kind: int
	{
		/// <summary>Text.</summary>
		TEXT = 0,
		/// <summary>Accessible name.</summary>
		ACCESSIBLE_NAME = 1,
		/// <summary>Accessible description.</summary>
		ACCESSIBLE_DESCRIPTION = 2
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _Slot                                                                                                    //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class stores the symbol and the default text of a localized value.</summary>
	[SupportedOSPlatform("windows")]
	private sealed class _Slot
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets or sets the symbol, an empty string if no symbol is set.</summary>
		public string Symbol
		{
			get; set;
		} = string.Empty;


		/// <summary>Gets or sets the default text.</summary>
		public string? DefaultText
		{
			get; set;
		} = null;


		/// <summary>Gets or sets if the default text has been captured.</summary>
		public bool Captured
		{
			get; set;
		} = false;



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public methods                                                                                               //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Removes the symbol and the default text.</summary>
		public void Clear()
		{
			Symbol = string.Empty;
			DefaultText = null;
			Captured = false;
		}
	}



	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// [class] _Entry                                                                                                   //
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/// <summary>This class stores the localized values of a component.</summary>
	[SupportedOSPlatform("windows")]
	private sealed class _Entry
	{
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// private members                                                                                              //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Slots by value kind.</summary>
		private readonly _Slot[] _Slots = new _Slot[] { new(), new(), new() };



		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		// public properties                                                                                            //
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>Gets the slot for a value kind.</summary>
		/// <param name="kind">Value kind.</param>
		public _Slot this[_Kind kind]
		{
			get { return _Slots[(int) kind]; }
		}


		/// <summary>Gets if no symbol is set.</summary>
		public bool IsEmpty
		{
			get { return _Slots.All(m => m.Symbol.Length == 0); }
		}
	}
}

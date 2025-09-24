using EQProtocol.Streams.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static OpenEQ.Netcode.Utility;

// C++ Structure Definition:
// struct OnLevelMessage_Struct {
// /*0000*/	char    Title[128];
// /*0128*/	char    Text[4096];
// /*4224*/	char	ButtonName0[25];	// If Buttons = 1, these two are the text for the left and right buttons respectively
// /*4249*/	char	ButtonName1[25];
// /*4274*/	uint8	Buttons;
// /*4275*/	uint8	SoundControls;	// Something to do with audio controls
// /*4276*/	uint32  Duration;
// /*4280*/	uint32  PopupID;	// If none zero, a response packet with 00 00 00 00 <PopupID> is returned on clicking the left button
// /*4284*/	uint32  NegativeID;	// If none zero, a response packet with 01 00 00 00 <NegativeID> is returned on clicking the right button
// /*4288*/	uint32  Unknown4288;
// /*4292*/
// };

// ENCODE/DECODE Section:
// ENCODE(OP_OnLevelMessage)
// {
// ENCODE_LENGTH_EXACT(OnLevelMessage_Struct);
// SETUP_DIRECT_ENCODE(OnLevelMessage_Struct, structs::OnLevelMessage_Struct);
// 
// memcpy(eq->Title, emu->Title, sizeof(eq->Title));
// memcpy(eq->Text, emu->Text, sizeof(eq->Text));
// OUT(Buttons);
// OUT(SoundControls);
// OUT(Duration);
// OUT(PopupID);
// OUT(NegativeID);
// // These two field names are used if Buttons == 1.
// OUT_str(ButtonName0);
// OUT_str(ButtonName1);
// 
// FINISH_ENCODE();
// }

namespace OpenEQ.Netcode {
	/// <summary>
	/// Represents the OnLevelMessage packet structure for EverQuest network communication.
	/// </summary>
	public struct OnLevelMessage : IEQStruct {
		/// <summary>
		/// Gets or sets the title value.
		/// </summary>
		public byte[] Title { get; set; }

		/// <summary>
		/// Gets or sets the text value.
		/// </summary>
		public byte[] Text { get; set; }

		/// <summary>
		/// Gets or sets the buttonname0 value.
		/// </summary>
		public byte[] Buttonname0 { get; set; }

		/// <summary>
		/// Gets or sets the buttonname1 value.
		/// </summary>
		public byte[] Buttonname1 { get; set; }

		/// <summary>
		/// Gets or sets the buttons value.
		/// </summary>
		public byte Buttons { get; set; }

		/// <summary>
		/// Gets or sets the soundcontrols value.
		/// </summary>
		public byte Soundcontrols { get; set; }

		/// <summary>
		/// Gets or sets the duration value.
		/// </summary>
		public uint Duration { get; set; }

		/// <summary>
		/// Gets or sets the popupid value.
		/// </summary>
		public uint Popupid { get; set; }

		/// <summary>
		/// Gets or sets the negativeid value.
		/// </summary>
		public uint Negativeid { get; set; }

		/// <summary>
		/// Initializes a new instance of the OnLevelMessage struct with specified field values.
		/// </summary>
		/// <param name="Title">The title value.</param>
		/// <param name="Text">The text value.</param>
		/// <param name="ButtonName0">The buttonname0 value.</param>
		/// <param name="ButtonName1">The buttonname1 value.</param>
		/// <param name="Buttons">The buttons value.</param>
		/// <param name="SoundControls">The soundcontrols value.</param>
		/// <param name="Duration">The duration value.</param>
		/// <param name="PopupID">The popupid value.</param>
		/// <param name="NegativeID">The negativeid value.</param>
		public OnLevelMessage(byte[] Title, byte[] Text, byte[] ButtonName0, byte[] ButtonName1, byte Buttons, byte SoundControls, uint Duration, uint PopupID, uint NegativeID) : this() {
			Title = Title;
			Text = Text;
			Buttonname0 = ButtonName0;
			Buttonname1 = ButtonName1;
			Buttons = Buttons;
			Soundcontrols = SoundControls;
			Duration = Duration;
			Popupid = PopupID;
			Negativeid = NegativeID;
		}

		/// <summary>
		/// Initializes a new instance of the OnLevelMessage struct from binary data.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public OnLevelMessage(byte[] data, int offset = 0) : this() {
			Unpack(data, offset);
		}

		/// <summary>
		/// Initializes a new instance of the OnLevelMessage struct from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public OnLevelMessage(BinaryReader br) : this() {
			Unpack(br);
		}

		/// <summary>
		/// Unpacks the struct data from a byte array.
		/// </summary>
		/// <param name="data">The binary data to unpack.</param>
		/// <param name="offset">The offset in the data to start unpacking from.</param>
		public void Unpack(byte[] data, int offset = 0) {
			using(var ms = new MemoryStream(data, offset, data.Length - offset)) {
				using(var br = new BinaryReader(ms)) {
					Unpack(br);
				}
			}
		}

		/// <summary>
		/// Unpacks the struct data from a BinaryReader.
		/// </summary>
		/// <param name="br">The BinaryReader to read data from.</param>
		public void Unpack(BinaryReader br) {
			// TODO: Array reading for Title - implement based on actual array size
			// Title = new byte[size];
			// TODO: Array reading for Text - implement based on actual array size
			// Text = new byte[size];
			// TODO: Array reading for Buttonname0 - implement based on actual array size
			// Buttonname0 = new byte[size];
			// TODO: Array reading for Buttonname1 - implement based on actual array size
			// Buttonname1 = new byte[size];
			Buttons = br.ReadByte();
			Soundcontrols = br.ReadByte();
			Duration = br.ReadUInt32();
			Popupid = br.ReadUInt32();
			Negativeid = br.ReadUInt32();
		}

		/// <summary>
		/// Packs the struct data into a byte array.
		/// </summary>
		/// <returns>A byte array containing the packed struct data.</returns>
		public byte[] Pack() {
			using(var ms = new MemoryStream()) {
				using(var bw = new BinaryWriter(ms)) {
					Pack(bw);
					return ms.ToArray();
				}
			}
		}

		/// <summary>
		/// Packs the struct data into a BinaryWriter.
		/// </summary>
		/// <param name="bw">The BinaryWriter to write data to.</param>
		public void Pack(BinaryWriter bw) {
			// TODO: Array writing for Title - implement based on actual array size
			// foreach(var item in Title) bw.Write(item);
			// TODO: Array writing for Text - implement based on actual array size
			// foreach(var item in Text) bw.Write(item);
			// TODO: Array writing for Buttonname0 - implement based on actual array size
			// foreach(var item in Buttonname0) bw.Write(item);
			// TODO: Array writing for Buttonname1 - implement based on actual array size
			// foreach(var item in Buttonname1) bw.Write(item);
			bw.Write(Buttons);
			bw.Write(Soundcontrols);
			bw.Write(Duration);
			bw.Write(Popupid);
			bw.Write(Negativeid);
		}

		/// <summary>
		/// Returns a string representation of the struct with all field values.
		/// </summary>
		/// <returns>A formatted string containing all field names and values.</returns>
		public override string ToString() {
			var ret = "struct OnLevelMessage {\n";
			ret += "	Title = ";
			try {
				ret += $"{ Indentify(Title) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Text = ";
			try {
				ret += $"{ Indentify(Text) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Buttonname0 = ";
			try {
				ret += $"{ Indentify(Buttonname0) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Buttonname1 = ";
			try {
				ret += $"{ Indentify(Buttonname1) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Buttons = ";
			try {
				ret += $"{ Indentify(Buttons) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Soundcontrols = ";
			try {
				ret += $"{ Indentify(Soundcontrols) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Duration = ";
			try {
				ret += $"{ Indentify(Duration) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Popupid = ";
			try {
				ret += $"{ Indentify(Popupid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			ret += "	Negativeid = ";
			try {
				ret += $"{ Indentify(Negativeid) },\n";
			} catch(NullReferenceException) {
				ret += "!!NULL!!\n";
			}
			
			return ret;
		}
	}
}
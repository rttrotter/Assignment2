﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
/************************************************************
 * Assignment 1
 * Programmers: Robert Tyler Trotter z1802019
 *              Mitchell Trafton     z1831076
 ***********************************************************/

namespace Assignment2

{
	public class GameFile
	{
		//constructor
		public GameFile()
		{

			LoadIn();

		}

		/***************************************************************
		 * LoadIn()
		 * purpose: initialize our dictionaries by reading the files in 
		 * bin/init, for the player and item classes, as well as our guilds
		 * 
		 ***************************************************************/
		private void LoadIn()
		{
			try
			{
				//loading in the guilds into our Dictionary for use
				using (StreamReader inGuild = new StreamReader("init/guilds.txt"))
				{
					string line;
					uint id;
					string name;
					//we go line by line and begin creating our guilds from the file
					while ((line = inGuild.ReadLine()) != null)
					{
						string[] subs = line.Split('\t');
						id = UInt32.Parse(subs[0]);
						name = subs[1];
						Globals.guilds.Add(id, new Guild(id, (GuildType)0, name));
					}

				}
			}
			catch (Exception e)
			{
				Console.WriteLine("guilds.txt could not be read");
				Console.WriteLine(e.Message);
			}

			try
			{
				//loading in the items into our Dictionary for use
				using (StreamReader inItems = new StreamReader("init/equipment.txt"))
				{
					string line;
					uint id;
					string name;
					int type;
					uint ilvl;
					uint primary;
					uint stamina;
					uint requirement;
					string flavor;
					//this goes line by line to split up the variables so we can assign them properly within our items objects
					while ((line = inItems.ReadLine()) != null)
					{
						string[] subs = line.Split('\t');
						id = UInt32.Parse(subs[0]);
						name = subs[1];
						type = Int32.Parse(subs[2]);
						ilvl = UInt32.Parse(subs[3]);
						primary = UInt32.Parse(subs[4]);
						stamina = UInt32.Parse(subs[5]);
						requirement = UInt32.Parse(subs[6]);
						flavor = subs[7];
						Globals.items.Add(id, new Item(id, name, type, ilvl, primary, stamina, requirement, flavor));
					}
					Globals.items.Add(0, new Item(0, "N/A", 13, 0, 0, 0, 0, "N/A")); // handles no item available
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("equipment.txt could not be read");
				Console.WriteLine(e.Message);
			}
			try
			{ 
				//importing Players now
				using (StreamReader inCharacter = new StreamReader("init/players.txt"))
				{
					string line;
					uint id;
					string name;
					int race;
					int cclass;
					uint level;
					uint exp;
					uint? guildID;
					uint[] gearSlots;
					uint[] inventory = new uint[Constants.MAX_INVENTORY_SIZE];

					while ((line = inCharacter.ReadLine()) != null)
					{
						string[] subs = line.Split('\t');
						id = UInt32.Parse(subs[0]);
						name = subs[1];
						race = Int32.Parse(subs[2]);
						cclass = Int32.Parse(subs[3]);
						level = UInt32.Parse(subs[4]);
						exp = UInt32.Parse(subs[5]);
						guildID = UInt32.Parse(subs[6]);
						
						gearSlots = new uint[subs.Length - 7];
						/*
						for(int i = 7; i<subs.Length-1; i++)//the rest of the file is inventory, this will record the IDs of player inventory and store them
                        {
							gearSlots[i-7] = UInt32.Parse(subs[i]);
                        }
						*/
						Globals.characters.Add(id, new Player(id, name, (Race?)race, level, exp, guildID, gearSlots, inventory, (Class)cclass, Constants.allowedRolls[(Class)cclass][0]));
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Characters.txt could not be read");
				Console.WriteLine(e.Message);
			}

		}
		/*****************************************************************
		 * void PrintItems()
		 * this will loop through the items dictionary and print out each
		 * entry using the ToString overwrite we did within that file
		 ****************************************************************/
		public List<string> PrintItems()
        {
			List<string> itemList = new List<string>();//list of items; return variable

			foreach(KeyValuePair<uint, Item> item in Globals.items)
            {
				itemList.Add(item.ToString());
            }
			itemList.Add("End of items");

			return itemList;
        }
		/****************************************************************
		* void PrintGuild() 
		* This will loop through each guild entry and print out the string
		* holding the guild name.
		*****************************************************************/
		public List<string> PrintGuild()
        {
			List<string> guildList = new List<string>();//list of guilds; return variable

			foreach(KeyValuePair<uint, string> guild in Globals.guilds)
            {
				guildList.Add(guild.Value);
            }
			guildList.Add("End of Guilds");

			return guildList;
        }
		/****************************************************************
		 * List<string> PrintPlayer()
		 * this will loop through each player entry in the dictionary
		 * using the ToString override within that class, and place it
		 * in a list of strings for use.
		 ****************************************************************/
		public List<string> PrintPlayer()
        {
			List<string> playerList = new List<string>(); //list of strings containing player information; return variable

			foreach(KeyValuePair<uint, Player> character in Globals.characters)
            {
				playerList.Add(character.Value.ToString());
            }
			playerList.Add("End of players");

			return playerList;
        }
		/***************************************************************************
		 * public int SelectPlayer()
		 * returns: int version of a Dictionary Player ID, or -1 for invalid entry
		 * This method lists out all the available characters within the dictionary
		 * for a user to choose from, and then passes that choice back to menu, where
		 * it can then pass it further to another method for use. 
		 * 
		 * 
		 *************************************************************************/
		public int SelectPlayer()
        {
			int selection = 0;//used for user selection
			string userIn;//used to catch user input
			Dictionary<int, uint> translate = new Dictionary<int, uint>();//used to match user input to the key of the character for selection
			foreach(KeyValuePair<uint, Player> character in Globals.characters)
            {
				Console.WriteLine(selection + " " + character.Value.Name);
				translate.Add(selection, character.Key);// match the character key with a shorthand user selection
				selection++;
			}
			Console.WriteLine("Please select the Player: ");
			userIn = Console.ReadLine();
			//try block for input scrubbing, if we catch an exception then we kick back out to menu with no changes done
            try 
			{
				selection = Int32.Parse(userIn);
				if(selection < 0 || selection >= Globals.characters.Count)
                {
					Console.WriteLine("Invalid entry, returning to Menu with no changes");
                }
                else
                {
					return (int)translate[selection];
                }
			}
			catch(Exception e)
            {
				Console.WriteLine("Invalid entry, returning to Menu with no changes");
            }
			return -1;
		}
		/***********************************************************************
		 * public int SelectGuild
		 * input: none
		 * returns: -1 on failure or the ID of a guild cast as an int
		 * The user is given numerical options to select which guild they'd like
		 * to use in whatever function is called. this is passed back.
		 * 
		 * 
		 **********************************************************************/
		public int SelectGuild()
		{
			int selection = 0;//used for user selection
			string userIn;//used to catch user input
			Dictionary<int, uint> translate = new Dictionary<int, uint>();//used to match user input to the key of the character for selection
			foreach (KeyValuePair<uint, string> guild in Globals.guilds)
			{
				Console.WriteLine(selection + " " + guild.Value);
				translate.Add(selection, guild.Key);// match the character key with a shorthand user selection
				selection++;
			}
			Console.WriteLine("Please select the guild");
			userIn = Console.ReadLine();
			//try block for input scrubbing, if we catch an exception then we kick back out to menu with no changes done
			try
			{
				selection = Int32.Parse(userIn);
				if (selection < 0 || selection >= Globals.guilds.Count)
				{
					Console.WriteLine("Invalid entry, returning to Menu with no changes");
				}
				else
				{
					return (int)translate[selection];
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("Invalid entry, returning to Menu with no changes");
			}
			return -1;
		}
		/*************************************************************************
		 * public int selectGear
		 * input: none
		 * returns: -1 on failure or GearID cast to an int
		 * this method lists all the gear available for the user to select from using 
		 * a menu choice and then returns the selected gear's id to menu
		 * 
		 * 
		 *************************************************************************/
		public int SelectGear()
		{
			int selection = 0;//used for user selection
			string userIn;//used to catch user input
			Dictionary<int, uint> translate = new Dictionary<int, uint>();//used to match user input to the key of the character for selection
			foreach (KeyValuePair<uint, Item> item in Globals.items)
			{
				Console.WriteLine(selection + " " + item.Value.Name);
				translate.Add(selection, item.Key);// match the character key with a shorthand user selection
				selection++;
			}
			Console.WriteLine("Please select the Gear");
			userIn = Console.ReadLine();
			//try block for input scrubbing, if we catch an exception then we kick back out to menu with no changes done
			try
			{
				selection = Int32.Parse(userIn);
				if (selection < 0 || selection >= Globals.items.Count)
				{
					Console.WriteLine("Invalid entry, returning to Menu with no changes");
				}
				else
				{
					return (int)translate[selection];
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("Invalid entry, returning to Menu with no changes");
			}
			return -1;
		}

		/************************************************************************
		* public void LeaveGuild
		* input: uint player id in the character dictionary
		* this code sets the guild ID to zero, effectively removing a player 
		* from a guild.
		* *********************************************************************/
		public void LeaveGuild(uint pid)
		{
			Globals.characters[pid].GuildID = 0;
			Console.WriteLine(Globals.characters[pid].Name + " Is no longer in a guild.");
		}

		/************************************************************************
		 * public void JoinGuild
		 * input: uint pid(player ID), gid(guild ID)
		 * This assigns the character's guildID field to the one the user specified
		 * and prints a confirmation message
		 * *********************************************************************/
		public string JoinGuild(uint pid, uint gid)
        {
			Globals.characters[pid].GuildID = gid;
			return Globals.characters[pid].Name + " is now a member of " + Globals.guilds[gid].Name;
        }
		/**********************************************************************
		 * public void AddExp
		 * input uint pid (Player ID)
		 * this method prompts the user for a value of experience to assign
		 * to a player, and then returns a confirmation of the player's level
		 * and current experience
		 * *********************************************************************/
		public void AddExp(uint pid)
        {
			int xp;
			Console.WriteLine("How much experience would you like to award?");
            try
            {
				xp = Int32.Parse(Console.ReadLine());// read what the user wrote
				if(xp > 0)
                {
					Globals.characters[pid].Exp = (uint)xp;
					Console.WriteLine(Globals.characters[pid].Name + " is level: " + Globals.characters[pid].Level + " And has: " + Globals.characters[pid].Exp + "Experience");
                }
            }
			catch(Exception e)
            {

            }
        }
		/******************************************************************
		 * void Disband guild
		 * input: uint gid, the guild id
		 * this will first identify if the gid is valid, then will go through
		 * the player dictionary and identify members of the guild, setting their
		 * guild ID to 0 before printing a success message, and then removing 
		 * the guild from the guild dictionary
		 ******************************************************************/
		public void DisbandGuild(uint gid)
        {
			if(Globals.guilds.ContainsKey(gid))
            {
				foreach (KeyValuePair<uint, Player> character in Globals.characters)
                {
					if (character.Value.GuildID == gid) character.Value.GuildID = 0;
                }
				Console.WriteLine("Guild: " + Globals.guilds[gid].Name + " Successfully removed");
				Globals.guilds.Remove(gid);
			}
			else
            {
				Console.WriteLine("Error: guild with ID: " + gid + "Not found");
            }
        }
		/**********************************************************************
		 * Public Void Sorting()
		 * no inputs
		 * This function places both items and players into array lists, sorts them
		 * using the overloaded Icomparables that are written, and prints them out
		 * 
		 **********************************************************************/
		public void Sorting()
        {
			ArrayList itemList = new ArrayList();
			ArrayList playerList = new ArrayList();
			foreach(KeyValuePair<uint, Item> item in Globals.items)
            {
				itemList.Add(item.Value);
            }
			foreach(KeyValuePair<uint, Player> player in Globals.characters)
            {
				playerList.Add(player.Value);

            }
			itemList.Sort();
			playerList.Sort();

			foreach(Item item in itemList)
            {
				Console.WriteLine(item.ToString());
            }
			foreach(Player player in playerList)
            {
				Console.WriteLine(player.ToString());
            }
        }

	}
}
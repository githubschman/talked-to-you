﻿using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Characters;
using Microsoft.Xna.Framework.Input;
using StardewValley.Menus;
using System.Text;

namespace TalkedToyou
{
	
	public class ModEntry : Mod
	{
		private MouseState MState;

        private Point ClickPoint => new Point(this.MState.X, this.MState.Y);


		public override void Entry(IModHelper helper)
			{
				ControlEvents.KeyPressed += this.ControlEvents_KeyPress;
				ControlEvents.MouseChanged += this.ControlEvents_MouseChanged;
			}
			private void ControlEvents_MouseChanged(object sender, EventArgsMouseStateChanged e)
			{
			
			}

        private static System.Collections.Generic.Stack<Dialogue> GetCurrentDialogue(NPC character)
        {
            return character.CurrentDialogue;
        }
		
		// includes characters you talked to
		// MOVED THIS TO GLOBAL SCOPE? made it an empty string array.
		public String[] TalkedToYou = {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"}; 

        private void ControlEvents_KeyPress(object sender, EventArgsKeyPressed e)
        {

            if (Context.IsWorldReady && Context.IsPlayerFree && e.KeyPressed.ToString() == "Q")
            {
				
				
				var allCharacters = new String[27] {"Elliott", "Shane", "Sebastian", "Harvey", "Abigail", "Emily", "Haley", "Leah", "Maru", "Penny", "Caroline", "Clint", "Demetrius", "Evelyn", "George", "Gus", "Jas", "Jodi", "Lewis", "Marnie", "Linus", "Pam", "Pierre", "Robin", "Vincent", "Willy", "Wizard"};

				string currLocation = Game1.currentLocation.ToString();
				this.Monitor.Log($"The current location is {currLocation}!");
				String[] locArr = currLocation.Split('.');
				String location = locArr[2];

				
				if (Context.IsWorldReady) {

				// should include ALL characters and players children
				var NotTalked = new String[27] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};
				
				// if the person is here, they'll be in this array. resets on every location change.
				var isHere = new String[27] {"null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null", "null"};

				if(Game1.newDay){
					this.Monitor.Log("New day, making all the talk arrays null!");
					int x = 0;
					while(x >= 26){
						NotTalked[x] = null;
						TalkedToYou[x] = null;
						isHere[x] = null;
						x++;
					}
				}


				foreach (NPC character in Game1.currentLocation.characters)
					{

						if(character.ToString() == "StardewValley.NPC" && !(character.ToString() == "StardewValley.Characters.Child"))
						{
							this.Monitor.Log($"{character.name} is here!");
							int talkStack = GetCurrentDialogue(character).Count;

							// add name to isHere array
							int j = 0;
							foreach (String name in isHere){
								if(isHere[j] == character.name){
									this.Monitor.Log($"{character.name} is already in the isHere array!");
									break;
								}

								if(isHere[j] == "null"){
									this.Monitor.Log($"adding {character.name} to the ishere array because they are here!");
									
									isHere[j] = character.name;
									String check = isHere[j];
									this.Monitor.Log($"confirming {check} is on the list");
									break;
								}
								
								j++;
							}

							
							// if they have no more dialogue...
							if(talkStack <= 0){
								int k = 0;
								foreach(String name in TalkedToYou) {


									this.Monitor.Log($"Going through TalkedToYou!, checking on Not Talked: {NotTalked[k]}");

									// if the person with no dialogue is in the not talked array, take them out
									if(NotTalked[k] == character.name){
										this.Monitor.Log($"you talked to {character.name} so we are removing them from the Not Talked array!");
										NotTalked[k] = "null";
									}
									// and put them in the talked to array
									if(TalkedToYou[k] == "null"){
											TalkedToYou[k] = character.name;
									
											String check2 = TalkedToYou[k];
											// THIS WORKS!
											break;
										}
										k++;
									}
								this.Monitor.Log($"You talked to {character.name} today!");
													
							}else{
								this.Monitor.Log($"You haven't spoken to {character.name} and they are here!");
							}
						}

					}

					foreach(String name in allCharacters){
						
						int y = 0;
						bool talkedTo = false;

						while(y<=26){

							string check = TalkedToYou[y];
							this.Monitor.Log($"looking to see if {name} is in talked to you, at {check}");

							// if they are in the arrays talkedtoyou, don't include in nottalked
							if(TalkedToYou[y] == name){
								this.Monitor.Log($"Yep, you talked to {name}!");
								talkedTo = true;
								// THIS WORKS!
								break;
							}
							y++;
						}
						if(talkedTo == false){
							int z = 0;
							while(z <= 26){
								string check2 = NotTalked[z];
								this.Monitor.Log($"NOT talked to array: {check2}");
								if(NotTalked[z] == "null"){
									this.Monitor.Log($"havent talked to {name} yet today, adding them to the not talked to array!");
									NotTalked[z] = name;
									break;
								}
								z++;
							}
						}else if(talkedTo == true){
							// Talked to whomever, so remove them from notTalked
							int s = 0;
							while(s <= 26){
								if(NotTalked[s] == name){
									this.Monitor.Log($"Talkd to {name} so removing them from Not Talked, lol this is crazy");
									NotTalked[s] = "null";
								}
								s++;
							}
						}
					}

						// NotTalked, isHere, & TalkedToYou	
						String talkedToHere;
						String notTalkedToHere = "";

						foreach(String name in isHere){
							int f = 0;
							int num = 1;

							while(f <= 26){
								this.Monitor.Log($"loopin thru this, to up to: {name}");
								if(name == NotTalked[f] && name != "null"){
									this.Monitor.Log($"adding {name} to this janky ass string");
									notTalkedToHere += num + ". " + name;
									num++;
								}
								f++;
							}
						}

						Game1.drawObjectDialogue($"Here are the players in {location} who you haven't talked to: {notTalkedToHere}");
					
				}
            }
       	 }
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Bannerlord_Mod_Prefixes {
    class Program {
        static string[] updatePaths = {@"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\SandBoxCore\ModuleData\spnpccharacters.xml",
               @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\SandBox\ModuleData\lords.xml",
               @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\SandBox\ModuleData\spspecialcharacters.xml",
               @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\StoryMode\ModuleData\storymode_characters.xml",
			   @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\SandBox\ModuleData\bandits.xml"};

        static string modPath = @"C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\Modules\NativeUnitsNamePrefixes\ModuleData\nativeunitsnameprefixes.xml";


		static void Main(string[] args) {
			NPCCharacters npcs = new NPCCharacters();
			NPCCharacters updatesFrom = new NPCCharacters();
			NPCCharacters updatesTo = new NPCCharacters();
			updatesTo.NPCCharacter = new List<NPCCharacter>();
			List<string> ids = new List<string>();
			List<string> names = new List<string>();

			using (FileStream fs = File.OpenRead(modPath)) {
                XmlSerializer deserializer = new XmlSerializer(typeof(NPCCharacters));
                npcs = (NPCCharacters)deserializer.Deserialize(fs);
                fs.Close();
            }

			for (int i = 0; i < npcs.NPCCharacter.Count; i++) {
				ids.Add(npcs.NPCCharacter[i].Id);
				names.Add(npcs.NPCCharacter[i].Name);
			}

			foreach(string u in updatePaths) {
				using (FileStream fs = File.OpenRead(u)) {
					XmlSerializer deserializer = new XmlSerializer(typeof(NPCCharacters));
					updatesFrom = (NPCCharacters)deserializer.Deserialize(fs);
					fs.Close();
				}

				for (int i = 0; i < updatesFrom.NPCCharacter.Count; i++) {
					for (int j = 0; j < ids.Count; j++) {
						if (updatesFrom.NPCCharacter[i].Id == ids[j]) {
							updatesTo.NPCCharacter.Add(updatesFrom.NPCCharacter[i]);
							break;
						}
					}
				}
			}

			for (int i = 0; i < updatesTo.NPCCharacter.Count; i++) {
				var up = updatesTo.NPCCharacter[i];
				string type;
				int tier = 1;

				if (up.Level != null) { tier = Int32.Parse(up.Level) / 5; }
				if (tier == 0) tier = 1;

				switch (up.Default_group) {
					case "Infantry": type = "I";  break;
					case "Ranged": type = "A"; break;
					case "Cavalry": type = "C"; break;
					case "HorseArcher": type = "H"; break;
					default: type = "####DEBUG####"; break;
				}

                #region HANDLE EXCEPTIONS (developer's fault)
				switch (up.Id) {
					case "steppe_bandits_raider": type = "H";  break;
					case "steppe_bandits_chief": type = "H"; break;
					case "steppe_bandits_boss": type = "H"; break;
					case "tutorial_placeholder_volunteer": type = "C"; break;
					case "aserai_youth": type = "C"; break;
					case "aserai_mameluke_regular": type = "C"; break;
					case "jawwal_tier_2": type = "C"; break;
					case "tutorial_npc_ranged": type = "C"; break;
					case "tutorial_npc_basic_melee": type = "C"; break;
					case "tutorial_npc_advanced_melee_easy": type = "C"; break;
					case "tutorial_npc_advanced_melee_normal": type = "C"; break;
					case "company_of_the_boar_tier_1": type = "A"; break;
					case "guardians_tier_1": type = "A"; break;
					case "guardians_tier_2": type = "A"; break;
					case "guardians_tier_3": type = "A"; break;
					case "vlandian_levy_crossbowman": type = "A"; break;
					case "wolfskins_tier_1": type = "A"; break;
					case "wolfskins_tier_2": type = "A"; break;
					case "wolfskins_tier_3": type = "A"; break;
					case "mercenary_2": type = "A"; break;
					case "mercenary_5": type = "A"; break;
					case "sturgian_woodsman": type = "I"; break;
					case "galloglass_tier_2": type = "I"; break;
					case "forest_bandits_boss": type = "I"; break;
					case "deserter": type = "I"; break;
					case "mountain_bandits_boss": type = "I"; break;
					case "desert_bandits_boss": type = "I"; break;
					default: break;
				}
                #endregion

                up.Name = up.Name.Insert(up.Name.IndexOf(@"}") + 1, (type + tier + " "));
			}

			using (Stream fs = new FileStream(modPath, FileMode.Create, FileAccess.Write, FileShare.None)) {
                XmlSerializer serializer = new XmlSerializer(typeof(NPCCharacters));
				serializer.Serialize(fs, updatesTo);
				//serializer.Serialize(fs, npcs);
				fs.Close();
            }

			for (int i = 0; i < names.Count; i++) {
				names[i] = names[i].Remove(0, 11);
				Console.WriteLine(names[i]);
			}

			Console.WriteLine("Task done.");
        }
    }
	#region NPCCharacters class
	[XmlRoot(ElementName = "NPCCharacters")]
	public class NPCCharacters {
		[XmlElement(ElementName = "NPCCharacter")]
		public List<NPCCharacter> NPCCharacter { get; set; }
	}

	[XmlRoot(ElementName = "NPCCharacter")]
	public class NPCCharacter {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "default_group")]
		public string Default_group { get; set; }
		[XmlAttribute(AttributeName = "level")]
		public string Level { get; set; }
		[XmlAttribute(AttributeName = "occupation")]
		public string Occupation { get; set; }
		[XmlAttribute(AttributeName = "culture")]
		public string Culture { get; set; }
		[XmlAttribute(AttributeName = "formation_position_preference")]
		public string Formation_position_preference { get; set; }
		[XmlAttribute(AttributeName = "is_template")]
		public string Is_template { get; set; }
		[XmlAttribute(AttributeName = "is_hero")]
		public string Is_hero { get; set; }
		[XmlAttribute(AttributeName = "is_basic_troop")]
		public string Is_basic_troop { get; set; }
		[XmlAttribute(AttributeName = "is_female")]
		public string Is_female { get; set; }
		[XmlAttribute(AttributeName = "age")]
		public string Age { get; set; }
		[XmlAttribute(AttributeName = "banner_symbol_mesh_name")]
		public string Banner_symbol_mesh_name { get; set; }
		[XmlAttribute(AttributeName = "banner_symbol_color")]
		public string Banner_symbol_color { get; set; }
		[XmlAttribute(AttributeName = "civilianTemplate")]
		public string CivilianTemplate { get; set; }
		[XmlAttribute(AttributeName = "battleTemplate")]
		public string BattleTemplate { get; set; }
		[XmlAttribute(AttributeName = "skill_template")]
		public string Skill_template { get; set; }
		[XmlAttribute(AttributeName = "voice")]
		public string Voice { get; set; }
		[XmlElement(ElementName = "equipmentSet")]
		public List<EquipmentSet> EquipmentSet { get; set; }
		[XmlElement(ElementName = "face")]
		public Face Face { get; set; }
		[XmlElement(ElementName = "skills")]
		public Skills Skills { get; set; }
		[XmlElement(ElementName = "Traits")]
		public Traits Traits { get; set; }
		[XmlElement(ElementName = "upgrade_targets")]
		public Upgrade_targets Upgrade_targets { get; set; }
	}

	[XmlRoot(ElementName = "equipmentSet")]
	public class EquipmentSet {
		[XmlElement(ElementName = "equipment")]
		public List<Equipment> Equipment { get; set; }
		[XmlAttribute(AttributeName = "civilian")]
		public string Civilian { get; set; }
	}

	[XmlRoot(ElementName = "equipment")]
	public class Equipment {
		[XmlAttribute(AttributeName = "slot")]
		public string Slot { get; set; }
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "amount")]
		public string Amount { get; set; }
	}

	[XmlRoot(ElementName = "face")]
	public class Face {
		[XmlElement(ElementName = "BodyProperties")]
		public BodyProperties BodyProperties { get; set; }
		[XmlElement(ElementName = "BodyPropertiesMax")]
		public BodyPropertiesMax BodyPropertiesMax { get; set; }
		[XmlElement(ElementName = "face_key_template")]
		public Face_key_template Face_key_template { get; set; }
		[XmlElement(ElementName = "hair_tags")]
		public Hair_tags Hair_tags { get; set; }
		[XmlElement(ElementName = "beard_tags")]
		public Beard_tags Beard_tags { get; set; }
	}

	[XmlRoot(ElementName = "beard_tags")]
	public class Beard_tags {
		[XmlElement(ElementName = "beard_tag")]
		public List<Beard_tag> Beard_tag { get; set; }
	}

	[XmlRoot(ElementName = "beard_tag")]
	public class Beard_tag {
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "BodyProperties")]
	public class BodyProperties {
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "age")]
		public string Age { get; set; }
		[XmlAttribute(AttributeName = "weight")]
		public string Weight { get; set; }
		[XmlAttribute(AttributeName = "build")]
		public string Build { get; set; }
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }
	}

	[XmlRoot(ElementName = "BodyPropertiesMax")]
	public class BodyPropertiesMax {
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "age")]
		public string Age { get; set; }
		[XmlAttribute(AttributeName = "weight")]
		public string Weight { get; set; }
		[XmlAttribute(AttributeName = "build")]
		public string Build { get; set; }
		[XmlAttribute(AttributeName = "key")]
		public string Key { get; set; }
	}

	[XmlRoot(ElementName = "face_key_template")]
	public class Face_key_template {
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "hair_tags")]
	public class Hair_tags {
		[XmlElement(ElementName = "hair_tag")]
		public List<Hair_tag> Hair_tag { get; set; }
	}

	[XmlRoot(ElementName = "hair_tag")]
	public class Hair_tag {
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
	}

	[XmlRoot(ElementName = "skills")]
	public class Skills {
		[XmlElement(ElementName = "skill")]
		public List<Skill> Skill { get; set; }
	}

	[XmlRoot(ElementName = "skill")]
	public class Skill {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "Traits")]
	public class Traits {
		[XmlElement(ElementName = "Trait")]
		public List<Trait> Trait { get; set; }
	}

	[XmlRoot(ElementName = "Trait")]
	public class Trait {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
		[XmlAttribute(AttributeName = "value")]
		public string Value { get; set; }
	}

	[XmlRoot(ElementName = "upgrade_targets")]
	public class Upgrade_targets {
		[XmlElement(ElementName = "upgrade_target")]
		public Upgrade_target Upgrade_target { get; set; }
	}

	[XmlRoot(ElementName = "upgrade_target")]
	public class Upgrade_target {
		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }
	}
	#endregion
}
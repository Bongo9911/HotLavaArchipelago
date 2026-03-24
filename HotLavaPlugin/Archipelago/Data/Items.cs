using HotLavaArchipelagoPlugin.Archipelago.Models.Items;
using HotLavaArchipelagoPlugin.GameData;
using HotLavaArchipelagoPlugin.Models.Game;
using HotLavaArchipelagoPlugin.Properties;
using Klei.HotLava.Audio;
using System.Collections.Generic;
using System.Linq;

namespace HotLavaArchipelagoPlugin.Archipelago.Data
{
    internal static class Items
    {
        private static Dictionary<long, Item>? _allItems = null;

        public static AbilityItem DoubleJump { get; } = new AbilityItem(10, "Double Jump", Resources.DoubleJump);
        public static AbilityItem BoostJump { get; } = new AbilityItem(11, "Boost Jump", Resources.BoostJump);
        public static AbilityItem SlideJump { get; } = new AbilityItem(12, "Slide Jump", Resources.SlideJump);
        public static AbilityItem VaultJump { get; } = new AbilityItem(13, "Vault Jump", Resources.VaultJump);

        public static AbilityItem Crouch { get; } = new AbilityItem(20, "Crouch", Resources.Crouch);
        public static AbilityItem Grab { get; } = new AbilityItem(21, "Grab", Resources.Grab);
        public static AbilityItem Surf { get; } = new AbilityItem(22, "Surf", Resources.Surf);
        public static AbilityItem WallJump { get; } = new AbilityItem(23, "Wall Jump", Resources.WallJump);
        public static AbilityItem Swing { get; } = new AbilityItem(24, "Swing", Resources.Swing);
        public static AbilityItem Climb { get; } = new AbilityItem(25, "Climb", Resources.Climb);

        public static AbilityItem Pogo { get; } = new AbilityItem(30, "Pogo", Resources.Pogo);
        public static AbilityItem TinyToy { get; } = new AbilityItem(31, "Tiny Toy", Resources.TinyToy);
        public static AbilityItem Jetpack { get; } = new AbilityItem(32, "Jetpack", Resources.Jetpack);

        public static List<CharacterItem> CharacterItems { get; } = [
            //new CharacterItem(40, "Hazard", eVoiceCharacter.HAZARD),
            new CharacterItem(41, "Jen Forcer", "skin_jen_forcer_default", eVoiceCharacter.JENFORCER),
            new CharacterItem(42, "Lex Splorer", "skin_lex_splorer_default", eVoiceCharacter.LEXSPLORER),
            new CharacterItem(43, "Sue Nami", "skin_sue_nami_default", eVoiceCharacter.SUENAMI),
            new CharacterItem(44, "Lord Sludge", "skin_lord_sludge_default", eVoiceCharacter.LORDSLUDGE),
            new CharacterItem(45, "Poizone", "skin_poizone_default", eVoiceCharacter.POIZONE),
            new CharacterItem(46, "Infantry", "skin_infantry_default", eVoiceCharacter.INFANTRY),
            new CharacterItem(47, "Megamortabeast", "skin_megamortabeast_default", eVoiceCharacter.MEGAMORTABEAST),
            new CharacterItem(48, "Rambull", "skin_rambull_default", eVoiceCharacter.RAMBULL),
            new CharacterItem(49, "Stink Bomb", "skin_stink_bomb_default", eVoiceCharacter.STINKBOMB),
            new CharacterItem(50, "Venomess", "skin_venomess_default", eVoiceCharacter.VENOMESS),
            new CharacterItem(51, "Tyler Rex", "skin_tyrone_rex_default", eVoiceCharacter.TYRONEREX),
            new CharacterItem(52, "Hera Scarlet", "skin_hera_scarlet_default", eVoiceCharacter.HERASCARLET),
            new CharacterItem(53, "Leo Satomi", "skin_leo_default", eVoiceCharacter.LEO),
        ];

        public static List<CosmeticItem> HeadAccessories { get; } = [
            new CosmeticItem(2000, "Bandana Hair", "accessory_80s_hair_01"),
            new CosmeticItem(2001, "'80s Hair", "accessory_80s_hair_02"),
            new CosmeticItem(2002, "The Natural", "accessory_afro"),
            new CosmeticItem(2003, "Ancient Techno Helmet", "accessory_ancient_helmet"),
            new CosmeticItem(2004, "Angel Halo", "accessory_angel_halo"),
            new CosmeticItem(2005, "Swan Headpiece", "accessory_ballet_swan"),
            new CosmeticItem(2006, "Balloon Glasses", "accessory_balloon_glasses"),
            new CosmeticItem(2007, "Shower Cap", "accessory_bath_hat"),
            new CosmeticItem(2008, "Beaksy", "accessory_beaksy"),
            new CosmeticItem(2009, "Beret", "accessory_beret"),
            new CosmeticItem(2010, "Bicycle Helmet", "accessory_bicycle_helmet"),
            new CosmeticItem(2011, "Rad Hair", "accessory_bikini_hair"),
            new CosmeticItem(2012, "Bow Hair Band", "accessory_bowknot"),
            new CosmeticItem(2013, "Cardboard Knight Helm", "accessory_boxknight_helmet"),
            new CosmeticItem(2014, "Buffalo Skull", "accessory_buffalo_skull"),
            new CosmeticItem(2015, "Bunny Ears and Nose", "accessory_bunny_face"),
            new CosmeticItem(2016, "Bunny Mask", "accessory_bunny_mask"),
            new CosmeticItem(2017, "Calavera", "accessory_calavera"),
            new CosmeticItem(2018, "Cheerleader Hair", "accessory_cheerleader_hair"),
            new CosmeticItem(2019, "Devil Ninja", "accessory_devil_ninja"),
            new CosmeticItem(2020, "Dog Ears and Snout", "accessory_dog_face"),
            new CosmeticItem(2021, "Dragon Horns", "accessory_dragon_horn"),
            new CosmeticItem(2022, "Wilderness Troop Hat", "accessory_drill_instrutor"),
            new CosmeticItem(2023, "Don't Starve Knit Cap", "accessory_ds_tuque"),
            new CosmeticItem(2024, "Eets", "accessory_eets"),
            new CosmeticItem(2025, "Elf Hat", "accessory_elf_hat"),
            new CosmeticItem(2026, "Fez", "accessory_fez"),
            new CosmeticItem(2027, "Fish Tank", "accessory_fish_tank"),
            new CosmeticItem(2028, "Flower Crown", "accessory_flower_crowns"),
            new CosmeticItem(2029, "Kitsune Mask", "accessory_fox_mask"),
            new CosmeticItem(2030, "Grrr-illa Head", "accessory_gorilla_head"),
            new CosmeticItem(2031, "Happy Little Hair", "accessory_happy_little_hair"),
            new CosmeticItem(2032, "Headphones", "accessory_headphone"),
            new CosmeticItem(2033, "Retro Headphones", "accessory_headphone_2"),
            new CosmeticItem(2034, "Holmes Hat", "accessory_holmes_hat"),
            new CosmeticItem(2035, "Jester Hat", "accessory_jester_hat"),
            new CosmeticItem(2036, "King Wig", "accessory_king_wig"),
            new CosmeticItem(2037, "Knight Helm", "accessory_knight_bucket"),
            new CosmeticItem(2038, "Toque", "accessory_knitted_hat"),
            new CosmeticItem(2039, "Leo's Helmet", "accessory_leo_helmet"),
            new CosmeticItem(2040, "Kangaroo Mascot", "accessory_macot_kangaroo"),
            new CosmeticItem(2041, "Mariachi Hat", "accessory_mariachi_hat"),
            new CosmeticItem(2042, "Bobby the Bobcat", "accessory_mascot_tiger"),
            new CosmeticItem(2043, "Turtle Mascot", "accessory_mascot_turtle"),
            new CosmeticItem(2044, "Mask", "accessory_mask"),
            new CosmeticItem(2045, "Motobug Rider Head", "accessory_motobug_head"),
            new CosmeticItem(2046, "Nest", "accessory_nest"),
            new CosmeticItem(2047, "Mark of the Ninja Head", "accessory_ninja"),
            new CosmeticItem(2048, "Octopus", "accessory_octupus"),
            new CosmeticItem(2049, "Party Hat", "accessory_party_hat"),
            new CosmeticItem(2050, "Plague Mask", "accessory_plague_mask"),
            new CosmeticItem(2051, "Plumeria", "accessory_polynesian_1"),
            new CosmeticItem(2052, "Yellow Polynesian", "accessory_polynesian_2"),
            new CosmeticItem(2053, "Pink Polynesian", "accessory_polynesian_3"),
            new CosmeticItem(2054, "My Pretty Pompadour", "accessory_pompadour"),
            new CosmeticItem(2055, "Prospector Hat", "accessory_prospector_hat"),
            new CosmeticItem(2056, "Jack-o'-lantern Head", "accessory_pumpkin"),
            new CosmeticItem(2057, "K.A.P.O.W! Helmet", "accessory_ranger_head"),
            new CosmeticItem(2058, "Helmet", "accessory_rider"),
            new CosmeticItem(2059, "Road Crow Head", "accessory_roadcrow_head"),
            new CosmeticItem(2060, "Rockabilly Wig", "accessory_rockabilly_wig"),
            new CosmeticItem(2061, "Samurai Helmet", "accessory_samurai_helmet"),
            new CosmeticItem(2062, "Hera's Helmet", "accessory_scarlet_helmet"),
            new CosmeticItem(2063, "\"Sheep...\"", "accessory_sheep_head"),
            new CosmeticItem(2064, "Sombrero", "accessory_sombrero"),
            new CosmeticItem(2065, "Swashbuckler Cap", "accessory_swashbuckler_hat"),
            new CosmeticItem(2066, "Thai-ger Face", "accessory_tiger_face"),
            new CosmeticItem(2067, "Turkey Hat", "accessory_turkey_hat"),
            new CosmeticItem(2068, "Umbrella Hat", "accessory_umbrella_hat"),
            new CosmeticItem(2069, "Visor", "accessory_visor"),
            new CosmeticItem(2070, "Virtual Reality Mask", "accessory_vr_mask"),
            new CosmeticItem(2071, "Don't Starve Wilson Mask", "accessory_wilson_mask"),
            new CosmeticItem(2072, "Wizard Hat", "accessory_wizard_hat"),
            new CosmeticItem(2073, "Wolf Ears and Snout", "accessory_wolf_face"),
            new CosmeticItem(2074, "Workout Hair", "accessory_workout_hair"),
            new CosmeticItem(2075, "Holiday Hat", "accessory_xmas_hat"),
            new CosmeticItem(2076, "Bee Antennae", "beta_accessory_bee_feeler"),
            new CosmeticItem(2077, "Boater Hat", "beta_accessory_boaterhat"),
            new CosmeticItem(2078, "Bonnet", "beta_accessory_bonnet"),
            new CosmeticItem(2079, "Bucket Hat", "beta_accessory_bucket_hat"),
            new CosmeticItem(2080, "Cat Ears", "beta_accessory_catears"),
            new CosmeticItem(2081, "Cowboy Hat", "beta_accessory_cowboy_hat"),
            new CosmeticItem(2082, "Orange Daisy", "beta_accessory_daisy_1"),
            new CosmeticItem(2083, "White Daisy", "beta_accessory_daisy_3"),
            new CosmeticItem(2084, "Devil Horns", "beta_accessory_devil_horns"),
            new CosmeticItem(2085, "Football Helmet", "beta_accessory_football_helmet"),
            new CosmeticItem(2086, "Fur Hat", "beta_accessory_fur_hat"),
            new CosmeticItem(2087, "Hard Hat", "beta_accessory_hard_hat"),
            new CosmeticItem(2088, "Laser Knight Helmet", "beta_accessory_laser_knight_helmet"),
            new CosmeticItem(2089, "Military Helmet", "beta_accessory_military_cap"),
            new CosmeticItem(2090, "Monocle", "beta_accessory_monocle"),
            new CosmeticItem(2091, "Paper Hat", "beta_accessory_paper_hat"),
            new CosmeticItem(2092, "Robin Hood Hat", "beta_accessory_robin_hood_hat"),
            new CosmeticItem(2093, "Skater Helmet", "beta_accessory_skaer_helmet"),
            new CosmeticItem(2094, "Tinfoil Hat", "beta_accessory_tinfoil_hat"),
            new CosmeticItem(2095, "Top Hat", "beta_accessory_tophat"),
            new CosmeticItem(2096, "Watch Cap", "beta_accessory_toque_hat"),
            new CosmeticItem(2097, "Horned Helmet", "beta_accessory_viking_helmet"),
        ];

        public static List<CosmeticItem> BackAccessories { get; } = [
            new CosmeticItem(2200, "Ancient Techno Armor", "accessory_ancient_armor"),
            new CosmeticItem(2201, "Don't Starve Ancient Cane", "accessory_ancient_cane"),
            new CosmeticItem(2202, "Angel Wings", "accessory_angel_wings"),
            new CosmeticItem(2203, "Hatchet", "accessory_axe_a"),
            new CosmeticItem(2204, "Carving Axe", "accessory_axe_b"),
            new CosmeticItem(2205, "Wrapped Hatchet", "accessory_axe_c"),
            new CosmeticItem(2206, "\"...Lollipop!\"", "accessory_back_lollipops"),
            new CosmeticItem(2207, "Open Backpack", "accessory_back_to_school_bp"),
            new CosmeticItem(2208, "Balloon Sword & Armor", "accessory_balloon_armor"),
            new CosmeticItem(2209, "Barbarian Armor", "accessory_barbarian_armour"),
            new CosmeticItem(2210, "Don't Starve Bee Fire Staff", "accessory_bee_fire_staff"),
            new CosmeticItem(2211, "Don't Starve Bee Ice Staff", "accessory_bee_ice_staff"),
            new CosmeticItem(2212, "Don't Starve Bee Spear", "accessory_bee_spear"),
            new CosmeticItem(2213, "Cardboard Knight Armor", "accessory_boxknight_chest"),
            new CosmeticItem(2214, "Bunny Body", "accessory_bunny_body"),
            new CosmeticItem(2215, "Don't Starve Walking Cane", "accessory_cane_a"),
            new CosmeticItem(2216, "Cheerleader Top", "accessory_cheerleader_top"),
            new CosmeticItem(2217, "Crab Back Pack", "accessory_crab_backpack"),
            new CosmeticItem(2218, "Bomb", "accessory_da_bomb"),
            new CosmeticItem(2219, "Dog Body", "accessory_dog_body"),
            new CosmeticItem(2220, "Dragon Wings", "accessory_dragon_wings"),
            new CosmeticItem(2221, "Don't Starve Umbrella", "accessory_ds_umbrella"),
            new CosmeticItem(2222, "Don't Starve Eye Bone", "accessory_eye_bone"),
            new CosmeticItem(2223, "Fork", "accessory_fork"),
            new CosmeticItem(2224, "Grrr-illa Chest", "accessory_gorilla_chest"),
            new CosmeticItem(2225, "Don't Starve Ham Bat", "accessory_ham_back"),
            new CosmeticItem(2226, "Happy Little Palette", "accessory_happy_little_palette"),
            new CosmeticItem(2227, "Jester Collar", "accessory_jester_collar"),
            new CosmeticItem(2228, "Jetpack (Accessory)", "accessory_jetpack"),
            new CosmeticItem(2229, "Skeleton Key", "accessory_key"),
            new CosmeticItem(2230, "Royal Cape", "accessory_king_cape"),
            new CosmeticItem(2231, "Klei Fest Shirt", "accessory_klei_fest_2021"),
            new CosmeticItem(2232, "Lollipop", "accessory_lollipop"),
            new CosmeticItem(2233, "Mango Creamsicle", "accessory_mango_creamsicle"),
            new CosmeticItem(2234, "Motobug Armor", "accessory_motobug_armor"),
            new CosmeticItem(2235, "Paint Roller", "accessory_paint_roller"),
            new CosmeticItem(2236, "Panda", "accessory_panda"),
            new CosmeticItem(2237, "Plague Chest Armor", "accessory_plague_chest"),
            new CosmeticItem(2238, "Rainbow Lollipop", "accessory_rainbow_lollipop"),
            new CosmeticItem(2239, "Rainbow Popsicle", "accessory_rainbow_popsicle"),
            new CosmeticItem(2240, "Rainbow Wings", "accessory_rainbow_wings"),
            new CosmeticItem(2241, "Rambull's Vest", "accessory_rambull_vest"),
            new CosmeticItem(2242, "K.A.P.O.W! Chest Plate", "accessory_ranger_chest"),
            new CosmeticItem(2243, "Road Crow Chest", "accessory_roadc_chest"),
            new CosmeticItem(2244, "Rockabilly Microphone", "accessory_rockabilly_microphone"),
            new CosmeticItem(2245, "Sai", "accessory_sai"),
            new CosmeticItem(2246, "Scuba Gear", "accessory_scubatank"),
            new CosmeticItem(2247, "Shovel", "accessory_shovel"),
            new CosmeticItem(2248, "Leaf Spear", "accessory_spear_b"),
            new CosmeticItem(2249, "Crooked Spear", "accessory_spear_c"),
            new CosmeticItem(2250, "Stinkbomb's Keytar", "accessory_stinkbomb_keytar"),
            new CosmeticItem(2251, "Golden Hilted Sword", "accessory_stylish_sword"),
            new CosmeticItem(2252, "Flying V Guitar", "accessory_thrasher"),
            new CosmeticItem(2253, "Thai-ger Chest", "accessory_tiger_chest"),
            new CosmeticItem(2254, "Powder Keg", "accessory_tnt_barrel"),
            new CosmeticItem(2255, "Sue's Ugly Xmas Sweater", "accessory_ugly_sweater"),
            new CosmeticItem(2256, "Tape Player", "accessory_walkman"),
            new CosmeticItem(2257, "Water Blaster", "accessory_water_gun"),
            new CosmeticItem(2258, "Wizard Broom", "accessory_wizard_broom"),
            new CosmeticItem(2259, "Wolf Body", "accessory_wolf_body"),
            new CosmeticItem(2260, "Workout Clothes", "accessory_workout_gear"),
            new CosmeticItem(2261, "Pugly Sweater", "accessory_xmassweater"),
            new CosmeticItem(2262, "Backpack", "beta_accessory_backpack_plain"),
            new CosmeticItem(2263, "Battle Axe", "beta_accessory_battle_axe"),
            new CosmeticItem(2264, "Bazooka", "beta_accessory_bazooka"),
            new CosmeticItem(2265, "Bee Wings", "beta_accessory_bee_wings"),
            new CosmeticItem(2266, "Boom Box", "beta_accessory_boombox"),
            new CosmeticItem(2267, "Gold Boom Box", "beta_accessory_boombox_gold"),
            new CosmeticItem(2268, "Robin Hood Bow", "beta_accessory_bow"),
            new CosmeticItem(2269, "Cat Paw", "beta_accessory_cat_paw"),
            new CosmeticItem(2270, "The Magenta Saber", "beta_accessory_darkstar"),
            new CosmeticItem(2271, "Devil Wings and Pitchfork", "beta_accessory_devil_wand"),
            new CosmeticItem(2272, "Time Bomb", "beta_accessory_dynamite_clock"),
            new CosmeticItem(2273, "Fairy Wings", "beta_accessory_fairywings"),
            new CosmeticItem(2274, "Football Jersey", "beta_accessory_football_jersey"),
            new CosmeticItem(2275, "Acoustic Guitar", "beta_accessory_gitar"),
            new CosmeticItem(2276, "Keytar", "beta_accessory_keytar"),
            new CosmeticItem(2277, "Laser Knight Armour", "beta_accessory_laser_knight_armour"),
            new CosmeticItem(2278, "Glowing Sword and Shield", "beta_accessory_lazersword"),
            new CosmeticItem(2279, "Paper Sword", "beta_accessory_paper_sword"),
            new CosmeticItem(2280, "Parachute Knapsack", "beta_accessory_parachute"),
            new CosmeticItem(2281, "Torpedo", "beta_accessory_rocketpack"),
            new CosmeticItem(2282, "Shark Fin", "beta_accessory_sharkfin"),
            new CosmeticItem(2283, "Skateboard", "beta_accessory_skateboard"),
            new CosmeticItem(2284, "Surfboard", "beta_accessory_surfboard"),
            new CosmeticItem(2285, "Bear Backpack", "beta_accessory_teddybrown"),
            new CosmeticItem(2286, "Viking Shield", "beta_accessory_vikingshield"),
        ];

        public static List<CosmeticItem> Trinkets { get; } = [
            new CosmeticItem(2400, "3D Glasses", "accessory_3dglasses"),
            new CosmeticItem(2401, "8-Bit Glasses", "accessory_8bit_glasses"),
            new CosmeticItem(2402, "Don't Starve's Abigail", "accessory_abigail"),
            new CosmeticItem(2403, "Ancient Techno Gloves", "accessory_ancient_glove"),
            new CosmeticItem(2404, "Angel Wand", "accessory_angel_wand"),
            new CosmeticItem(2405, "Balloon Shoes", "accessory_balloon_feet"),
            new CosmeticItem(2406, "Barbarian Gauntlets", "accessory_barbarian_wristlet"),
            new CosmeticItem(2407, "Bikini", "accessory_bikini"),
            new CosmeticItem(2408, "Black Beard", "accessory_black_beard"),
            new CosmeticItem(2409, "Cardboard Knight Gauntlets", "accessory_boxknight_wrist"),
            new CosmeticItem(2410, "Brainstorm's Glasses", "accessory_brainstorm_glasses"),
            new CosmeticItem(2411, "Brimstone Gilded Book", "accessory_brimstone_guilded_book"),
            new CosmeticItem(2412, "Bunny Tail and Feet", "accessory_bunny_parts"),
            new CosmeticItem(2413, "Cheerleader Dress", "accessory_cheerleader_dress"),
            new CosmeticItem(2414, "The Chevron", "accessory_chevron"),
            new CosmeticItem(2415, "Dog Tail and Feet", "accessory_dog_parts"),
            new CosmeticItem(2416, "Gaming Gauntlet", "accessory_dpad_gauntlet"),
            new CosmeticItem(2417, "Dragon Tail", "accessory_dragon_tail"),
            new CosmeticItem(2418, "Elf Ear", "accessory_elf_ear"),
            new CosmeticItem(2419, "Fox Tail", "accessory_fox_tail"),
            new CosmeticItem(2420, "Don't Starve's Goose/Moose", "accessory_goosemoose"),
            new CosmeticItem(2421, "Grrr-illa Feet", "accessory_gorilla_feet"),
            new CosmeticItem(2422, "Handlebar", "accessory_handlebar"),
            new CosmeticItem(2423, "Heart Glasses", "accessory_heart_glasses"),
            new CosmeticItem(2424, "Oxygen Not Included Dupe", "accessory_hold_ren"),
            new CosmeticItem(2425, "Don't Starve Wilson", "accessory_hold_wilson"),
            new CosmeticItem(2426, "The Imperial", "accessory_imperial"),
            new CosmeticItem(2427, "Red Nose", "accessory_jester_nose"),
            new CosmeticItem(2428, "Clown Shoes", "accessory_jester_shoes"),
            new CosmeticItem(2429, "King Scepter", "accessory_king_scepter"),
            new CosmeticItem(2430, "Maple Pin", "accessory_maple_pin"),
            new CosmeticItem(2431, "Mertail", "accessory_mermaid_tail"),
            new CosmeticItem(2432, "Motobug Belt", "accessory_motobug_belt"),
            new CosmeticItem(2433, "Cockatiel", "accessory_parrot"),
            new CosmeticItem(2434, "Party Disguise Glasses", "accessory_party_glasses"),
            new CosmeticItem(2435, "Don't Starve Abigail Pin", "accessory_pin_abigail"),
            new CosmeticItem(2436, "Don't Starve's Chester Pin", "accessory_pin_chester"),
            new CosmeticItem(2437, "Don't Starve's Wendy Pin", "accessory_pin_wendy"),
            new CosmeticItem(2438, "Don't Starve's Willow Pin", "accessory_pin_willow"),
            new CosmeticItem(2439, "Don't Starve Wilson Pin", "accessory_pin_wilson"),
            new CosmeticItem(2440, "Don't Starve's WX 78 Pin", "accessory_pin_wx_78"),
            new CosmeticItem(2441, "The Petite Handlebar", "accessory_pitit_handlebar"),
            new CosmeticItem(2442, "Plague Gauntlets", "accessory_plague_wristlet"),
            new CosmeticItem(2443, "Prospecting Scarf and Beard", "accessory_prospector_scarf_beard"),
            new CosmeticItem(2444, "Oxygen Not Included Puft", "accessory_puft"),
            new CosmeticItem(2445, "Raccoon Tail", "accessory_racoon_tail"),
            new CosmeticItem(2446, "\"...Rainbow...\"", "accessory_rainbow_ribbons"),
            new CosmeticItem(2447, "Rainbow Tail", "accessory_rainbow_tail"),
            new CosmeticItem(2448, "K.A.P.O.W! Leg Greaves", "accessory_ranger_legs"),
            new CosmeticItem(2449, "Reading Glasses", "accessory_reading_glasses"),
            new CosmeticItem(2450, "Red Beard", "accessory_red_beard"),
            new CosmeticItem(2451, "Road Crow Arm Bands", "accessory_roadcrow_arms"),
            new CosmeticItem(2452, "Rockabilly Glasses", "accessory_rockabilly_glasses"),
            new CosmeticItem(2453, "Sheriff Badge", "accessory_sheriff_badge"),
            new CosmeticItem(2454, "Trainers", "accessory_shoes_01"),
            new CosmeticItem(2455, "Basketball Shoes", "accessory_shoes_2"),
            new CosmeticItem(2456, "Workout Shoes", "accessory_shoes_3"),
            new CosmeticItem(2457, "Running Shoes", "accessory_shoes_4"),
            new CosmeticItem(2458, "Sloooooth", "accessory_sloth"),
            new CosmeticItem(2459, "Shutter Shades", "accessory_slotted_glasses"),
            new CosmeticItem(2460, "T.O.X.I.C.'s Lord Sludge", "accessory_sludge"),
            new CosmeticItem(2461, "Don't Starve Spider", "accessory_spider"),
            new CosmeticItem(2462, "Star Glasses", "accessory_star_glasses"),
            new CosmeticItem(2463, "Squirrel Tail", "accessory_suqirrel_tail"),
            new CosmeticItem(2464, "Thai-ger Feet and Tail", "accessory_tiger_parts"),
            new CosmeticItem(2465, "Wizard Wand", "accessory_wizard_wand"),
            new CosmeticItem(2466, "Wolf Feet and Tail", "accessory_wolf_parts"),
            new CosmeticItem(2467, "Wolf Tail", "accessory_wolf_tail"),
            new CosmeticItem(2468, "Workout Wristbands", "accessory_workout_wrist"),
            new CosmeticItem(2469, "Seasonal Slippers", "accessory_xmas_slipper"),
            new CosmeticItem(2470, "Yellow Beard", "accessory_yellow_beard"),
            new CosmeticItem(2471, "Bee Tail", "beta_accessory_bee_tail"),
            new CosmeticItem(2472, "Boomerang", "beta_accessory_boomerang_a"),
            new CosmeticItem(2473, "Buddy", "beta_accessory_buddy"),
            new CosmeticItem(2474, "Cat Tail", "beta_accessory_cat_tail"),
            new CosmeticItem(2475, "Compass", "beta_accessory_compass"),
            new CosmeticItem(2476, "The Dali", "beta_accessory_dali"),
            new CosmeticItem(2477, "Devil Tail", "beta_accessory_devil_tail"),
            new CosmeticItem(2478, "Dog Tail", "beta_accessory_dog_tail"),
            new CosmeticItem(2479, "Shellholder", "beta_accessory_forearm_shellholder"),
            new CosmeticItem(2480, "Blue Laser Blade", "beta_accessory_laser_knight_blade"),
            new CosmeticItem(2481, "Nunchuck", "beta_accessory_nunchuck"),
            new CosmeticItem(2482, "Powder Horn", "beta_accessory_powder_horn"),
            new CosmeticItem(2483, "Robin Hood", "beta_accessory_robin_hood"),
            new CosmeticItem(2484, "Rubber Duck", "beta_accessory_rubber_duck"),
            new CosmeticItem(2485, "Satchel", "beta_accessory_satchel"),
            new CosmeticItem(2486, "4 Point Shuriken", "beta_accessory_shuriken_4_point"),
            new CosmeticItem(2487, "Forearm Spikes", "beta_accessory_spiked_forearm"),
            new CosmeticItem(2488, "Wrist Sweatband", "beta_accessory_sweat_band"),
            new CosmeticItem(2489, "Viking Axe", "beta_accessory_throwing_axe"),
            new CosmeticItem(2490, "Digital Watch", "beta_accessory_watch"),
        ];

        public static List<CosmeticItem> Decals { get; } = [
            new CosmeticItem(2600, "Burger (Sticker)", "beta_decal_burger"),
            new CosmeticItem(2601, "Cactus (Sticker)", "beta_decal_cactus"),
            new CosmeticItem(2602, "Can't Touch This (Sticker)", "beta_decal_cant_touch_this"),
            new CosmeticItem(2603, "Caution Hot Lava! (Sticker)", "beta_decal_caution_hot_lava"),
            new CosmeticItem(2604, "Arrow (Sticker)", "beta_decal_downward_arrow"),
            new CosmeticItem(2605, "Fresh (Sticker)", "beta_decal_fresh"),
            new CosmeticItem(2606, "Bonehead (Sticker)", "beta_decal_grossglobes_bonehead"),
            new CosmeticItem(2607, "Hazardous Waste (Sticker)", "beta_decal_grossglobes_gathazard"),
            new CosmeticItem(2608, "Franken Jen (Sticker)", "beta_decal_grossglobes_gatjen"),
            new CosmeticItem(2609, "Lex Hex (Sticker)", "beta_decal_grossglobes_gatlex"),
            new CosmeticItem(2610, "Deep Blue Sue (Sticker)", "beta_decal_grossglobes_gatsue"),
            new CosmeticItem(2611, "Moccasin (Sticker)", "beta_decal_grossglobes_mocassin"),
            new CosmeticItem(2612, "Rusty (Sticker)", "beta_decal_grossglobes_rusty"),
            new CosmeticItem(2613, "Slime Zero (Sticker)", "beta_decal_grossglobes_sludgezero"),
            new CosmeticItem(2614, "Ice Cream Cone (Sticker)", "beta_decal_icecream_cone"),
            new CosmeticItem(2615, "Lightning Bolt (Sticker)", "beta_decal_lightning_bolt"),
            new CosmeticItem(2616, "Five out of Seven (Sticker)", "beta_decal_popculture_fiveoutofseven"),
            new CosmeticItem(2617, "RNG Hands (Sticker)", "beta_decal_popculture_rnghands"),
            new CosmeticItem(2618, "Skip (Sticker)", "beta_decal_popculture_skip"),
            new CosmeticItem(2619, "Trebuchet (Sticker)", "beta_decal_popculture_trebuchet"),
            new CosmeticItem(2620, "Rainbow (Sticker)", "beta_decal_rainbow"),
            new CosmeticItem(2621, "Rambull Logo (Sticker)", "beta_decal_rambull"),
            new CosmeticItem(2622, "Rest In Peace (Sticker)", "beta_decal_rip"),
            new CosmeticItem(2623, "Bulldog Clip (Sticker)", "beta_decal_schoolsout_bulldogclip"),
            new CosmeticItem(2624, "Calculator Watch (Sticker)", "beta_decal_schoolsout_calculatorwatch"),
            new CosmeticItem(2625, "Cellphone (Sticker)", "beta_decal_schoolsout_cellphone"),
            new CosmeticItem(2626, "Portable Game (Sticker)", "beta_decal_schoolsout_game"),
            new CosmeticItem(2627, "White Glue (Sticker)", "beta_decal_schoolsout_glue"),
            new CosmeticItem(2628, "Scissors (Sticker)", "beta_decal_schoolsout_scissors"),
            new CosmeticItem(2629, "Red Stapler (Sticker)", "beta_decal_schoolsout_stapler"),
            new CosmeticItem(2630, "Tack (Sticker)", "beta_decal_schoolsout_tack"),
            new CosmeticItem(2631, "Smiley Face (Sticker)", "beta_decal_smiley_face"),
            new CosmeticItem(2632, "Watermelon (Sticker)", "beta_decal_watermelon"),
            new CosmeticItem(2633, "You Must Be This Tall (Sticker)", "beta_decal_you_must_be_this_tall"),
            new CosmeticItem(2634, "Hazard (Sticker)", "decal_16bit_hazard"),
            new CosmeticItem(2635, "Jen Forcer (Sticker)", "decal_16bit_jen_forcer"),
            new CosmeticItem(2636, "Lex Splorer (Sticker)", "decal_16bit_lex_splorer"),
            new CosmeticItem(2637, "Poizone (Sticker)", "decal_16bit_poizone"),
            new CosmeticItem(2638, "Sludge (Sticker)", "decal_16bit_sludge"),
            new CosmeticItem(2639, "Sue Nami (Sticker)", "decal_16bit_sue_nami"),
            new CosmeticItem(2640, "Happy Dolphin (Sticker)", "decal_lindafranke_dolphin"),
            new CosmeticItem(2641, "C.A.T. Hazard (Sticker)", "decal_lindafranke_hazard"),
            new CosmeticItem(2642, "Rainbow Heart (Sticker)", "decal_lindafranke_heart"),
            new CosmeticItem(2643, "C.A.T. Jen Forcer (Sticker)", "decal_lindafranke_jen"),
            new CosmeticItem(2644, "C.A.T. Lex Splorer (Sticker)", "decal_lindafranke_lex"),
            new CosmeticItem(2645, "Pretty Pony (Sticker)", "decal_lindafranke_pony"),
            new CosmeticItem(2646, "Shelly (Sticker)", "decal_lindafranke_seaturtle"),
            new CosmeticItem(2647, "C.A.T. Sue Nami (Sticker)", "decal_lindafranke_sue"),
            new CosmeticItem(2648, "Stink Bomb (Sticker)", "decal_mutantmayhem_stinkbomb"),
            new CosmeticItem(2649, "Tyler Rex (Sticker)", "decal_mutantmayhem_tyronerex"),
            new CosmeticItem(2650, "Venomess (Sticker)", "decal_mutantmayhem_venomess"),
            new CosmeticItem(2651, "On Point (Sticker)", "decal_popculture_arrow"),
            new CosmeticItem(2652, "On Target (Sticker)", "decal_popculture_bullseye"),
            new CosmeticItem(2653, "Easy Mode (Sticker)", "decal_popculture_ezbutton"),
            new CosmeticItem(2654, "Hazard, the Immortalixer Thrower (Sticker)", "decal_popculture_hazardthrow"),
            new CosmeticItem(2655, "Participation Ribbon (Sticker)", "decal_popculture_participation"),
            new CosmeticItem(2656, "Pogo Gang (Sticker)", "decal_popculture_pogogang"),
            new CosmeticItem(2657, "Salt (Sticker)", "decal_popculture_saltshaker"),
            new CosmeticItem(2658, "Time Smash (Sticker)", "decal_popculture_timesmash"),
            new CosmeticItem(2659, "Pilot (Sticker)", "decal_puglife_aircontrol"),
            new CosmeticItem(2660, "Full Cast Buddy (Sticker)", "decal_puglife_buddycast"),
            new CosmeticItem(2661, "Sore foot Buddy (Sticker)", "decal_puglife_buddyfoot"),
            new CosmeticItem(2662, "Injured Buddy (Sticker)", "decal_puglife_buddyinjury"),
            new CosmeticItem(2663, "Neck Brace Buddy (Sticker)", "decal_puglife_buddyneckbrace"),
            new CosmeticItem(2664, "Sore paw Buddy (Sticker)", "decal_puglife_buddypaw"),
            new CosmeticItem(2665, "Sick Buddy (Sticker)", "decal_puglife_buddysick"),
            new CosmeticItem(2666, "Chef Buddy (Sticker)", "decal_puglife_chef"),
            new CosmeticItem(2667, "Budzo The Clown (Sticker)", "decal_puglife_clown"),
            new CosmeticItem(2668, "Diner Dog (Sticker)", "decal_puglife_diner"),
            new CosmeticItem(2669, "Pug, As You Are (Sticker)", "decal_puglife_grunge"),
            new CosmeticItem(2670, "Pugman Hart (Sticker)", "decal_puglife_hitman"),
            new CosmeticItem(2671, "Mountie Buddy (Sticker)", "decal_puglife_mountie"),
            new CosmeticItem(2672, "Buddy Accountant (Sticker)", "decal_puglife_nerd"),
            new CosmeticItem(2673, "Nurse Buddy (Sticker)", "decal_puglife_nurse"),
            new CosmeticItem(2674, "Mr. Bud-T (Sticker)", "decal_puglife_pityfool"),
            new CosmeticItem(2675, "Poncho Pug (Sticker)", "decal_puglife_poncho"),
            new CosmeticItem(2676, "Pugula (Sticker)", "decal_puglife_vampire"),
            new CosmeticItem(2677, "Buddy Worker (Sticker)", "decal_puglife_worker"),
            new CosmeticItem(2678, "Anna Ka-roll-ina (Sticker)", "decal_radicaldino_ankylosaurus"),
            new CosmeticItem(2679, "Brisco McBaggins (Sticker)", "decal_radicaldino_bagpipes"),
            new CosmeticItem(2680, "MC Bronto (Sticker)", "decal_radicaldino_bronto"),
            new CosmeticItem(2681, "Dylan Dimes (Sticker)", "decal_radicaldino_dimetrodon"),
            new CosmeticItem(2682, "DJ Spinosaurus (Sticker)", "decal_radicaldino_djspinosaur"),
            new CosmeticItem(2683, "Radical Dino Hazard (Sticker)", "decal_radicaldino_hazard"),
            new CosmeticItem(2684, "Radical Dino Jen Forcer (Sticker)", "decal_radicaldino_jenforcer"),
            new CosmeticItem(2685, "Radical Dino Lex Splorer (Sticker)", "decal_radicaldino_lex"),
            new CosmeticItem(2686, "Terry Dacta (Sticker)", "decal_radicaldino_ptero"),
            new CosmeticItem(2687, "Jack the Sax Raptor (Sticker)", "decal_radicaldino_saxraptor"),
            new CosmeticItem(2688, "Stevie Steg (Sticker)", "decal_radicaldino_stegosaurus"),
            new CosmeticItem(2689, "Radical Dino Sue Nami (Sticker)", "decal_radicaldino_suenami"),
            new CosmeticItem(2690, "Rex Tyrano (Sticker)", "decal_radicaldino_trex"),
            new CosmeticItem(2691, "Tracey Tops (Sticker)", "decal_radicaldino_triceratops"),
            new CosmeticItem(2692, "Yoyo Joe (Sticker)", "decal_radicaldino_yoyojoe"),
            new CosmeticItem(2693, "Apple Scratch n' Sniff (Sticker)", "decal_smellyou_apple"),
            new CosmeticItem(2694, "Artichoke Scratch n' Sniff (Sticker)", "decal_smellyou_artichoke"),
            new CosmeticItem(2695, "Bacon Scratch n' Sniff (Sticker)", "decal_smellyou_bacon"),
            new CosmeticItem(2696, "Banana Scratch n' Sniff (Sticker)", "decal_smellyou_banana"),
            new CosmeticItem(2697, "Broccoli Scratch n' Sniff (Sticker)", "decal_smellyou_broccoli"),
            new CosmeticItem(2698, "Cherry Scratch n' Sniff (Sticker)", "decal_smellyou_cherry"),
            new CosmeticItem(2699, "Cottoncandy Scratch n' Sniff (Sticker)", "decal_smellyou_cottoncandy"),
            new CosmeticItem(2700, "Cupcake Scratch n' Sniff (Sticker)", "decal_smellyou_cupcake"),
            new CosmeticItem(2701, "Kiwi Scratch n' Sniff (Sticker)", "decal_smellyou_kiwi"),
            new CosmeticItem(2702, "Lemon Scratch n' Sniff (Sticker)", "decal_smellyou_lemon"),
            new CosmeticItem(2703, "Peach Scratch n' Sniff (Sticker)", "decal_smellyou_peach"),
            new CosmeticItem(2704, "Pear Scratch n' Sniff (Sticker)", "decal_smellyou_pear"),
            new CosmeticItem(2705, "Pizza Scratch n' Sniff (Sticker)", "decal_smellyou_pizza"),
            new CosmeticItem(2706, "Plum Scratch n' Sniff (Sticker)", "decal_smellyou_plum"),
            new CosmeticItem(2707, "Poop Scratch n' Sniff (Sticker)", "decal_smellyou_poop"),
            new CosmeticItem(2708, "Popcorn Scratch n' Sniff (Sticker)", "decal_smellyou_popcorn"),
            new CosmeticItem(2709, "Strawberry Scratch n' Sniff (Sticker)", "decal_smellyou_strawberry"),
            new CosmeticItem(2710, "Teen Cyborca (Sticker)", "decal_teentoon_cyborca"),
            new CosmeticItem(2711, "Teen Hazard (Sticker)", "decal_teentoon_hazard"),
            new CosmeticItem(2712, "Teen Jen (Sticker)", "decal_teentoon_jen_forcer"),
            new CosmeticItem(2713, "Teen Lex (Sticker)", "decal_teentoon_lex_splorer"),
            new CosmeticItem(2714, "Young Sludge (Sticker)", "decal_teentoon_sludge"),
            new CosmeticItem(2715, "Teen Sue (Sticker)", "decal_teentoon_sue_nami"),
        ];

        public static Dictionary<long, Item> AllItems
        {
            get
            {
                if (_allItems == null)
                {
                    _allItems = LoadItems();
                }

                return _allItems;
            }
        }

        /// <summary>
        /// Builds the item dictionary
        /// </summary>
        /// <returns>The item dictionary</returns>
        private static Dictionary<long, Item> LoadItems()
        {
            Dictionary<long, Item> itemDictionary = new Dictionary<long, Item>()
            {
                { 1, new XpShardItem(1) },
                { DoubleJump.Id, DoubleJump },
                { BoostJump.Id, BoostJump },
                { SlideJump.Id, SlideJump },
                { VaultJump.Id, VaultJump },
                { Crouch.Id, Crouch },
                { Grab.Id, Grab },
                { Surf.Id, Surf },
                { WallJump.Id, WallJump },
                { Swing.Id, Swing },
                { Climb.Id, Climb },
                { Pogo.Id, Pogo },
                { TinyToy.Id, TinyToy },
                { Jetpack.Id, Jetpack },
            };

            LoadWorldItems(itemDictionary);

            LoadItemList(itemDictionary, CharacterItems);
            LoadItemList(itemDictionary, HeadAccessories);
            LoadItemList(itemDictionary, BackAccessories);
            LoadItemList(itemDictionary, Trinkets);
            LoadItemList(itemDictionary, Decals);

            return itemDictionary;
        }

        /// <summary>
        /// Builds all the items specific to each world and adds them to the dictionary
        /// </summary>
        /// <param name="itemDictionary">The item dictionary</param>
        private static void LoadWorldItems(Dictionary<long, Item> itemDictionary)
        {
            foreach (WorldInfo world in Worlds.AllWorlds)
            {
                itemDictionary.Add(world.ItemId, new WorldUnlockItem(world.ItemId, world));

                foreach (ForceFieldInfo forceField in world.ForceFields)
                {
                    itemDictionary.Add(forceField.ItemId, new ForceFieldItem(forceField.ItemId, forceField));
                }
            }
        }

        private static void LoadItemList<T>(Dictionary<long, Item> itemDictionary, List<T> items) where T : Item
        {
            foreach (Item item in items)
            {
                itemDictionary[item.Id] = item;
            }
        }

        /// <summary>
        /// Gets all items of a provided type
        /// </summary>
        /// <returns>The list of items</returns>
        public static IEnumerable<T> GetItems<T>() where T : Item
        {
            return AllItems.Values
                .Where(m => m is T)
                .Select(m => (T)m);
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static Item? GetItem(long id)
        {
            return AllItems.GetValueOrDefault(id);
        }

        /// <summary>
        /// Gets an item by ID
        /// </summary>
        /// <param name="id">The ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static T? GetItem<T>(long id) where T : Item
        {
            return GetItems<T>()
                .FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Gets an item by the unlockable ID
        /// </summary>
        /// <param name="unlockableId">The Unlockable ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static UnlockableItem? GetUnlockableItem(string unlockableId)
        {
            return GetUnlockableItem<UnlockableItem>(unlockableId);
        }

        /// <summary>
        /// Gets an item by the unlockable ID
        /// </summary>
        /// <param name="unlockableId">The Unlockable ID of the item</param>
        /// <returns>The item if found, else null</returns>
        public static T? GetUnlockableItem<T>(string unlockableId) where T : UnlockableItem
        {
            return GetItems<T>()
                .FirstOrDefault(m => m.UnlockableId == unlockableId);
        }
    }
}

using HotLavaArchipelagoPlugin.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    internal static class Worlds
    {
        /// <summary>
        /// TODO: internal names: 
        ///     "tutorial",
        ///  "playground",
        ///      "school",
        ///    "wholesale_expanded",
        ///    "mastery_gym",
        ///     "basement",
        ///    "fun_centre",
        ///   "Intro"
        /// </summary>

        //TODO: This unlock is for the tutorial first load, is there a better one to use?
        public static WorldInfo GymClass => new WorldInfo("786e99dd02dbd5c479922d67978d8ceb", "tutorial", "Gym Class", [
            new CourseInfo("Gym Jam", [
                new StarInfo("8e72934736772e94eaa50290f7ae341a", "Complete the course", StarType.CourseComplete),
                new StarInfo("2c2765d96036f40438807ce21e92b229", "Complete in under 08:00", StarType.MinTime),
                new StarInfo("2b6db09b813d3f54cb1424cbf312ca8d", "Complete in under 02:10", StarType.MinTime),
                new StarInfo("e7991396471eede4d82f90334472ae2d", "Reach a speed of 9", StarType.Challenge),
                new StarInfo("2f492a67f28d82c429c93fa9e48bfe3e", "No Deaths", StarType.NoDeaths),
                new StarInfo("718fac78ec94caa478f347b892adae7a", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("7680ae1ce1a1c1f49afc80c4e50df8a1", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Trampoline Trouble", [
                new StarInfo("52813bff26455a34c92918c183c034fe", "Complete the course", StarType.CourseComplete),
                new StarInfo("181644ae625180e4eb6989129d83e2fb", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("3262cf7f58754504dbb378912ea87f07", "Complete in under 01:00", StarType.MinTime),
                new StarInfo("0059648cfcaf1d3459f89b8b76db5eef", "Don't perform any swings", StarType.Challenge),
                new StarInfo("5d26c3d1697052c45a3f29ab54d8bb0d", "No Deaths", StarType.NoDeaths),
                new StarInfo("1a2ff5486c095154c9bf507773051774", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("540b0bdc06ed2dd4887deff906e0575f", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Livin' on the Ledge", [
                new StarInfo("f2aca0ab34ef5a34fb9b3d9a52087aff", "Complete the course", StarType.CourseComplete),
                new StarInfo("c689392b82845f948b1a088de15c27ab", "Complete in under 02:00", StarType.MinTime),
                new StarInfo("513a99591da7c8f49855898242e338e7", "Complete in under 00:55", StarType.MinTime),
                new StarInfo("dc67bb49b6bdd3f4ba6af59e7f46dc7e", "Spend less than 25 seconds on the ground", StarType.Challenge),
                new StarInfo("df11767c8cafed7429faabbe863fac18", "No Deaths", StarType.NoDeaths),
                new StarInfo("3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("2b821237eb81a37448942997afb21e9e", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Surfing Surfaces", [
                new StarInfo("b772330c648bc5b4dbf560e1636a765b", "Complete the course", StarType.CourseComplete),
                new StarInfo("720fa9c239a1b224eae03b1dd031c9c5", "Complete in under 01:50", StarType.MinTime),
                new StarInfo("4218afbc7f4895141a4ebd26d61272d3", "Complete in under 00:50", StarType.MinTime),
                new StarInfo("a6179cec5a395964a807d262ec059a9e", "Reach a speed of 10.5", StarType.Challenge),
                new StarInfo("5bd4d45837fdac649965d0e2b89e63d7", "No Deaths", StarType.NoDeaths),
                new StarInfo("7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("d2a264a37b85ab045a885dc98fa2ad56", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Pole Vault", [
                new StarInfo("b854fd83bb9c6b2438d05cfdaba66a91", "Complete the course", StarType.CourseComplete),
                new StarInfo("3bb1fbe4b84c03a4086c6ac366f2ba27", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("f89d1fb8ad92a634d8703d0b8bb1c976", "Complete in under 01:10", StarType.MinTime),
                new StarInfo("6bc239d7f01e9ee40a4ce3687f205482", "Spend less than 30 seconds on the ground", StarType.Challenge),
                new StarInfo("f8a55461338a712468a59dcd64888350", "No Deaths", StarType.NoDeaths),
                new StarInfo("5101c9e9015f4204bb42649a58127217", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("b6fb416d4c5c4984db326bab8e23079a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Your Sister", [
                new StarInfo("39d3e311aa4b8dc4e92da8ec2fc093e1", "Complete the course", StarType.CourseComplete),
                new StarInfo("46a134cc36e348d4eb3416dc4f9df9de", "Complete in under 01:10", StarType.MinTime),
                new StarInfo("f6a106bb664df5a4089b4fa5101f0152", "Complete in under 00:45", StarType.MinTime),
                new StarInfo("34f041456579a904a98c746ea6e6bb2e", "Don't perform any swings", StarType.Challenge),
                new StarInfo("34a0bf50ab8394f46a9a94542fbfc553", "Tag your sister", StarType.Challenge),
                new StarInfo("a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("078d212bcd4e55a4f9fe32de2cf0ad2b", "Buddy Mode", StarType.Buddy), //TODO: should this be treated as a separate course/trial?
            ]),
            new CourseInfo("Pogo Trial", CourseType.Pogo, [
                new StarInfo("5069ece9ed4a8ed479d1743620a34023", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo("98310f580476ff04384f6abd4357370e", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo("62137bb4c16995d4aaf2f385225156e8", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo("e9ee84430ecf11740beb41326451d414", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo("Gym/Office Hallway", new Vector3(33.752f, 1.76f, 1.6f)),
                new ForceFieldInfo("Office Hallway/Janitor's Closet", new Vector3(24.622f, 1.758f, 25.966f)),
                new ForceFieldInfo("Office Hallway/Back Hallway", new Vector3(18.55f, 1.86f, 29.089f)),
                new ForceFieldInfo("Computer Lab Hallway/Back Hallway", new Vector3(0.853f, 1.633f, 25.97f)),
                new ForceFieldInfo("Back Hallway/Side Entrance", new Vector3(-1.35f, 1.86f, 29.089f)),
            ]
        };

        public static WorldInfo Playground => new WorldInfo("83328d77e87a0d34b8cc4aae038bd158", "playground", "Playground", [
            new CourseInfo("Recess", [
                new StarInfo("60dff4f3d39f9664d8cde41acf5a9465", "Complete the course", StarType.CourseComplete),
                new StarInfo("e6704c37a77263d4fb352b2ebe0c9dd5", "Complete in under 07:00", StarType.MinTime),
                new StarInfo("4e708a47372bdfd4c96d9853dd7204ac", "Complete in under 02:10", StarType.MinTime),
                new StarInfo("724bcc52ae9d6df46a1fe83bc9e5f3b7", "No Deaths", StarType.NoDeaths),
                new StarInfo("5a0c11e0ebfe8a642acdcb3f224e44c5", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("f0804c3773358e54ea3381ab1d9a4fa4", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("36f06de3ffe852648bbf28e3f8383a1a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Big Kids Side", [
                new StarInfo("1ce626b7d30c7984bafa0241356ca1aa", "Complete the course", StarType.CourseComplete),
                new StarInfo("d0a1872e506424544a944d02f1273cba", "Complete in under 08:00", StarType.MinTime),
                new StarInfo("a92ec2a42e5efbf4eaf8b574dd625253", "Complete in under 02:45", StarType.MinTime),
                new StarInfo("f417c8cd9958fe44a8c8bfb747a14856", "No Deaths", StarType.NoDeaths),
                new StarInfo("f3605b943b5651b4eb7289ce220d4cef", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("985db81c1b97b00498884c81ce9c1001", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("fe07e2dc513868a4cad555546bd3ce1c", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Bouncy Castle", [
                new StarInfo("25da304df7917554c932e0db2ed12bbe", "Complete the course", StarType.CourseComplete),
                new StarInfo("45851bf0a9623944fad09518b9be8a54", "Complete in under 04:30", StarType.MinTime),
                new StarInfo("d7a3953016f961c43b8c97b3c538d838", "Complete in under 01:45", StarType.MinTime),
                new StarInfo("15ba77a1f2ff6ec45a5d8a6ce1a2d7d3", "No Deaths", StarType.NoDeaths),
                new StarInfo("b13ffe80cf201d54b9f48af60809cbd6", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("81d32f5a10c05fe4c88a74abb7db2b84", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("623714b5444588f43a7e8d01a84b6525", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Back to Class", [
                new StarInfo("888924c3b32682b4eba329c0840548d8", "Complete the course", StarType.CourseComplete),
                new StarInfo("bcceae8fcf65da644b7252e135889074", "Complete in under 06:00", StarType.MinTime),
                new StarInfo("1a524b89d300c44468b7057596730d82", "Complete in under 01:45", StarType.MinTime),
                new StarInfo("3377dfa30ba551447b5c15d4f276c2c6", "No Deaths", StarType.NoDeaths),
                new StarInfo("8c335e082a2eb024aa36d8c2e6cadb48", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("69f0cf9071aad05479efee996377ea21", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("748605251b94a514f8ec0f85415ffa91", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Sports Day", [
                new StarInfo("b9de4719a85dd0f48be1ef9df00248f5", "Complete the course", StarType.CourseComplete),
                new StarInfo("8d1c5b5442d995f428a89236adaa7900", "Complete in under 01:40", StarType.MinTime),
                new StarInfo("1b1ddc847b094bd4aafe998d05829548", "Complete in under 01:10", StarType.MinTime),
                new StarInfo("d6d01532f4668284382bdbde28e3b59d", "No Deaths", StarType.NoDeaths),
                new StarInfo("b3eb8fe6d65356a49a0cf46e288c69f6", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("04ffbbd04e97bb441a90e7c7fb0d052e", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("c96eefdfbaa06d0459b7eec5221cd510", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase the Big Kid", [
                new StarInfo("edd7311d9d1623e4891f8d4b955d345e", "Complete the course", StarType.CourseComplete),
                new StarInfo("9a08d0a4fd4585c4fac47e975c79ad0d", "Complete in under 01:15", StarType.MinTime),
                new StarInfo("a1b0ed2d2c267ae4d8391a364830160f", "Complete in under 00:58", StarType.MinTime),
                new StarInfo("ce5ef6bbd31e325499fb9cf28fd80d36", "Tag your sister", StarType.Challenge),
                new StarInfo("e621f981cf18e2242b26195512632798", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("732eafb0ad0d6c547921c904d1ee4073", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("c96eefdfbaa06d0459b7eec5221cd510", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo("1a56d171c35fcf547980a37d8f7f7960", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo("9fc0edaa2793c9c45927c92caf5634b3", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 1", CourseType.TinyToy, [
                new StarInfo("415b0dd0ab657c840a3073f2e952cb6c", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 2", CourseType.TinyToy, [
                new StarInfo("5896145f43d1a6a4dbce56b1dbf7ca61", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo("6065e19024fa4cc4d9d9ea20e581a25b", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo("d5c3b1aaf1343a143a7186eafdcdeaad", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo("234d3bf0d0df2af408329011a45dfd1b", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo("Spawn/Basketball Courts", new Vector3(17.603f, 2.501f, 14.035f)),
                new ForceFieldInfo("Basketball Courts/Sports Day Side", new Vector3(17.4f, 2.5f, -11.5f)),
            ]
        };

        public static WorldInfo School => new WorldInfo("212f728535833224287d420c81f43ef1", "school", "School", [
            new CourseInfo("ABCs and 123s", [
                new StarInfo("57e0344bd3abfb4449346d182cd8e901", "Complete the course", StarType.CourseComplete),
                new StarInfo("2ee56bdf1e09d1a4eb0170081b60b883", "Complete in under 12:00", StarType.MinTime),
                new StarInfo("b2311faf93b83d0428c2a97d1ee72c6b", "Complete in under 05:00", StarType.MinTime),
                new StarInfo("a14a4456df2c527479a4889c2555ae0a", "No Deaths", StarType.NoDeaths),
                new StarInfo("9f6ad9759e59fc14cbffdd76792c9796", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("d15988c45557cd641b5c1fed6d28b93c", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("21c2d3bc6b1078546a556f6065b9944f", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Middle School Mischief", [
                new StarInfo("73c90f4f92fb86e4d8643e0efe67f386", "Complete the course", StarType.CourseComplete),
                new StarInfo("f495576fc3c4f654d97c874f8c48235f", "Complete in under 11:00", StarType.MinTime),
                new StarInfo("bf0d228482a8f564d80bc8fc3919834e", "Complete in under 03:00", StarType.MinTime),
                new StarInfo("bcc4ecc8b68979143924c78e147e54cb", "No Deaths", StarType.NoDeaths),
                new StarInfo("f0271c912e6562745aa87f64120386ca", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("c4c3c7f707f83a74a91fd35a3c996200", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("b2f7d84f93346b84880bf32ccb684c5a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Repeat the Grade", [
                new StarInfo("72a71e5e6cb185843940831aff9f05d9", "Complete the course", StarType.CourseComplete),
                new StarInfo("12bc63b0a54486449b09fa0883f5937a", "Complete in under 10:00", StarType.MinTime),
                new StarInfo("ab81decce36f68b4db6801c220184dad", "Complete in under 03:30", StarType.MinTime),
                new StarInfo("77e419b13b4e62d4f857c36a8a9622b7", "No Deaths", StarType.NoDeaths),
                new StarInfo("de32cab8a4473d045a1095a17f779697", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("d419edaf69289854f95c5c075c934b8c", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("893408fe1ea036b4db838b1f48e25271", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Senior Trip", [
                new StarInfo("972ec486a5a1b8d4c9d5d5c6894ff904", "Complete the course", StarType.CourseComplete),
                new StarInfo("28b6b575cd513074fb9b9408bf3b46d9", "Complete in under 08:00", StarType.MinTime),
                new StarInfo("ee745e4fd95cc00468fa3ce854a39245", "Complete in under 03:00", StarType.MinTime),
                new StarInfo("18afc8a3a23bee045a7e502e8ee50216", "No Deaths", StarType.NoDeaths),
                new StarInfo("b7a195d971b1d7848bb37ca8fcc68112", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("29f436273460d574fbdfa1d797f29d13", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("1620c9eae36334a4783950bdacc65a5d", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Freshman Frenzy", [
                new StarInfo("180ece11a7fb5c841bcbb633544e131f", "Complete the course", StarType.CourseComplete),
                new StarInfo("6b267b4581572464d8340e34babd9492", "Complete in under 10:00", StarType.MinTime),
                new StarInfo("a1eb55827bd43be4bbb57ddd7845ee9f", "Complete in under 03:30", StarType.MinTime),
                new StarInfo("c0e9b9f96d82eb84f8c579a1418440d4", "No Deaths", StarType.NoDeaths),
                new StarInfo("a003111804c160e4a96cbad1914e793e", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("3706423d30537ba4191a10e58d3b1611", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("2fa7368db5bfb58489ec7e1a2adffa31", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Your Sister", [
                new StarInfo("4d0edbf502e968b4a8a672e0a7355c81", "Complete the course", StarType.CourseComplete),
                new StarInfo("93120cacc4fb33b4a813d188d5696fd5", "Complete in under 02:00", StarType.MinTime),
                new StarInfo("c563a09d63593a54092506d5aca87747", "Complete in under 00:50", StarType.MinTime),
                new StarInfo("a8ff9a42e05c9c0448b37c1b8eb03412", "Tag your sister", StarType.Challenge),
                new StarInfo("0fc1239839daab54686b01a5b36332c4", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("b27ebe301bf36cf46b6a42735638d6fa", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("30fdf0d70ac2f11469ea3a313dd76c2c", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo("e641257639cc2354aa58580dae083b03", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo("e09457a7ecb51044fb72744ce6699e25", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 3", CourseType.Pogo, [
                new StarInfo("a9c4d26b69466bd45886f8825aff4054", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo("ef5401ba94301e74fa8f32d8af2f429d", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo("b635ffc7a94e4eb4787160b9dc2b77fe", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo("bc1ad6587b18207469969deace0ac6c5", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo("4aa7293d5e6db8d409a3655cfe6efefa", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo("Atrium/Cafeteria", new Vector3(5.079f, 2.141f, 16.822f)),
                new ForceFieldInfo("Gym Hallway/Computer Lab", new Vector3(32.16f, 2.168f, -24.77f)),
                new ForceFieldInfo("Social Studies Hallway/Art Hallway", new Vector3(19f, 2.85f, -12.65f)),
                new ForceFieldInfo("Teacher's Lounge Hallway/Art Hallway", new Vector3(-23.64f, 2.85f, -12.65f)),
                new ForceFieldInfo("Social Studies Class Left", new Vector3(22.022f, 2.1f, 1.9f)),
                new ForceFieldInfo("English Hallway/Teacher's Lounge Hallway", new Vector3(-23.745f, 3.46f, 1.31f)),
                new ForceFieldInfo("Science Lab/Art Closet", new Vector3(4.046f, 2.177f, -24.828f)),
                new ForceFieldInfo("Art Hallway/Art Class", new Vector3(-10.526f, 2.704f, -18.816f)),
                new ForceFieldInfo("Gym Hallway/Science Lab", new Vector3(15.927f, 2.177f, -29.253f)),
                new ForceFieldInfo("Atrium/Social Studies Hallway", new Vector3(15.945f, 2.146f, 6.217f)),
                new ForceFieldInfo("Art Closet/Art Class", new Vector3(-0.077f, 2.687f, -28.871f)),
                new ForceFieldInfo("Social Studies Class Right", new Vector3(22.022f, 2.1f, -10.116f)),
                new ForceFieldInfo("Teacher's Lounge/Courtyard", new Vector3(-8.333f, 3.724f, -6.338f)),
            ]
        };

        public static WorldInfo Wholesale => new WorldInfo("f5bd38e199f54fa46aff759206fd0806", "wholesale_expanded", "Wholesale", [
            new CourseInfo("To the Top", [
                new StarInfo("16834aaf2d1962e44b22cbf1bd2d3a6f", "Complete the course", StarType.CourseComplete),
                new StarInfo("058594844899efe45b805507f1addd7b", "Complete in under 10:00", StarType.MinTime),
                new StarInfo("f87018bfc1f842740b56c2a1a92cf4a5", "Complete in under 03:00", StarType.MinTime),
                new StarInfo("9f8de75e70e8b5942842e8fb62b50a58", "No Deaths", StarType.NoDeaths),
                new StarInfo("d73a042fdcb801841864b76903065548", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("89dcbb351c916b944ae2d04c72a1ba22", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("996d266c518979d438cbbe4a4a6b7b4a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Duct and Cover", [
                new StarInfo("2d0738601a766234087120f1a016a344", "Complete the course", StarType.CourseComplete),
                new StarInfo("72bc9354d22810544ba7a1b8f66bc942", "Complete in under 08:00", StarType.MinTime),
                new StarInfo("41ecfbd3c9df62543be573b0253c6f54", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("c7bfa995db564b9418bf9c523b38c5ce", "No Deaths", StarType.NoDeaths),
                new StarInfo("57cd7507ede3def4da02d8e46cb66d30", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("4c9dedd9fc5625e47afbd9dae457de29", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("1b289b5ad606abb4e9f982b3d9d4fa32", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Meat Market", [
                new StarInfo("edac1a5bb7363854086f7896a6a38255", "Complete the course", StarType.CourseComplete),
                new StarInfo("81ac63c1ad8c4d5418ea125eaf3bb1f7", "Complete in under 08:00", StarType.MinTime),
                new StarInfo("e9c3888f1669e1344a9dc063e961efb8", "Complete in under 03:30", StarType.MinTime),
                new StarInfo("ecbef8112dd43cb40a7515ac45e52731", "No Deaths", StarType.NoDeaths),
                new StarInfo("aaf16c7eb791b4a4b9bd82b86c6c98ea", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("33124f465d203ae49a6cec0a4764dc33", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("2545f6cd979390b408e0d6d40de14a95", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Returns", [
                new StarInfo("bab110cfadbf0584992291250189509a", "Complete the course", StarType.CourseComplete),
                new StarInfo("4069629cbae76e44c85bad4829835876", "Complete in under 04:00", StarType.MinTime),
                new StarInfo("75d3d573dae4a4f42a2b391d09746933", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("e8fdb40f968d6aa40841f7bcd0f46170", "No Deaths", StarType.NoDeaths),
                new StarInfo("f518343abbc940042b21423aef055173", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("292e394b0fcd3fc47b647f3364ca70f6", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("2031f866b2c2f884ba940f5384f34ec7", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Meat Grinder", [
                new StarInfo("d647509540004f049a23d32a2f4d795c", "Complete the course", StarType.CourseComplete),
                new StarInfo("0cc12dc8e0f5270458650b573b883ee3", "Complete in under 10:00", StarType.MinTime),
                new StarInfo("869fef92e76d8c149b574ada1d109d9c", "Complete in under 04:30", StarType.MinTime),
                new StarInfo("2d2c927fe825ed24a907e6e1d8dadf5e", "No Deaths", StarType.NoDeaths),
                new StarInfo("6a7f4265256d34d408021ba7f9473f41", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("9c1c28cdc25a53341b6eafb0a0a87768", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("8ec1308f20c329147ac1a3f5661b79d4", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Through the Store", [
                new StarInfo("b13b5b41ee88fe144bfa267efb59c710", "Complete the course", StarType.CourseComplete),
                new StarInfo("952e95490fb1bc34a93e5a3d2a543472", "Complete in under 00:50", StarType.MinTime),
                new StarInfo("9cf25623f3fd60a44818bd90028dba35", "Complete in under 00:33", StarType.MinTime),
                new StarInfo("aac1119a19a3e7743995599868918357", "No Deaths", StarType.NoDeaths),
                new StarInfo("550fcd82164dcea41b524a107a2632c2", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo("efe2e7dbec062e2448a4f80219fdfe78", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo("c241690961d459243ad294a277746e36", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo("314ee7741f7851e4e82f7d5581088470", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo("d7fe16db846211043ae130ad5db97bd3", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 3", CourseType.Pogo, [
                new StarInfo("70ae3c38be714cc40bba4feeea5fd355", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo("4604d3c043b5320489bd6eaac865277b", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo("6a8ec6917998047489e769a72a418d5a", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo("eee4a71e511af8f498f55c55fe06a8aa", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo("1d4f91a1fb1547b429b539972938e5d9", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo("Employee Hallway/Checkout", new Vector3(-32.53f, 2.7f, -12.203f)),
                new ForceFieldInfo("Employee Hallway/Janitor's Closet", new Vector3(-10.862f, 6.123f, -3.862f)),
                new ForceFieldInfo("Under the Shelves", new Vector3(-2.871f, 1.38f, -37.507f)),
                new ForceFieldInfo("Checkout/Cart Storage", new Vector3(-29.134f, 2.125f, -40.67f)),
                new ForceFieldInfo("Employee Hallway/Breakroom", new Vector3(-4.788f, 6.112f, 2.11f)),
                new ForceFieldInfo("Checkout/Shopping Area", new Vector3(26.5f, 2.700001f, -17.8f)),
            ]
        };

        public static WorldInfo MasterClass => new WorldInfo("6bd78323a271b804890f9bd232ae0abb", "mastery_gym", "Master Class", [
            new CourseInfo("Air Control Mastery", [
                new StarInfo("399017d32e64e7147850bb221ad9a330", "Complete the course", StarType.CourseComplete),
                new StarInfo("d9f265807b6647a46a071e38d0f43199", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("5745104145f93b54fb585474ed7f395f", "Complete in under 01:00", StarType.MinTime),
                new StarInfo("a19ee0938f049324b8e0f5c2994926f9", "No Deaths", StarType.NoDeaths),
                new StarInfo("05207fb7837a51645b97a0b17b018f4c", "Reach a speed of 4.5", StarType.Challenge),
                new StarInfo("59b9c6018baa64c48961b3f4d897e49e", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Wall Jump Mastery", [
                new StarInfo("eb5e3cb0b7b99b04284d730869ce9fb6", "Complete the course", StarType.CourseComplete),
                new StarInfo("377e30517bff4ac47929b80aecda0062", "Complete in under 02:30", StarType.MinTime),
                new StarInfo("52e25e2c6bf4c454c8d44ef4d6b66688", "Complete in under 00:50", StarType.MinTime),
                new StarInfo("b90b53beac85f6c4098e99c45a51b279", "No Deaths", StarType.NoDeaths),
                new StarInfo("3517d5ce270ecd64c9c75ffb7f3f475e", "Spend less than 10 seconds on the ground", StarType.Challenge),
                new StarInfo("1a2ff5486c095154c9bf507773051774", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Surf Mastery", [
                new StarInfo("4c31e47b2b82aa64e94b5a4e67139775", "Complete the course", StarType.CourseComplete),
                new StarInfo("079222fa9d56f2242a92321dd8d5a599", "Complete in under 01:00", StarType.MinTime),
                new StarInfo("4a6c828b9643ba4459bd1ae1f978dadb", "Complete in under 00:30", StarType.MinTime),
                new StarInfo("75645b25d426a094090d1cccadcf9d8d", "No Deaths", StarType.NoDeaths),
                new StarInfo("417d208b1ed26b148987a5aa7c4ca388", "Complete in 5 jumps or less", StarType.Challenge),
                new StarInfo("5101c9e9015f4204bb42649a58127217", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Boosting Mastery", [
                new StarInfo("5b3155d2c67f23b41b5b0ba6bf1d6740", "Complete the course", StarType.CourseComplete),
                new StarInfo("b7922c4cbf053554bb909c37ec79be1a", "Complete in under 04:00", StarType.MinTime),
                new StarInfo("ac633c1cbf3212d4f95012155cd973e3", "Complete in under 01:00", StarType.MinTime),
                new StarInfo("245dcd87539561241bc955e5fa833fab", "No Deaths", StarType.NoDeaths),
                new StarInfo("9e048a77386525d4297f9775efcb19bc", "Reach a speed of 5.5", StarType.Challenge),
                new StarInfo("3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Wind Tunnel Mastery", [
                new StarInfo("cd8c91a38849df5459afed878e70071b", "Complete the course", StarType.CourseComplete),
                new StarInfo("bdf016c29782aae43b9774c23d505fd0", "Complete in under 01:00", StarType.MinTime),
                new StarInfo("a7ce1e8f9ec2b6948a3b6ad889c26876", "Complete in under 00:30", StarType.MinTime),
                new StarInfo("2a07db3d496a8ac4fa4ab923375af82f", "No Deaths", StarType.NoDeaths),
                new StarInfo("b5c4c4bffc5aac04c864a1f502e10e35", "Spend less than 5 seconds on the ground", StarType.Challenge),
                new StarInfo("7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Honors Gym Class", [
                new StarInfo("878f8f8195d19424aa87e2208d772141", "Complete the course", StarType.CourseComplete),
                new StarInfo("5ea9aa8d87657b944b5c2dcc3d4cb3e3", "Complete in under 04:30", StarType.MinTime),
                new StarInfo("ccf6b59e15b3b7d4b8b69bbc4dbcda13", "Complete in under 02:00", StarType.MinTime),
                new StarInfo("61f2faa484a77ca4f946157b8a8db437", "No Deaths", StarType.NoDeaths),
                new StarInfo("837d73aefc94673408942e14f960f13a", "Reach a speed of 9", StarType.Challenge),
                new StarInfo("a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Pogo Trial", CourseType.Pogo, [
                new StarInfo("18fb5a39cfc1f434694fdb7f9f2e87b2", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo("a4cb9f0819847fa4a85dc913af64ac57", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo("57638f812bae42d43b2a164910db9f86", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo("4ed34eb6dd88bce4e8f33c59d6a122f5", "Complete the course", StarType.TrialComplete),
            ]),
        ]);

        //public static WorldInfo Basement => new WorldInfo("baf5e18a2fbcc64409f83e53e836cb42", "basement", "Basement", [
        //    new CourseInfo("Race to the Summit", [
        //        new StarInfo("16834aaf2d1962e44b22cbf1bd2d3a6f", "Complete the course", StarType.CourseComplete),
        //        new StarInfo("058594844899efe45b805507f1addd7b", "Complete in under 10:00", StarType.MinTime),
        //        new StarInfo("f87018bfc1f842740b56c2a1a92cf4a5", "Complete in under 03:00", StarType.MinTime),
        //        new StarInfo("9f8de75e70e8b5942842e8fb62b50a58", "No Deaths", StarType.NoDeaths),
        //        new StarInfo("d73a042fdcb801841864b76903065548", "Grab the golden pin", StarType.GoldenPin),
        //        new StarInfo("89dcbb351c916b944ae2d04c72a1ba22", "Find the hidden G.A.T. comic", StarType.Comic),
        //        new StarInfo("996d266c518979d438cbbe4a4a6b7b4a", "Buddy Mode", StarType.Buddy),
        //    ]),
        //]);

        public static IEnumerable<WorldInfo> AllWorlds => [
            GymClass,
            Playground,
            School,
            Wholesale,
            MasterClass,
            //Basement,
        ];
    }
}

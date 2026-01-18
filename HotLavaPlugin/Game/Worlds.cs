using HotLavaArchipelagoPlugin.Enums;
using System.Collections.Generic;

namespace HotLavaArchipelagoPlugin.Game
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
        public static World GymClass => new World("786e99dd02dbd5c479922d67978d8ceb", "tutorial", "Gym Class", [
            new Course("Gym Jam", [
                new Star("8e72934736772e94eaa50290f7ae341a", "Complete the course", StarType.CourseComplete),
                new Star("2c2765d96036f40438807ce21e92b229", "Complete in under 08:00", StarType.MinTime),
                new Star("2b6db09b813d3f54cb1424cbf312ca8d", "Complete in under 02:10", StarType.MinTime),
                new Star("e7991396471eede4d82f90334472ae2d", "Reach a speed of 9", StarType.Challenge),
                new Star("2f492a67f28d82c429c93fa9e48bfe3e", "No Deaths", StarType.NoDeaths),
                new Star("718fac78ec94caa478f347b892adae7a", "Grab the golden pin", StarType.GoldenPin),
                new Star("7680ae1ce1a1c1f49afc80c4e50df8a1", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Trampoline Trouble", [
                new Star("52813bff26455a34c92918c183c034fe", "Complete the course", StarType.CourseComplete),
                new Star("181644ae625180e4eb6989129d83e2fb", "Complete in under 02:30", StarType.MinTime),
                new Star("3262cf7f58754504dbb378912ea87f07", "Complete in under 01:00", StarType.MinTime),
                new Star("0059648cfcaf1d3459f89b8b76db5eef", "Don't perform any swings", StarType.Challenge),
                new Star("5d26c3d1697052c45a3f29ab54d8bb0d", "No Deaths", StarType.NoDeaths),
                new Star("1a2ff5486c095154c9bf507773051774", "Grab the golden pin", StarType.GoldenPin),
                new Star("540b0bdc06ed2dd4887deff906e0575f", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Livin' on the Ledge", [
                new Star("f2aca0ab34ef5a34fb9b3d9a52087aff", "Complete the course", StarType.CourseComplete),
                new Star("c689392b82845f948b1a088de15c27ab", "Complete in under 02:00", StarType.MinTime),
                new Star("513a99591da7c8f49855898242e338e7", "Complete in under 00:55", StarType.MinTime),
                new Star("dc67bb49b6bdd3f4ba6af59e7f46dc7e", "Spend less than 25 seconds on the ground", StarType.Challenge),
                new Star("df11767c8cafed7429faabbe863fac18", "No Deaths", StarType.NoDeaths),
                new Star("3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin", StarType.GoldenPin),
                new Star("2b821237eb81a37448942997afb21e9e", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Surfing Surfaces", [
                new Star("b772330c648bc5b4dbf560e1636a765b", "Complete the course", StarType.CourseComplete),
                new Star("720fa9c239a1b224eae03b1dd031c9c5", "Complete in under 01:50", StarType.MinTime),
                new Star("4218afbc7f4895141a4ebd26d61272d3", "Complete in under 00:50", StarType.MinTime),
                new Star("a6179cec5a395964a807d262ec059a9e", "Reach a speed of 10.5", StarType.Challenge),
                new Star("5bd4d45837fdac649965d0e2b89e63d7", "No Deaths", StarType.NoDeaths),
                new Star("7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin", StarType.GoldenPin),
                new Star("d2a264a37b85ab045a885dc98fa2ad56", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Pole Vault", [
                new Star("b854fd83bb9c6b2438d05cfdaba66a91", "Complete the course", StarType.CourseComplete),
                new Star("3bb1fbe4b84c03a4086c6ac366f2ba27", "Complete in under 02:30", StarType.MinTime),
                new Star("f89d1fb8ad92a634d8703d0b8bb1c976", "Complete in under 01:10", StarType.MinTime),
                new Star("6bc239d7f01e9ee40a4ce3687f205482", "Spend less than 30 seconds on the ground", StarType.Challenge),
                new Star("f8a55461338a712468a59dcd64888350", "No Deaths", StarType.NoDeaths),
                new Star("5101c9e9015f4204bb42649a58127217", "Grab the golden pin", StarType.GoldenPin),
                new Star("b6fb416d4c5c4984db326bab8e23079a", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Chase Your Sister", [
                new Star("39d3e311aa4b8dc4e92da8ec2fc093e1", "Complete the course", StarType.CourseComplete),
                new Star("46a134cc36e348d4eb3416dc4f9df9de", "Complete in under 01:10", StarType.MinTime),
                new Star("f6a106bb664df5a4089b4fa5101f0152", "Complete in under 00:45", StarType.MinTime),
                new Star("34f041456579a904a98c746ea6e6bb2e", "Don't perform any swings", StarType.Challenge),
                new Star("34a0bf50ab8394f46a9a94542fbfc553", "Tag your sister", StarType.Challenge),
                new Star("a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin", StarType.GoldenPin),
                new Star("078d212bcd4e55a4f9fe32de2cf0ad2b", "Buddy Mode", StarType.Buddy), //TODO: should this be treated as a separate course/trial?
            ]),
            new Course("Pogo Trial", CourseType.Pogo, [
                new Star("5069ece9ed4a8ed479d1743620a34023", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new Course("Tiny Toy Trial", CourseType.TinyToy, [
                new Star("98310f580476ff04384f6abd4357370e", "Get to the finish line", StarType.TrialComplete),
            ]),
            new Course("Jetpack Trial", CourseType.Jetpack, [
                new Star("62137bb4c16995d4aaf2f385225156e8", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new Course("All Course Marathon", CourseType.AllCourseMarathon, [
                new Star("e9ee84430ecf11740beb41326451d414", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceField("gamemode_completed_force_field", "Gym/Office Hallway"),
                new ForceField("gamemode_completed_force_field (1)", "Office Hallway/Janitor's Closet"),
                new ForceField("gamemode_completed_force_field (2)", "Office Hallway/Back Hallway"),
                new ForceField("gamemode_completed_force_field (3)", "Computer Lab Hallway/Back Hallway"),
                new ForceField("gamemode_completed_force_field (4)", "Back Hallway/Side Entrance"),
            ]
        };

        public static World Playground => new World("83328d77e87a0d34b8cc4aae038bd158", "playground", "Playground", [
            new Course("Recess", [
                new Star("60dff4f3d39f9664d8cde41acf5a9465", "Complete the course", StarType.CourseComplete),
                new Star("e6704c37a77263d4fb352b2ebe0c9dd5", "Complete in under 07:00", StarType.MinTime),
                new Star("4e708a47372bdfd4c96d9853dd7204ac", "Complete in under 02:10", StarType.MinTime),
                new Star("724bcc52ae9d6df46a1fe83bc9e5f3b7", "No Deaths", StarType.NoDeaths),
                new Star("5a0c11e0ebfe8a642acdcb3f224e44c5", "Grab the golden pin", StarType.GoldenPin),
                new Star("f0804c3773358e54ea3381ab1d9a4fa4", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("36f06de3ffe852648bbf28e3f8383a1a", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Big Kids Side", [
                new Star("1ce626b7d30c7984bafa0241356ca1aa", "Complete the course", StarType.CourseComplete),
                new Star("d0a1872e506424544a944d02f1273cba", "Complete in under 08:00", StarType.MinTime),
                new Star("a92ec2a42e5efbf4eaf8b574dd625253", "Complete in under 02:45", StarType.MinTime),
                new Star("f417c8cd9958fe44a8c8bfb747a14856", "No Deaths", StarType.NoDeaths),
                new Star("f3605b943b5651b4eb7289ce220d4cef", "Grab the golden pin", StarType.GoldenPin),
                new Star("985db81c1b97b00498884c81ce9c1001", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("fe07e2dc513868a4cad555546bd3ce1c", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Bouncy Castle", [
                new Star("25da304df7917554c932e0db2ed12bbe", "Complete the course", StarType.CourseComplete),
                new Star("45851bf0a9623944fad09518b9be8a54", "Complete in under 04:30", StarType.MinTime),
                new Star("d7a3953016f961c43b8c97b3c538d838", "Complete in under 01:45", StarType.MinTime),
                new Star("15ba77a1f2ff6ec45a5d8a6ce1a2d7d3", "No Deaths", StarType.NoDeaths),
                new Star("b13ffe80cf201d54b9f48af60809cbd6", "Grab the golden pin", StarType.GoldenPin),
                new Star("81d32f5a10c05fe4c88a74abb7db2b84", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("623714b5444588f43a7e8d01a84b6525", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Back to Class", [
                new Star("888924c3b32682b4eba329c0840548d8", "Complete the course", StarType.CourseComplete),
                new Star("bcceae8fcf65da644b7252e135889074", "Complete in under 06:00", StarType.MinTime),
                new Star("1a524b89d300c44468b7057596730d82", "Complete in under 01:45", StarType.MinTime),
                new Star("3377dfa30ba551447b5c15d4f276c2c6", "No Deaths", StarType.NoDeaths),
                new Star("8c335e082a2eb024aa36d8c2e6cadb48", "Grab the golden pin", StarType.GoldenPin),
                new Star("69f0cf9071aad05479efee996377ea21", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("748605251b94a514f8ec0f85415ffa91", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Sports Day", [
                new Star("b9de4719a85dd0f48be1ef9df00248f5", "Complete the course", StarType.CourseComplete),
                new Star("8d1c5b5442d995f428a89236adaa7900", "Complete in under 01:40", StarType.MinTime),
                new Star("1b1ddc847b094bd4aafe998d05829548", "Complete in under 01:10", StarType.MinTime),
                new Star("d6d01532f4668284382bdbde28e3b59d", "No Deaths", StarType.NoDeaths),
                new Star("b3eb8fe6d65356a49a0cf46e288c69f6", "Grab the golden pin", StarType.GoldenPin),
                new Star("04ffbbd04e97bb441a90e7c7fb0d052e", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("c96eefdfbaa06d0459b7eec5221cd510", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Chase the Big Kid", [
                new Star("edd7311d9d1623e4891f8d4b955d345e", "Complete the course", StarType.CourseComplete),
                new Star("9a08d0a4fd4585c4fac47e975c79ad0d", "Complete in under 01:15", StarType.MinTime),
                new Star("a1b0ed2d2c267ae4d8391a364830160f", "Complete in under 00:58", StarType.MinTime),
                new Star("ce5ef6bbd31e325499fb9cf28fd80d36", "Tag your sister", StarType.Challenge),
                new Star("e621f981cf18e2242b26195512632798", "Grab the golden pin", StarType.GoldenPin),
                new Star("732eafb0ad0d6c547921c904d1ee4073", "Find the hidden G.A.T. comic", StarType.Comic),
                new Star("c96eefdfbaa06d0459b7eec5221cd510", "Buddy Mode", StarType.Buddy),
            ]),
            new Course("Pogo Trial 1", CourseType.Pogo, [
                new Star("1a56d171c35fcf547980a37d8f7f7960", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new Course("Pogo Trial 2", CourseType.Pogo, [
                new Star("9fc0edaa2793c9c45927c92caf5634b3", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new Course("Tiny Toy Trial 1", CourseType.TinyToy, [
                new Star("415b0dd0ab657c840a3073f2e952cb6c", "Get to the finish line", StarType.TrialComplete),
            ]),
            new Course("Tiny Toy Trial 2", CourseType.TinyToy, [
                new Star("5896145f43d1a6a4dbce56b1dbf7ca61", "Get to the finish line", StarType.TrialComplete),
            ]),
            new Course("Jetpack Trial", CourseType.Jetpack, [
                new Star("6065e19024fa4cc4d9d9ea20e581a25b", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new Course("Chase the Grade", CourseType.Chase, [
                new Star("d5c3b1aaf1343a143a7186eafdcdeaad", "Complete the course", StarType.TrialComplete),
            ]),
            new Course("All Course Marathon", CourseType.AllCourseMarathon, [
                new Star("234d3bf0d0df2af408329011a45dfd1b", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceField("gamemode_completed_force_field", "Spawn/Basketball Courts"),
                new ForceField("gamemode_completed_force_field (1)", "Basketball Courts/Sports Day Side"),
            ]
        };

        public static IEnumerable<World> AllWorlds => [
            GymClass,
            Playground
        ];
    }
}

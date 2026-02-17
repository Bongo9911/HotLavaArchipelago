using HotLavaArchipelagoPlugin.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace HotLavaArchipelagoPlugin.Models.Game
{
    internal static class Worlds
    {
        //TODO: No unlockable for Gym Class
        public static WorldInfo GymClass => new WorldInfo(100, "", "tutorial", "Gym Class", [
            new CourseInfo("Gym Jam", [
                new StarInfo(100, "8e72934736772e94eaa50290f7ae341a", "Complete the course", StarType.CourseComplete),
                new StarInfo(101, "2c2765d96036f40438807ce21e92b229", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(102, "2b6db09b813d3f54cb1424cbf312ca8d", "Complete in under 02:10", StarType.MinTime),
                new StarInfo(103, "e7991396471eede4d82f90334472ae2d", "Reach a speed of 9", StarType.Challenge),
                new StarInfo(104, "2f492a67f28d82c429c93fa9e48bfe3e", "No Deaths", StarType.NoDeaths),
                new StarInfo(105, "718fac78ec94caa478f347b892adae7a", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(106, "7680ae1ce1a1c1f49afc80c4e50df8a1", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Trampoline Trouble", [
                new StarInfo(110, "52813bff26455a34c92918c183c034fe", "Complete the course", StarType.CourseComplete),
                new StarInfo(111, "181644ae625180e4eb6989129d83e2fb", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(112, "3262cf7f58754504dbb378912ea87f07", "Complete in under 01:00", StarType.MinTime),
                new StarInfo(113, "0059648cfcaf1d3459f89b8b76db5eef", "Don't perform any swings", StarType.Challenge),
                new StarInfo(114, "5d26c3d1697052c45a3f29ab54d8bb0d", "No Deaths", StarType.NoDeaths),
                new StarInfo(115, "1a2ff5486c095154c9bf507773051774", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(116, "540b0bdc06ed2dd4887deff906e0575f", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Livin' on the Ledge", [
                new StarInfo(120, "f2aca0ab34ef5a34fb9b3d9a52087aff", "Complete the course", StarType.CourseComplete),
                new StarInfo(121, "c689392b82845f948b1a088de15c27ab", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(122, "513a99591da7c8f49855898242e338e7", "Complete in under 00:55", StarType.MinTime),
                new StarInfo(123, "dc67bb49b6bdd3f4ba6af59e7f46dc7e", "Spend less than 25 seconds on the ground", StarType.Challenge),
                new StarInfo(124, "df11767c8cafed7429faabbe863fac18", "No Deaths", StarType.NoDeaths),
                new StarInfo(125, "3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(126, "2b821237eb81a37448942997afb21e9e", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Surfing Surfaces", [
                new StarInfo(130, "b772330c648bc5b4dbf560e1636a765b", "Complete the course", StarType.CourseComplete),
                new StarInfo(131, "720fa9c239a1b224eae03b1dd031c9c5", "Complete in under 01:50", StarType.MinTime),
                new StarInfo(132, "4218afbc7f4895141a4ebd26d61272d3", "Complete in under 00:50", StarType.MinTime),
                new StarInfo(133, "a6179cec5a395964a807d262ec059a9e", "Reach a speed of 10.5", StarType.Challenge),
                new StarInfo(134, "5bd4d45837fdac649965d0e2b89e63d7", "No Deaths", StarType.NoDeaths),
                new StarInfo(135, "7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(136, "d2a264a37b85ab045a885dc98fa2ad56", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Pole Vault", [
                new StarInfo(140, "b854fd83bb9c6b2438d05cfdaba66a91", "Complete the course", StarType.CourseComplete),
                new StarInfo(141, "3bb1fbe4b84c03a4086c6ac366f2ba27", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(142, "f89d1fb8ad92a634d8703d0b8bb1c976", "Complete in under 01:10", StarType.MinTime),
                new StarInfo(143, "6bc239d7f01e9ee40a4ce3687f205482", "Spend less than 30 seconds on the ground", StarType.Challenge),
                new StarInfo(144, "f8a55461338a712468a59dcd64888350", "No Deaths", StarType.NoDeaths),
                new StarInfo(145, "5101c9e9015f4204bb42649a58127217", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(146, "b6fb416d4c5c4984db326bab8e23079a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Your Sister", [
                new StarInfo(150, "39d3e311aa4b8dc4e92da8ec2fc093e1", "Complete the course", StarType.CourseComplete),
                new StarInfo(151, "46a134cc36e348d4eb3416dc4f9df9de", "Complete in under 01:10", StarType.MinTime),
                new StarInfo(152, "f6a106bb664df5a4089b4fa5101f0152", "Complete in under 00:45", StarType.MinTime),
                new StarInfo(153, "34f041456579a904a98c746ea6e6bb2e", "Don't perform any swings", StarType.Challenge),
                new StarInfo(154, "34a0bf50ab8394f46a9a94542fbfc553", "Tag your sister", StarType.Challenge),
                new StarInfo(155, "a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(156, "078d212bcd4e55a4f9fe32de2cf0ad2b", "Wild Buddy Chase", StarType.BuddyChase), //TODO: should this be treated as a separate course/trial?
            ]),
            new CourseInfo("Pogo Trial", CourseType.Pogo, [
                new StarInfo(160, "5069ece9ed4a8ed479d1743620a34023", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo(161, "98310f580476ff04384f6abd4357370e", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(162, "62137bb4c16995d4aaf2f385225156e8", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(163, "e9ee84430ecf11740beb41326451d414", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo(110, "Gym/Office Hallway", new Vector3(33.752f, 1.76f, 1.6f)),
                new ForceFieldInfo(111, "Office Hallway/Janitor's Closet", new Vector3(24.622f, 1.758f, 25.966f)),
                new ForceFieldInfo(112, "Office Hallway/Back Hallway", new Vector3(18.55f, 1.86f, 29.089f)),
                new ForceFieldInfo(113, "Computer Lab Hallway/Back Hallway", new Vector3(0.853f, 1.633f, 25.97f)),
                new ForceFieldInfo(114, "Back Hallway/Side Entrance", new Vector3(-1.35f, 1.86f, 29.089f)),
            ]
        };

        public static WorldInfo Playground => new WorldInfo(200, "83328d77e87a0d34b8cc4aae038bd158", "playground", "Playground", [
            new CourseInfo("Recess", [
                new StarInfo(200, "60dff4f3d39f9664d8cde41acf5a9465", "Complete the course", StarType.CourseComplete),
                new StarInfo(201, "e6704c37a77263d4fb352b2ebe0c9dd5", "Complete in under 07:00", StarType.MinTime),
                new StarInfo(202, "4e708a47372bdfd4c96d9853dd7204ac", "Complete in under 02:10", StarType.MinTime),
                new StarInfo(203, "724bcc52ae9d6df46a1fe83bc9e5f3b7", "No Deaths", StarType.NoDeaths),
                new StarInfo(204, "5a0c11e0ebfe8a642acdcb3f224e44c5", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(205, "f0804c3773358e54ea3381ab1d9a4fa4", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(206, "36f06de3ffe852648bbf28e3f8383a1a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Big Kids Side", [
                new StarInfo(210, "1ce626b7d30c7984bafa0241356ca1aa", "Complete the course", StarType.CourseComplete),
                new StarInfo(211, "d0a1872e506424544a944d02f1273cba", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(212, "a92ec2a42e5efbf4eaf8b574dd625253", "Complete in under 02:45", StarType.MinTime),
                new StarInfo(213, "f417c8cd9958fe44a8c8bfb747a14856", "No Deaths", StarType.NoDeaths),
                new StarInfo(214, "f3605b943b5651b4eb7289ce220d4cef", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(215, "985db81c1b97b00498884c81ce9c1001", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(216, "fe07e2dc513868a4cad555546bd3ce1c", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Bouncy Castle", [
                new StarInfo(220, "25da304df7917554c932e0db2ed12bbe", "Complete the course", StarType.CourseComplete),
                new StarInfo(221, "45851bf0a9623944fad09518b9be8a54", "Complete in under 04:30", StarType.MinTime),
                new StarInfo(222, "d7a3953016f961c43b8c97b3c538d838", "Complete in under 01:45", StarType.MinTime),
                new StarInfo(223, "15ba77a1f2ff6ec45a5d8a6ce1a2d7d3", "No Deaths", StarType.NoDeaths),
                new StarInfo(224, "b13ffe80cf201d54b9f48af60809cbd6", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(225, "81d32f5a10c05fe4c88a74abb7db2b84", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(226, "623714b5444588f43a7e8d01a84b6525", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Back to Class", [
                new StarInfo(230, "888924c3b32682b4eba329c0840548d8", "Complete the course", StarType.CourseComplete),
                new StarInfo(231, "bcceae8fcf65da644b7252e135889074", "Complete in under 06:00", StarType.MinTime),
                new StarInfo(232, "1a524b89d300c44468b7057596730d82", "Complete in under 01:45", StarType.MinTime),
                new StarInfo(233, "3377dfa30ba551447b5c15d4f276c2c6", "No Deaths", StarType.NoDeaths),
                new StarInfo(234, "8c335e082a2eb024aa36d8c2e6cadb48", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(235, "69f0cf9071aad05479efee996377ea21", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(236, "748605251b94a514f8ec0f85415ffa91", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Sports Day", [
                new StarInfo(240, "b9de4719a85dd0f48be1ef9df00248f5", "Complete the course", StarType.CourseComplete),
                new StarInfo(241, "8d1c5b5442d995f428a89236adaa7900", "Complete in under 01:40", StarType.MinTime),
                new StarInfo(242, "1b1ddc847b094bd4aafe998d05829548", "Complete in under 01:10", StarType.MinTime),
                new StarInfo(243, "d6d01532f4668284382bdbde28e3b59d", "No Deaths", StarType.NoDeaths),
                new StarInfo(244, "b3eb8fe6d65356a49a0cf46e288c69f6", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(245, "04ffbbd04e97bb441a90e7c7fb0d052e", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(246, "c96eefdfbaa06d0459b7eec5221cd510", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase the Big Kid", [
                new StarInfo(250, "edd7311d9d1623e4891f8d4b955d345e", "Complete the course", StarType.CourseComplete),
                new StarInfo(251, "9a08d0a4fd4585c4fac47e975c79ad0d", "Complete in under 01:15", StarType.MinTime),
                new StarInfo(252, "a1b0ed2d2c267ae4d8391a364830160f", "Complete in under 00:58", StarType.MinTime),
                new StarInfo(253, "ce5ef6bbd31e325499fb9cf28fd80d36", "Tag your sister", StarType.Challenge),
                new StarInfo(254, "e621f981cf18e2242b26195512632798", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(255, "732eafb0ad0d6c547921c904d1ee4073", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(256, "c96eefdfbaa06d0459b7eec5221cd510", "Chase Buddy", StarType.BuddyChase),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo(260, "1a56d171c35fcf547980a37d8f7f7960", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo(261, "9fc0edaa2793c9c45927c92caf5634b3", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 1", CourseType.TinyToy, [
                new StarInfo(262, "415b0dd0ab657c840a3073f2e952cb6c", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 2", CourseType.TinyToy, [
                new StarInfo(263, "5896145f43d1a6a4dbce56b1dbf7ca61", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(264, "6065e19024fa4cc4d9d9ea20e581a25b", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo(265, "d5c3b1aaf1343a143a7186eafdcdeaad", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(266, "234d3bf0d0df2af408329011a45dfd1b", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo(210, "Spawn/Basketball Courts", new Vector3(17.603f, 2.501f, 14.035f)),
                new ForceFieldInfo(211, "Basketball Courts/Sports Day Side", new Vector3(17.4f, 2.5f, -11.5f)),
            ]
        };

        public static WorldInfo School => new WorldInfo(300, "212f728535833224287d420c81f43ef1", "school", "School", [
            new CourseInfo("ABCs and 123s", [
                new StarInfo(300, "57e0344bd3abfb4449346d182cd8e901", "Complete the course", StarType.CourseComplete),
                new StarInfo(301, "2ee56bdf1e09d1a4eb0170081b60b883", "Complete in under 12:00", StarType.MinTime),
                new StarInfo(302, "b2311faf93b83d0428c2a97d1ee72c6b", "Complete in under 05:00", StarType.MinTime),
                new StarInfo(303, "a14a4456df2c527479a4889c2555ae0a", "No Deaths", StarType.NoDeaths),
                new StarInfo(304, "9f6ad9759e59fc14cbffdd76792c9796", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(305, "d15988c45557cd641b5c1fed6d28b93c", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(306, "21c2d3bc6b1078546a556f6065b9944f", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Middle School Mischief", [
                new StarInfo(310, "73c90f4f92fb86e4d8643e0efe67f386", "Complete the course", StarType.CourseComplete),
                new StarInfo(311, "f495576fc3c4f654d97c874f8c48235f", "Complete in under 11:00", StarType.MinTime),
                new StarInfo(312, "bf0d228482a8f564d80bc8fc3919834e", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(313, "bcc4ecc8b68979143924c78e147e54cb", "No Deaths", StarType.NoDeaths),
                new StarInfo(314, "f0271c912e6562745aa87f64120386ca", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(315, "c4c3c7f707f83a74a91fd35a3c996200", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(316, "b2f7d84f93346b84880bf32ccb684c5a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Repeat the Grade", [
                new StarInfo(320, "72a71e5e6cb185843940831aff9f05d9", "Complete the course", StarType.CourseComplete),
                new StarInfo(321, "12bc63b0a54486449b09fa0883f5937a", "Complete in under 10:00", StarType.MinTime),
                new StarInfo(322, "ab81decce36f68b4db6801c220184dad", "Complete in under 03:30", StarType.MinTime),
                new StarInfo(323, "77e419b13b4e62d4f857c36a8a9622b7", "No Deaths", StarType.NoDeaths),
                new StarInfo(324, "de32cab8a4473d045a1095a17f779697", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(325, "d419edaf69289854f95c5c075c934b8c", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(326, "893408fe1ea036b4db838b1f48e25271", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Senior Trip", [
                new StarInfo(330, "972ec486a5a1b8d4c9d5d5c6894ff904", "Complete the course", StarType.CourseComplete),
                new StarInfo(331, "28b6b575cd513074fb9b9408bf3b46d9", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(332, "ee745e4fd95cc00468fa3ce854a39245", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(333, "18afc8a3a23bee045a7e502e8ee50216", "No Deaths", StarType.NoDeaths),
                new StarInfo(334, "b7a195d971b1d7848bb37ca8fcc68112", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(335, "29f436273460d574fbdfa1d797f29d13", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(336, "1620c9eae36334a4783950bdacc65a5d", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Freshman Frenzy", [
                new StarInfo(340, "180ece11a7fb5c841bcbb633544e131f", "Complete the course", StarType.CourseComplete),
                new StarInfo(341, "6b267b4581572464d8340e34babd9492", "Complete in under 10:00", StarType.MinTime),
                new StarInfo(342, "a1eb55827bd43be4bbb57ddd7845ee9f", "Complete in under 03:30", StarType.MinTime),
                new StarInfo(343, "c0e9b9f96d82eb84f8c579a1418440d4", "No Deaths", StarType.NoDeaths),
                new StarInfo(344, "a003111804c160e4a96cbad1914e793e", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(345, "3706423d30537ba4191a10e58d3b1611", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(346, "2fa7368db5bfb58489ec7e1a2adffa31", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Your Sister", [
                new StarInfo(350, "4d0edbf502e968b4a8a672e0a7355c81", "Complete the course", StarType.CourseComplete),
                new StarInfo(351, "93120cacc4fb33b4a813d188d5696fd5", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(352, "c563a09d63593a54092506d5aca87747", "Complete in under 00:50", StarType.MinTime),
                new StarInfo(353, "a8ff9a42e05c9c0448b37c1b8eb03412", "Tag your sister", StarType.Challenge),
                new StarInfo(354, "0fc1239839daab54686b01a5b36332c4", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(355, "b27ebe301bf36cf46b6a42735638d6fa", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(356, "30fdf0d70ac2f11469ea3a313dd76c2c", "Chase The Escaped Dog", StarType.BuddyChase),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo(360, "e641257639cc2354aa58580dae083b03", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo(361, "e09457a7ecb51044fb72744ce6699e25", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 3", CourseType.Pogo, [
                new StarInfo(362, "a9c4d26b69466bd45886f8825aff4054", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo(363, "ef5401ba94301e74fa8f32d8af2f429d", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(364, "b635ffc7a94e4eb4787160b9dc2b77fe", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo(365, "bc1ad6587b18207469969deace0ac6c5", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(366, "4aa7293d5e6db8d409a3655cfe6efefa", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo(310, "Gym Hallway/Computer Lab", new Vector3(32.16f, 2.168f, -24.77f)),
                new ForceFieldInfo(311, "Social Studies Hallway/Art Hallway", new Vector3(19f, 2.85f, -12.65f)),
                new ForceFieldInfo(312, "Teacher's Lounge Hallway/Art Hallway", new Vector3(-23.64f, 2.85f, -12.65f)),
                new ForceFieldInfo(313, "English Hallway/Teacher's Lounge Hallway", new Vector3(-23.745f, 3.46f, 1.31f)),
                new ForceFieldInfo(314, "Science Lab/Art Closet", new Vector3(4.046f, 2.177f, -24.828f)),
                new ForceFieldInfo(315, "Art Hallway/Art Class", new Vector3(-10.526f, 2.704f, -18.816f)),
                new ForceFieldInfo(316, "Art Closet/Art Class", new Vector3(-0.077f, 2.687f, -28.871f)),
                new ForceFieldInfo(317, "Teacher's Lounge/Courtyard", new Vector3(-8.333f, 3.724f, -6.338f)),
            ],
            DisabledForceFields = [
                new ForceFieldInfo(318, "Atrium/Cafeteria", new Vector3(5.079f, 2.141f, 16.822f)),
                new ForceFieldInfo(319, "Social Studies Class Left", new Vector3(22.022f, 2.1f, 1.9f)),
                new ForceFieldInfo(320, "Gym Hallway/Science Lab", new Vector3(15.927f, 2.177f, -29.253f)),
                new ForceFieldInfo(321, "Atrium/Social Studies Hallway", new Vector3(15.945f, 2.146f, 6.217f)),
                new ForceFieldInfo(322, "Social Studies Class Right", new Vector3(22.022f, 2.1f, -10.116f)),
            ]
        };

        public static WorldInfo Wholesale => new WorldInfo(400, "f5bd38e199f54fa46aff759206fd0806", "wholesale_expanded", "Wholesale", [
            new CourseInfo("To the Top", [
                new StarInfo(400, "16834aaf2d1962e44b22cbf1bd2d3a6f", "Complete the course", StarType.CourseComplete),
                new StarInfo(401, "058594844899efe45b805507f1addd7b", "Complete in under 10:00", StarType.MinTime),
                new StarInfo(402, "f87018bfc1f842740b56c2a1a92cf4a5", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(403, "9f8de75e70e8b5942842e8fb62b50a58", "No Deaths", StarType.NoDeaths),
                new StarInfo(404, "d73a042fdcb801841864b76903065548", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(405, "89dcbb351c916b944ae2d04c72a1ba22", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(406, "996d266c518979d438cbbe4a4a6b7b4a", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Duct and Cover", [
                new StarInfo(410, "2d0738601a766234087120f1a016a344", "Complete the course", StarType.CourseComplete),
                new StarInfo(411, "72bc9354d22810544ba7a1b8f66bc942", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(412, "41ecfbd3c9df62543be573b0253c6f54", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(413, "c7bfa995db564b9418bf9c523b38c5ce", "No Deaths", StarType.NoDeaths),
                new StarInfo(414, "57cd7507ede3def4da02d8e46cb66d30", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(415, "4c9dedd9fc5625e47afbd9dae457de29", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(416, "1b289b5ad606abb4e9f982b3d9d4fa32", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Meat Market", [
                new StarInfo(420, "edac1a5bb7363854086f7896a6a38255", "Complete the course", StarType.CourseComplete),
                new StarInfo(421, "81ac63c1ad8c4d5418ea125eaf3bb1f7", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(422, "e9c3888f1669e1344a9dc063e961efb8", "Complete in under 03:30", StarType.MinTime),
                new StarInfo(423, "ecbef8112dd43cb40a7515ac45e52731", "No Deaths", StarType.NoDeaths),
                new StarInfo(424, "aaf16c7eb791b4a4b9bd82b86c6c98ea", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(425, "33124f465d203ae49a6cec0a4764dc33", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(426, "2545f6cd979390b408e0d6d40de14a95", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Returns", [
                new StarInfo(430, "bab110cfadbf0584992291250189509a", "Complete the course", StarType.CourseComplete),
                new StarInfo(431, "4069629cbae76e44c85bad4829835876", "Complete in under 04:00", StarType.MinTime),
                new StarInfo(432, "75d3d573dae4a4f42a2b391d09746933", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(433, "e8fdb40f968d6aa40841f7bcd0f46170", "No Deaths", StarType.NoDeaths),
                new StarInfo(434, "f518343abbc940042b21423aef055173", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(435, "292e394b0fcd3fc47b647f3364ca70f6", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(436, "2031f866b2c2f884ba940f5384f34ec7", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Meat Grinder", [
                new StarInfo(440, "d647509540004f049a23d32a2f4d795c", "Complete the course", StarType.CourseComplete),
                new StarInfo(441, "0cc12dc8e0f5270458650b573b883ee3", "Complete in under 10:00", StarType.MinTime),
                new StarInfo(442, "869fef92e76d8c149b574ada1d109d9c", "Complete in under 04:30", StarType.MinTime),
                new StarInfo(443, "2d2c927fe825ed24a907e6e1d8dadf5e", "No Deaths", StarType.NoDeaths),
                new StarInfo(444, "6a7f4265256d34d408021ba7f9473f41", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(445, "9c1c28cdc25a53341b6eafb0a0a87768", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(446, "8ec1308f20c329147ac1a3f5661b79d4", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase Through the Store", [
                new StarInfo(450, "b13b5b41ee88fe144bfa267efb59c710", "Complete the course", StarType.CourseComplete),
                new StarInfo(451, "952e95490fb1bc34a93e5a3d2a543472", "Complete in under 00:50", StarType.MinTime),
                new StarInfo(452, "9cf25623f3fd60a44818bd90028dba35", "Complete in under 00:33", StarType.MinTime),
                new StarInfo(453, "aac1119a19a3e7743995599868918357", "No Deaths", StarType.NoDeaths),
                new StarInfo(454, "550fcd82164dcea41b524a107a2632c2", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(455, "efe2e7dbec062e2448a4f80219fdfe78", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(456, "c241690961d459243ad294a277746e36", "Chase the Runaway Dog", StarType.BuddyChase),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo(460, "314ee7741f7851e4e82f7d5581088470", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo(461, "d7fe16db846211043ae130ad5db97bd3", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 3", CourseType.Pogo, [
                new StarInfo(462, "70ae3c38be714cc40bba4feeea5fd355", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo(463, "4604d3c043b5320489bd6eaac865277b", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(464, "6a8ec6917998047489e769a72a418d5a", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("Chase the Grade", CourseType.Chase, [
                new StarInfo(465, "eee4a71e511af8f498f55c55fe06a8aa", "Complete the course", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(466, "1d4f91a1fb1547b429b539972938e5d9", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo(410, "Employee Hallway/Checkout", new Vector3(-32.53f, 2.7f, -12.203f)),
                new ForceFieldInfo(411, "Employee Hallway/Janitor's Closet", new Vector3(-10.862f, 6.123f, -3.862f)),
                new ForceFieldInfo(412, "Under the Shelves", new Vector3(-2.871f, 1.38f, -37.507f)),
                new ForceFieldInfo(413, "Checkout/Cart Storage", new Vector3(-29.134f, 2.125f, -40.67f)),
                new ForceFieldInfo(414, "Employee Hallway/Breakroom", new Vector3(-4.788f, 6.112f, 2.11f)),
                new ForceFieldInfo(415, "Checkout/Shopping Area", new Vector3(-26.5f, 2.700001f, -17.8f)),
            ]
        };

        public static WorldInfo MasterClass => new WorldInfo(500, "6bd78323a271b804890f9bd232ae0abb", "mastery_gym", "Master Class", [
            new CourseInfo("Air Control Mastery", [
                new StarInfo(500, "399017d32e64e7147850bb221ad9a330", "Complete the course", StarType.CourseComplete),
                new StarInfo(501, "d9f265807b6647a46a071e38d0f43199", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(502, "5745104145f93b54fb585474ed7f395f", "Complete in under 01:00", StarType.MinTime),
                new StarInfo(503, "a19ee0938f049324b8e0f5c2994926f9", "No Deaths", StarType.NoDeaths),
                new StarInfo(504, "05207fb7837a51645b97a0b17b018f4c", "Reach a speed of 4.5", StarType.Challenge),
                new StarInfo(505, "59b9c6018baa64c48961b3f4d897e49e", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Wall Jump Mastery", [
                new StarInfo(510, "eb5e3cb0b7b99b04284d730869ce9fb6", "Complete the course", StarType.CourseComplete),
                new StarInfo(511, "377e30517bff4ac47929b80aecda0062", "Complete in under 02:30", StarType.MinTime),
                new StarInfo(512, "52e25e2c6bf4c454c8d44ef4d6b66688", "Complete in under 00:50", StarType.MinTime),
                new StarInfo(513, "b90b53beac85f6c4098e99c45a51b279", "No Deaths", StarType.NoDeaths),
                new StarInfo(514, "3517d5ce270ecd64c9c75ffb7f3f475e", "Spend less than 10 seconds on the ground", StarType.Challenge),
                new StarInfo(515, "1a2ff5486c095154c9bf507773051774", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Surf Mastery", [
                new StarInfo(520, "4c31e47b2b82aa64e94b5a4e67139775", "Complete the course", StarType.CourseComplete),
                new StarInfo(521, "079222fa9d56f2242a92321dd8d5a599", "Complete in under 01:00", StarType.MinTime),
                new StarInfo(522, "4a6c828b9643ba4459bd1ae1f978dadb", "Complete in under 00:30", StarType.MinTime),
                new StarInfo(523, "75645b25d426a094090d1cccadcf9d8d", "No Deaths", StarType.NoDeaths),
                new StarInfo(524, "417d208b1ed26b148987a5aa7c4ca388", "Complete in 5 jumps or less", StarType.Challenge),
                new StarInfo(525, "5101c9e9015f4204bb42649a58127217", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Boosting Mastery", [
                new StarInfo(530, "5b3155d2c67f23b41b5b0ba6bf1d6740", "Complete the course", StarType.CourseComplete),
                new StarInfo(531, "b7922c4cbf053554bb909c37ec79be1a", "Complete in under 04:00", StarType.MinTime),
                new StarInfo(532, "ac633c1cbf3212d4f95012155cd973e3", "Complete in under 01:00", StarType.MinTime),
                new StarInfo(533, "245dcd87539561241bc955e5fa833fab", "No Deaths", StarType.NoDeaths),
                new StarInfo(534, "9e048a77386525d4297f9775efcb19bc", "Reach a speed of 5.5", StarType.Challenge),
                new StarInfo(535, "3a0ab619a819e514cb7aa6c7ceaa3175", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Wind Tunnel Mastery", [
                new StarInfo(540, "cd8c91a38849df5459afed878e70071b", "Complete the course", StarType.CourseComplete),
                new StarInfo(541, "bdf016c29782aae43b9774c23d505fd0", "Complete in under 01:00", StarType.MinTime),
                new StarInfo(542, "a7ce1e8f9ec2b6948a3b6ad889c26876", "Complete in under 00:30", StarType.MinTime),
                new StarInfo(543, "2a07db3d496a8ac4fa4ab923375af82f", "No Deaths", StarType.NoDeaths),
                new StarInfo(544, "b5c4c4bffc5aac04c864a1f502e10e35", "Spend less than 5 seconds on the ground", StarType.Challenge),
                new StarInfo(545, "7ed7e2810fd00a14aaea4f267e33400e", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Honors Gym Class", [
                new StarInfo(550, "878f8f8195d19424aa87e2208d772141", "Complete the course", StarType.CourseComplete),
                new StarInfo(551, "5ea9aa8d87657b944b5c2dcc3d4cb3e3", "Complete in under 04:30", StarType.MinTime),
                new StarInfo(552, "ccf6b59e15b3b7d4b8b69bbc4dbcda13", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(553, "61f2faa484a77ca4f946157b8a8db437", "No Deaths", StarType.NoDeaths),
                new StarInfo(554, "837d73aefc94673408942e14f960f13a", "Reach a speed of 9", StarType.Challenge),
                new StarInfo(555, "a45613d4c2daadb4aa581548048d2c7a", "Grab the golden pin", StarType.GoldenPin),
            ]),
            new CourseInfo("Pogo Trial", CourseType.Pogo, [
                new StarInfo(560, "18fb5a39cfc1f434694fdb7f9f2e87b2", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo(561, "a4cb9f0819847fa4a85dc913af64ac57", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(562, "57638f812bae42d43b2a164910db9f86", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(563, "4ed34eb6dd88bce4e8f33c59d6a122f5", "Complete the course", StarType.TrialComplete),
            ]),
        ]);

        public static WorldInfo Basement => new WorldInfo(600, "baf5e18a2fbcc64409f83e53e836cb42", "basement", "Basement", [
            new CourseInfo("Race to the Summit", [
                new StarInfo(600, "e9684256332e65f418b64e8d7c7f1083", "Complete the course", StarType.CourseComplete),
                new StarInfo(601, "7ab28797b20035d488ff0f3c0cfc1c98", "Complete in under 05:00", StarType.MinTime),
                new StarInfo(602, "65445040deaac5e439c9926b978befb8", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(603, "3c9e33538aac447439e7a663e123652c", "No Deaths", StarType.NoDeaths),
                new StarInfo(604, "ce2c9f759d68d004ba81ee6cddb350bc", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(605, "6d2fd769cbf276844aa8000559cf1dde", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(606, "d7f3d5b0a214e734aaa4bf3c163437d7", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Lights Out", [
                new StarInfo(610, "2567589f5e43ea64b90b794dc26ad549", "Complete the course", StarType.CourseComplete),
                new StarInfo(611, "f06fe218b38d7a74bb24f0519376abfa", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(612, "d45f9fffe87307a45b29d8e206340869", "Complete in under 00:45", StarType.MinTime),
                new StarInfo(613, "85a44c29137af204082bf25a249fb034", "No Deaths", StarType.NoDeaths),
                new StarInfo(614, "a9fe5a26ad9c854488e1af94f556a140", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(615, "b469c7dc9ab662e4a9e49c9bfb0af06e", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(616, "bdeea14bf70a23d4399cabe73c3b0f63", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Temple Sprint", [
                new StarInfo(620, "7a9fc04771d0bc74cb3eb77a37154b04", "Complete the course", StarType.CourseComplete),
                new StarInfo(621, "e51bf9051e4f37246a94bcc0a2623cc6", "Complete in under 04:20", StarType.MinTime),
                new StarInfo(622, "990e4a1fe47afba429bff232c94a4f72", "Complete in under 01:30", StarType.MinTime),
                new StarInfo(623, "85a44c29137af204082bf25a249fb034", "No Deaths", StarType.NoDeaths),
                new StarInfo(624, "48afaf66fe2ec6c41834505aa259bb09", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(625, "9ef52ac567b7f134dbb25daa37cb20fb", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(626, "887c43d4f3391654ea31c1c63502d4f2", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Fancy Footwork", [
                new StarInfo(630, "23bd55daa347ba2479460ba058dda3dc", "Complete the course", StarType.CourseComplete),
                new StarInfo(631, "5fb475ac42879ff4c95f3108e99aa5ab", "Complete in under 06:00", StarType.MinTime),
                new StarInfo(632, "60734bb36a050544587517fc1d07b0a8", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(633, "1f474d92398472040b1fcfa02c69e470", "No Deaths", StarType.NoDeaths),
                new StarInfo(634, "f7a769806969c2e4f952e731eeaed8f4", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(635, "ba6127dff14d6cc4fb43181bc5ab2da2", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(636, "2eab13332f36cee418049ace462d3afc", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Minecart Carnage", [
                new StarInfo(640, "c4f662f027b94e04b9b7515ca4697cb1", "Complete the course", StarType.CourseComplete),
                new StarInfo(641, "820f31c7b04f8234689d920452441a02", "Complete in under 08:00", StarType.MinTime),
                new StarInfo(642, "3c1f4f9c6972bab4ebf185a920c02d59", "Complete in under 02:45", StarType.MinTime),
                new StarInfo(643, "4a3d5f503d20c4d4b8a7eb35549d2f0d", "No Deaths", StarType.NoDeaths),
                new StarInfo(644, "b54421460675e7041b5f8579a3e986f9", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(645, "474bf210742ef8349a3704799ee50cab", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(646, "8bea8301696abde4d949694e60998281", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase The Meaning", [
                new StarInfo(650, "f41dcc3163d3a6e44b1e63e50aef6203", "Complete the course", StarType.CourseComplete),
                new StarInfo(651, "632f4236200df684097e0c208be9f7cc", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(652, "e8a89364a03ff9148b9e8fdd8cef0881", "Complete in under 01:30", StarType.MinTime),
                new StarInfo(653, "2ae00895938d8944b92613a97364ddae", "Tag your sister", StarType.Challenge),
                new StarInfo(654, "dcdf12844ab861b4aa611b5d82b86d69", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(655, "381a82e80aa2b544f93638befa072438", "Find the hidden G.A.T. comic", StarType.Comic),
                new StarInfo(656, "8e6b8c0b2c298004dad92e10625712b9", "Wild Teddy Chase", StarType.BuddyChase),
            ]),
            new CourseInfo("Pogo Trial", CourseType.Pogo, [
                new StarInfo(660, "0dd75e2d4a1de8a41ad6d6d559efc3ea", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial", CourseType.TinyToy, [
                new StarInfo(661, "ae2f8bc0810d0b34bb39bafa448eb903", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(662, "d314f1d6e3dfa7f44b710b731fc91a1b", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(663, "2b2d24eea94f2884db79dd2a24720bea", "Complete the course", StarType.TrialComplete),
            ]),
        ])
        {
            ForceFields = [
                new ForceFieldInfo(610, "Foyer/Living Room", new Vector3(37.874f, 8.016f, 10.712f)),
                new ForceFieldInfo(611, "Foyer/Dining Room", new Vector3(36.5f, 8.05f, 18f)),
                new ForceFieldInfo(612, "Kitchen/Basement Storage", new Vector3(26.247f, 7.945f, 19.363f)),
                new ForceFieldInfo(613, "Basement Storage/Den", new Vector3(34.6f, 3.35f, 30.2f)),
            ]
        };

        //TODO: No Unlockable ID exists for Rocco's Arcade
        public static WorldInfo RoccosArcade => new WorldInfo(700, "", "fun_centre", "Rocco's Arcade", [
            new CourseInfo("Arcade Action", [
                new StarInfo(700, "def601d8323ecaf4fb65504a3f73192c", "Complete the course", StarType.CourseComplete),
                new StarInfo(701, "9fc84b49f92528142b1d847cbc2d7076", "Complete in under 02:50", StarType.MinTime),
                new StarInfo(702, "6c06cd3d4a063bd43b7f3907c1cb845b", "Complete in under 01:15", StarType.MinTime),
                new StarInfo(703, "d4cb50cf712af3b42b2913bf1488b220", "No Deaths", StarType.NoDeaths),
                new StarInfo(704, "879548bdea3315a468c217c7347d1156", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(705, "fd1c5375abccef44cb5859c68e8ae767", "Find the Mini Rocco", StarType.Comic),
                new StarInfo(706, "b7da07bfefcdab049ad77dfd640733ca", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Rocco's Rumpus Room", [
                new StarInfo(710, "4360d78fb741961408ad39277bab8569", "Complete the course", StarType.CourseComplete),
                new StarInfo(711, "509cc0ce876e60f44825c9fec48d2846", "Complete in under 02:20", StarType.MinTime),
                new StarInfo(712, "43ef3811606de8f41a7f04dba809bbbd", "Complete in under 00:45", StarType.MinTime),
                new StarInfo(713, "c8c5fd1cbefd8b147a8fc93d201f5177", "Spend less than 24 seconds on the ground", StarType.Challenge),
                new StarInfo(714, "46be6e61ceff30040b2d5ad104fe427f", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(715, "4661b76be6f35644d9c4ecd5077cdf45", "Find the Mini Rocco", StarType.Comic),
                new StarInfo(716, "58fc1ca34eabb944d96dd3b79f8046f0", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Employees Only", [
                new StarInfo(720, "f00b65ab24cc4294eb1b6f789b4fa5e5", "Complete the course", StarType.CourseComplete),
                new StarInfo(721, "5590fcc88aea11d4bbde7f9154311c9a", "Complete in under 04:20", StarType.MinTime),
                new StarInfo(722, "75ca91dee56785c4eb545f27a08f6477", "Complete in under 01:20", StarType.MinTime),
                new StarInfo(723, "85465cc4d88f37747874af642e374fc4", "No Deaths", StarType.NoDeaths),
                new StarInfo(724, "f3970acce4d9d0249b4525dea7e29dbe", "Find and open all the gas valves", StarType.Challenge),
                new StarInfo(725, "46e85cc6d2ecc284480793e01c66ad09", "Find the Mini Rocco", StarType.Comic),
                new StarInfo(726, "61e4e68c37bac3849a98162aad462baa", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Storage Room Spree", [
                new StarInfo(730, "12080125e1eed3e4ea145410df8528e8", "Complete the course", StarType.CourseComplete),
                new StarInfo(731, "ff3e742bc84f4b242a4ad86c72a50f3c", "Complete in under 02:00", StarType.MinTime),
                new StarInfo(732, "daceedb0d8bde56419ddc1f8c8f05d08", "Complete in under 00:45", StarType.MinTime),
                new StarInfo(733, "21bed3d7ca947254a95a9ac508584f0e", "Find and open all the gas valves", StarType.Challenge),
                new StarInfo(734, "f191a515520153d42a31e4361daa25d8", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(735, "16cd44721efa3e647b6dc75a424fa585", "Find the Mini Rocco", StarType.Comic),
                new StarInfo(736, "2f681afeeb6dfd245bcff249e8ca150d", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Birthday Blowout", [
                new StarInfo(740, "7b6ec6b095231fb4d84022e058bb9f53", "Complete the course", StarType.CourseComplete),
                new StarInfo(741, "8aafd5d3de65d9e4b90d2875e2b67b13", "Complete in under 03:00", StarType.MinTime),
                new StarInfo(742, "6e354066417749141a55c5c916ee99c3", "Complete in under 00:50", StarType.MinTime),
                new StarInfo(743, "5272c3fadb5946f499db0b553fb0eefb", "Find and open all the gas valves", StarType.Challenge),
                new StarInfo(744, "ce5b071025807c444ae7da9e7e348687", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(745, "9a85e15e907facb4fb6a48e0e0bd468b", "No Deaths", StarType.NoDeaths),
                new StarInfo(746, "e6f8303c99230264bad389031ad58fc0", "Buddy Mode", StarType.Buddy),
            ]),
            new CourseInfo("Chase to the Arcade", [
                new StarInfo(750, "e442a16f1da076a409aadf5584f28161", "Complete the course", StarType.CourseComplete),
                new StarInfo(751, "df5fae0241f4cf443bba120eb89713ca", "Complete in under 00:55", StarType.MinTime),
                new StarInfo(752, "c8e591626c816e344be403b1411e1b30", "Complete in under 00:35", StarType.MinTime),
                new StarInfo(753, "1a1eb323b4515c74793443fe24a91a9e", "Tag your sister", StarType.Challenge),
                new StarInfo(754, "d494f370cec9bc84ca84837a50f071f7", "Grab the golden pin", StarType.GoldenPin),
                new StarInfo(755, "47f87b32bbb4ac746ab54919f323786f", "Find the Mini Rocco", StarType.Comic),
                new StarInfo(756, "dfe34b01be6b7814ba491f4da69fa75f", "Where's Buddy?", StarType.BuddyChase),
            ]),
            new CourseInfo("Pogo Trial 1", CourseType.Pogo, [
                new StarInfo(760, "21b743df5b4bf694498aa3767394ba60", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Pogo Trial 2", CourseType.Pogo, [
                new StarInfo(761, "bb25c6331ce2f6f4e9e33f0ad73b1364", "Find all the checkpoints using the Pogo Stick", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 1", CourseType.TinyToy, [
                new StarInfo(762, "2001a9f8edfe4d845aa91cd9f12cabd7", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Tiny Toy Trial 2", CourseType.TinyToy, [
                new StarInfo(763, "9dc7036229864094bae7c57f1f4df399", "Get to the finish line", StarType.TrialComplete),
            ]),
            new CourseInfo("Jetpack Trial", CourseType.Jetpack, [
                new StarInfo(764, "6bed6a2b0e10a754d83714792bc01057", "Find all the checkpoints using the Jetpack", StarType.TrialComplete),
            ]),
            new CourseInfo("All Course Marathon", CourseType.AllCourseMarathon, [
                new StarInfo(765, "84226baa8e2082048869e81e1b53e167", "Complete the course", StarType.TrialComplete),
            ]),
        ]);

        public static IEnumerable<WorldInfo> AllWorlds => [
            GymClass,
            Playground,
            School,
            Wholesale,
            MasterClass,
            Basement,
            RoccosArcade,
        ];
    }
}